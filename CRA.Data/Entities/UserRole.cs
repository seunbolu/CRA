using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class UserRole : EntityBase
    {
        [Key]
        public long UserRoleId { get; set; }

        [Index("IX_UserRole_UserId_UserRoleTypeId",Order =1)]
        public long UserRoleTypeId { get; set; }

        [ForeignKey("UserRoleTypeId")]
        public virtual UserRoleType UserRoleType { get; set; }

        [Index("IX_UserRole_UserId_UserRoleTypeId",Order =2)]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}
