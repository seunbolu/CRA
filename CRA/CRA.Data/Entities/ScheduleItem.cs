using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class ScheduleItem : EntityBase
    {
        [Key]
        public long ScheduleItemId { get; set; }
        public long ScheduleId { get; set; }

        [ForeignKey("ScheduleId")]
        public virtual Schedule Schedule { get; set; }

        [Required,MaxLength(20)]
        public string Day { get; set; }

        [Required, MaxLength(20)]
        public string Time { get; set; }

        public int WeekNumber { get; set; }

        public long ContactId { get; set; }

        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

        public long CHGSiteId { get; set; }

        [ForeignKey("CHGSiteId")]
        public virtual CHGSite CHGSite { get; set; }
        public long ReferralSourceId { get; set; }

        [ForeignKey("ReferralSourceId")]
        public virtual ReferralSource ReferralSource { get; set; }

    }
}
