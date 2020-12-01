using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class PreScreenStatusLog : EntityBase
    {
        [Key] 
        public long PreScreenStatusLogId { get; set; }


        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }


        public long PreScreenId { get; set; }

        [ForeignKey("PreScreenId")]
        public virtual PreScreen PreScreen { get; set; }

        [MaxLength(100),Required]
        public string Status { get; set; }

    }
}
