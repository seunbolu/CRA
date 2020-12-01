using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class Organization : EntityBase
    {
        [Key]
        public long OrganizationId { get; set; }

        [MaxLength(Constant.FULL_NAME_LENGTH), Required,Index]
        public string Name { get; set; }
    }
}
