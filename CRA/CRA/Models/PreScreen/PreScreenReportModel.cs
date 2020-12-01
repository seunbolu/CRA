using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.PreScreen
{
    public class PreScreenReportModel
    {

        public List<PreScreenReportSectionModel> Sections { get; set; }

        public PreScreenReportModel()
        {
            Sections = new List<PreScreenReportSectionModel>();
        }
    }
}