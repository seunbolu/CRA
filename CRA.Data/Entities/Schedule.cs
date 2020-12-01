using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class Schedule : EntityBase
    {
        [Key]
        public long ScheduleId { get; set; }

        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required, MaxLength(30)]
        public string WeekType { get; set; }


        public string Notes { get; set; }

        //public bool Active { get; set; }

        public bool Decided { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ActivationDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeactivationDate { get; set; }

    }
}
