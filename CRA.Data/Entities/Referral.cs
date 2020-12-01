using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class Referral : EntityBase
    {
        [Key]
        public long ReferralId { get; set; }

        public long LiaisonId { get; set; }

        [ForeignKey("LiaisonId")]
        public virtual User Liaison { get; set; }

        public long CHGSiteId { get; set; }

        [ForeignKey("CHGSiteId")]
        public virtual CHGSite CHGSite { get; set; }

        public long? ReferralContactId { get; set; }

        [ForeignKey("ReferralContactId")]
        public virtual Contact ReferralContact { get; set; }

        public long ReferralSourceId { get; set; }

        [ForeignKey("ReferralSourceId")]
        public virtual ReferralSource ReferralSource  { get; set; }

     
    
        public long? PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }


        public long PreScreenId { get; set; }

        [ForeignKey("PreScreenId")]
        public virtual PreScreen PreScreen { get; set; }


    }
}
