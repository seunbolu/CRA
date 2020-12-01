using CRA.Data;
using CRA.Data.Entities;
using CRA.Models.CHGSite;
using CRA.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{
    public class ReportController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }


        private IQueryable<Schedule> GetScheduleBaseQuery(DateTime fromDate, DateTime toDate)
        {

            var query = (from c in _dataContext.Schedules
                         where c.Deleted == false
                         && c.ActivationDate != null
                         &&
                         (
                         (c.ActivationDate >= fromDate && c.ActivationDate <= toDate)
                       ||
                         (c.ActivationDate < fromDate && (c.DeactivationDate > fromDate || c.DeactivationDate == null))
                         )
                         select c).OrderBy(p => p.ScheduleId);

            return query;
        }

        public JsonResult GetScheduleReport(string fromDate, string toDate)
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
            

            //Get the list of schedules for the activation date.
            var schedules = GetScheduleBaseQuery(fromDt, toDt);

            //Get the list of all the schedule items to run the report.
            var scheduleItems = (from s in schedules
                                 join si in _dataContext.ScheduleItems on s.ScheduleId equals si.ScheduleId
                                 join sv in _dataContext.ScheduleVisits on si.ScheduleItemId equals sv.ScheduleItemId into lj
                                 from x in lj.DefaultIfEmpty()
                                 where si.Deleted == false
                                 select new { Schedule = s, ScheduleItem = si, ScheduleVisit = x }).ToList();

            var sites = (from c in _dataContext.CHGSites select c).ToList();


            List<DateTime> dates = new List<DateTime>();
            DateTime dt = fromDt;
            while (dt <= toDt)
            {
                dates.Add(dt);
                dt = dt.AddDays(1);
            }
            

            var dateItems = dates.Select(p => new { Date = p, Day = p.DayOfWeek.ToString() });


            var reportItems = (from c in scheduleItems
                               join d in dateItems on c.ScheduleItem.Day equals d.Day
                               select new
                               {
                                   ScheduleId = c.Schedule.ScheduleId,
                                   ScheduleItemId = c.ScheduleItem.ScheduleItemId,
                                   ServiceId = sites.Where(p => p.CHGSiteId == c.ScheduleItem.CHGSiteId).Single().ServiceTypeId,
                                   RegionId = sites.Where(p => p.CHGSiteId == c.ScheduleItem.CHGSiteId).Single().RegionTypeId,
                                   CHGSiteId = c.ScheduleItem.CHGSiteId,
                                   ReferralSourceId = c.ScheduleItem.ReferralSourceId,
                                   ContactId = c.ScheduleItem.ContactId,
                                   UserId = c.Schedule.UserId,
                                   Day = c.ScheduleItem.Day,
                                   Date = d.Date,
                                   ScheduleStart = c.Schedule.ActivationDate,
                                   ScheduleEnd = c.Schedule.DeactivationDate,
                                   VisitDate = c.ScheduleVisit == null ? null : (DateTime?)c.ScheduleVisit.Date

                               }).Where(p => (p.Date >= p.ScheduleStart && p.Date <= p.ScheduleEnd) || (p.Date >= p.ScheduleStart && p.ScheduleEnd == null))
                               .OrderBy(p => p.Date).ToList();
              


            return Json(new { Status = Constant.RESPONSE_OK, Items = reportItems }, JsonRequestBehavior.AllowGet);

            
            
        
        }


    }

}