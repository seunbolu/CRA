using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class Document : EntityBase
    {
        [Key]
        public long DocumentId { get; set; }

        [MaxLength(Constant.FILE_NAME_LENGTH), Required]
        public string Name { get; set; } 

        [MaxLength(Constant.SHORT_NAME_LENGTH)]
        public string Extension { get; set; }

        public long ContentId { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

    }
}
