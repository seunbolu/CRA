using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class ReferralSourceCommercialPayorType : EntityBase
    {

        [Key]
        public long ReferralSourceCommercialPayorTypeId { get; set; }

        [Index("IX_ReferralSourceCommercialPayorType_ReferralSourceId_CommercialPayorTypeId",Order =1)]
        public long ReferralSourceId { get; set; }

        [Index("IX_ReferralSourceCommercialPayorType_ReferralSourceId_CommercialPayorTypeId",Order =2)]
        public long CommercialPayorTypeId { get; set; }


        [ForeignKey("ReferralSourceId")]
        public virtual ReferralSource ReferralSource { get; set; }

        [ForeignKey("CommercialPayorTypeId")]
        public virtual CommercialPayorType CommercialPayorType { get; set; }


    }
}
