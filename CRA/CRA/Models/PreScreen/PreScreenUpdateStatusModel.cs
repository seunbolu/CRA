using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.PreScreen
{
    public class PreScreenUpdateStatusModel
    {

        public string AdmissionNotes { get; set; }

        public string AdmissionStatus { get; set; }

        public string DenialReason { get; set; }

        public string NonAdmitReason { get; set; }

        public string AuthReference { get; set; }

        public bool  Agree { get; set; }
    }
}