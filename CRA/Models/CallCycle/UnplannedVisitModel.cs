using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.CallCycle
{
    public class UnplannedVisitModel
    {
        public string ContactType { get; set; }

        public long? ContactId { get; set; }

        public long? ReferralSourceId{ get; set; }

        public long? CHGSiteId { get; set; }

        public string Notes { get; set; }


    }
}