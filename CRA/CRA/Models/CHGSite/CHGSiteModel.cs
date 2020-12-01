using CRA.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRA.Models.CHGSite
{
    public class CHGSiteModel
    {

        [Key, Display(Name = "CHG Site ID")]
        public long CHGSiteId { get; set; }

        [MaxLength(Constant.FULL_NAME_LENGTH), Required, Index(IsUnique = true), Display(Name = "Full Name")]
        public string FullName { get; set; }

        [MaxLength(Constant.SHORT_NAME_LENGTH), Display(Name = "Short Name")]
        public string ShortName { get; set; }

        public long RegionTypeId { get; set; }

        public string RegionName { get; set; }


        public long ServiceTypeId { get; set; }

        public string ServiceName { get; set; }

        public long? PreScreenUpdateTypeId { get; set; }

        public string PreScreenUpdateTypeName { get; set; }

        [ForeignKey("PreScreenUpdateTypeId")]
        public virtual PreScreenUpdateTypeModel PreScreenUpdateTypeModel { get; set; }

        [MaxLength(Constant.ADDRESS_LENGTH), Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Total Beds")]
        public int? BedCount { get; set; }
        public int? OperationalICUBedCount { get; set; }

        public string DateofReferral { get; set; }

    }
}