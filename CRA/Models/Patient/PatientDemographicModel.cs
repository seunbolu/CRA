using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRA.Models.Patient
{
    public class PatientDemographicModel
    {

        [Required, MaxLength(Constant.NAME_LENGTH)]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required, MaxLength(Constant.NAME_LENGTH)]
        public string LastName { get; set; }

        [MaxLength(Constant.SSN_LENGTH)]
        public string SSN { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(Constant.ADDRESS_LENGTH)]
        public string Address { get; set; }

        [MaxLength(Constant.ADDRESS_LENGTH)]
        public string City { get; set; }

       
        public long StateTypeId { get; set; }

        [MaxLength(Constant.CODE_LENGTH)]
        public string ZipCode { get; set; }
        public long CountryTypeId { get; set; }
        public string Phone { get; set; }
        public long GenderTypeId { get; set; }
        public long? RaceTypeId { get; set; }
        public long? EthnicityTypeId { get; set; }
        public long? MaritalStatusTypeId { get; set; }
        public string ReligiousConsideration { get; set; }
        public long? PrimaryLanguageTypeId { get; set; }
        public long? SecondaryLanguageTypeId { get; set; }

        public string NextOfKin { get; set; }
        public long? NextOfKinRelationshipTypeId { get; set; }
        public string ContactNumber { get; set; }
        public string AdvanceDirective { get; set; }
        public string CodeStatus { get; set; }
        public string POA { get; set; }
        public string Name { get; set; }
        public long? RelationshipTypeId { get; set; }
        public string PhoneNumber { get; set; }

        public string DateOfBirthString { get; set; }

    }
}