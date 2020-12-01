using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class ScheduleVisit : EntityBase
    {
        [Key]
        public long ScheduleVisitId { get; set; }
        public long ScheduleItemId { get; set; }

        [ForeignKey("ScheduleItemId")]
        public virtual ScheduleItem ScheduleItem { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


    }
}
