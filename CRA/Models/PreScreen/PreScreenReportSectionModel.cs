using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.PreScreen
{
    public class PreScreenReportSectionModel
    {
        public string Label { get; set; }

        public List<PreScreenReportSectionItemModel> Items { get; set; }

        public PreScreenReportSectionModel()
        {
            Items = new List<PreScreenReportSectionItemModel>();
        }

    }
}