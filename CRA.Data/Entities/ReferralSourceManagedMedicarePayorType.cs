using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class ReferralSourceManagedMedicarePayorType : EntityBase
    {

        [Key]
        public long ReferralSourceManagedMedicarePayorTypeId { get; set; }

        [Index("IX_ReferralSourceManagedMedicarePayorType_ReferralSourceId_ManagedMedicarePayorTypeId",Order =1)]
        public long ReferralSourceId { get; set; }

        [Index("IX_ReferralSourceManagedMedicarePayorType_ReferralSourceId_ManagedMedicarePayorTypeId",Order =2)]
        public long ManagedMedicarePayorTypeId { get; set; }


        [ForeignKey("ReferralSourceId")]
        public virtual ReferralSource ReferralSource { get; set; }

        [ForeignKey("ManagedMedicarePayorTypeId")]
        public virtual ManagedMedicarePayorType ManagedMedicarePayorType { get; set; }


    }
}
