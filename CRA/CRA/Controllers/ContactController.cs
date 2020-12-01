using CRA.Data;
using CRA.Data.Entities;
using CRA.Models.Contact;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{
    public class ContactController : BaseController
    {

        public ActionResult Index()
        {
            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return RedirectToUnauthorized();
            }

            return View();
        }

        public ActionResult Create()
        {
            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return RedirectToUnauthorized();
            }

            return View();
        }


        public ActionResult Detail()
        {
            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return RedirectToUnauthorized();
            }

            return View();
        }

        public ActionResult Edit()
        {
            if (!HasContextRole(new string[] { "CRO"}))
            {
                return RedirectToUnauthorized();
            }
            return View();
        }

        public JsonResult Contacts(string keyword, long? contactRoleTypeId)
        {
            List<ContactModel> model = new List<ContactModel>();

            var query = (from c in _dataContext.Contacts.Where(p => p.Deleted == false).Except(from c in _dataContext.Users select c.Contact) select c);

          
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.FirstName.StartsWith(keyword) || p.LastName.StartsWith(keyword) || p.Email.StartsWith(keyword));
            }

            if (contactRoleTypeId != null && contactRoleTypeId != 0)
            {
                query = query.Join(_dataContext.ContactAttributes.Where(p => p.ContactRoleTypeId == contactRoleTypeId && p.Deleted == false), s => s.ContactId, t => t.ContactId, (s, t) => new { Contact = s }).Select(p => p.Contact);
            }


            var query2 = (from c in query join d in _dataContext.ContactReferralSources on c.ContactId equals d.ContactId where d.Deleted == false select new { Contact = c, ReferralSource = d.ReferralSource });

            var query3 = (from c in _dataContext.CHGSiteReferralSources join d in query2 on c.ReferralSourceId equals d.ReferralSource.ReferralSourceId where c.Deleted == false && c.Deleted == false select new { ReferralSource = d, CHGSite = c.CHGSite, Contact=d.Contact }).ToList();

            var results = (from c in query3 join d in _dataContext.GetUserSites(_dataContext.UserId.Value) on c.CHGSite.CHGSiteId equals d.CHGSiteId select c.Contact).Distinct().ToList();



            foreach (var entity in results.ToList())
            {
                model.Add(new ContactModel()
                {
                    ContactId = entity.ContactId,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    Phone = entity.Phone,
                    Mobile = entity.Mobile,
                    IsApproved = entity.IsApproved
                });
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Create(ContactModel model)
        {
            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }

            if (ModelState.IsValid)
            {

                if (model.ReferralSources == null || model.ReferralSources.Count() <= 0)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "The contact must be assigned to one or more referral source." });

                }

                using (var transaction = _dataContext.Database.BeginTransaction())
                {
                    if (_dataContext.Contacts.Where(p => p.Email == model.Email && p.Deleted == false).Count() <= 0)
                    {
                        Contact entity = new Contact()
                        {

                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Phone = model.Phone,
                            Mobile = model.Mobile
                        };

                        _dataContext.Contacts.Add(entity);
                        _dataContext.SaveChanges();

                        var contactAttribute = new ContactAttribute()
                        {
                            CategoryTypeId = model.CategoryTypeId,
                            Contact = entity
                        };

                        contactAttribute.Location = model.Location;
                        contactAttribute.Note = model.Note;
                        contactAttribute.ContactRoleTypeId = _dataContext.ContactRoleTypes.Where(p => p.Name == model.ContactRoleTypeName && p.Deleted == false).Single().ContactRoleTypeId;

                        if (model.ContactRoleTypeName == "Physician" || model.ContactRoleTypeName == "Nurse Practitioner")
                        {
                            if (model.SpecialityTypeId == null || model.SpecialityTypeId == 0)
                            {
                                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Please select the speciality for the contact role." });
                            }

                            contactAttribute.SpecialityTypeId = model.SpecialityTypeId;

                        }
                        else
                        {
                            contactAttribute.SpecialityTypeId = null;
                        }


                        _dataContext.ContactAttributes.Add(contactAttribute);
                        _dataContext.SaveChanges();

                        if (model.ReferralSources != null)
                        {
                            foreach (var item in model.ReferralSources)
                            {
                                ContactReferralSource contactReferralSource = new ContactReferralSource()
                                {
                                    Contact = entity,
                                    ReferralSourceId = item.ReferralSourceId

                                };

                                _dataContext.ContactReferralSources.Add(contactReferralSource);

                            }
                        }

                        _dataContext.SaveChanges();

                        ApprovalProcess(entity.ContactId);

                        _dataContext.SaveChanges();

                        transaction.Commit();

                        return Json(new { Status = Constant.RESPONSE_OK, Description = "Contact created successfully." });
                    }
                    else
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "The contact with the same email already exists. Please enter a different email." });
                    }
                }

            }

            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });

        }



        public JsonResult DetailEntity(long? id)
        {
            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." }, JsonRequestBehavior.AllowGet);
            }

            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." });
            }



            var query = (from c in _dataContext.Contacts.Where(p => p.Deleted == false).Except(from c in _dataContext.Users select c.Contact) select c);

   

            var query2 = (from c in query join d in _dataContext.ContactReferralSources on c.ContactId equals d.ContactId where d.Deleted == false select new { Contact = c, ReferralSource = d.ReferralSource });

            var query3 = (from c in _dataContext.CHGSiteReferralSources join d in query2 on c.ReferralSourceId equals d.ReferralSource.ReferralSourceId where c.Deleted == false && c.Deleted == false select new { ReferralSource = d, CHGSite = c.CHGSite, Contact = d.Contact }).ToList();

            var results = (from c in query3 join d in _dataContext.GetUserSites(_dataContext.UserId.Value) on c.CHGSite.CHGSiteId equals d.CHGSiteId where c.Contact.ContactId == id.Value select c.Contact).ToList();

            if (results.Count() <= 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." }, JsonRequestBehavior.AllowGet);
            }

            var entity = _dataContext.Contacts.Where(p => p.ContactId == id && p.Deleted == false).SingleOrDefault();
            if (entity == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Contact not found. Invalid contact." }, JsonRequestBehavior.AllowGet);
            }

            ContactModel model = new ContactModel()
            {
                ContactId = entity.ContactId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                Phone = entity.Phone,
                Mobile = entity.Mobile
            };

            var contactAttribute = _dataContext.ContactAttributes.Include("CategoryType").Include("ContactRoleType").Include("SpecialityType").Where(p => p.ContactId == id.Value && p.Deleted == false).SingleOrDefault();
            if (contactAttribute != null)
            {
                model.CategoryName = contactAttribute.CategoryType.Name;
                model.CategoryTypeId = contactAttribute.CategoryTypeId;
                model.Location = contactAttribute.Location;
                model.Note = contactAttribute.Note;
                model.ContactRoleTypeName = contactAttribute.ContactRoleType.Name;

                if (contactAttribute.SpecialityType != null)
                {
                    model.SpecialityTypeName = contactAttribute.SpecialityType.Name;
                    model.SpecialityTypeId = contactAttribute.SpecialityTypeId;
                }
            }

            model.ReferralSources = _dataContext.ContactReferralSources.Include("ReferralSource").Include("ReferralSource.ReferralSourceType").Where(p => p.ContactId == id.Value && p.Deleted == false).Select(p => new ReferralSourceContactModel()
            {
                FullName = p.ReferralSource.FullName,
                ReferralSourceId = p.ReferralSource.ReferralSourceId,
                ReferralSourceTypeName = p.ReferralSource.ReferralSourceType.Name,
                ShortName = p.ReferralSource.ShortName
            }).ToList();

            var task = (from t in _dataContext.UserTasks join p in _dataContext.UserTaskParameters on t.UserTaskId equals p.UserTaskId where t.Deleted == false && p.Deleted == false && p.Key == "CONTACT_ID" && t.UserId == _dataContext.UserId.Value && t.Status == "Created" && p.Value == entity.ContactId.ToString() select t).Distinct().Take(1).SingleOrDefault();

            return Json(new { Status = Constant.RESPONSE_OK, Data = model, PendingTask = task }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult Edit(long? id, ContactModel model)
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }

            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." });
            }

            var query = (from c in _dataContext.Contacts.Where(p => p.Deleted == false).Except(from c in _dataContext.Users select c.Contact) select c);

            query = query.Where(p => p.IsApproved == true);

            var query2 = (from c in query join d in _dataContext.ContactReferralSources on c.ContactId equals d.ContactId where d.Deleted == false select new { Contact = c, ReferralSource = d.ReferralSource });

            var query3 = (from c in _dataContext.CHGSiteReferralSources join d in query2 on c.ReferralSourceId equals d.ReferralSource.ReferralSourceId where c.Deleted == false && c.Deleted == false select new { ReferralSource = d, CHGSite = c.CHGSite, Contact = d.Contact }).ToList();

            var results = (from c in query3 join d in _dataContext.GetUserSites(_dataContext.UserId.Value) on c.CHGSite.CHGSiteId equals d.CHGSiteId where c.Contact.ContactId == id.Value select c.Contact).ToList();

            if (results.Count() <= 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }



            if (model.ReferralSources == null || model.ReferralSources.Count() <= 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "The contact must be assigned to one or more referral source." });

            }

            if (ModelState.IsValid)
            {
                using (var transaction = _dataContext.Database.BeginTransaction())
                {
                    if (_dataContext.Contacts.Where(p => p.Email == model.Email && p.ContactId != id && p.Deleted == false).Count() <= 0)
                    {
                        var entity = _dataContext.Contacts.Where(p => p.ContactId == id && p.Deleted == false).SingleOrDefault();
                        if (entity == null)
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Contact not found. Invalid contact." });
                        }

                        entity.FirstName = model.FirstName;
                        entity.LastName = model.LastName;
                        entity.Email = model.Email;
                        entity.Mobile = model.Mobile;
                        entity.Phone = model.Phone;
                        _dataContext.SaveChanges();

                        var contactAttribute = _dataContext.ContactAttributes.Include("CategoryType").Where(p => p.ContactId == id.Value && p.Deleted == false).SingleOrDefault();

                        if (contactAttribute == null)
                        {
                            contactAttribute = new ContactAttribute();
                            contactAttribute.ContactId = entity.ContactId;

                            contactAttribute.CategoryTypeId = model.CategoryTypeId;
                            _dataContext.ContactAttributes.Add(contactAttribute);
                        }

                        contactAttribute.Location = model.Location;
                        contactAttribute.Note = model.Note;
                        contactAttribute.ContactRoleTypeId = _dataContext.ContactRoleTypes.Where(p => p.Name == model.ContactRoleTypeName && p.Deleted == false).Single().ContactRoleTypeId;
                        contactAttribute.CategoryTypeId = model.CategoryTypeId;

                        if (model.ContactRoleTypeName == "Physician" || model.ContactRoleTypeName == "Nurse Practitioner")
                        {
                            if (model.SpecialityTypeId == null || model.SpecialityTypeId == 0)
                            {
                                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Please select the speciality for the contact role." });
                            }

                            contactAttribute.SpecialityTypeId = model.SpecialityTypeId;

                        }
                        else
                        {
                            contactAttribute.SpecialityTypeId = null;
                        }

                        _dataContext.SaveChanges();

                        _dataContext.ContactReferralSources.RemoveRange(_dataContext.ContactReferralSources.Where(p => p.ContactId == id.Value));
                        _dataContext.SaveChanges();


                        if (model.ReferralSources != null)
                        {
                            foreach (var item in model.ReferralSources)
                            {
                                ContactReferralSource contactReferralSource = new ContactReferralSource()
                                {
                                    Contact = entity,
                                    ReferralSourceId = item.ReferralSourceId

                                };

                                _dataContext.ContactReferralSources.Add(contactReferralSource);

                            }
                        }

                        _dataContext.SaveChanges();

                        transaction.Commit();


                        return Json(new { Status = Constant.RESPONSE_OK, Description = "Contact saved successfully." });
                    }
                    else
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "The contact with the same email already exists. Please enter a different email." });
                    }
                }


            }

            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });

        }


        [HttpPost]
        public JsonResult Delete(long? id)
        {

            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." });
            }

            var query = (from c in _dataContext.Contacts.Where(p => p.Deleted == false).Except(from c in _dataContext.Users select c.Contact) select c);

            query = query.Where(p => p.IsApproved == true);

            var query2 = (from c in query join d in _dataContext.ContactReferralSources on c.ContactId equals d.ContactId where d.Deleted == false select new { Contact = c, ReferralSource = d.ReferralSource });

            var query3 = (from c in _dataContext.CHGSiteReferralSources join d in query2 on c.ReferralSourceId equals d.ReferralSource.ReferralSourceId where c.Deleted == false && c.Deleted == false select new { ReferralSource = d, CHGSite = c.CHGSite, Contact = d.Contact }).ToList();

            var results = (from c in query3 join d in _dataContext.GetUserSites(_dataContext.UserId.Value) on c.CHGSite.CHGSiteId equals d.CHGSiteId where c.Contact.ContactId == id.Value select c.Contact).ToList();

            if (results.Count() <= 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }


            var entity = _dataContext.Contacts.Where(p => p.ContactId == id).SingleOrDefault();
            if (entity != null)
            {
                entity.Deleted = true;
                _dataContext.SaveChanges();

                return Json(new { Status = Constant.RESPONSE_OK });
            }
            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
        }



        private void ApprovalProcess(long contactId)
        {
            Dictionary<string, string> values = null;
            List<long> CHGSiteIds = _dataContext.GetUserSites(_dataContext.UserId.Value).Select(p => p.CHGSiteId).ToList();
            List<User> users = null;
            var contact = _dataContext.Contacts.Find(contactId);

            //Approval process.
            if (_dataContext.UserHasRole(_dataContext.UserId.Value, "CRO"))
            {
                _dataContext.ApproveContact(contactId);

                foreach (var chgSiteId in CHGSiteIds)
                {
                    values = new Dictionary<string, string>();
                    values.Add("TMP_SUBJECT", "CONTACT_APPROVAL_REQUEST_APPROVED_SUBJECT");
                    values.Add("TMP_BODY", "CONTACT_APPROVAL_REQUEST_APPROVED_BODY");
                    values.Add("CONTACT_NAME", $"{contact.FirstName} {contact.LastName}");
                    values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                    values.Add("EMAIL_FROM_NAME", "CRA");
                    values.Add("DECISION_NOTES", "");

                    users = _dataContext.GetRoleUsersForCHGSite(chgSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(chgSiteId, "DBD")).Union(_dataContext.GetRoleUsersForCHGSite(chgSiteId, "AVP")).Distinct().ToList();

                    if (users != null)
                    {
                        foreach (var user in users)
                        {
                            _dataContext.CreateEmailAlert(user.UserId, values);
                            _dataContext.CreateNotification("Success", "Approved", $"Contact request for {contact.FirstName} {contact.LastName} approved.", user.UserId);
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
                            var task = _dataContext.CreateTask("Contact Approval", user.UserId, "Created", "Contact Approval Request");
                            _dataContext.SaveChanges();
                            _dataContext.UpdateTaskParameter(task.UserTaskId, "APPROVAL_LEVEL", approvalLevel);
                            _dataContext.UpdateTaskParameter(task.UserTaskId, "CONTACT_ID", contactId.ToString());
                            _dataContext.UpdateTaskParameter(task.UserTaskId, "CHGSITE_ID", chgSiteId.ToString());

                            _dataContext.SaveChanges();

                            //Send task and alert.
                            values = new Dictionary<string, string>();
                            values.Add("TMP_SUBJECT", "CONTACT_APPROVAL_REQUEST_CREATED_SUBJECT");
                            values.Add("TMP_BODY", "CONTACT_APPROVAL_REQUEST_CREATED_BODY");
                            values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                            values.Add("EMAIL_FROM_NAME", "CRA");
                            values.Add("CONTACT_NAME", $"{contact.FirstName} {contact.LastName}");
                            values.Add("TASK_URL", ConfigurationManager.AppSettings["APP_URL"] + "/Task/ResolveContact/" + task.UserTaskId);


                            _dataContext.CreateEmailAlert(user.UserId, values);
                            _dataContext.CreateNotification("Info", "Request", $"New contact request for {contact.FirstName} {contact.LastName}.", user.UserId);

                            _dataContext.SaveChanges();
                        }
                    }

                }



            }

        }



    }

}