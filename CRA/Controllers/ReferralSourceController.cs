using CRA.Data;
using CRA.Data.Entities;
using CRA.Data.Tracking;
using CRA.Models.CHGSite;
using CRA.Models.ReferralSource;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{
    public class ReferralSourceController : BaseController
    {
        public ActionResult Index()
        {
            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return RedirectToUnauthorized();
            }
            return View();
        }

        public JsonResult ReferralSources(long? referralSourceTypeId, string keyword)
        {
            List<ReferralSourceModel> model = new List<ReferralSourceModel>();

            var query = (from c in _dataContext.ReferralSources.Include("ReferralSourceType") where c.Deleted == false select c);

            if (referralSourceTypeId != null && referralSourceTypeId > 0)
            {
                query = query.Where(p => p.ReferralSourceTypeId == referralSourceTypeId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.FullName.Contains(keyword));
            }

            var query2 = (from c in _dataContext.CHGSiteReferralSources join d in query on c.ReferralSourceId equals d.ReferralSourceId where c.Deleted == false && c.Deleted == false select new { ReferralSource = d, CHGSite = c.CHGSite}).ToList();

            var results = (from c in query2 join d in _dataContext.GetUserSites(_dataContext.UserId.Value) on c.CHGSite.CHGSiteId equals d.CHGSiteId select c.ReferralSource).Distinct().ToList();
 
            foreach (var entity in results)
            {
                model.Add(new ReferralSourceModel()
                {
                    BedCount = entity.BedCount,
                    CCUBedCount = entity.CCUBedCount,
                    ConfirmedBedsCodingICURevCodes = entity.ConfirmedBedsCodingICURevCodes,
                    FullName = entity.FullName,
                    ICUBedCount = entity.ICUBedCount,
                    LTACHBedCount = entity.LTACHBedCount,
                    MICUBedCount = entity.MICUBedCount,
                    NeuroICUBedCount = entity.NeuroICUBedCount,
                    OperationalICUBedCount = entity.OperationalICUBedCount,
                    ReferralSourceId = entity.ReferralSourceId,
                    ReferralSourceTypeId = entity.ReferralSourceTypeId,
                    ReferralSourceTypeName = entity.ReferralSourceType.Name,
                    RehabBedCount = entity.RehabBedCount,
                    ShortName = entity.ShortName,
                    SICUBedCount = entity.SICUBedCount,
                    SkilledNursingBedCount = entity.SkilledNursingBedCount,
                    IsApproved = entity.IsApproved


                });
            }

            return Json(model, JsonRequestBehavior.AllowGet); 
        }


        public ActionResult Create()
        {
            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return RedirectToUnauthorized();
            }
            return View();
        }

        public JsonResult AutoCompleteReferralSources(string term)
        {
            var query = (from c in _dataContext.ReferralSources.Include("ReferralSourceType") where c.Deleted == false && c.IsApproved == true select c);

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(p => p.FullName.StartsWith(term) || p.ShortName.StartsWith(term));
            }

            var query2 = (from c in _dataContext.CHGSiteReferralSources join d in query on c.ReferralSourceId equals d.ReferralSourceId where c.Deleted == false && c.Deleted == false select new { ReferralSource = d, CHGSite = c.CHGSite }).ToList();

            var results = (from c in query2 join d in _dataContext.GetUserSites(_dataContext.UserId.Value) on c.CHGSite.CHGSiteId equals d.CHGSiteId select c.ReferralSource).ToList();



            List<object> items = new List<object>();
            foreach (var entity in results)
            {
                items.Add(new
                {
                    label = string.Format(@"{0} - {1}", entity.ReferralSourceType.Name, entity.FullName),
                    value = entity.ReferralSourceId,
                    FullName = entity.FullName,
                    ShortName = entity.ShortName,
                    ReferralSourceTypeName = entity.ReferralSourceType.Name,
                    ReferralSourceId = entity.ReferralSourceId
                });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Create(ReferralSourceModel model)
        {
            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return  Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." }); 
            }

            if (ModelState.IsValid)
            {
                if (model.Sites == null || model.Sites.Where(p => p.Selected == true).Count() <= 0)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Please select at least one site to associate the referral source to." });

                }

                using (var transaction = _dataContext.Database.BeginTransaction())
                {

                    if (_dataContext.ReferralSources.Where(p => p.FullName == model.FullName && p.Deleted == false).Count() <= 0)
                    {
                        ReferralSource entity = new ReferralSource()
                        {

                            FullName = model.FullName,
                            ShortName = model.ShortName,
                            BedCount = model.BedCount,
                            OperationalICUBedCount = model.OperationalICUBedCount,
                            CCUBedCount = model.CCUBedCount,
                            SkilledNursingBedCount = model.SkilledNursingBedCount,
                            SICUBedCount = model.SICUBedCount,
                            RehabBedCount = model.RehabBedCount,
                            ConfirmedBedsCodingICURevCodes = model.ConfirmedBedsCodingICURevCodes,
                            ICUBedCount = model.ICUBedCount,
                            LTACHBedCount = model.LTACHBedCount,
                            MICUBedCount = model.MICUBedCount,
                            NeuroICUBedCount = model.NeuroICUBedCount,
                            ReferralSourceTypeId = model.ReferralSourceTypeId

                        };

                        _dataContext.ReferralSources.Add(entity);
                        _dataContext.SaveChanges();

                        foreach (var site in model.Sites.Where(p => p.Selected == true))
                        {
                            CHGSiteReferralSource siteReferralSource = new CHGSiteReferralSource()
                            {
                                CHGSiteId = site.CHGSiteId,

                                ReferralSource = entity
                            };

                            _dataContext.CHGSiteReferralSources.Add(siteReferralSource);
                        }

                        _dataContext.SaveChanges();

                        if (model.ElectronicReferralTypes != null)
                        {
                            foreach (var electronicReferralType in model.ElectronicReferralTypes.Where(p => p.Selected == true))
                            {

                                ReferralSourceElectronicReferralType item = new ReferralSourceElectronicReferralType()
                                {


                                    ElectronicReferralTypeId = electronicReferralType.ElectronicReferralTypeId,
                                    ReferralSource = entity

                                };

                                _dataContext.ReferralSourceElectronicReferralTypes.Add(item);

                            }
                        }

                        _dataContext.SaveChanges();


                        if (model.CommercialPayorTypes != null)
                        {
                            foreach (var commercialPayorType in model.CommercialPayorTypes.Where(p => p.Selected == true))
                            {

                                ReferralSourceCommercialPayorType item = new ReferralSourceCommercialPayorType()
                                {


                                    CommercialPayorTypeId = commercialPayorType.CommercialPayorTypeId,
                                    ReferralSource = entity

                                };

                                _dataContext.ReferralSourceCommercialPayorTypes.Add(item);

                            }
                        }

                        _dataContext.SaveChanges();


                        if (model.ManagedMedicarePayorTypes != null)
                        {
                            foreach (var ManagedMedicarePayorType in model.ManagedMedicarePayorTypes.Where(p => p.Selected == true))
                            {

                                ReferralSourceManagedMedicarePayorType item = new ReferralSourceManagedMedicarePayorType()
                                {


                                    ManagedMedicarePayorTypeId = ManagedMedicarePayorType.ManagedMedicarePayorTypeId,
                                    ReferralSource = entity

                                };

                                _dataContext.ReferralSourceManagedMedicarePayorTypes.Add(item);

                            }
                        }



                        _dataContext.SaveChanges();

                        ApprovalProcess(entity.ReferralSourceId);

                        transaction.Commit();

                        return Json(new { Status = Constant.RESPONSE_OK, Description = "Referral source created successfully." });
                    }
                    else
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "The referral source with the same full name already exists. Please enter a different name." });
                    }

                }

            }

            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });

        }

        private void ApprovalProcess(long referralSourceId)
        {
            Dictionary<string, string> values = null;
            List<long> CHGSiteIds = _dataContext.CHGSiteReferralSources.Where(p => p.ReferralSourceId == referralSourceId && p.Deleted == false).Select(p => p.CHGSiteId).ToList();
            List<User> users = null;
            var referralSource = _dataContext.ReferralSources.Find(referralSourceId);

            //Approval process.
            if (_dataContext.UserHasRole(_dataContext.UserId.Value, "CRO"))
            {
                _dataContext.ApproveReferralSource(referralSourceId);

                foreach (var chgSiteId in CHGSiteIds)
                {
                    values = new Dictionary<string, string>();
                    values.Add("TMP_SUBJECT", "REFERRAL_SOURCE_APPROVAL_REQUEST_APPROVED_SUBJECT");
                    values.Add("TMP_BODY", "REFERRAL_SOURCE_APPROVAL_REQUEST_APPROVED_BODY");
                    values.Add("REFERRAL_SOURCE_NAME", $"{referralSource.FullName}");
                    values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                    values.Add("EMAIL_FROM_NAME", "CRA");
                    values.Add("DECISION_NOTES", "");

                    users = _dataContext.GetRoleUsersForCHGSite(chgSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(chgSiteId, "DBD")).Union(_dataContext.GetRoleUsersForCHGSite(chgSiteId, "AVP")).Distinct().ToList();

                    if (users != null)
                    {
                        foreach (var user in users)
                        {
                            _dataContext.CreateEmailAlert(user.UserId, values);
                            _dataContext.CreateNotification("Success", "Approved", $"Referral Source request for {referralSource.FullName} approved.", user.UserId);
                        }
                    }
                }

                _dataContext.SaveChanges();


            }
            else
            {

                foreach (var chgSiteId in CHGSiteIds)
                {

                    string approvalLevel = "";

                    if (_dataContext.IsUserRoleForCHGSite(_dataContext.UserId.Value, chgSiteId, "CEO") || _dataContext.IsUserRoleForCHGSite(_dataContext.UserId.Value, chgSiteId, "DBD"))
                    {
                        //Start three layer.
                        users = _dataContext.GetRoleUsersForCHGSite(chgSiteId, "AVP");
                        approvalLevel = "AVP";
                    }

                    if (_dataContext.IsUserRoleForCHGSite(_dataContext.UserId.Value, chgSiteId, "AVP"))
                    {
                        //Start 2 layer.
                        //Send approval task to CRO.    
                        users = _dataContext.GetRoleUsers("CRO");
                        approvalLevel = "CRO";

                    }

                    if (users != null)
                    {

                        //Get the CRO and create the task, alert and email for it.
                        foreach (var user in users)
                        {

                            //Create a new task for the user with the required parameters.
                            var task = _dataContext.CreateTask("Referral Source Approval", user.UserId, "Created", "Referral Source Approval Request");
                            _dataContext.SaveChanges();
                            _dataContext.UpdateTaskParameter(task.UserTaskId, "APPROVAL_LEVEL", approvalLevel);
                            _dataContext.UpdateTaskParameter(task.UserTaskId, "REFERRAL_SOURCE_ID", referralSourceId.ToString());
                            _dataContext.UpdateTaskParameter(task.UserTaskId, "CHGSITE_ID", chgSiteId.ToString());

                            _dataContext.SaveChanges();

                            //Send task and alert.
                            values = new Dictionary<string, string>();
                            values.Add("TMP_SUBJECT", "REFERRAL_SOURCE_APPROVAL_REQUEST_CREATED_SUBJECT");
                            values.Add("TMP_BODY", "REFERRAL_SOURCE_APPROVAL_REQUEST_CREATED_BODY");
                            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                            values.Add("EMAIL_FROM_NAME", "CRA");
                            values.Add("REFERRAL_SOURCE_NAME", $"{referralSource.FullName}");
                            values.Add("TASK_URL", ConfigurationManager.AppSettings["APP_URL"] + "/Task/ResolveReferralSource/" + task.UserTaskId);


                            _dataContext.CreateEmailAlert(user.UserId, values);
                            _dataContext.CreateNotification("Info", "Request", $"New referral source request for {referralSource.FullName}.", user.UserId);

                            _dataContext.SaveChanges();
                        }
                    }

                }



            }
            
        }
        [HttpPost]
        public JsonResult Delete(long? id)
        {

            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." });
            }

            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }

            var query = (from c in _dataContext.ReferralSources.Include("ReferralSourceType") where c.Deleted == false select c);

            var query2 = (from c in _dataContext.CHGSiteReferralSources join d in query on c.ReferralSourceId equals d.ReferralSourceId where c.Deleted == false && c.Deleted == false select new { ReferralSource = d, CHGSite = c.CHGSite }).ToList();

            var results = (from c in query2 join d in _dataContext.GetUserSites(_dataContext.UserId.Value) on c.CHGSite.CHGSiteId equals d.CHGSiteId where c.ReferralSource.ReferralSourceId == id select c.ReferralSource).ToList();

            if (results.Count() <= 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }

            var entity = _dataContext.ReferralSources.Where(p => p.ReferralSourceId == id && p.Deleted == false).SingleOrDefault();


            if (entity != null)
            {
                using (var transaction = _dataContext.Database.BeginTransaction())
                {

                    entity.Deleted = true;
                    _dataContext.SaveChanges();
                    transaction.Commit();

                }


                return Json(new { Status = Constant.RESPONSE_OK });
            }
            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
        }

        public JsonResult DetailEntity(long? id)
        {
           

            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." }, JsonRequestBehavior.AllowGet);
            }

            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." }, JsonRequestBehavior.AllowGet);
            }

            var query = (from c in _dataContext.ReferralSources.Include("ReferralSourceType") where c.Deleted == false select c);

            var query2 = (from c in _dataContext.CHGSiteReferralSources join d in query on c.ReferralSourceId equals d.ReferralSourceId where c.Deleted == false && c.Deleted == false select new { ReferralSource = d, CHGSite = c.CHGSite }).ToList();

            var results = (from c in query2 join d in _dataContext.GetUserSites(_dataContext.UserId.Value) on c.CHGSite.CHGSiteId equals d.CHGSiteId where c.ReferralSource.ReferralSourceId == id select c.ReferralSource).ToList();

            if (results.Count() <= 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." }, JsonRequestBehavior.AllowGet);
            }

            var entity = _dataContext.ReferralSources.Where(p => p.ReferralSourceId == id && p.Deleted == false).SingleOrDefault();
            if (entity == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Referral source not found. Invalid referral source." });
            }

            ReferralSourceModel model = new ReferralSourceModel()
            {
                FullName = entity.FullName,
                ShortName = entity.ShortName,
                BedCount = entity.BedCount,
                OperationalICUBedCount = entity.OperationalICUBedCount,
                CCUBedCount = entity.CCUBedCount,
                SkilledNursingBedCount = entity.SkilledNursingBedCount,
                SICUBedCount = entity.SICUBedCount,
                RehabBedCount = entity.RehabBedCount,
                ConfirmedBedsCodingICURevCodes = entity.ConfirmedBedsCodingICURevCodes,
                ICUBedCount = entity.ICUBedCount,
                LTACHBedCount = entity.LTACHBedCount,
                MICUBedCount = entity.MICUBedCount,
                NeuroICUBedCount = entity.NeuroICUBedCount,
                ReferralSourceTypeId = entity.ReferralSourceTypeId,
                ReferralSourceId = entity.ReferralSourceId,
                ReferralSourceTypeName = entity.ReferralSourceType.Name,
                IsApproved = entity.IsApproved,
                Sites = _dataContext.CHGSiteReferralSources.Where(p => p.ReferralSourceId == id.Value).Select(p => new ReferralSourceCHGSiteModel()
                {
                    CHGSiteId = p.CHGSiteId,
                    Selected = true
                }).ToList(), 
                ElectronicReferralTypes = _dataContext.ReferralSourceElectronicReferralTypes.Where(p => p.ReferralSourceId == id.Value).Select(p => new ReferralSourceElectronicReferralTypeModel()
                {
                    ElectronicReferralTypeId = p.ElectronicReferralTypeId,
                    Selected = true
                }).ToList(),
                CommercialPayorTypes = _dataContext.ReferralSourceCommercialPayorTypes.Where(p => p.ReferralSourceId == id.Value).Select(p => new ReferralSourceCommercialPayorTypeModel()
                {
                    CommercialPayorTypeId = p.CommercialPayorTypeId,
                    Selected = true              
                }).ToList(),
                ManagedMedicarePayorTypes = _dataContext.ReferralSourceManagedMedicarePayorTypes.Where(p => p.ReferralSourceId == id.Value).Select(p => new ReferralSourceManagedMedicarePayorTypeModel()
                {
                    ManagedMedicarePayorTypeId = p.ManagedMedicarePayorTypeId,
                    Selected = true
                }).ToList()
            };


            var task = (from t in _dataContext.UserTasks join p in _dataContext.UserTaskParameters on t.UserTaskId equals p.UserTaskId where t.Deleted == false && p.Deleted == false && p.Key == "REFERRAL_SOURCE_ID" && t.UserId == _dataContext.UserId.Value && t.Status == "Created" && p.Value == entity.ReferralSourceId.ToString() select t).Distinct().Take(1).SingleOrDefault();


            return Json(new { Status = Constant.RESPONSE_OK, Data = model, PendingTask = task }, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult Edit()
        {
            if (!HasContextRole(new string[] { "CRO"})) //Only CRO can edit.
            {
                return RedirectToUnauthorized();
            }

            return View();
        }
        
        [HttpPost]
        public JsonResult Edit(long? id, ReferralSourceModel model)
        {
            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." });
            }

            if (!HasContextRole(new string[] { "CRO" })) //Only CRO can edit.
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }



            if (ModelState.IsValid)
            {
                if (model.Sites == null || model.Sites.Where(p => p.Selected == true).Count() <= 0)
                    
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Please select at least one site to associate the referral source to." });

                }

                using (var transaction = _dataContext.Database.BeginTransaction())
                {

                    if (_dataContext.ReferralSources.Where(p => p.FullName == model.FullName && p.ReferralSourceId != id && p.Deleted == false).Count() <= 0)
                    {
                        var entity = _dataContext.ReferralSources.Where(p => p.ReferralSourceId == id).SingleOrDefault();
                        if (entity == null)
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Referral source not found. Invalid referral source." });
                        }

                        entity.FullName = model.FullName;
                        entity.ShortName = model.ShortName;

                        entity.BedCount = model.BedCount;
                        entity.OperationalICUBedCount = model.OperationalICUBedCount;
                        entity.CCUBedCount = model.CCUBedCount;
                        entity.SkilledNursingBedCount = model.SkilledNursingBedCount;
                        entity.SICUBedCount = model.SICUBedCount;
                        entity.RehabBedCount = model.RehabBedCount;
                        entity.ConfirmedBedsCodingICURevCodes = model.ConfirmedBedsCodingICURevCodes;
                        entity.ICUBedCount = model.ICUBedCount;
                        entity.LTACHBedCount = model.LTACHBedCount;
                        entity.MICUBedCount = model.MICUBedCount;
                        entity.NeuroICUBedCount = model.NeuroICUBedCount;
                        entity.ReferralSourceTypeId = model.ReferralSourceTypeId;
                        entity.IsApproved = true;
                        _dataContext.SaveChanges();

                        _dataContext.CHGSiteReferralSources.RemoveRange(_dataContext.CHGSiteReferralSources.Where(p => p.ReferralSourceId == id.Value));
                        _dataContext.SaveChanges();

                        foreach (var site in model.Sites.Where(p => p.Selected == true))
                        {
                            CHGSiteReferralSource siteReferralSource = new CHGSiteReferralSource()
                            {
                                CHGSiteId = site.CHGSiteId,

                                ReferralSource = entity
                            };

                            _dataContext.CHGSiteReferralSources.Add(siteReferralSource);
                        }

                        _dataContext.SaveChanges();


                        _dataContext.ReferralSourceElectronicReferralTypes.RemoveRange(_dataContext.ReferralSourceElectronicReferralTypes.Where(p => p.ReferralSourceId == id.Value));
                        _dataContext.SaveChanges();

                        if (model.ElectronicReferralTypes != null)
                        {
                            foreach (var electronicReferralType in model.ElectronicReferralTypes.Where(p => p.Selected == true))
                            {

                                ReferralSourceElectronicReferralType item = new ReferralSourceElectronicReferralType()
                                {

                                    ElectronicReferralTypeId = electronicReferralType.ElectronicReferralTypeId,
                                    ReferralSource = entity

                                };

                                _dataContext.ReferralSourceElectronicReferralTypes.Add(item);

                            }
                        }

                        _dataContext.SaveChanges();

                        _dataContext.ReferralSourceCommercialPayorTypes.RemoveRange(_dataContext.ReferralSourceCommercialPayorTypes.Where(p => p.ReferralSourceId == id.Value));
                        _dataContext.SaveChanges();


                        if (model.CommercialPayorTypes != null)
                        {
                            foreach (var commercialPayorType in model.CommercialPayorTypes.Where(p => p.Selected == true))
                            {

                                ReferralSourceCommercialPayorType item = new ReferralSourceCommercialPayorType()
                                {


                                    CommercialPayorTypeId = commercialPayorType.CommercialPayorTypeId,
                                    ReferralSource = entity

                                };

                                _dataContext.ReferralSourceCommercialPayorTypes.Add(item);

                            }
                        }

                        _dataContext.SaveChanges();

                        _dataContext.ReferralSourceManagedMedicarePayorTypes.RemoveRange(_dataContext.ReferralSourceManagedMedicarePayorTypes.Where(p => p.ReferralSourceId == id.Value));
                        _dataContext.SaveChanges();


                        if (model.ManagedMedicarePayorTypes != null)
                        {
                            foreach (var ManagedMedicarePayorType in model.ManagedMedicarePayorTypes.Where(p => p.Selected == true))
                            {

                                ReferralSourceManagedMedicarePayorType item = new ReferralSourceManagedMedicarePayorType()
                                {

                                    ManagedMedicarePayorTypeId = ManagedMedicarePayorType.ManagedMedicarePayorTypeId,
                                    ReferralSource = entity

                                };

                                _dataContext.ReferralSourceManagedMedicarePayorTypes.Add(item);

                                 
                                

                            }     
                        }

                        _dataContext.SaveChanges();

                        transaction.Commit();

                        return Json(new { Status = Constant.RESPONSE_OK, Description = "Referral source saved successfully." });
                    }
                    else
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "The referral source with the same full name already exists. Please enter a different name." });
                    }

                }

            }

            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });

        }

        public ActionResult Detail()
        {
            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return RedirectToUnauthorized();
            }

            return View();
        }
        
        
    }

}