using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class PreScreen : EntityBase
    {
        [Key] 
        public long PreScreenId { get; set; }

        public long PatientId { get; set; }

        public virtual Patient Patient  { get; set; }

        public long PreScreenTypeId { get; set; }

        public virtual PreScreenType PreScreenType { get; set; }

        public DateTime? LastSubmitted { get; set; }

        [MaxLength(100),Required]
        public string Status { get; set; }

        public bool VerificationComplete { get; set; }

        public string AdmissionType { get; set; }

        public string AdmissionNotes { get; set; }

        public string AdmissionStatus { get; set; }

        public bool PendingCEOApproval { get; set; }


        public string DenialReason { get; set; }

        public string NonAdmitReason { get; set; }

        public string AuthReference { get; set; }

        public string CEOApprovalRequestNotes { get; set; }


        public bool SCARequested { get; set; }

        public bool PlanInGrace { get; set; }

    }
}
