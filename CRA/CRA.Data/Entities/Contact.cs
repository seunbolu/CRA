using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class Contact : EntityBase
    {
        [Key]
        public long ContactId { get; set; }

        [MaxLength(Constant.NAME_LENGTH), Required, Index]
        public string FirstName { get; set; }

        [MaxLength(Constant.NAME_LENGTH), Required]
        public string LastName { get; set; }

        [MaxLength(Constant.EMAIL_LENGTH), Index]
        public string Email { get; set; }

        [MaxLength(Constant.PHONE_LENGTH)]
        public string Phone { get; set; }

        [MaxLength(Constant.PHONE_LENGTH)]
        public string Mobile { get; set; }

        public bool IsApproved { get; set; }



    }
}
