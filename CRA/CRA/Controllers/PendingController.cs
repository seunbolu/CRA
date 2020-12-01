using CRA.Data;
using CRA.Data.Entities;
using CRA.Models.CHGSite;
using CRA.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{
    public class PendingController : BaseController
    {

        public ActionResult Index()
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
                    CHGSiteId = p.CHGSiteId

                }
                );

            var userSites = _dataContext.GetUserSites(_dataContext.UserId.Value);
            items = (from c in items join s in userSites on c.CHGSiteId equals s.CHGSiteId select c).ToList();

            return Json(items, JsonRequestBehavior.AllowGet);
        }


    }
}