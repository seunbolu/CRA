using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class ReferralSource : EntityBase
    {
        [Key]
        public long ReferralSourceId { get; set; }

        [MaxLength(Constant.FULL_NAME_LENGTH), Required, Index]
        public string FullName { get; set; }

        [MaxLength(Constant.SHORT_NAME_LENGTH)]
        public string ShortName { get; set; }
        public long ReferralSourceTypeId { get; set; }

        [ForeignKey("ReferralSourceTypeId")]
        public virtual ReferralSourceType ReferralSourceType { get; set; }

        public int? BedCount { get; set; }
        public int? OperationalICUBedCount { get; set; }
        public int? ICUBedCount { get; set; }

        public int? CCUBedCount { get; set; }

        public int? SICUBedCount { get; set; }

        public int? MICUBedCount { get; set; }

        public int? NeuroICUBedCount { get; set; }

        public int? RehabBedCount { get; set; }

        public int? SkilledNursingBedCount { get; set; }

        public int? LTACHBedCount { get; set; }

        public bool ConfirmedBedsCodingICURevCodes { get; set; }

        public bool IsApproved { get; set; }
    }
}
