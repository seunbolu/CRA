using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class UnplannedVisit : EntityBase
    {
        [Key]
        public long UnplannedVisitId { get; set; }

        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [DataType(DataType.Date), Index]
        public DateTime VisitDate { get; set; }

        [MaxLength(50), Required]
        public string ContactType { get; set; }

        public long? CHGSiteId { get; set; }

        [ForeignKey("CHGSiteId")]
        public virtual CHGSite CHGSite { get; set; }

        public long? ReferralSourceId { get; set; }

        [ForeignKey("ReferralSourceId")]
        public virtual ReferralSource ReferralSource { get; set; }

        public long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

        public string Notes { get; set; }


      


    }
}
