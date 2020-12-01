using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class ContactReferralSource : EntityBase
    {
        [Key]
        public long ContactReferralSourceId { get; set; }

        [Index("IX_ContactReferralSource_ContactId_ReferralSourceId", Order =1)]
        public long ReferralSourceId { get; set; }

        [ForeignKey("ReferralSourceId")]
        public virtual ReferralSource ReferralSource { get; set; }

        [Index("IX_ContactReferralSource_ContactId_ReferralSourceId",  Order = 2)]
        public long ContactId { get; set; }

        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

    }
}
