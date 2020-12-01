using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class UserPreScreen : EntityBase
    {
        [Key]
        public long UserPreScreenId { get; set; }

        [Index("IX_UserService_UserId_PreScreenTypeId",Order =1)]
        public long PreScreenTypeId { get; set; }

        [ForeignKey("PreScreenTypeId")]
        public virtual PreScreenType PreScreenType { get; set; }

        [Index("IX_UserService_UserId_PreScreenTypeId",Order =2)]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}
