using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class UserTask : EntityBase
    {
        [Key]
        public long UserTaskId { get; set; }

        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }


        [MaxLength(100), Required,Index]
        public string TaskType { get; set; }


        [MaxLength(100), Required, Index]
        public string Status { get; set; }

        [MaxLength(100),  Index]
        public string SubStatus { get; set; }

        [MaxLength(Constant.SHORT_DESCRIPTION_LENGTH)]
        public string Description { get; set; }


        public string Notes { get; set; }

        public string UserNotes { get; set; }
    }
}
