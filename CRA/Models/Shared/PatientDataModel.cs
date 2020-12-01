using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class PatientDataModel 
    {

        public long ContextId { get; set; }
        public string Context { get; set; }
        public string SectionCode { get; set; }
        public long PreScreenId { get; set; }


        public List<KeyValuePairModel> Items { get; set; }

        public List<TableDataModel> Tables { get; set; }

    }
}