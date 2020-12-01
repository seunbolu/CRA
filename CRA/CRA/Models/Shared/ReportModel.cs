using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class ReportModel
    {

        public List<ReportSectionModel> Sections { get; set; }

        public ReportModel()
        {
            Sections = new List<ReportSectionModel>();
        }
    }
}