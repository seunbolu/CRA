using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class ReferralSourceElectronicReferralType : EntityBase
    {

        [Key]
        public long ReferralSourceElectronicReferralTypeId { get; set; }

        [Index("IX_ReferralSourceElectronicReferralType_ReferralSourceId_ElectronicReferralTypeId",Order =1)]
        public long ReferralSourceId { get; set; }

        [Index("IX_ReferralSourceElectronicReferralType_ReferralSourceId_ElectronicReferralTypeId",Order =2)]
        public long ElectronicReferralTypeId { get; set; }


        [ForeignKey("ReferralSourceId")]
        public virtual ReferralSource ReferralSource { get; set; }

        [ForeignKey("ElectronicReferralTypeId")]
        public virtual ElectronicReferralType ElectronicReferralType { get; set; }


    }
}
