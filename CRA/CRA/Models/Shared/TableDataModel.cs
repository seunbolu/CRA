using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class TableDataModel
    {
        public string ClientCode { get; set; }
        public string[] HeaderCodes { get; set; }
        public string[][] CellValues { get; set; }
    }
}