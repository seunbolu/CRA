using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class Notification : EntityBase
    {
        [Key]
        public long NotificationId { get; set; }

        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        [MaxLength(100)]
        public string Category { get; set; }


    }
}
