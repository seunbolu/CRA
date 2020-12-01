using CRA.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.PreScreen
{
    public class PreScreenModel
    {
        public List<TabModel> Tabs { get; set; }

        public long? PendingTaskId { get; set; }

        public string ResolveLocation { get; set; }

        public string AdmissionStatus { get; set; }
        public PreScreenModel()
        {
            Tabs = new List<TabModel>();
        }

    }
}