using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class RegionServiceType : EntityBase
    {

        [Key]
        public long RegionServiceId { get; set; }

        [Index("IX_RegionService_RegionTypeId_ServiceTypeId",Order =1)]
        public long RegionTypeId { get; set; }

        [Index("IX_RegionService_RegionTypeId_ServiceTypeId",Order =2)]
        public long ServiceTypeId { get; set; }


        [ForeignKey("RegionTypeId")]
        public virtual RegionType RegionType { get; set; }

        [ForeignKey("ServiceTypeId")]
        public virtual ServiceType ServiceType { get; set; }


    }
}
