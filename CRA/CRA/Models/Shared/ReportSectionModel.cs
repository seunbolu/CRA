using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class ReportSectionModel
    {

        public string Label { get; set; }
        public List<ReportItemModel> Items { get; set; }

        public bool DisplaySeparate { get; set; }

        public ReportSectionModel()
        {
            Items = new List<ReportItemModel>();
        }
    }
}