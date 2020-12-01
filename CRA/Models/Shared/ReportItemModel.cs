using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class ReportItemModel
    {

        public string Type { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public TableDataModel Table { get; set; }

        public string[] HeaderList { get; set; }


    }
}