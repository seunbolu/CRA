using CRA.Data;
using CRA.Data.Entities;
using CRA.Helper;
using CRA.Models.CHGSite;
using CRA.Models.PreScreen;
using CRA.Models.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace CRA.Controllers
{
    public partial class PreScreenController : BaseController
    {

        [HttpPost]
        public JsonResult VerificationComplete(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
            if (preScreen == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
            }

            if (!preScreen.VerificationComplete)
            {
                preScreen.VerificationComplete = true;
                _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, "Verification Complete", _dataContext.UserId.Value);
                _dataContext.SaveChanges();
            }
            else
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Pre-Screen already marked as Verification Complete." });
            }

            return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen successfully marked as Verification Complete." });

        }


        public void PreScreenInProgress(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            if (preScreen.Status == "Created")
            {
                preScreen.Status = "Pre-Screen In Progress";
                _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, "Pre-Screen In Progress", _dataContext.UserId.Value);
                _dataContext.SaveChanges();
            }

        }

        public JsonResult UpdatePreScreenStatus(long preScreenId)
        {
            return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen status updated successfully." });
        }

        [HttpPost]
        public JsonResult Submit(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
            if (preScreen == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
            }

            preScreen.LastSubmitted = DateTime.Now;

            var items = ValidatePreScreen(preScreen.PreScreenId);

            if (preScreen.Status == "Created" || preScreen.Status == "Pre-Screen In Progress")
            {
                if (TabsValid(new string[] { "pslabs", "psrespiratory", "psfunctions", "pssystems", "psstatus", "referralsourceinfo" }, items))
                {
                    preScreen.Status = "Pre-Screen Complete";
                    _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, "Pre-Screen Complete", _dataContext.UserId.Value);

                    //Automatically set the status of the corresponding task to completed.

                    var tasks = _dataContext.UserTaskParameters.Where(p => p.Deleted == false && p.Key == "PRE_SCREEN_ID" && p.Value == preScreen.PreScreenId.ToString()).Select(p => p.UserTask).Distinct();
                    foreach (var task in tasks)
                    {
                        if (task.Status == "Created")
                        {
                            task.Status = "Completed";
                        }
                    }

                    SendPreScreenCompleteAlertAndNotification(preScreen.PreScreenId);
                }

            }

            _dataContext.SaveChanges();

            return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen successfully submitted.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

        }


        private void SendPreScreenCompleteAlertAndNotification(long preScreenId)
        {
            Dictionary<string, string> values = null;
            List<long> CHGSiteIds = _dataContext.GetUserSites(_dataContext.UserId.Value).Select(p => p.CHGSiteId).ToList();
            List<User> users = null;

            var preScreen = _dataContext.PreScreens.Find(preScreenId);


            values = new Dictionary<string, string>();
            values.Add("TMP_SUBJECT", "PRE_SCREEN_COMPLETE_SUBJECT");
            values.Add("TMP_BODY", "PRE_SCREEN_COMPLETE_BODY");
            values.Add("PATIENT_IDENTIFIER", $"{preScreen.PatientId}");
            values.Add("PRE_SCREEN_ID", $"{preScreen.PreScreenId}");
            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
            values.Add("EMAIL_FROM_NAME", "CRA");

            users = _dataContext.GetRoleUsers("CAC").Distinct().ToList();

            if (users != null)
            {
                foreach (var user in users)
                {
                    _dataContext.CreateEmailAlert(user.UserId, values);
                    _dataContext.CreateNotification("Success", "Approved", $"Pre-Screen {preScreen.PreScreenId} completed.", user.UserId);
                }
            }




        }

        private bool TabsValid(string[] tabs, List<KeyValuePairModel> items)
        {
            foreach (var tab in tabs)
            {
                if (!bool.Parse(items.Where(p => p.Key == tab).Single().Value))
                {
                    return false;
                }
            }

            return true;
        }


        public JsonResult GetCompletePrescreenFields(long preScreenId)
        {
            var patientId = _dataContext.PreScreens.Find(preScreenId).PatientId;

            var preScreenItems = _dataContext.PreScreenData.Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList();
            var patientData = _dataContext.PatientData.Where(p => p.PatientId == patientId && p.Deleted == false).ToList();

            return Json(new { Status = Constant.RESPONSE_OK, PreScreenItems = preScreenItems, PatientItems = patientData });

        }


        private string GetPatientDataItem(string sectionCode, string itemCode, long patientId)
        {
            var patientData = _dataContext.PatientData.Where(p => p.PatientId == patientId && p.Deleted == false).ToList();
            var item = patientData.Where(p => p.SectionCode == sectionCode && p.ItemCode == itemCode).SingleOrDefault();
            if (item != null)
            {
                return item.Value;
            }

            return null;
        }


        private string GetPreScreenDataItem(string sectionCode, string itemCode, long preScreenId)
        {
            var patientData = _dataContext.PreScreenData.Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList();
            var item = patientData.Where(p => p.SectionCode == sectionCode && p.ItemCode == itemCode).SingleOrDefault();
            if (item != null)
            {
                return item.Value;
            }

            return null;
        }

        [HttpPost]
        public JsonResult Medicare(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }

                string payorType = GetPatientDataItem("payorinfo.primary.all", "payortype", preScreen.PatientId);
                string payorCategory = GetPatientDataItem("payorinfo.primary.all", "payorcategory", preScreen.PatientId);

                if (payorCategory == "Medicare" && payorType == "Medicare")
                {

                    preScreen.AdmissionNotes = null;
                    preScreen.DenialReason = null;
                    preScreen.NonAdmitReason = null;


                    preScreen.AdmissionStatus = model.AdmissionStatus;
                    preScreen.AdmissionNotes = model.AdmissionNotes;

                    if (model.AdmissionStatus == "Non Admit")
                    {
                        if (string.IsNullOrEmpty(model.NonAdmitReason))
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid Non Admit Reason. Please select a valid Non Admit reason." });
                        }

                        preScreen.NonAdmitReason = model.NonAdmitReason;
                    }

                    if (model.AdmissionStatus == "Denial")
                    {
                        if (string.IsNullOrEmpty(model.DenialReason))
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid Denial Reason. Please select a valid Denial reason." });
                        }

                        preScreen.DenialReason = model.DenialReason;
                    }


                    if (model.AdmissionStatus == "Pre Admit")
                    {
                        preScreen.DenialReason = null;
                        preScreen.NonAdmitReason = null;
                    }

                    preScreen.Status = $"Medicare {model.AdmissionStatus}";



                    _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, preScreen.Status, _dataContext.UserId.Value);
                    _dataContext.SaveChanges();

                    transaction.Commit();
                    return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen medicare admission status set successfully.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });
                }


                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Not a medicare patient. The payor category and payor type in the primary payor all section should be set to medicare." });
            }

        }


        [HttpPost]
        public JsonResult Admit(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }

                
                    preScreen.AdmissionNotes = null;
                    preScreen.DenialReason = null;
                    preScreen.NonAdmitReason = null;

                    preScreen.AdmissionStatus = "Admit";
                    preScreen.AdmissionNotes = model.AdmissionNotes;

                              
                    preScreen.Status = "Completed";

                    _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, preScreen.Status, _dataContext.UserId.Value);
                    _dataContext.SaveChanges();

                    transaction.Commit();
                    return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen patient admission status set successfully.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });
          


               
            }

        }


        #region "workflow"



        [HttpPost]
        public JsonResult CEOApprovalRequest(long preScreenId, CEOApprovalModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                if (!preScreen.PendingCEOApproval)
                {

                    //Create tasks and notifications for CEO.

                    var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreen.PreScreenId && p.Deleted == false).Single();

                    Dictionary<string, string> values = null;

                    List<User> users = null;

                    users = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD")).Distinct().ToList();

                    foreach (var user in users)
                    {
                        var task = _dataContext.CreateTask("Pre-Screen Approval", user.UserId, "Created", "Pre-Screen Approval Request", null, model.CEOApprovalNotes);

                        _dataContext.SaveChanges();
                        _dataContext.UpdateTaskParameter(task.UserTaskId, "PRE_SCREEN_ID", preScreen.PreScreenId.ToString());
                        _dataContext.UpdateTaskParameter(task.UserTaskId, "CHGSITE_ID", referral.CHGSiteId.ToString());


                        _dataContext.CreateNotification("Info", "Pre-Screen Approval Request", $"Approval Required for Pre-Screen {preScreenId}.", user.UserId);

                        values = new Dictionary<string, string>();
                        values.Add("TMP_SUBJECT", "PRE_SCREEN_APPROVAL_SUBJECT");
                        values.Add("TMP_BODY", "PRE_SCREEN_APPROVAL_BODY");
                        values.Add("PATIENT_IDENTIFIER", $"{preScreen.PatientId}");
                        values.Add("PRE_SCREEN_ID", $"{preScreen.PreScreenId}");
                        values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                        values.Add("EMAIL_FROM_NAME", "CRA");
                        values.Add("TASK_URL", ConfigurationManager.AppSettings["APP_URL"] + "/Task/ResolvePreScreenApproval/" + task.UserTaskId);


                        _dataContext.CreateEmailAlert(user.UserId, values);

                    }

                    if (users.Count() > 0)
                    {
                        preScreen.PendingCEOApproval = true;
                        preScreen.CEOApprovalRequestNotes = model.CEOApprovalNotes;
                        _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, "CEO Approval Requested", _dataContext.UserId.Value);

                    }

                    _dataContext.SaveChanges();
                    transaction.Commit();
                }
                else
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Pre-Screen already waiting on a pending CEO Approval Request." });
                }

                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen task created for CEO Approval." });



            }
        }


        private void ResetAdmissionFields(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);
            preScreen.AdmissionType = null;
            preScreen.AdmissionNotes = null;
            preScreen.AdmissionStatus = null;
            preScreen.DenialReason = null;
            preScreen.NonAdmitReason = null;
        }

        [HttpPost]
        public JsonResult AuthInitiated(long preScreenId, PreScreenUpdateStatusModel model)
        {
            if (string.IsNullOrEmpty(model.AuthReference))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid Auth Reference. Please enter the auth reference and try again." });
            }
            using (var transaction = _dataContext.Database.BeginTransaction())
            {


                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                ResetAdmissionFields(preScreenId);
                preScreen.AuthReference = model.AuthReference;
                preScreen.Status = "Auth Initiated";
                _dataContext.AddPreScreenStatusLog(preScreenId, "Auth Initiated", _dataContext.UserId.Value);
                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Auth Initiated.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }


        [HttpPost]
        public JsonResult AuthSubmitted(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }
                if (!model.Agree)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Please agree by selecting the check box." });
                }
                ResetAdmissionFields(preScreenId);
                preScreen.Status = "Auth Submitted";
                _dataContext.AddPreScreenStatusLog(preScreenId, "Auth Submitted", _dataContext.UserId.Value);
                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Auth Submitted.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }


        [HttpPost]
        public JsonResult AuthApproved(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                ResetAdmissionFields(preScreenId);

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }


                preScreen.Status = "Auth Approved";

                preScreen.AdmissionStatus = model.AdmissionStatus;
                preScreen.AdmissionNotes = model.AdmissionNotes;

                if (model.AdmissionStatus == "Non Admit")
                {
                    if (string.IsNullOrEmpty(model.NonAdmitReason))
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid Non Admit Reason. Please select a valid Non Admit reason." });
                    }

                    preScreen.NonAdmitReason = model.NonAdmitReason;
                }



                if (model.AdmissionStatus == "Pre Admit")
                {
                    preScreen.DenialReason = null;
                    preScreen.NonAdmitReason = null;
                }


                preScreen.AdmissionStatus = model.AdmissionStatus;

                SendAuthApprovedAlert(preScreen.PreScreenId);
                _dataContext.AddPreScreenStatusLog(preScreenId, "Auth Approved", _dataContext.UserId.Value);
                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Auth Approved.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }



        [HttpPost]
        public JsonResult AuthDenied(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }
                ResetAdmissionFields(preScreenId);

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }

                if (model.AdmissionStatus == "Denial")
                {
                    if (string.IsNullOrEmpty(model.DenialReason))
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid Denial Reason. Please select a valid Denial reason." });
                    }

                    preScreen.DenialReason = model.DenialReason;
                }

                preScreen.AdmissionStatus = model.AdmissionStatus;

                preScreen.Status = "Auth Denied";
                SendAuthDeniedAlert(preScreen.PreScreenId);

                _dataContext.AddPreScreenStatusLog(preScreenId, "Auth Denied", _dataContext.UserId.Value);
                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Auth Denied.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }




        [HttpPost]
        public JsonResult SCASubmitted(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                if (!model.Agree)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Please agree by selecting the check box." });
                }

                ResetAdmissionFields(preScreenId);
                preScreen.Status = "SCA Submitted";

                _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, "SCA Submitted", _dataContext.UserId.Value);
                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to SCA Submitted.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }

        [HttpPost]
        public JsonResult SCADenied(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                ResetAdmissionFields(preScreenId);

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }

                if (model.AdmissionStatus == "Denial")
                {
                    if (string.IsNullOrEmpty(model.DenialReason))
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid Denial Reason. Please select a valid Denial reason." });
                    }

                    preScreen.DenialReason = model.DenialReason;
                }

                preScreen.AdmissionStatus = model.AdmissionStatus;


                preScreen.Status = "SCA Denied";

                SendSCADeniedAlert(preScreen.PreScreenId);
                _dataContext.AddPreScreenStatusLog(preScreenId, "SCA Denied", _dataContext.UserId.Value);
                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to SCA Denied.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }


        [HttpPost]
        public JsonResult SCAApproved(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                ResetAdmissionFields(preScreenId);

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }
                preScreen.Status = "SCA Approved";

                preScreen.AdmissionStatus = model.AdmissionStatus;




                _dataContext.AddPreScreenStatusLog(preScreenId, "SCA Approved", _dataContext.UserId.Value);


                SendSCAApprovedAlert(preScreen.PreScreenId);
                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to SCA Approved.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }




        [HttpPost]
        public JsonResult PeerToPeerSubmitted(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                if (!model.Agree)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Please agree by selecting the check box." });
                }

                ResetAdmissionFields(preScreenId);
                preScreen.Status = "Peer To Peer Submitted";

                _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, "Peer To Peer Submitted", _dataContext.UserId.Value);

                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Peer To Peer Submitted.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }



        [HttpPost]
        public JsonResult PeerToPeerApproved(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                ResetAdmissionFields(preScreenId);

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }
                preScreen.Status = "Peer To Peer Approved";

                preScreen.AdmissionStatus = model.AdmissionStatus;




                _dataContext.AddPreScreenStatusLog(preScreenId, "Peer To Peer Approved", _dataContext.UserId.Value);

                SendPeerToPeerApprovedAlert(preScreen.PreScreenId);
                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Peer To Peer Approved.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }


        [HttpPost]
        public JsonResult PeerToPeerDenied(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                ResetAdmissionFields(preScreenId);

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }

                if (model.AdmissionStatus == "Denial")
                {
                    if (string.IsNullOrEmpty(model.DenialReason))
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid Denial Reason. Please select a valid Denial reason." });
                    }

                    preScreen.DenialReason = model.DenialReason;
                }

                preScreen.AdmissionStatus = model.AdmissionStatus;


                preScreen.Status = "Peer To Peer Denied";

                SendPeerToPeerDeniedAlert(preScreen.PreScreenId);
                _dataContext.AddPreScreenStatusLog(preScreenId, "Peer To Peer Denied", _dataContext.UserId.Value);
                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Peer To Peer Denied.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }




        [HttpPost]
        public JsonResult ExpeditedAppealSubmitted(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                if (!model.Agree)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Please agree by selecting the check box." });
                }

                ResetAdmissionFields(preScreenId);
                preScreen.Status = "Expedited Appeal Submitted";

                _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, "Expedited Appeal Submitted", _dataContext.UserId.Value);


                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Expedited Appeal Submitted.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }



        [HttpPost]
        public JsonResult ExpeditedAppealApproved(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                ResetAdmissionFields(preScreenId);

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }
                preScreen.Status = "Expedited Appeal Approved";

                preScreen.AdmissionStatus = model.AdmissionStatus;




                _dataContext.AddPreScreenStatusLog(preScreenId, "Expedited Appeal Approved", _dataContext.UserId.Value);

                SendExpeditedAppealApprovedAlert(preScreen.PreScreenId);

                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Expedited Appeal Approved.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }



        [HttpPost]
        public JsonResult ExpeditedAppealDenied(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                ResetAdmissionFields(preScreenId);

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }

                if (model.AdmissionStatus == "Denial")
                {
                    if (string.IsNullOrEmpty(model.DenialReason))
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid Denial Reason. Please select a valid Denial reason." });
                    }

                    preScreen.DenialReason = model.DenialReason;
                }

                preScreen.AdmissionStatus = model.AdmissionStatus;


                preScreen.Status = "Expedited Appeal Denied";

                SendExpeditedAppealDeniedAlert(preScreen.PreScreenId);
                _dataContext.AddPreScreenStatusLog(preScreenId, "Expedited Appeal Denied", _dataContext.UserId.Value);

                _dataContext.SaveChanges();


                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Expedited Appeal Denied.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }


        [HttpPost]
        public JsonResult MaximusSubmitted(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                if (!model.Agree)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Please agree by selecting the check box." });
                }

                ResetAdmissionFields(preScreenId);
                preScreen.Status = "Maximus Submitted";

                _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, "Maximus Submitted", _dataContext.UserId.Value);



                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Maximus Submitted.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }


        [HttpPost]
        public JsonResult MaximusApproved(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                ResetAdmissionFields(preScreenId);

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }
                preScreen.Status = "Maximus Approved";

                preScreen.AdmissionStatus = model.AdmissionStatus;




                _dataContext.AddPreScreenStatusLog(preScreenId, "Maximus Approved", _dataContext.UserId.Value);

                SendMaximusApprovedAlert(preScreen.PreScreenId);
                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Maximus Approved.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }


        [HttpPost]
        public JsonResult MaximusDenied(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                ResetAdmissionFields(preScreenId);

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }


                if (preScreen.PendingCEOApproval)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status until pre-screen is pending CEO approval. Please close the pending CEO approval task and try again and try again." });
                }

                if (model.AdmissionStatus == "Denial")
                {
                    if (string.IsNullOrEmpty(model.DenialReason))
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid Denial Reason. Please select a valid Denial reason." });
                    }

                    preScreen.DenialReason = model.DenialReason;
                }

                preScreen.AdmissionStatus = model.AdmissionStatus;


                preScreen.Status = "Maximus Denied";
                _dataContext.AddPreScreenStatusLog(preScreenId, "Maximus Denied", _dataContext.UserId.Value);

                SendMaximusDeniedAlert(preScreen.PreScreenId);

                _dataContext.SaveChanges();
                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen set to Maximus Denied.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });

            }

        }



        [HttpPost]
        public JsonResult OverrideAdmission(long preScreenId, PreScreenUpdateStatusModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var preScreen = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
                if (preScreen == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
                }

                if (!preScreen.VerificationComplete)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Cannot set the admission status unless verification is complete. Please click the verification from the workflow menu and try again." });
                }

                ResetAdmissionFields(preScreen.PreScreenId);

                preScreen.AdmissionStatus = model.AdmissionStatus;
                preScreen.AdmissionNotes = model.AdmissionNotes;

                if (model.AdmissionStatus == "Non Admit")
                {
                    if (string.IsNullOrEmpty(model.NonAdmitReason))
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid Non Admit Reason. Please select a valid Non Admit reason." });
                    }

                    preScreen.NonAdmitReason = model.NonAdmitReason;
                }

                if (model.AdmissionStatus == "Denial")
                {
                    if (string.IsNullOrEmpty(model.DenialReason))
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid Denial Reason. Please select a valid Denial reason." });
                    }

                    preScreen.DenialReason = model.DenialReason;
                }


                if (model.AdmissionStatus == "Pre Admit")
                {
                    preScreen.DenialReason = null;
                    preScreen.NonAdmitReason = null;
                }

                preScreen.Status = "Override";


                if(model.AdmissionStatus == "Pre-Screen Complete")
                {
                    preScreen.Status = "Pre-Screen Complete";
                    preScreen.AdmissionStatus = null;
                    preScreen.AdmissionNotes = null;
                    preScreen.AdmissionType = null;
                    preScreen.DenialReason = null;
                    preScreen.NonAdmitReason = null;
                }


                _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, preScreen.Status, _dataContext.UserId.Value);
                _dataContext.SaveChanges();

                transaction.Commit();
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen override admission status set successfully.", PSStatus = preScreen.Status, AdmissionStatus = preScreen.AdmissionStatus });


            }

        }



        private void SendSCAApprovedAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();


            //Send alert and email.

            var users = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD")).Union(_dataContext.Users.Where(p => p.UserId == referral.LiaisonId)).Distinct().ToList();


            foreach (var user in users)
            {
                _dataContext.CreateNotification("Success", "Pre-Screen SCA Approved", $"SCA is approved for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);


                Dictionary<string, string> values = null;
                values = new Dictionary<string, string>();
                values.Add("TMP_SUBJECT", "PRE_SCREEN_SCA_APPROVED_SUBJECT");
                values.Add("TMP_BODY", "PRE_SCREEN_SCA_APPROVED_BODY");
                values.Add("PRE_SCREEN_ID", preScreenId.ToString());
                values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                values.Add("EMAIL_FROM_NAME", "CRA");
                _dataContext.CreateEmailAlert(user.UserId, values);


            }



        }

        private void SendSCADeniedAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();


            //Send alert and email.

            var users = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD")).Union(_dataContext.Users.Where(p => p.UserId == referral.LiaisonId)).Distinct().ToList();


            foreach (var user in users)
            {
                _dataContext.CreateNotification("Error", "Pre-Screen SCA Denied", $"SCA is denied for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);

                Dictionary<string, string> values = null;
                values = new Dictionary<string, string>();
                values.Add("TMP_SUBJECT", "PRE_SCREEN_SCA_DENIED_SUBJECT");
                values.Add("TMP_BODY", "PRE_SCREEN_SCA_DENIED_BODY");
                values.Add("PRE_SCREEN_ID", preScreenId.ToString());
                values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                values.Add("EMAIL_FROM_NAME", "CRA");
                _dataContext.CreateEmailAlert(user.UserId, values);


            }



        }



        private void SendPeerToPeerApprovedAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();


            //Send alert and email.

            var users = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD")).Union(_dataContext.Users.Where(p => p.UserId == referral.LiaisonId)).Distinct().ToList();


            foreach (var user in users)
            {
                _dataContext.CreateNotification("Success", "Pre-Screen Peer To Peer Approved", $"Peer To Peer is approved for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);


                Dictionary<string, string> values = null;
                values = new Dictionary<string, string>();
                values.Add("TMP_SUBJECT", "PRE_SCREEN_PEER_TO_PEER_APPROVED_SUBJECT");
                values.Add("TMP_BODY", "PRE_SCREEN_PEER_TO_PEER_APPROVED_BODY");
                values.Add("PRE_SCREEN_ID", preScreenId.ToString());
                values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                values.Add("EMAIL_FROM_NAME", "CRA");
                _dataContext.CreateEmailAlert(user.UserId, values);


            }



        }
        private void SendPeerToPeerDeniedAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();


            //Send alert and email.

            var users = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD")).Union(_dataContext.Users.Where(p => p.UserId == referral.LiaisonId)).Distinct().ToList();


            foreach (var user in users)
            {
                _dataContext.CreateNotification("Error", "Pre-Screen Peer To Peer Denied", $"Peer To Peer is denied for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);


                Dictionary<string, string> values = null;
                values = new Dictionary<string, string>();
                values.Add("TMP_SUBJECT", "PRE_SCREEN_PEER_TO_PEER_DENIED_SUBJECT");
                values.Add("TMP_BODY", "PRE_SCREEN_PEER_TO_PEER_DENIED_BODY");
                values.Add("PRE_SCREEN_ID", preScreenId.ToString());
                values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                values.Add("EMAIL_FROM_NAME", "CRA");
                _dataContext.CreateEmailAlert(user.UserId, values);


            }



        }



        private void SendExpeditedAppealApprovedAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();


            //Send alert and email.

            var users = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD")).Union(_dataContext.Users.Where(p => p.UserId == referral.LiaisonId)).Distinct().ToList();


            foreach (var user in users)
            {
                _dataContext.CreateNotification("Success", "Pre-Screen Expedited Appeal Approved", $"Expedited Appeal is approved for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);


                Dictionary<string, string> values = null;
                values = new Dictionary<string, string>();
                values.Add("TMP_SUBJECT", "PRE_SCREEN_EXPEDITED_APPEAL_APPROVED_SUBJECT");
                values.Add("TMP_BODY", "PRE_SCREEN_EXPEDITED_APPEAL_APPROVED_BODY");
                values.Add("PRE_SCREEN_ID", preScreenId.ToString());
                values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                values.Add("EMAIL_FROM_NAME", "CRA");
                _dataContext.CreateEmailAlert(user.UserId, values);


            }



        }


        private void SendExpeditedAppealDeniedAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();


            //Send alert and email.

            var users = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD")).Union(_dataContext.Users.Where(p => p.UserId == referral.LiaisonId)).Distinct().ToList();


            foreach (var user in users)
            {
                _dataContext.CreateNotification("Error", "Pre-Screen Expedited Appeal Denied", $"Expedited Appeal is denied for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);


                Dictionary<string, string> values = null;
                values = new Dictionary<string, string>();
                values.Add("TMP_SUBJECT", "PRE_SCREEN_EXPEDITED_APPEAL_DENIED_SUBJECT");
                values.Add("TMP_BODY", "PRE_SCREEN_EXPEDITED_APPEAL_DENIED_BODY");
                values.Add("PRE_SCREEN_ID", preScreenId.ToString());
                values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                values.Add("EMAIL_FROM_NAME", "CRA");
                _dataContext.CreateEmailAlert(user.UserId, values);


            }



        }



        private void SendMaximusApprovedAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();


            //Send alert and email.

            var users = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD")).Union(_dataContext.Users.Where(p => p.UserId == referral.LiaisonId)).Distinct().ToList();


            foreach (var user in users)
            {
                _dataContext.CreateNotification("Success", "Pre-Screen Maxmimus Approved", $"Maximus is approved for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);


                Dictionary<string, string> values = null;
                values = new Dictionary<string, string>();
                values.Add("TMP_SUBJECT", "PRE_SCREEN_MAXIMUS_APPROVED_SUBJECT");
                values.Add("TMP_BODY", "PRE_SCREEN_MAXIMUS_APPROVED_BODY");
                values.Add("PRE_SCREEN_ID", preScreenId.ToString());
                values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                values.Add("EMAIL_FROM_NAME", "CRA");
                _dataContext.CreateEmailAlert(user.UserId, values);


            }



        }


        private void SendMaximusDeniedAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();


            //Send alert and email.

            var users = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD")).Union(_dataContext.Users.Where(p => p.UserId == referral.LiaisonId)).Distinct().ToList();


            foreach (var user in users)
            {
                _dataContext.CreateNotification("Error", "Pre-Screen Maxmimus Denied", $"Maximus is denied for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);


                Dictionary<string, string> values = null;
                values = new Dictionary<string, string>();
                values.Add("TMP_SUBJECT", "PRE_SCREEN_MAXIMUS_DENIED_SUBJECT");
                values.Add("TMP_BODY", "PRE_SCREEN_MAXIMUS_DENIED_BODY");
                values.Add("PRE_SCREEN_ID", preScreenId.ToString());
                values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                values.Add("EMAIL_FROM_NAME", "CRA");
                _dataContext.CreateEmailAlert(user.UserId, values);


            }



        }


        private void SendAuthApprovedAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();


            //Send alert and email.

            var users = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD")).Union(_dataContext.Users.Where(p => p.UserId == referral.LiaisonId)).Distinct().ToList();


            foreach (var user in users)
            {
                _dataContext.CreateNotification("Success", "Pre-Screen Auth Approved", $"Auth is approved for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);


                Dictionary<string, string> values = null;
                values = new Dictionary<string, string>();
                values.Add("TMP_SUBJECT", "PRE_SCREEN_AUTH_APPROVED_SUBJECT");
                values.Add("TMP_BODY", "PRE_SCREEN_AUTH_APPROVED_BODY");
                values.Add("PRE_SCREEN_ID", preScreenId.ToString());
                values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                values.Add("EMAIL_FROM_NAME", "CRA");
                _dataContext.CreateEmailAlert(user.UserId, values);


            }



        }


        private void SendAuthDeniedAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();


            //Send alert and email.

            var users = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD")).Union(_dataContext.Users.Where(p => p.UserId == referral.LiaisonId)).Distinct().ToList();


            foreach (var user in users)
            {
                _dataContext.CreateNotification("Error", "Pre-Screen Auth Denied", $"Auth is denied for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);


                Dictionary<string, string> values = null;
                values = new Dictionary<string, string>();
                values.Add("TMP_SUBJECT", "PRE_SCREEN_AUTH_DENIED_SUBJECT");
                values.Add("TMP_BODY", "PRE_SCREEN_AUTH_DENIED_BODY");
                values.Add("PRE_SCREEN_ID", preScreenId.ToString());
                values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                values.Add("EMAIL_FROM_NAME", "CRA");
                _dataContext.CreateEmailAlert(user.UserId, values);


            }



        }
        private void SendSCARequestedAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();

            var item = GetPatientDataItem("payorinfo.primary.sca", "scarequested", preScreen.PatientId);

            if (!preScreen.SCARequested && item != null && item == "Yes")
            {
                preScreen.SCARequested = true;

                //Send alert and email.

                var users = _dataContext.GetRoleUsers("MC");

                foreach (var user in users)
                {
                    _dataContext.CreateNotification("Info", "Pre-Screen SCA Requested", $"SCA is requested for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);

                    var task = _dataContext.CreateTask("Pre-Screen SCA Requested", user.UserId, "Created", "Pre-Screen SCA Requested Acknoweledgement", null, $"Please Acknoweledge the receipt of SCA Request for the Pre-Screen {preScreen.PreScreenId}");

                    _dataContext.SaveChanges();

                    _dataContext.UpdateTaskParameter(task.UserTaskId, "PRE_SCREEN_ID", preScreenId.ToString());
                    _dataContext.UpdateTaskParameter(task.UserTaskId, "CHGSITE_ID", referral.CHGSiteId.ToString());

                    _dataContext.SaveChanges();


                    Dictionary<string, string> values = null;

                    values = new Dictionary<string, string>();
                    values.Add("TMP_SUBJECT", "PRE_SCREEN_SCA_REQUESTED_SUBJECT");
                    values.Add("TMP_BODY", "PRE_SCREEN_SCA_REQUESTED_BODY");
                    values.Add("PRE_SCREEN_ID", preScreenId.ToString());

                    values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                    values.Add("EMAIL_FROM_NAME", "CRA");
                    values.Add("TASK_URL", ConfigurationManager.AppSettings["APP_URL"] + "/Task/ResolveSCARequested/" + task.UserTaskId);


                    _dataContext.CreateEmailAlert(user.UserId, values);


                }


            }
        }


        private void SendPlanInGraceAlert(long preScreenId)
        {
            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Single();

             
            var item = GetPatientDataItem("payorinfo.primary.general", "pisingp", preScreen.PatientId);

            if (!preScreen.PlanInGrace && item != null && item == "Yes")
            {
                preScreen.PlanInGrace = true;

                //Send alert and email.


                var ceoUsers = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "CEO");
                var dbdUsers = _dataContext.GetRoleUsersForCHGSite(referral.CHGSiteId, "DBD");
                var clUsers = _dataContext.Users.Where(p => p.UserId == referral.LiaisonId).ToList();

                var users = ceoUsers.Union(dbdUsers).Union(clUsers).Distinct().ToList();

                foreach (var user in users)
                {
                    _dataContext.CreateNotification("Info", "Pre-Screen plan is in grace", $"Plan is in grace period for Pre-Screen ID {preScreen.PreScreenId}", user.UserId);

                 

                    Dictionary<string, string> values = null;

                    values = new Dictionary<string, string>();
                    values.Add("TMP_SUBJECT", "PRE_SCREEN_PLAN_IN_GRACE_SUBJECT");
                    values.Add("TMP_BODY", "PRE_SCREEN_PLAN_IN_GRACE_BODY");
                    values.Add("PRE_SCREEN_ID", preScreenId.ToString());

                    values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                    values.Add("EMAIL_FROM_NAME", "CRA");
                 

                    _dataContext.CreateEmailAlert(user.UserId, values);


                }


            }
        }
        #endregion


    }

}






