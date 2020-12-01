using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class CHGSite : EntityBase
    {
        [Key]
        public long CHGSiteId { get; set; }

        [MaxLength(Constant.FULL_NAME_LENGTH), Required, Index]
        public string FullName { get; set; }

        [MaxLength(Constant.SHORT_NAME_LENGTH)]
        public string ShortName { get; set; }

        public long RegionTypeId { get; set; }

        [ForeignKey("RegionTypeId")]
        public virtual RegionType RegionType { get; set; }

        public long ServiceTypeId { get; set; }

        [ForeignKey("ServiceTypeId")]
        public virtual ServiceType ServiceType { get; set; }

        public long? PreScreenUpdateTypeId { get; set; }

        [ForeignKey("PreScreenUpdateTypeId")]
        public virtual PreScreenUpdateType PreScreenUpdateType { get; set; }


        [Required,MaxLength(Constant.ADDRESS_LENGTH)]
        public string Address { get; set; }

        public int? BedCount { get; set; }
        public int? OperationalICUBedCount { get; set; }
    }
}
