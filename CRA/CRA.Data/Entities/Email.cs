using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class Email : EntityBase
    {
        [Key]
        public long EmailId { get; set; }

        [MaxLength(Constant.FULL_NAME_LENGTH), Required]
        public string FromName { get; set; }

        [MaxLength(Constant.EMAIL_LENGTH), Required]
        public string FromAddress { get; set; }

        [MaxLength(Constant.SUBJECT_LENGTH)]
        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsHtml { get; set; }


        public string ErrorMessage { get; set; }
        [Index]
        public long EmailStatusTypeId { get; set; }



        [ForeignKey("EmailStatusTypeId")]
        public virtual EmailStatusType EmailStatusType { get; set; } 

    }
}
