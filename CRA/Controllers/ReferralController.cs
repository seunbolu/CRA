using CRA.Data;
using CRA.Data.Entities;
using CRA.Models.CHGSite;
using CRA.Models.Referral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{
    public class ReferralController : BaseController
    {
        public ActionResult Create()
        {
            return View();
        }

        

        public JsonResult AutoCompleteReferrals(string term)
        {

            var query1 = (from c in _dataContext.PatientData
                          join d in _dataContext.PatientData on c.PatientId equals d.PatientId
                          where c.Deleted == false
                          && d.Deleted == false
                          &&
                          (
                          (c.SectionCode == "demographics.general" && c.ItemCode == "firstname" && c.Value.StartsWith(term))
                          || (c.SectionCode == "demographics.general" && c.ItemCode == "lastname" && c.Value.StartsWith(term))
                          || (c.SectionCode == "demographics.general" && c.ItemCode == "middleinitial" && c.Value.StartsWith(term))
                          || (c.SectionCode == "payorinfo.primary.all" && c.ItemCode == "ssn" && c.Value.StartsWith(term))
                          )

                          select d).Distinct().ToList();

            List<long> patientIds = query1.Select(p => p.PatientId).Distinct().ToList();

            var items = patientIds.Select(p => new
            {
                PatientId = p,
                FirstName = GetPatientData(p, "demographics.general", "firstname", query1),
                LastName = GetPatientData(p, "demographics.general", "lastname", query1),
                MiddleName = GetPatientData(p, "demographics.general", "middleinitial", query1),
                DOB = GetPatientData(p, "demographics.general", "dob", query1),
                SSN = GetPatientData(p, "payorinfo.primary.all", "ssn", query1)
            }).ToList();


            List<object> items1 = new List<object>();
            foreach (var item in items)
            {

                items1.Add(new
                {
                    label = string.Format(@"{3} - {0} {1} {2} - {4:D}", item.FirstName, item.MiddleName, item.LastName, item.SSN, item.DOB),
                    value = item.PatientId,
                    firstName = item.FirstName,
                    middleName = item.MiddleName,
                    lastName = item.LastName,
                    ssn = item.SSN,
                    dob = item.DOB
                });
            }

            return Json(items1, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public JsonResult Referrals(long? siteId, string status)
        {
            var query = _dataContext.Referrals
                .Include("CHGSite")
                .Include("ReferralContact")
                .Include("ReferralSource")
                .Include("Patient")
                .Include("Liaison")
                .Include("Liaison.Contact")
                .Include("PreScreen")
                .Where(p => p.Deleted == false)
                .AsQueryable();

            if (siteId != null && siteId > 0)
            {
                query = query.Where(p => p.CHGSiteId == siteId);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.PreScreen.Status == status);
            }

            var patientData = (from c in _dataContext.PatientData join d in query on c.PatientId equals d.PatientId select c).ToList();

            var items = query
                .ToList()
                .Select(p =>
                new
                {
                    ReferralId = p.ReferralId,
                    PatientName = string.Format("{0} {1}", GetPatientData(p.PatientId.Value, "demographics.general", "firstname", patientData), GetPatientData(p.PatientId.Value, "demographics.general", "lastname", patientData)),
                    SiteName = p.CHGSite.FullName,
                    ReferralSourceName = p.ReferralSource.FullName,
                    LiaisonName = string.Format("{0} {1}", p.Liaison.Contact.FirstName, p.Liaison.Contact.LastName),
                    ContactName = string.Format("{0} {1}", p.ReferralContact.FirstName, p.ReferralContact.LastName),
                    StatusTypeName = p.PreScreen.Status,
                    PatientId = p.PatientId,
                    PreScreenId = p.PreScreenId,
                    CHGSiteId = p.CHGSiteId,
                    

                 

                }
                );
            var userSites = _dataContext.GetUserSites(_dataContext.UserId.Value);
            items = (from c in items join s in userSites on c.CHGSiteId equals s.CHGSiteId select c).ToList();

            return Json(items, JsonRequestBehavior.AllowGet);
        }

       
        public JsonResult SiteLiaisons(long siteId)
        {
            var liaisons = (
                from c in _dataContext.UserCHGSites
                join u in _dataContext.Users on c.UserId equals u.UserId
                join r in _dataContext.UserRoles on u.UserId equals r.UserId
                where c.CHGSite.CHGSiteId == siteId && r.UserRoleType.Name == "CL" && c.Deleted == false && u.Deleted == false && r.Deleted == false
                select new { UserId = u.UserId, FullName = u.Contact.FirstName + " " + u.Contact.LastName }
                ).ToList();

            return Json(liaisons, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteReferralSources(long? siteId, string term)
        {
            var query = (from c in _dataContext.ReferralSources.Include("ReferralSourceType") where c.Deleted == false && c.IsApproved==true select c);

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(p => p.FullName.StartsWith(term) || p.ShortName.StartsWith(term));
            }

            query = query.Intersect((from c in _dataContext.CHGSiteReferralSources where c.CHGSiteId == siteId && c.Deleted == false select c.ReferralSource));

            List<object> items = new List<object>();
            foreach (var entity in query.ToList())
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
        public JsonResult AutoCompleteReferralSourceContacts(long? referralSourceId, string term)
        {
            var query = (from c in _dataContext.ContactReferralSources.Include("Contact") where c.ReferralSourceId == referralSourceId && c.Deleted == false select c);

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(p => p.Contact.FirstName.StartsWith(term) || p.Contact.LastName.StartsWith(term));
            }
            
            

            List<object> items = new List<object>();
            foreach (var entity in query.ToList())
            {
                items.Add(new
                {
                    label = string.Format(@"{0} - {1} {2}", entity.Contact.Email, entity.Contact.FirstName, entity.Contact.LastName),
                    value = entity.ContactId,
                    ContactId = entity.ContactId,
                    FirstName = entity.Contact.FirstName,
                    LastName = entity.Contact.LastName
                });
            }
           

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(CreateReferralModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CHGSiteId <= 0 || model.ReferralSourceId <= 0 || model.ContactId <= 0 || model.LiaisonId <= 0)
                {
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });
                }

                if (!string.IsNullOrEmpty(model.SSN) && model.PatientId <= 0)
                {
                    if (_dataContext.PatientData.Where(p => p.SectionCode == "payorinfo.primary.all" && p.ItemCode == "ssn" && p.Value == model.SSN && p.Deleted == false).Count() > 0)
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "The patient with the entered social security number already exists." });
                    }
                }

                using (var transaction = _dataContext.Database.BeginTransaction())
                {

                    if (model.PatientId <= 0)
                    {
                        Patient patient = new Patient();


                        _dataContext.Patients.Add(patient);
                        _dataContext.SaveChanges();

                        _dataContext.PatientData.Add(GetNewPatientDataEntity("payorinfo.primary.all", "ssn", model.SSN, "SSN", "Social Security Number", patient.PatientId));
                        _dataContext.PatientData.Add(GetNewPatientDataEntity("demographics.general", "dob", model.DOB == null ? null : model.DOB.ToShortDateString(), "Date", "Date of Birth", patient.PatientId));
                        _dataContext.PatientData.Add(GetNewPatientDataEntity("demographics.general", "firstname", model.FirstName, "Text", "First Name", patient.PatientId));
                        _dataContext.PatientData.Add(GetNewPatientDataEntity("demographics.general", "middleinitial", model.MiddleName, "Text", "Middle Initial", patient.PatientId));
                        _dataContext.PatientData.Add(GetNewPatientDataEntity("demographics.general", "lastname", model.LastName, "Text", "Last Name", patient.PatientId));
                  

                        model.PatientId = patient.PatientId;

                    }


                    var preScreenTypeId = _dataContext.UserPreScreens.Where(p => p.UserId == model.LiaisonId && p.Deleted == false).Take(1).Single().PreScreenTypeId;


                    var preScreen = new PreScreen()
                    {
                        PatientId = model.PatientId,
                        PreScreenTypeId = preScreenTypeId,
                        Status = "Created",
                        LastSubmitted = DateTime.Now

                    };

                    _dataContext.PreScreens.Add(preScreen);
                    _dataContext.SaveChanges();

                  

                    var referral = new Referral()
                    {

                        CHGSiteId = model.CHGSiteId,

                        ReferralContactId = model.ContactId,
                        LiaisonId = model.LiaisonId,
                        PatientId = model.PatientId,
                        ReferralSourceId = model.ReferralSourceId,

                        PreScreenId = preScreen.PreScreenId
                    };

                    _dataContext.Referrals.Add(referral);


                    _dataContext.SaveChanges();

                    _dataContext.PreScreenData.Add(GetNewPreScreenDataEntity("referralsourceinfo.general", "chgdestination", model.CHGSiteId.ToString(), "Dropdown", "CHG Destination", preScreen.PreScreenId));
                    _dataContext.PreScreenData.Add(GetNewPreScreenDataEntity("referralsourceinfo.general", "dateofreferral", referral.DateCreated.ToShortDateString(), "Date", "Date of Referral", preScreen.PreScreenId));
                    _dataContext.PreScreenData.Add(GetNewPreScreenDataEntity("referralsourceinfo.general", "liaison", model.LiaisonId.ToString(), "Dropdown", "Name of Liaison", preScreen.PreScreenId));


                    _dataContext.SaveChanges();
                    transaction.Commit();

                    return Json(new { Status = Constant.RESPONSE_OK, Description = "Referral created successfully.", PatientId = model.PatientId, ReferralId = referral.ReferralId, PreScreenId = referral.PreScreenId });

                }
            }

            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });

        }


        

        public JsonResult GetUserSites()
        {
            var items = _dataContext.GetUserSites(_dataContext.UserId.Value);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
     
        
         
    }

}