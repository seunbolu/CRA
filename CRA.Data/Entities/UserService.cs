using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class UserService : EntityBase
    {
        [Key]
        public long UserServiceId { get; set; }

        [Index("IX_UserService_UserId_ServiceTypeId",Order =1)]
        public long ServiceTypeId { get; set; }

        [ForeignKey("ServiceTypeId")]
        public virtual ServiceType ServiceType { get; set; }

        [Index("IX_UserService_UserId_ServiceTypeId",Order =2)]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}
