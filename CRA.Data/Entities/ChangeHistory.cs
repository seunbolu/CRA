using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class ChangeHistory 
    {

        [Key]
        public long ChangeHistoryId { get; set; }

        [Index]
        public long ChangeSetId { get; set; }

        [ForeignKey("ChangeSetId")]
        public virtual ChangeSet ChangeSet { get; set; }

        [Index]
        public long EntityKey { get; set; }

        [MaxLength(200), Required]
        [Index]
        public string Entity { get; set; }

        [MaxLength(200), Required]
        [Index]
        public string Field { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        [MaxLength(200), Required]
        public string Operation { get; set; }



    }
}
