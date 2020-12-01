using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class CHGSiteReferralSource : EntityBase
    {
        [Key]
        public long CHGSiteReferralSourceId { get; set; }

        [Index("IX_CHGSiteReferralSource_CHGSiteId_ReferralSourceId", Order = 1)]
        public long ReferralSourceId { get; set; }

        [ForeignKey("ReferralSourceId")]
        public virtual ReferralSource ReferralSource { get; set; }

        [Index("IX_CHGSiteReferralSource_CHGSiteId_ReferralSourceId", Order = 2)]
        public long CHGSiteId { get; set; }

        [ForeignKey("CHGSiteId")]
        public virtual CHGSite CHGSite { get; set; }

    }
}
