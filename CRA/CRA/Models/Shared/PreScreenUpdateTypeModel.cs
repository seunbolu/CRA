using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRA.Models.Shared
{
    public class PreScreenUpdateTypeModel
    {
        [Key]
        public long PreScreenUpdateTypeId { get; set; }

        [MaxLength(Constant.NAME_LENGTH), Required, Index(IsUnique = true), Display(Name = "CHG Service")]
        public string Name { get; set; }




    }
}