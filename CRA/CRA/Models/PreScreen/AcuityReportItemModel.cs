using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.PreScreen
{
    public class AcuityReportItemModel
    {

        public string Section { get; set; }
        public string SectionLabel { get; set; }

        public string ItemCode { get; set; }

        public string Label { get; set; }

        public string Category { get; set; }

        public bool Positive { get; set; }

        public string Value { get; set; }

        public string AssertValue { get; set; }


    }
}