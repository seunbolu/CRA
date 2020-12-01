using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class CommonPatientData : EntityBase
    {
        [MaxLength(300), Required, Index]
        public string SectionCode { get; set; }

        [MaxLength(300), Required, Index]
        public string ItemCode { get; set; }

        [MaxLength(300), Required]
        public string Type { get; set; }

        [MaxLength(500), Required]
        public string Label { get; set; }
        public string Value { get; set; }

        [MaxLength(300)]
        public string DependsOnCode { get; set; }


        [MaxLength(300)]
        public string DependsOnAssertValue { get; set; }

    }
}
