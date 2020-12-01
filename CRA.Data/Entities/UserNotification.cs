using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class UserNotification : EntityBase
    {
        [Key]

        public long UserNotificationId { get; set; }


        public long? LastNotificationId { get; set; }

        [ForeignKey("LastNotificationId")]
        public virtual Notification LastNotification { get; set; }

        [Index(IsUnique =true)]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}
