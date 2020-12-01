using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA
{
    public class Constant
    {
        public const int FULL_NAME_LENGTH = 300;
        public const int SHORT_NAME_LENGTH = 100;
        public const int NAME_LENGTH = 200;
        public const int ADDRESS_LENGTH = 400;
        public const string DEFAULT_SELECTALL_OPTION = "-Select Option-";
        public const string DEFAULT_SELECT_NA = "Not Applicable";
        public const string DEFAULT_ALL_OPTION = "All";
        public const string RESPONSE_OK = "OK";
        public const string RESPONSE_ERROR = "ERROR";
        public const int CUSTOM_ERROR_CODE = 412;
        public const int SSN_LENGTH = 50;
        public const int CODE_LENGTH = 50;

        public static List<string> PrimaryPayorType = new List<string>() { "Commercial", "Medicare", "Medicare Part A", "Medicare Part B", "Managed Medicare", "Medicare Exhaust", "Medicare Advantage", "Medicare Dual", "Medicare MMP", "Medicaid", "Managed Medicaid", "Affordable Care Act (ACA)", "Employer self-funded", "COBRA", "Workman's Comp", "Supplemental", "Government(VA, Tricare)", "Other" };
        public static List<string> PlanType = new List<string>() { "HMO", "Indemnity", "Exchange - Platinum", "Exchange - Gold", "Exchange - Silver", "Exchange - Bronze", "Exchange - Catastrophic", "PPO" };
        public static List<string> YesNoNA = new List<string>() { "Yes", "No", "N/A" };
        public static List<string> Bowel = new List<string>() { "Continent", "Incontinent", "Unspecified", "Bowel Program" };
        public static List<string> PainScale = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        public static List<string> YesNoUnspecified = new List<string>() { "Yes", "No", "Unspecified" };
        public static List<string> YesNoInformationNotAvailable = new List<string>() { "Yes", "No", "Information not Available" };
        public static List<string> CuffCuffless = new List<string>() { "Cuff", "Cuffless" };
        public static List<string> YesNo = new List<string>() { "Yes", "No" };
        public static List<string> PayorCategory = new List<string>() { "Commercial", "Managed Medicare", "Managed Medicaid", "Medicare", "Medicaid", "Medicaid Pending", "Workman's Comp", "Government", "Self-Pay", "Charity" };
        public static List<string> TypeOfPolicyProduct = new List<string>() { "HMO", "PPO", "POS", "PFFS", "Indemnity", "EPO", "Other" };
        public static List<string> MedicarePartA = new List<string>() { "N/A", "Deductible", "Co-insurance", "LTR co-insurance" };
        public static List<string> MedicarePartB = new List<string>() { "N/A", "Deductible", "Co-insurance" };
        public static List<string> MethodOfVerification = new List<string>() { "Payor portal", "DCS global", "Phone", "Other" };
        public static List<string> ResultOfMaximus = new List<string>() { "Approved", "Denied" };
        public static List<string> OHOT = new List<string>() { "OH", "OT" };
        public static List<string> WeightBearingStatus = new List<string>() { "Yes", "No" };
        public static List<string> PrimaryLanguage = new List<string>() { "English", "Spanish", "French", "German", "Italian", "Russian", "Chinese", "Japanese", "Korean", "Vietnamese", "Arabic", "Portuguese", "Persian", "Turkish", "Tamil", "Dutch", "Hindi", "Bengali", "Telugu", "Marathi", "Lahnda", "Serbo-Croatian", "Malay based", "Swahili", "Quechua" };
        public static List<string> SecondaryLanguage = new List<string>() { "English", "Spanish", "French", "German", "Italian", "Russian", "Chinese", "Japanese", "Korean", "Vietnamese", "Arabic", "Portuguese", "Persian", "Turkish", "Tamil", "Dutch", "Hindi", "Bengali", "Telugu", "Marathi", "Lahnda", "Serbo-Croatian", "Malay based", "Swahili", "Quechua" };
        public static List<string> Race = new List<string>() { "American Indian or Alaskan Native", "Asian", "Black or African American", "Native Hawiian or Other Pacific Islander", "White" };
        public static List<string> Relationship = new List<string>() { "Mother", "Father", "Daughter", "Son", "Sister", "Brother", "Nephew", "Uncle", "Aunt", "Niece" };
        public static List<string> Ethnicity = new List<string>() { "Hispanic or Latino", "Not Hispanic or Latino" };
        public static List<string> MaritalStatus = new List<string>() { "Married", "Unmarried" };
        public static List<string> Gender = new List<string>() { "Male", "Female" };
        public static List<string> PreHospitalLiving = new List<string>() { "Home", "SNF", "Long Term Care", "Assisted Living", "Other" };
        public static List<string> CHGNetworkStatus = new List<string>() { "In-network", "Out of network", "Out of network with out-of-network benefits", "Network not applicable", "Other" };

        public static List<string> Denial = new List<string>() { "Denial - No days", "Denial - No coverage", "Denial - Medicaid", "Denial - Did not meet criteria", "Denial - Patient expired", "N/A" };
        public static List<string> NoAdmit = new List<string>() {"Non Admit - Lost to competitor", "Non-Admit - Went to competing LTACH", "Non-Admit - Went to IRF",
           "Non Admit - Family chose competitor", "Non Admit - Went to Hospice", "Non Admit - Went to SNF", "Non Admit - no contract", "Non-Admit - insurance denial",
           "Non-Admit - Peer to Peer denial STACH Physician", "Non-Admit - Peer to Peer denial Insurance Physician", "Non-Admit - Expedited Appeal denial", "Non-Admit - SCA denial (us)",
           "Non-Admit - SCA denial (them)", "Non-Admit - OON with no SCA possible", "Non-Admit - CEO Denial", "Non-Admit - VP Denial", "Non-Admit - Administrative Denial",
           "Non-Admit - No secondary to cover exhaust", "Non-Admit - Site Neutral Denial", "Non-Admit - No beds", "Non-Admit - no ICU beds", "N/A" };

        public static List<string> ReferringSourceTypes = new List<string>() { "STACH",
             "VA",
              "Physician Offices",
               "SNFs",
                "Assisted Living",
                 "Other Behavorial Health",
                  "Clinic",
                   "Any CHG facility",
                    "Non-CHG LTACH"

        };
        public static List<string> MethodOfBusinessRequest = new List<string>() { "Allscripts",
             "NaviHealth",
              "Silvervue",
               "In-person",
                "Text",
                 "CHG Hotline",
                  "Other"


        };


        public static List<string> DispositionTypes = new List<string>() { "Pre-Admitted", "Not Admitted" };

        public static List<string> Limitations = new List<string>() { "Copays", "Deductible", "Deductible not met to-date", "Reimbursement after deductible met" };

        public static List<string> LimitationBasis = new List<string>() { "Admit", "Illness", "Year" };

        public static List<string> DayLimitation = new List<string>() { "Days to recuperate on", "Not available"};

        public static List<string> BenefitMaximumCap = new List<string>() { "Per year", "Per diagnosis", "Not available" };

        public static List<string> DenialReason = new List<string>() { "No Days", "No coverage", "Medicaid", "Did not meet criteria", "N/A" };


        public static List<string> ApprovalStatus = new List<string>()
        { "Approved","Denied"};

        public static List<string> ApprovalStatusNA = new List<string>()
        { "Approved","Denied","N/A"};

        public static List<string> PayorNames = new List<string>() {
           "AARP Medicare Advantage - Managed by Wellmed",
"Aetna Better Health",
"Aetna MCR TRS",
"Aetna MCR Advantage",
"Aetna TRS Commercial",
"Aetna Commercial",
"AHCCCS MCD",
"Ambetter HIX",
"Ambetter Marketplace ",
"Amerigroup Medicaid ",
"Amerigroup MMP ",
"Amerihealth Caritas MCD ",
"Amerivantage MCR Advantage",
"Anthem BCBS",
"Anthem BCBS MCR Advantage",
"Anthem BCBS ",
"Anthem KY MCD",
"Anthem MCR Advantage",
"APIPA Medicare Advantage",
"APIPA MCD",
"Arkansas BCBS (Walmart)",
"Arkansas BCBS ",
"Banner UFC Medicare Advantage",
"Banner UFC MCD",
"BCBS Out Of State (Need to identify shich state)",
"BCBS Advantage ",
"BCBS Commercial",
"BCBS Medicare Advantage",
"BCBS MCD",
"BCBS Federal ",
"BCBS Health Advantage",
"BCBS HIX ",
"BCBS Medi-pak",
"Bridge Pointe Worker's Comp",
"Care1st Medicare",
"Care1st MCD",
"Caremore MCD",
"Caremore Medicare Advantage",
"Caresource MCD",
"Caresource Medicare",
"Cenpatico Intergrated Services",
"Cigna Healthspring - Managed by Wellmed",
"Cigna Healthspring Medicare Advantage",
"Cigna Healthspring MCD",
"Cigna Commercial",
"Coresource PPO",
"Coventry Medicare Advantage",
"Coventry MCD",
"Gateway MCR",
"GEHA Commercial",
"Gilsbar Worker's Comp",
"Healthnet MCD",
"Healthnet Medicare Advantage",
"Healthy Blue MCD ",
"Highmark BCBS",
"Hospital contract",
"Humana MCD",
"Humana Military/Tricare",
"Humana Commercail",
"Indian Health Services MCD",
"Kelsey Seybold Medicare Advantage",
"LA  Healthcare Connections MCD",
"Medicare",
"Medicare Part A Only",
"Medicare Part B Only",
"Medicaid (Need to identify which state)",
"Medicare Exhaust",
"Medicare- LOA",
"Mercy Care Medicare Advantage",
"Mercy Care MCD",
"Molina Medicare Advantage",
"Molina MCD ",
"Molina MMP",
"Multiplan Commercial",
"Network Health MCR Advantage",
"Passport KY MCD",
"QualChoice HIX",
"Qualchoice Marketplace",
"QualChoice Medicare Advantage",
"Scott & White MCR Advantage",
"Scott & White Supplemental",
"Seton Contract",
"Superior Medicare Advantage",
"Superior MMP",
"Superior MCD",
"Texan Plus MCR- Managed by Kelsey Seybold",
"Texan Plus MCR Advantage",
"UFC Medicare Advantage",
"UFC MCD",
"UHC Commercial",
"UHC AARP Medicare Advantage",
"UHC MCD",
"UHC Medicare Advantage",
"UHC Medicare Dual",
"UHC Community Plan",
"UHC Global/International",
"UHC Medica Prime",
"UHC MMP",
"UHC Shared Services",
"UMR Commercail",
"Unicare MCD",
"Unicare Medicare Advantage",
"University Physician's Care Medicare Advantage",
"University Physician's Care MCD",
"University Family Care MCD",
"University Family Care Medicare Advantage",
"US Marshalls",
"USFHP Medicare Replacement",
"Veteran's Administration",
"Vantage Medicare Advantage",
"WellCare Medicare Advantage",
"WellCare MCD",
"Wellmed Medicare Advantage",
"Other"

        };

        public static List<string> BenefitsPaitAt = new List<string>() { "Contracted rate", "Medicare allowable/DRG", "Percentage of charges" };

        public static List<string> BenefitsQuoted = new List<string>() { "In network", "Out of network", "General benefits - network not applicable" };

        public static List<string> AuthorizationMethods = new List<string>() {
            "Email", "Voicemail", "Phone conversation","Fax","Alternative contact" };

        public static List<string> LevelOfCareAuthorized = new List<string>() {
            "ICU", "Intermediate Care", "Med/Surg" };




        public static List<string> GetTaskStatus()
        {
            List<string> status = new List<string>();
            status.Add("New");
            status.Add("Resolving");
            status.Add("Completed");
            return status;

        }

        public static List<string> GetTaskTypes()
        {
            List<string> list = new List<string>();
            list.Add("Call Cycle Approval");
            return list;

        }




    }
}