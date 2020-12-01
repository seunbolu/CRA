using CRA.Data.Tracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class EntityAuditBase
    {

        [NotTracked]
        public DateTime DateCreated { get; set; }

        [NotTracked]
        public DateTime? DateModified { get; set; }

        [Index]
       
        public bool Deleted { get; set; }
    }
}
