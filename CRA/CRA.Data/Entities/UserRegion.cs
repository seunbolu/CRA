using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class UserRegion : EntityBase
    {
        [Key]
        public long UserRegionId { get; set; }

        [Index("IX_UserRegion_UserId_RegionTypeId",Order =1)]
        public long RegionTypeId { get; set; }

        [ForeignKey("RegionTypeId")]
        public virtual RegionType RegionType { get; set; }

        [Index("IX_UserRegion_UserId_RegionTypeId",Order =2)]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}
