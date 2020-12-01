using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class EmailCc : EntityBase
    {
        [Key]
        public long EmailCcId { get; set; }

        [Index]
        public long EmailId { get; set; }
         
        [ForeignKey("EmailId")]
        public virtual Email Email { get; set; }


        [MaxLength(Constant.FULL_NAME_LENGTH), Required]
        public string Name { get; set; }

        [MaxLength(Constant.EMAIL_LENGTH), Required]
        public string Address { get; set; }

     
    }
}
