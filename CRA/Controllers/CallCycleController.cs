using CRA.Data;
using CRA.Data.Entities;
using CRA.Models.CallCycle;
using CRA.Models.CHGSite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{
    public class CallCycleController : BaseController
    {
        public ActionResult Admin()
        {
            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP" }))
            {
                return RedirectToUnauthorized();
            }

            CallCycleModel model = new CallCycleModel();
            ViewBag.FromDate = DateTime.Now.AddDays(-30).ToShortDateString();
            ViewBag.ToDate = DateTime.Now.ToShortDateString();

            return View(model);
        }
        public ActionResult Index()
        {
            if (!HasContextRole(new string[] { "CRO", "CEO", "DBD", "AVP", "CL" }))
            {
                return RedirectToUnauthorized();
            }
            CallCycleModel model = new CallCycleModel();
            ViewBag.FromDate = DateTime.Now.AddDays(-30).ToShortDateString();
            ViewBag.ToDate = DateTime.Now.ToShortDateString();
            return View(model);
        }

        public JsonResult GetUserSites()
        {
            var items = _dataContext.GetUserSites(_dataContext.UserId.Value);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLiasons(long CHGSiteId)
        {

            var sites = _dataContext.GetUserSites(_dataContext.UserId.Value).Where(p => p.CHGSiteId == CHGSiteId).ToList();

            if (sites.Count() > 0)
            {

                var query = _dataContext.UserRoles.Include("User").Include("RoleType")
                .Where(p => p.Deleted == false && p.User.Deleted == false && p.UserRoleType.Deleted == false && p.User.Enabled == true && p.UserRoleType.Name == "CL");
                var query1 = _dataContext.UserCHGSites.Include("User").Include("CHGSite").Include("User.Contact").Where(p => p.Deleted == false && p.CHGSite.Deleted == false && p.CHGSiteId == CHGSiteId);
                var items = (from c in query join d in query1 on c.UserId equals d.UserId select c.User)
                    .ToList()
                       .Select(p => new { UserId = p.UserId, Name = $"{p.Contact.FirstName} {p.Contact.LastName}" });

                return Json(items, JsonRequestBehavior.AllowGet);

            }

            var nullArray = new string[0];

            return Json(nullArray, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserReferralSources(long CHGSiteId)
        {
            var sites = _dataContext.GetUserSites(_dataContext.UserId.Value).Where(p => p.CHGSiteId == CHGSiteId).ToList();

            if (sites.Count() > 0)
            {

                var items = _dataContext.CHGSiteReferralSources.Include("ReferralSource").Where(p => p.Deleted == false && p.ReferralSource.Deleted == false && p.ReferralSource.IsApproved == true && p.CHGSiteId == CHGSiteId)
                            .Select(p => p.ReferralSource)
                            .ToList();
                return Json(items, JsonRequestBehavior.AllowGet);

            }

            var nullArray = new string[0];

            return Json(nullArray, JsonRequestBehavior.AllowGet);
        }


        //Done security till.


        public JsonResult GetReferralSourceContacts(long ReferralSourceId)
        {
            var query = _dataContext
                .ContactReferralSources
                .Include("Contact")
                .Include("ReferralSource")
                .Where(p => p.Deleted == false && p.Contact.Deleted == false && p.ReferralSource.Deleted == false && p.ReferralSourceId == ReferralSourceId && p.Contact.IsApproved==true);


            var query2 = (from c in query join d in _dataContext.ContactAttributes.Include("CategoryType").Include("ContactRoleType").Include("SpecialityType") on c.Contact.ContactId equals d.ContactId join e in _dataContext.CategoryTypes on d.CategoryTypeId equals e.CategoryTypeId where c.Deleted == false && d.Deleted == false && e.Deleted == false select new { Contact = c.Contact, CategoryName = e.Name, Role = d.ContactRoleType.Name, Speciality = d.SpecialityType.Name });

            var items = query2
             .ToList()
             .Select(p => new { ContactId = p.Contact.ContactId, Name = $"{p.Contact.FirstName} {p.Contact.LastName}", CategoryName = p.CategoryName, Role = p.Role, Speciality = p.Speciality, Description = $"{p.Contact.FirstName} {p.Contact.LastName} ({p.Role} - {p.Speciality})" })
             ;
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserSchedule(long userId)
        {
            var query = (from a in _dataContext.ContactAttributes join c in _dataContext.Contacts on a.ContactId equals c.ContactId join s in _dataContext.ScheduleItems on c.ContactId equals s.ContactId join sh in _dataContext.Schedules on s.ScheduleId equals sh.ScheduleId where sh.UserId == userId && sh.ActivationDate != null && sh.DeactivationDate == null && sh.ActivationDate <= DateTime.Now && s.Deleted == false && sh.Deleted == false && a.Deleted == false && c.Deleted == false && c.IsApproved==true select new { ContactId = c.ContactId, FirstName = c.FirstName, LastName = c.LastName, CategoryName = a.CategoryType.Name, Day = s.Day, Time = s.Time, ScheduleItemId = s.ScheduleItemId, RoleName = a.ContactRoleType.Name, Speciality = a.SpecialityTypeId == null ? null : a.SpecialityType.Name });
            bool viewPending = false;
            var pendingSchedule = _dataContext.Schedules.Where(p => p.UserId == userId && p.Deleted == false && p.Decided == false).SingleOrDefault();
            long ScheduleId = 0;
            if (pendingSchedule != null)
            {
                viewPending = true;
                ScheduleId = pendingSchedule.ScheduleId;

            }


            var referralSources = (from a in query join c in _dataContext.ContactReferralSources on a.ContactId equals c.ContactId where c.Deleted == false && c.ReferralSource.Deleted == false && c.ReferralSource.IsApproved==true  select new { ContactId = a.ContactId, ReferralSourceName = c.ReferralSource.FullName }).Distinct().ToList();

            var scheduleVisits = (from c in _dataContext.ScheduleVisits join q in query on c.ScheduleItemId equals q.ScheduleItemId where c.Deleted == false select c).ToList();


            var obj = new { ScheduleId = ScheduleId, ViewPending = viewPending, Items = query.OrderBy(p => p.ScheduleItemId).ToList().Select(p => new { ContactId = p.ContactId, ContactName = $"{p.FirstName} {p.LastName}", CategoryName = p.CategoryName, Day = p.Day, Time = p.Time, ScheduleItemId = p.ScheduleItemId, Visited = scheduleVisits.Where(g => g.ScheduleItemId == p.ScheduleItemId).Count() > 0, RoleName = p.RoleName, Speciality = p.Speciality, ReferralSources = referralSources.Where(d => d.ContactId == p.ContactId).Select(e => new { Name = e.ReferralSourceName }).Distinct().ToArray() }).ToList() };

            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        

        public JsonResult GetSchedule(long scheduleId)
        {

            var query = (from a in _dataContext.ContactAttributes join c in _dataContext.Contacts on a.ContactId equals c.ContactId join s in _dataContext.ScheduleItems on c.ContactId equals s.ContactId join sh in _dataContext.Schedules on s.ScheduleId equals sh.ScheduleId where c.IsApproved==true && sh.ScheduleId == scheduleId && s.Deleted == false && sh.Deleted == false && a.Deleted == false && c.Deleted == false select new { ContactId = c.ContactId, FirstName = c.FirstName, LastName = c.LastName, CategoryName = a.CategoryType.Name, Day = s.Day, Time = s.Time, ScheduleItemId = s.ScheduleItemId });
            bool viewPending = false;
            var pendingSchedule = _dataContext.Schedules.Where(p => p.ScheduleId == scheduleId && p.Deleted == false && p.Decided == false).SingleOrDefault();
            long ScheduleId = 0;
            if (pendingSchedule != null)
            {
                viewPending = true;
                ScheduleId = pendingSchedule.ScheduleId;

            }

            var task = (from t in _dataContext.UserTasks join p in _dataContext.UserTaskParameters on t.UserTaskId equals p.UserTaskId where t.Deleted == false && p.Deleted == false && p.Key == "SCHEDULE_ID" && t.UserId == _dataContext.UserId.Value && t.Status == "Created" && p.Value == scheduleId.ToString() select t).Distinct().Take(1).SingleOrDefault();

            var obj = new { UserName = $"{pendingSchedule.User.Contact.FirstName} {pendingSchedule.User.Contact.LastName}", Notes = pendingSchedule.Notes, ScheduleId = ScheduleId, ViewPending = viewPending, PendingTask = task, Items = query.OrderBy(p => p.ScheduleItemId).ToList().Select(p => new { ContactId = p.ContactId, ContactName = $"{p.FirstName} {p.LastName}", CategoryName = p.CategoryName, Day = p.Day, Time = p.Time, ScheduleItemId = p.ScheduleItemId }).ToList() };

            return Json(obj, JsonRequestBehavior.AllowGet);

                

        }

        public JsonResult GetCurrentSchedule()
        {
            var userId = _dataContext.UserId.Value;
            return GetUserSchedule(userId);
        }
         
       
       

        
        [HttpPost]
        public JsonResult CreateSchedule(ScheduleModel model)
        {
            if (model.Items == null || model.Items.Count() <= 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "At least one schedule item must be specified." });
            }

            if (_dataContext.Schedules.Where(p => p.UserId == model.UserId && p.Decided == false && p.Deleted == false).Count() > 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "There is already a schedule that is awaiting approval." });
            }

            var userEntity = _dataContext.Users.Find(model.UserId);

            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var schedule = new Schedule();
                schedule.UserId = model.UserId;
                schedule.WeekType = "1 Week";
                schedule.Notes = model.Notes;
                _dataContext.Schedules.Add(schedule);
                _dataContext.SaveChanges();

                foreach (var item in model.Items)
                {
                    ScheduleItem scheduleItem = new ScheduleItem();
                    scheduleItem.ScheduleId = schedule.ScheduleId;
                    scheduleItem.CHGSiteId = item.CHGSiteId;
                    scheduleItem.ContactId = item.ContactId;
                    scheduleItem.Day = item.Day;
                    scheduleItem.ReferralSourceId = item.ReferralSourceId;
                    scheduleItem.Time = item.Time;
                    scheduleItem.WeekNumber = 1;

                    _dataContext.ScheduleItems.Add(scheduleItem);

                }

                _dataContext.SaveChanges();
                Dictionary<string, string> values = null;
                List<User> users = null;
                List<long> CHGSiteIds = model.Items.Select(p => p.CHGSiteId).Distinct().ToList();

                //Approval process.
                if (_dataContext.UserHasRole(_dataContext.UserId.Value, "CRO"))
                {
                    _dataContext.ApproveSchedule(schedule.ScheduleId);
                    _dataContext.SaveChanges();

                    foreach (var chgSiteId in CHGSiteIds)
                    {
                        values = new Dictionary<string, string>();
                        values.Add("TMP_SUBJECT", "CALL_CYCLE_APPROVAL_REQUEST_APPROVED_SUBJECT");
                        values.Add("TMP_BODY", "CALL_CYCLE_APPROVAL_REQUEST_APPROVED_BODY");
                        values.Add("LIASON_NAME", $"{userEntity.Contact.FirstName} {userEntity.Contact.LastName}");
                        values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                        values.Add("EMAIL_FROM_NAME", "CRA");
                        values.Add("DECISION_NOTES", "");

                        users = _dataContext.GetRoleUsersForCHGSite(chgSiteId, "CEO").Union(_dataContext.GetRoleUsersForCHGSite(chgSiteId, "DBD")).Union(_dataContext.GetRoleUsersForCHGSite(chgSiteId, "AVP")).Distinct().ToList();

                        if (users != null)
                        {
                            foreach (var user in users)
                            {
                                _dataContext.CreateEmailAlert(user.UserId, values);
                                _dataContext.CreateNotification("Success", "Approved", $"Call cycle request for {userEntity.Contact.FirstName} {userEntity.Contact.LastName} approved.", user.UserId);
                            }
                        }
                    }



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
                                var task = _dataContext.CreateTask("Call Cycle Approval", user.UserId, "Created", "Call Cycle Approval Request");
                                _dataContext.SaveChanges();
                                _dataContext.UpdateTaskParameter(task.UserTaskId, "APPROVAL_LEVEL", approvalLevel);
                                _dataContext.UpdateTaskParameter(task.UserTaskId, "SCHEDULE_ID", schedule.ScheduleId.ToString());
                                _dataContext.UpdateTaskParameter(task.UserTaskId, "CHGSITE_ID", chgSiteId.ToString());

                                _dataContext.SaveChanges();

                                //Send task and alert.
                                values = new Dictionary<string, string>();
                                values.Add("TMP_SUBJECT", "CALL_CYCLE_APPROVAL_REQUEST_CREATED_SUBJECT");
                                values.Add("TMP_BODY", "CALL_CYCLE_APPROVAL_REQUEST_CREATED_BODY");
                                values.Add("EMAIL_FROM_ADDRESS", ConfigurationManager.AppSettings["SupportEmail"]);
                                values.Add("EMAIL_FROM_NAME", "CRA");
                                values.Add("LIASON_NAME", $"{userEntity.Contact.FirstName} {userEntity.Contact.LastName}");
                                values.Add("TASK_URL", ConfigurationManager.AppSettings["APP_URL"] + "/Task/Resolve/" + task.UserTaskId);


                                _dataContext.CreateEmailAlert(user.UserId, values);
                                _dataContext.CreateNotification("Info", "Request", $"New call cycle request for {userEntity.Contact.FirstName} {userEntity.Contact.LastName}.", user.UserId);

                                _dataContext.SaveChanges();
                            }
                        }

                    }


                }

                _dataContext.SaveChanges();

                transaction.Commit();

                var pendingSchedule = _dataContext.Schedules.Where(p => p.ScheduleId == schedule.ScheduleId && p.Deleted == false && p.Decided == false).SingleOrDefault();

                bool isPendingSchedule = false;

                if (pendingSchedule != null)
                {
                    isPendingSchedule = true;
                }
                return Json(new { Status = Constant.RESPONSE_OK, Description = "Schedule saved successfully.", ViewPending = isPendingSchedule, ScheduleId = schedule.ScheduleId });
            }


        }

        [HttpPost]
        public JsonResult ConfirmVisit(ConfirmVisitModel model)
        {

            if (model == null || model.ScheduleItemId <= 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid schedule data. Please correct the entries and try again." });
            }

            var date = DateTime.Parse(DateTime.Now.ToShortDateString());

            if (_dataContext.ScheduleVisits.Where(p => p.ScheduleItemId == model.ScheduleItemId && p.Deleted == false && p.Date == date).Count() > 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "The schedule item was already marked as visited." });
            }
            else
            {
                var scheduleVisit = new ScheduleVisit();
                scheduleVisit.ScheduleItemId = model.ScheduleItemId;
                scheduleVisit.Date = date;
                _dataContext.ScheduleVisits.Add(scheduleVisit);
                _dataContext.SaveChanges();

                return Json(new { Status = Constant.RESPONSE_OK, Description = "The schedule is marked as visited.", ScheduleItemId = model.ScheduleItemId, Visited = true, Date = date });
            }


        }

        public ActionResult PendingSchedule(long ScheduleId)
        {
            CallCycleModel model = new CallCycleModel();
            ViewBag.ScheduleId = ScheduleId;
            return View(model);
        }

        [HttpPost]
        public JsonResult UnplannedVisit(UnplannedVisitModel model)
        {
            if (model == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid request. One or more fields failed validation." });
            }

            if (string.IsNullOrEmpty(model.ContactType))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid request. One or more fields failed validation." });
            }

            if (_dataContext.UserId == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid request. One or more fields failed validation." });
            }


            if (model.ContactType == "Other")
            {
                model.ContactId = null;
                model.CHGSiteId = null;
                model.ReferralSourceId = null;
            }

            if (model.ContactType == "Contact")
            {
                if (model.CHGSiteId == null || model.ReferralSourceId == null || model.ContactId == null)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid request. One or more fields failed validation. Make sure to select site, referral source and contact." });
                }
            }

            if (model.ContactType == "Other" && string.IsNullOrEmpty(model.Notes))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid request. Please enter notes." });
            }

            UnplannedVisit entity = new UnplannedVisit()
            {
                CHGSiteId = model.CHGSiteId,
                ContactId = model.ContactId,
                ReferralSourceId = model.ReferralSourceId,
                ContactType = model.ContactType,
                Notes = model.Notes,
                UserId = _dataContext.UserId.Value,
                VisitDate = DateTime.Now
            };

            _dataContext.UnplannedVisits.Add(entity);
            _dataContext.SaveChanges();

            return Json(new { Status = Constant.RESPONSE_OK, Description = "The unplanned visit saved successfully." });
        }

        public JsonResult GetUnplannedActivity(long userId, string fromDate, string toDate)
        {
            DateTime fromDt;
            DateTime toDt;
            if (!DateTime.TryParse(fromDate, out fromDt))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid from date." }, JsonRequestBehavior.AllowGet);
            }

            if (!DateTime.TryParse(toDate, out toDt))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid to date." }, JsonRequestBehavior.AllowGet);
            }

            if (fromDt > toDt)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "From date cannot be greater than to date." }, JsonRequestBehavior.AllowGet);
            }

            var query = (from c in _dataContext.UnplannedVisits.OrderByDescending(p => p.VisitDate).OrderByDescending(p => p.UnplannedVisitId) where c.UserId == userId && c.Deleted == false && c.VisitDate >= fromDt && c.VisitDate <= toDt select new { VisitDate = c.VisitDate, FirstName = c.Contact.FirstName, LastName = c.Contact.LastName, ContactType = c.ContactType, ReferralSource = c.ReferralSource.FullName, Site = c.CHGSite.FullName, Notes = c.Notes }).ToList().Select(p => new { VisitDate = p.VisitDate.ToShortDateString(), ContactType = p.ContactType, Contact = $"{p.FirstName} {p.LastName}", Site = p.Site, ReferralSource = p.ReferralSource, Notes = p.Notes });
            return Json(new { Status = Constant.RESPONSE_OK, Items = query.ToList() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCurrentUnplannedActivity(string fromDate, string toDate)
        {
            long userId = 0;
            if (_dataContext.UserId != null)
            {
                userId = _dataContext.UserId.Value;
            }

            return GetUnplannedActivity(userId, fromDate, toDate);

        }


    }

}