using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Report
{
    public class ScheduleReportModel
    {
        public long ScheduleId { get; set; }

        public long UserId { get; set; }

        public long RegionId { get; set; }

        public long ServiceId { get; set; }

        public long CHGSiteId { get; set; }

        public long ReferralSourceId { get; set; }

        public long ContactId { get; set; }

        public string Day { get; set; }



    }
}