using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRA.Models.ReferralSource
{
    public class ReferralSourceModel
    {
 
        public long ReferralSourceId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string ShortName { get; set; }
        public long ReferralSourceTypeId { get; set; }

        public string ReferralSourceTypeName { get; set; }
    
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


        public List<ReferralSourceCHGSiteModel> Sites { get; set; }

        public List<ReferralSourceElectronicReferralTypeModel> ElectronicReferralTypes { get; set; }

        public List<ReferralSourceCommercialPayorTypeModel> CommercialPayorTypes { get; set; }

        public List<ReferralSourceManagedMedicarePayorTypeModel> ManagedMedicarePayorTypes { get; set; }
    }
}