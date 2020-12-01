using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.CallCycle
{
    public class ScheduleItemModel
    {

        public long CHGSiteId { get; set; }
        public long ReferralSourceId { get; set; }
        public long ContactId { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }

        

    }
}