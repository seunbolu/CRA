using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class PreScreenData : CommonPatientData
    {
        [Key]
        public long PreScreenDataId { get; set; }

        public long PreScreenId { get; set; }

        [ForeignKey("PreScreenId")]
        public virtual PreScreen PreScreen { get; set; }

       




    }
}
