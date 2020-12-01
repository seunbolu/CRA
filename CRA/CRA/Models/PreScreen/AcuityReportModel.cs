using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.PreScreen
{
    public class AcuityReportModel
    {

        public List<AcuityReportItemModel> Items { get; set; }

        public AcuityReportModel()
        {
            Items = new List<AcuityReportItemModel>();
        }
    }
}