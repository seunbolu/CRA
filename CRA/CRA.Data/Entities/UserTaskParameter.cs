using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class UserTaskParameter : EntityBase
    {
        [Key]
        public long UserTaskParameterId { get; set; }


        public long UserTaskId { get; set; }

        [ForeignKey("UserTaskId")]
        public virtual UserTask UserTask { get; set; }

        [MaxLength(200), Required]
        public string Key { get; set; }

        public string Value { get; set; }

    }
}
