using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class ContactAttribute : EntityBase
    {
        [Key]
        public long ContactAttributeId { get; set; }

        [Index(IsUnique =true)]
        public long ContactId { get; set; }

        [ForeignKey("ContactId")]
        public virtual Contact Contact  { get; set; }

        public long CategoryTypeId { get; set; }

        [ForeignKey("CategoryTypeId")]
        public CategoryType CategoryType { get; set; }

        [MaxLength(Constant.ADDRESS_LENGTH)]
        public string Location { get; set; }

        [MaxLength(Constant.NOTE_LENGTH)]
        public string Note { get; set; }

        public long ContactRoleTypeId { get; set; } 

        [ForeignKey("ContactRoleTypeId")]
        public virtual ContactRoleType ContactRoleType { get; set; }

        public long? SpecialityTypeId { get; set; }
        public virtual SpecialityType SpecialityType { get; set; }




    }
}
