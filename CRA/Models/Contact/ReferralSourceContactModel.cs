using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Contact
{
    public class ReferralSourceContactModel
    {

        public long ReferralSourceId { get; set; }

        public string  FullName { get; set; }

        public string ShortName { get; set; }

        public string ReferralSourceTypeName { get; set; }


    }
}