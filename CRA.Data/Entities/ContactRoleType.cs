using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class ContactRoleType : EntityBase
    {
        [Key]
        public long ContactRoleTypeId { get; set; }

        [MaxLength(Constant.NAME_LENGTH), Required]
        public string Name { get; set; }

    }
}
