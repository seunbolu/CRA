using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRA.Models.Contact
{
    public class ContactModel
    {
        public long ContactId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public long CategoryTypeId { get; set; }

        public string  CategoryName { get; set; }


        public string Location { get; set; }

        public string Note { get; set; }

        public string ContactRoleTypeName { get; set; }
        public long? SpecialityTypeId { get; set; }

        public string ReferalSource { get; set; }

        public string ReferalSourceType { get; set; }




        public string SpecialityTypeName { get; set; }

        public bool IsApproved { get; set; }
        public List<ReferralSourceContactModel> ReferralSources { get; set; }

    }
}