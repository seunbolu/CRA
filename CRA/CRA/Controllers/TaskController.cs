using CRA.Data;
using CRA.Data.Entities;
using CRA.Models.CHGSite;
using CRA.Models.Task;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{
    public class TaskController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCurrentTasks(string taskType, string status, string subStatus)
        {
            long userId = _dataContext.UserId.Value;

            var query = _dataContext.UserTasks.Where(p => p.UserId == userId && p.Deleted == false);

            if (!string.IsNullOrEmpty(taskType))
            {
                query = query.Where(p => p.TaskType == taskType);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status == status);
            }

            if (!string.IsNullOrEmpty(subStatus))
            {
                query = query.Where(p => p.SubStatus == subStatus);
            }

            List<TaskModel> items = new List<TaskModel>();

            foreach (var item in query.OrderByDescending(p=>p.UserTaskId).ToList())
            {
                items.Add(new TaskModel()
                {
                    UserTaskId = item.UserTaskId,
                    Description = item.Description,
                    Created = item.DateCreated.ToString(),
                    Modified = item.DateModified == null ? null : item.DateModified.ToString(),
                    Status = item.Status,
                    SubStatus = item.SubStatus,
                    TaskType = item.TaskType
                }
                );
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTopCurrentTasks()
        {
            long userId = _dataContext.UserId.Value;

            var query = _dataContext.UserTasks.Where(p => p.UserId == userId && p.Deleted == false);

             
            List<TaskModel> items = new List<TaskModel>();

            foreach (var item in query.OrderByDescending(p => p.UserTaskId).Take(10).ToList())
            {
                items.Add(new TaskModel()
                {
                    UserTaskId = item.UserTaskId,
                    Description = item.Description,
                    Created = item.DateCreated.ToString(),
                    Modified = item.DateModified == null ? null : item.DateModified.ToString(),
                    Status = item.Status,
                    SubStatus = item.SubStatus,
                    TaskType = item.TaskType
                }
                );
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Resolve(long id)
        {

            var model = new TaskModel();

            var task = _dataContext.UserTasks.Where(p=>p.UserTaskId==id).SingleOrDefault();
            if (task == null)
            {
                return RedirectToUnauthorized();
            }

            if (_dataContext.UserId == null || _dataContext.UserId.Value == 0)
            {
                return RedirectToUnauthorized();
            }

            if (task.UserId != _dataContext.UserId.Value)
            {
                return RedirectToUnauthorized();
            }

            model.UserTaskId = task.UserTaskId;
            model.Description = task.Description;
            model.Status = task.Status;
            model.SubStatus = task.SubStatus;
            model.TaskType = task.TaskType;
            model.Notes = task.Notes;

            Dictionary<string, string> parameters = _dataContext.GetUserTaskParameters(task.UserTaskId);

            if (task.TaskType == "Call Cycle Approval")
            {
                model.ScheduleId = long.Parse(parameters["SCHEDULE_ID"]);
            }

            if (task.TaskType == "Referral Source Approval")
            {
                model.ReferralSourceId = long.Parse(parameters["REFERRAL_SOURCE_ID"]);
            }


            if (task.TaskType == "Contact Approval")
            {
                model.ContactId = long.Parse(parameters["CONTACT_ID"]);
            }

            if (task.TaskType == "New Referral Completion")
            {
                model.PreScreenId = long.Parse(parameters["PRE_SCREEN_ID"]);
            }

            return View(model);
        }

         

        
        public ActionResult ResolveReferralSource(long id)
        {
            return Resolve(id);
        }


        public ActionResult ResolveNewReferralCompletion(long id)
        {
            return Resolve(id);
        }


        public ActionResult ResolveContact(long id)
        {
            return Resolve(id);
        }

        [HttpPost]
        public JsonResult TaskAction(TaskApprovalModel model)
        {
            string status = "Completed";
            var task = _dataContext.UserTasks.Where(p=>p.UserTaskId == model.UserTaskId).SingleOrDefault();

            if (task == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized" });
            }

            if (_dataContext.UserId == null || _dataContext.UserId.Value == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized" });
            }
            if (task.UserId != _dataContext.UserId.Value)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized" });
            }
            if (task.Status == "Completed")
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Task already resolved." });
            }

            using (var transaction = _dataContext.Database.BeginTransaction())
            {

                task.Status = status;

                Dictionary<string, string> parameters = _dataContext.GetUserTaskParameters(task.UserTaskId);
                Dictionary<string, string> values = null;
                List<User> users = null;

                long scheduleId = long.Parse(parameters["SCHEDULE_ID"]);
                long CHGSiteId = long.Parse(parameters["CHGSITE_ID"]);
                string approvalLevel = parameters["APPROVAL_LEVEL"];


                var userId = _dataContext.Schedules.Find(scheduleId).UserId;
                var userEntity = _dataContext.Users.Find(userId);

                if (model.Action == "Reject")
                {
                    task.SubStatus = "Rejected";
                    task.Notes = model.Notes;

                    switch (approvalLevel)
                    {
                        case "CRO":
                            _dataContext.RejectSchedule(scheduleId);
                            values = new Dictionary<string, string>();
                            values.Add("TMP_SUBJECT", "CALL_CYCLE_APPROVAL_REQUEST_REJECTED_SUBJECT");
                            values.Add("TMP_BODY", "CALL_CYCLE_APPROVAL_REQUEST_REJECTED_BODY");
                            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                            values.Add("EMAIL_FROM_NAME", "CRA");
                            values.Add("DECISION_NOTES", task.Notes);
                            values.Add("LIASON_NAME", $"{userEntity.Contact.FirstName} {userEntity.Contact.LastName}");

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "DBD")).Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "AVP")).Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Error", "Denied", $"Call cycle request for {userEntity.Contact.FirstName} {userEntity.Contact.LastName} denied.", user.UserId);

                                }
                            }
                            break;

                        case "AVP":
                            _dataContext.RejectSchedule(scheduleId);
                            values = new Dictionary<string, string>();
                            values.Add("TMP_SUBJECT", "CALL_CYCLE_APPROVAL_REQUEST_REJECTED_SUBJECT");
                            values.Add("TMP_BODY", "CALL_CYCLE_APPROVAL_REQUEST_REJECTED_BODY");
                            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                            values.Add("EMAIL_FROM_NAME", "CRA");
                            values.Add("DECISION_NOTES", task.Notes);
                            values.Add("LIASON_NAME", $"{userEntity.Contact.FirstName} {userEntity.Contact.LastName}");

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "DBD")).Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Error", "Denied", $"Call cycle request for {userEntity.Contact.FirstName} {userEntity.Contact.LastName} denied.", user.UserId);

                                }
                            }
                            break;
                    }

                }

                

                if (model.Action == "Approve")
                {
                    task.SubStatus = "Approved";
                    task.Notes = model.Notes;

                    switch (approvalLevel)
                    {
                        case "CRO":
                            _dataContext.ApproveSchedule(scheduleId);

                            values = new Dictionary<string, string>();
                            values.Add("TMP_SUBJECT", "CALL_CYCLE_APPROVAL_REQUEST_APPROVED_SUBJECT");
                            values.Add("TMP_BODY", "CALL_CYCLE_APPROVAL_REQUEST_APPROVED_BODY");
                            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                            values.Add("EMAIL_FROM_NAME", "CRA");
                            values.Add("DECISION_NOTES", task.Notes);
                            values.Add("LIASON_NAME", $"{userEntity.Contact.FirstName} {userEntity.Contact.LastName}");

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "DBD")).Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "AVP")).Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Success", "Approved", $"Call cycle request for {userEntity.Contact.FirstName} {userEntity.Contact.LastName} approved.", user.UserId);

                                }
                            }

                            //Send approval down the chain.
                            break;

                        case "AVP":

                            //send approval request to CRO.

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CRO").Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    var userTask = _dataContext.CreateTask("Call Cycle Approval", user.UserId, "Created", "Call Cycle Approval Request");
                                    _dataContext.SaveChanges();

                                    values = new Dictionary<string, string>();
                                    values.Add("TMP_SUBJECT", "CALL_CYCLE_APPROVAL_REQUEST_CREATED_SUBJECT");
                                    values.Add("TMP_BODY", "CALL_CYCLE_APPROVAL_REQUEST_CREATED_BODY");
                                    values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                                    values.Add("EMAIL_FROM_NAME", "CRA");
                                    values.Add("DECISION_NOTES", task.Notes);
                                    values.Add("LIASON_NAME", $"{userEntity.Contact.FirstName} {userEntity.Contact.LastName}");
                                    values.Add("TASK_URL", ConfigurationManager.AppSettings["APP_URL"] + "/Task/Resolve/" + userTask.UserTaskId);


                                    _dataContext.UpdateTaskParameter(userTask.UserTaskId, "APPROVAL_LEVEL", "CRO");
                                    _dataContext.UpdateTaskParameter(userTask.UserTaskId, "SCHEDULE_ID", scheduleId.ToString());
                                    _dataContext.UpdateTaskParameter(userTask.UserTaskId, "CHGSITE_ID", CHGSiteId.ToString());
                                    _dataContext.SaveChanges();

                                    //Create an email alert.
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Success", "Approved", $"Call cycle request for {userEntity.Contact.FirstName} {userEntity.Contact.LastName} approved.", user.UserId);
                                }
                            }


                            break;

                    }

                }

                _dataContext.SaveChanges();

                transaction.Commit();
            }

            return Json(task, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TaskActionReferralSource(TaskApprovalModel model)
        {
            string status = "Completed";
            var task = _dataContext.UserTasks.Where(p => p.UserTaskId == model.UserTaskId).SingleOrDefault();

            if (task == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized" });
            }

            if (_dataContext.UserId == null || _dataContext.UserId.Value == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized" });
            }
            if (task.UserId != _dataContext.UserId.Value)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized" });
            }
            if (task.Status == "Completed")
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Task already resolved." });
            }

            using (var transaction = _dataContext.Database.BeginTransaction())
            {

                task.Status = status;

                Dictionary<string, string> parameters = _dataContext.GetUserTaskParameters(task.UserTaskId);
                Dictionary<string, string> values = null;
                List<User> users = null;

                long referralSourceId = long.Parse(parameters["REFERRAL_SOURCE_ID"]);
                long CHGSiteId = long.Parse(parameters["CHGSITE_ID"]);
                string approvalLevel = parameters["APPROVAL_LEVEL"];

                var referralSource = _dataContext.ReferralSources.Find(referralSourceId);
             

                if (model.Action == "Reject")
                {
                    task.SubStatus = "Rejected";
                    task.Notes = model.Notes;

                    switch (approvalLevel)
                    {
                        case "CRO":
                            _dataContext.RejectReferralSource(referralSourceId);
                            values = new Dictionary<string, string>();
                            values.Add("TMP_SUBJECT", "REFERRAL_SOURCE_APPROVAL_REQUEST_REJECTED_SUBJECT");
                            values.Add("TMP_BODY", "REFERRAL_SOURCE_APPROVAL_REQUEST_REJECTED_BODY");
                            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                            values.Add("EMAIL_FROM_NAME", "CRA");
                            values.Add("DECISION_NOTES", task.Notes);
                            values.Add("REFERRAL_SOURCE_NAME", $"{referralSource.FullName}");

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "DBD")).Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "AVP")).Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Error", "Denied", $"Referral source request for {referralSource.FullName} denied.", user.UserId);

                                }
                            }
                            break;

                        case "AVP":
                            _dataContext.RejectReferralSource(referralSourceId);
                            values = new Dictionary<string, string>();
                            values.Add("TMP_SUBJECT", "REFERRAL_SOURCE_APPROVAL_REQUEST_REJECTED_SUBJECT");
                            values.Add("TMP_BODY", "REFERRAL_SOURCE_APPROVAL_REQUEST_REJECTED_BODY");
                            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                            values.Add("EMAIL_FROM_NAME", "CRA");
                            values.Add("DECISION_NOTES", task.Notes);
                            values.Add("REFERRAL_SOURCE_NAME", $"{referralSource.FullName}");

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "DBD")).Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Error", "Denied", $"Referral source request for {referralSource.FullName} denied.", user.UserId);

                                }
                            }
                            break;
                    }

                }


                if (model.Action == "Approve")
                {
                    task.SubStatus = "Approved";
                    task.Notes = model.Notes;

                    switch (approvalLevel)
                    {
                        case "CRO":
                            _dataContext.ApproveReferralSource(referralSourceId);

                            values = new Dictionary<string, string>();
                            values.Add("TMP_SUBJECT", "REFERRAL_SOURCE_APPROVAL_REQUEST_APPROVED_SUBJECT");
                            values.Add("TMP_BODY", "REFERRAL_SOURCE_APPROVAL_REQUEST_APPROVED_BODY");
                            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                            values.Add("EMAIL_FROM_NAME", "CRA");
                            values.Add("DECISION_NOTES", task.Notes);
                            values.Add("REFERRAL_SOURCE_NAME", $"{referralSource.FullName}");

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "DBD")).Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "AVP")).Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Success", "Approved", $"Referral source request for {referralSource.FullName} approved.", user.UserId);

                                }
                            }

                            //Send approval down the chain.
                            break;

                        case "AVP":

                            //send approval request to CRO.

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CRO").Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    var userTask = _dataContext.CreateTask("Referral Source Approval", user.UserId, "Created", "Referral Source Approval Request");
                                    _dataContext.SaveChanges();

                                    values = new Dictionary<string, string>();
                                    values.Add("TMP_SUBJECT", "REFERRAL_SOURCE_APPROVAL_REQUEST_CREATED_SUBJECT");
                                    values.Add("TMP_BODY", "REFERRAL_SOURCE_APPROVAL_REQUEST_CREATED_BODY");
                                    values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                                    values.Add("EMAIL_FROM_NAME", "CRA");
                                    values.Add("DECISION_NOTES", task.Notes);
                                    values.Add("REFERRAL_SOURCE_NAME", $"{referralSource.FullName}");
                                    values.Add("TASK_URL", ConfigurationManager.AppSettings["APP_URL"] + "/Task/ResolveContact/" + userTask.UserTaskId);


                                    _dataContext.UpdateTaskParameter(userTask.UserTaskId, "APPROVAL_LEVEL", "CRO");
                                    _dataContext.UpdateTaskParameter(userTask.UserTaskId, "REFERRAL_SOURCE_ID", referralSourceId.ToString());
                                    _dataContext.UpdateTaskParameter(userTask.UserTaskId, "CHGSITE_ID", CHGSiteId.ToString());
                                    _dataContext.SaveChanges();

                                    //Create an email alert.
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Success", "Approved", $"Referral source request for {referralSource.FullName} approved.", user.UserId);
                                }
                            }


                            break;

                    }

                }

                _dataContext.SaveChanges();

                transaction.Commit();
            }

            return Json(task, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult TaskActionContact(TaskApprovalModel model)
        {
            string status = "Completed";
            var task = _dataContext.UserTasks.Where(p => p.UserTaskId == model.UserTaskId).SingleOrDefault();

            if (task == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized" });
            }

            if (_dataContext.UserId == null || _dataContext.UserId.Value == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized" });
            }
            if (task.UserId != _dataContext.UserId.Value)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized" });
            }
            if (task.Status == "Completed")
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Task already resolved." });
            }

            using (var transaction = _dataContext.Database.BeginTransaction())
            {

                task.Status = status;

                Dictionary<string, string> parameters = _dataContext.GetUserTaskParameters(task.UserTaskId);
                Dictionary<string, string> values = null;
                List<User> users = null;

                long contactId = long.Parse(parameters["CONTACT_ID"]);
                long CHGSiteId = long.Parse(parameters["CHGSITE_ID"]);
                string approvalLevel = parameters["APPROVAL_LEVEL"];

                var contact = _dataContext.Contacts.Find(contactId);


                if (model.Action == "Reject")
                {
                    task.SubStatus = "Rejected";
                    task.Notes = model.Notes;

                    switch (approvalLevel)
                    {
                        case "CRO":
                            _dataContext.RejectContact(contactId);
                            values = new Dictionary<string, string>();
                            values.Add("TMP_SUBJECT", "CONTACT_APPROVAL_REQUEST_REJECTED_SUBJECT");
                            values.Add("TMP_BODY", "CONTACT_APPROVAL_REQUEST_REJECTED_BODY");
                            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                            values.Add("EMAIL_FROM_NAME", "CRA");
                            values.Add("DECISION_NOTES", task.Notes);
                            values.Add("CONTACT_NAME", $"{contact.FirstName} {contact.LastName}");

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "DBD")).Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "AVP")).Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Error", "Denied", $"Contact request for {contact.FirstName} {contact.LastName} denied.", user.UserId);

                                }
                            }
                            break;

                        case "AVP":
                            _dataContext.RejectContact(contactId);
                            values = new Dictionary<string, string>();
                            values.Add("TMP_SUBJECT", "CONTACT_APPROVAL_REQUEST_REJECTED_SUBJECT");
                            values.Add("TMP_BODY", "CONTACT_APPROVAL_REQUEST_REJECTED_BODY");
                            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                            values.Add("EMAIL_FROM_NAME", "CRA");
                            values.Add("DECISION_NOTES", task.Notes);
                            values.Add("CONTACT_NAME", $"{contact.FirstName} {contact.LastName}");

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "DBD")).Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Error", "Denied", $"Contact source request for {contact.FirstName} {contact.LastName} denied.", user.UserId);

                                }
                            }
                            break;
                    }

                }


                if (model.Action == "Approve")
                {
                    task.SubStatus = "Approved";
                    task.Notes = model.Notes;

                    switch (approvalLevel)
                    {
                        case "CRO":
                            _dataContext.ApproveContact(contactId);

                            values = new Dictionary<string, string>();
                            values.Add("TMP_SUBJECT", "CONTACT_APPROVAL_REQUEST_APPROVED_SUBJECT");
                            values.Add("TMP_BODY", "CONTACT_APPROVAL_REQUEST_APPROVED_BODY");
                            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                            values.Add("EMAIL_FROM_NAME", "CRA");
                            values.Add("DECISION_NOTES", task.Notes);
                            values.Add("CONTACT_NAME", $"{contact.FirstName} {contact.LastName}");

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "DBD")).Union(_dataContext.GetRoleUsersForCHGSite(CHGSiteId, "AVP")).Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Success", "Approved", $"Contact request for {contact.FirstName} {contact.LastName} approved.", user.UserId);

                                }
                            }

                            //Send approval down the chain.
                            break;

                        case "AVP":

                            //send approval request to CRO.

                            users = _dataContext.GetRoleUsersForCHGSite(CHGSiteId, "CRO").Distinct().ToList();

                            if (users != null)
                            {
                                foreach (var user in users)
                                {
                                    var userTask = _dataContext.CreateTask("Contact Approval", user.UserId, "Created", "Contact Approval Request");
                                    _dataContext.SaveChanges();

                                    values = new Dictionary<string, string>();
                                    values.Add("TMP_SUBJECT", "CONTACT_APPROVAL_REQUEST_CREATED_SUBJECT");
                                    values.Add("TMP_BODY", "CONTACT_APPROVAL_REQUEST_CREATED_BODY");
                                    values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                                    values.Add("EMAIL_FROM_NAME", "CRA");
                                    values.Add("DECISION_NOTES", task.Notes);
                                    values.Add("CONTACT_NAME", $"{contact.FirstName} {contact.LastName}");
                                    values.Add("TASK_URL", ConfigurationManager.AppSettings["APP_URL"] + "/Task/ResolveContact/" + userTask.UserTaskId);


                                    _dataContext.UpdateTaskParameter(userTask.UserTaskId, "APPROVAL_LEVEL", "CRO");
                                    _dataContext.UpdateTaskParameter(userTask.UserTaskId, "CONTACT_ID", contactId.ToString());
                                    _dataContext.UpdateTaskParameter(userTask.UserTaskId, "CHGSITE_ID", CHGSiteId.ToString());
                                    _dataContext.SaveChanges();

                                    //Create an email alert.
                                    _dataContext.CreateEmailAlert(user.UserId, values);
                                    _dataContext.CreateNotification("Success", "Approved", $"Contact request for {contact.FirstName} {contact.LastName} approved.", user.UserId);
                                }
                            }


                            break;

                    }

                }

                _dataContext.SaveChanges();

                transaction.Commit();
            }

            return Json(task, JsonRequestBehavior.AllowGet);
        }

    }

}