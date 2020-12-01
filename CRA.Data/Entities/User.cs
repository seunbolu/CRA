using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class User : EntityAuditBase
    {
        [Key]
        public long UserId { get; set; } 

        [MaxLength(Constant.DOMAIN_NAME_LENGTH), Required, Index("IX_User_Domain_UserName", Order = 1)]
        public string DomainName { get; set; }

        [MaxLength(Constant.USER_NAME_LENGTH), Required, Index("IX_User_Domain_UserName", Order = 2)]
        public string UserName { get; set; }

        [Index]
        public long ContactId { get; set; }

        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

        [Index]
        public bool Enabled { get; set; }

     

    }
}
