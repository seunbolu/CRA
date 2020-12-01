using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class Patient : EntityBase
    {
        [Key]
        public long PatientId { get; set; }

        //[Required, MaxLength(Constant.NAME_LENGTH)]
        //public string FirstName { get; set; }

        //public string MiddleName { get; set; }

        //[Required, MaxLength(Constant.NAME_LENGTH)] 
        //public string LastName { get; set; }

        //[MaxLength(Constant.SSN_LENGTH)]
        //public string SSN { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime DateOfBirth { get; set; }

       


    }
}
