using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRA.Models.Referral
{
    public class CreateReferralModel
    {

        public long PatientId { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }
        public DateTime DOB { get; set; }

        public string SSN { get; set; }
        public long CHGSiteId { get; set; }
        public long ReferralSourceId { get; set; }
        public long ContactId { get; set; }
        public long LiaisonId { get; set; }

        


    }
}