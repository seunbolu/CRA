using CRA.Data.Tracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class EntityBase : EntityAuditBase
    {

        [NotTracked]
        public long? CreatedByUserId { get; set; }

        [NotTracked]
        public long? ModifiedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("ModifiedByUserId")]
        public virtual User ModifiedByUser { get; set; }

      


    }
}
