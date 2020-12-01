using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class UserCHGSite : EntityBase
    {
        [Key]
        public long UserCHGSiteId { get; set; }

        [Index("IX_UserCHGSite_UserId_CHGSiteId",Order =1)]
        public long CHGSiteId { get; set; }

        [ForeignKey("CHGSiteId")]
        public virtual CHGSite CHGSite { get; set; }

        [Index("IX_UserCHGSite_UserId_CHGSiteId",Order =2)]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}
