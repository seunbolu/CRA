using CRA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;

namespace CRA.Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            TargetDatabase = new DbConnectionInfo("CRAConnectionString");
        }
        protected override void Seed(DataContext context)
        {
            SeedServiceTypes(context);
            SeedRegionTypes(context);
            SeedPreScreenUpdateTypes(context);
            SeedReferralSourceTypes(context);
            context.SaveChanges();
            SeedRegionServices(context);
            context.SaveChanges();
            SeedSpecialityTypes(context);
            context.SaveChanges();
            SeedPreScreenTypes(context);
            context.SaveChanges();
            SeedCategoryTypes(context);
            context.SaveChanges();
            SeedContactRoleType(context);
            context.SaveChanges();
            SeedUserRoleType(context);
            context.SaveChanges();
         
            SeedElectronicReferralType(context);
            context.SaveChanges();
            SeedCommercialPayorTypes(context);
            context.SaveChanges();
            SeedManagedMedicarePayorTypes(context);
            context.SaveChanges();
        
            SeedOrganization(context);
            context.SaveChanges();
            SeedOrganizationServiceTypes(context);
            context.SaveChanges();
            SeedEmailStatusType(context);
            context.SaveChanges();
            SeedGenderTypes(context);
            context.SaveChanges();
            SeedRaceTypes(context);
            context.SaveChanges();
            SeedEthnicityTypes(context);
            context.SaveChanges();
            SeedMaritalStatusTypes(context);
            context.SaveChanges();
            SeedLanguageTypes(context);
            context.SaveChanges();
            SeedRelationshipTypes(context);
            context.SaveChanges();
            SeedCountryTypes(context);
            context.SaveChanges();
            SeedUSStateTypes(context);
            context.SaveChanges();
            SeedTemplates(context);
            context.SaveChanges();

        }




        private void SeedTemplates(DataContext context)
        {

            Dictionary<string, string> values = new Dictionary<string, string>();

            //Call Cycle
            //Created
            values.Add("CALL_CYCLE_APPROVAL_REQUEST_CREATED_SUBJECT", "New Call Cycle needs Approval/Denial - ##LIASON_NAME##");
            values.Add("CALL_CYCLE_APPROVAL_REQUEST_CREATED_BODY", "A new call cycle was added to the system that needs your approval. Please click on the following link to approve/deny the request.<br/><br/><a href=\"##TASK_URL##\" target=\"_blank\">Click Here to Approve/Deny the request.</a>");

            //Approved
            values.Add("CALL_CYCLE_APPROVAL_REQUEST_APPROVED_SUBJECT", "New Call Cycle Approved  - ##LIASON_NAME##");
            values.Add("CALL_CYCLE_APPROVAL_REQUEST_APPROVED_BODY", "The call cycle was approved.<br/><br/><strong>Notes:</strong>##DECISION_NOTES##");


            //Reject
            values.Add("CALL_CYCLE_APPROVAL_REQUEST_REJECTED_SUBJECT", "New Call Cycle Denied  - ##LIASON_NAME##");
            values.Add("CALL_CYCLE_APPROVAL_REQUEST_REJECTED_BODY", "The call cycle was denied.<br/><br/><strong>Notes:</strong>##DECISION_NOTES##");


            //Referral Source


            //Created
            values.Add("REFERRAL_SOURCE_APPROVAL_REQUEST_CREATED_SUBJECT", "New Referral Source needs Approval/Denial - ##REFERRAL_SOURCE_NAME##");
            values.Add("REFERRAL_SOURCE_APPROVAL_REQUEST_CREATED_BODY", "A new referral source was added to the system that needs your approval. Please click on the following link to approve/deny the request.<br/><br/><a href=\"##TASK_URL##\" target=\"_blank\">Click Here to Approve/Deny the request.</a>");

            //Approved
            values.Add("REFERRAL_SOURCE_APPROVAL_REQUEST_APPROVED_SUBJECT", "New Referral Source Approved  - ##REFERRAL_SOURCE_NAME##");
            values.Add("REFERRAL_SOURCE_APPROVAL_REQUEST_APPROVED_BODY", "The referral source was approved.<br/><br/><strong>Notes:</strong>##DECISION_NOTES##");


            //Reject
            values.Add("REFERRAL_SOURCE_APPROVAL_REQUEST_REJECTED_SUBJECT", "New Referral Source Denied  - ##REFERRAL_SOURCE_NAME##");
            values.Add("REFERRAL_SOURCE_APPROVAL_REQUEST_REJECTED_BODY", "The referral source was denied.<br/><br/><strong>Notes:</strong>##DECISION_NOTES##");



            //Contact


            //Created
            values.Add("CONTACT_APPROVAL_REQUEST_CREATED_SUBJECT", "New Contact needs Approval/Denial - ##CONTACT_NAME##");
            values.Add("CONTACT_APPROVAL_REQUEST_CREATED_BODY", "A new contact was added to the system that needs your approval. Please click on the following link to approve/deny the request.<br/><br/><a href=\"##TASK_URL##\" target=\"_blank\">Click Here to Approve/Deny the request.</a>");

            //Approved
            values.Add("CONTACT_APPROVAL_REQUEST_APPROVED_SUBJECT", "New Contact Approved  - ##CONTACT_NAME##");
            values.Add("CONTACT_APPROVAL_REQUEST_APPROVED_BODY", "The contact was approved.<br/><br/><strong>Notes:</strong>##DECISION_NOTES##");


            //Reject
            values.Add("CONTACT_APPROVAL_REQUEST_REJECTED_SUBJECT", "New Contact Denied  - ##CONTACT_NAME##");
            values.Add("CONTACT_APPROVAL_REQUEST_REJECTED_BODY", "The contact was denied.<br/><br/><strong>Notes:</strong>##DECISION_NOTES##");


            //Referral & Pre-Screen

            values.Add("NEW_REFERRAL_SUBJECT", "CL to complete the pre-screen for patient ##PATIENT_IDENTIFIER##.");
            values.Add("NEW_REFERRAL_BODY", "A new pre-screen has been created which has you designated as the liason completing the pre-screen. Please visit the following link to complete the pre-screen.<br/><br/>Please click on the following link to complete the request.<br/><br/><a href=\"##TASK_URL##\" target=\"_blank\">Click Here to complete the request.</a>");


            //Referral & Pre-Screen

            values.Add("PRE_SCREEN_COMPLETE_SUBJECT", "Pre-Screen completed for patient ##PATIENT_IDENTIFIER##, Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_COMPLETE_BODY", "Pre-Screen with ID ##PRE_SCREEN_ID## has been completed.");


            values.Add("PRE_SCREEN_APPROVAL_SUBJECT", "Pre-Screen approval required for patient ##PATIENT_IDENTIFIER##, Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_APPROVAL_BODY", "Pre-Screen with ID ##PRE_SCREEN_ID## requires your approval.<br/><br/>Please click on the following link to complete the request.<br/><br/><a href=\"##TASK_URL##\" target=\"_blank\">Click Here to complete the request.</a>");


            values.Add("PRE_SCREEN_REJECTED_SUBJECT", "Pre-Screen CEO rejected for patient ##PATIENT_IDENTIFIER##, Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_REJECTED_BODY", "Pre-Screen with ID ##PRE_SCREEN_ID## is rejected by CEO.<br/><br/><br/><strong>Notes:</strong>##DECISION_NOTES##");


            values.Add("PRE_SCREEN_APPROVED_SUBJECT", "Pre-Screen CEO approved for patient ##PATIENT_IDENTIFIER##, Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_APPROVED_BODY", "Pre-Screen with ID ##PRE_SCREEN_ID## is approved by CEO.<br/><br/><br/><strong>Notes:</strong>##DECISION_NOTES##");

            
            values.Add("PRE_SCREEN_SCA_REQUESTED_SUBJECT", "Pre-Screen SCA Requested for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_SCA_REQUESTED_BODY", "SCA is requested for Pre-Screen ID ##PRE_SCREEN_ID##.<br/><br/>Please click on the following link to complete the request.<br/><br/><a href=\"##TASK_URL##\" target=\"_blank\">Click Here to complete the request.</a>");

            values.Add("PRE_SCREEN_SCA_APPROVED_SUBJECT", "Pre-Screen SCA Approved for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_SCA_APPROVED_BODY", "SCA is approved for Pre-Screen ID ##PRE_SCREEN_ID##.");

            values.Add("PRE_SCREEN_SCA_DENIED_SUBJECT", "Pre-Screen SCA Denied for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_SCA_DENIED_BODY", "SCA is denied for Pre-Screen ID ##PRE_SCREEN_ID##.");



            values.Add("PRE_SCREEN_PEER_TO_PEER_APPROVED_SUBJECT", "Pre-Screen Peer To Peer Approved for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_PEER_TO_PEER_APPROVED_BODY", "Peer To Peer is approved for Pre-Screen ID ##PRE_SCREEN_ID##.");

            values.Add("PRE_SCREEN_PEER_TO_PEER_DENIED_SUBJECT", "Pre-Screen Peer To Peer Denied for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_PEER_TO_PEER_DENIED_BODY", "Peer To Peer is denied for Pre-Screen ID ##PRE_SCREEN_ID##.");


            values.Add("PRE_SCREEN_EXPEDITED_APPEAL_APPROVED_SUBJECT", "Pre-Screen Expedited Appeal Approved for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_EXPEDITED_APPEAL_APPROVED_BODY", "Expedited Appeal is approved for Pre-Screen ID ##PRE_SCREEN_ID##.");

            values.Add("PRE_SCREEN_EXPEDITED_APPEAL_DENIED_SUBJECT", "Pre-Screen Expedited Appeal Denied for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_EXPEDITED_APPEAL_DENIED_BODY", "Expedited Appeal is denied for Pre-Screen ID ##PRE_SCREEN_ID##.");


            values.Add("PRE_SCREEN_MAXIMUS_APPROVED_SUBJECT", "Pre-Screen Maximus Approved for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_MAXIMUS_APPROVED_BODY", "Maximus is approved for Pre-Screen ID ##PRE_SCREEN_ID##.");

            values.Add("PRE_SCREEN_MAXIMUS_DENIED_SUBJECT", "Pre-Screen Maximus Denied for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_MAXIMUS_DENIED_BODY", "Maximus is denied for Pre-Screen ID ##PRE_SCREEN_ID##.");

            values.Add("PRE_SCREEN_AUTH_APPROVED_SUBJECT", "Pre-Screen Auth Approved for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_AUTH_APPROVED_BODY", "Auth is approved for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_AUTH_DENIED_SUBJECT", "Pre-Screen Auth Denied for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_AUTH_DENIED_BODY", "Auth is denied for Pre-Screen ID ##PRE_SCREEN_ID##.");



            values.Add("PRE_SCREEN_PLAN_IN_GRACE_SUBJECT", "Pre-Screen plan is in grace period for Pre-Screen ID ##PRE_SCREEN_ID##.");
            values.Add("PRE_SCREEN_PLAN_IN_GRACE_BODY", "Plan is in the grace period for Pre-Screen ID ##PRE_SCREEN_ID##");


            foreach (string key in values.Keys)
            {
                var entity = context.Templates.Where(p => p.Name == key && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new Template();
                 
                    context.Templates.Add(entity);
                }
                entity.Name = key;
                entity.Value = values[key];

            }


        }


        private void SeedCountryTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "USA"
            };

            foreach (string item in items)
            {
                var entity = context.CountryTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new CountryType();
                    entity.Name = item;
                    context.CountryTypes.Add(entity);
                }
            }


        }
        private void SeedUSStateTypes(DataContext context)
        {
            var countryTypeId = context.CountryTypes.Where(p => p.Name == "USA").Single().CountryTypeId;
            List<string> items = new List<string>()
            {
            "Alabama",
"Alaska",
"Arizona",
"Arkansas",
"California",
"Colorado",
"Connecticut",
"Delaware",
"Florida",
"Georgia",
"Hawaii",
"Idaho",
"Illinois",
"Indiana",
"Iowa",
"Kansas",
"Kentucky",
"Louisiana",
"Maine",
"Maryland",
"Massachusetts",
"Michigan",
"Minnesota",
"Mississippi",
"Missouri",
"Montana",
"Nebraska",
"Nevada",
"New Hampshire",
"New Jersey",
"New Mexico",
"New York",
"North Carolina",
"North Dakota",
"Ohio",
"Oklahoma",
"Oregon",
"Pennsylvania",
"Rhode Island",
"South Carolina",
"South Dakota",
"Tennessee",
"Texas",
"Utah",
"Vermont",
"Virginia",
"Washington",
"West Virginia",
"Wisconsin",
"Wyoming"

            };

            foreach (string item in items)
            {
                var entity = context.StateTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new StateType();
                    entity.Name = item;
                    entity.CountryTypeId = countryTypeId;
                    context.StateTypes.Add(entity);
                }
            }


        }
        private void SeedRelationshipTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "Mother", "Father", "Daughter", "Son", "Sister", "Brother", "Nephew", "Uncle", "Aunt", "Niece"
            };

            foreach (string item in items)
            {
                var entity = context.RelationshipTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new RelationshipType();
                    entity.Name = item;
                    context.RelationshipTypes.Add(entity);
                }
            }


        }
        private void SeedLanguageTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
            "English", "Spanish", "French", "German", "Italian", "Russian", "Chinese", "Japanese", "Korean", "Vietnamese", "Arabic", "Portuguese", "Persian", "Turkish", "Tamil", "Dutch", "Hindi", "Bengali", "Telugu", "Marathi", "Lahnda", "Serbo-Croatian", "Malay based", "Swahili", "Quechua"
            };

            foreach (string item in items)
            {
                var entity = context.LanguageTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new LanguageType();
                    entity.Name = item;
                    context.LanguageTypes.Add(entity);
                }
            }


        }
        private void SeedMaritalStatusTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
             "Married","Unmarried"
            };

            foreach (string item in items)
            {
                var entity = context.MaritalStatusTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new MaritalStatusType();
                    entity.Name = item;
                    context.MaritalStatusTypes.Add(entity);
                }
            }

        }
        private void SeedRaceTypes(DataContext context)
        {
            //List<string> items = new List<string>()
            //{
            //   "American Indian or Alaskan Native", "Asian", "Black or African American", "Native Hawiian or Other Pacific Islander", "White"
            //};

            //foreach (string item in items)
            //{
            //    var entity = context.RaceTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

            //    if (entity == null)
            //    {
            //        entity = new RaceType();
            //        entity.Name = item;
            //        context.RaceTypes.Add(entity);
            //    }
            //}

        }
        private void SeedEthnicityTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
               "Hispanic or Latino", "Not Hispanic or Latino"
            };

            foreach (string item in items)
            {
                var entity = context.EthnicityTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new EthnicityType();
                    entity.Name = item;
                    context.EthnicityTypes.Add(entity);
                }
            }

        }
        private void SeedGenderTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "Male",
                "Female"
            };

            foreach (string item in items)
            {
                var entity = context.GenderTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new GenderType();
                    entity.Name = item;
                    context.GenderTypes.Add(entity);
                }
            }

        }
        private void SeedServiceTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "LTACH",
                "Behavioral Health",
                "Senior Living",
                "Home Health",
                "SNF"
            };

            foreach (string item in items)
            {
                var entity = context.ServiceTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new ServiceType();
                    entity.Name = item;
                    context.ServiceTypes.Add(entity);
                }
            }

        }
        private void SeedRegionTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "North",
                "South",
                "BH-1",
                "SL-1",
                "SNF-1"
            };

            foreach (string item in items)
            {
                var entity = context.RegionTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new RegionType();
                    entity.Name = item;
                    context.RegionTypes.Add(entity);
                }
            }

        }
        private void SeedRegionServices(DataContext context)
        {

            if (context.RegionServiceTypes.Where(p => p.ServiceType.Name == "LTACH" && p.RegionType.Name == "North" && p.Deleted == false).Count() <= 0)
            {
                RegionServiceType regionService = new RegionServiceType();
                regionService.RegionTypeId = context.RegionTypes.Where(p => p.Name == "North").Single().RegionTypeId;
                regionService.ServiceTypeId = context.ServiceTypes.Where(p => p.Name == "LTACH").Single().ServiceTypeId;

                context.RegionServiceTypes.Add(regionService);

            }

            if (context.RegionServiceTypes.Where(p => p.ServiceType.Name == "LTACH" && p.RegionType.Name == "South" && p.Deleted == false).Count() <= 0)
            {
                RegionServiceType regionService = new RegionServiceType();
                regionService.RegionTypeId = context.RegionTypes.Where(p => p.Name == "South").Single().RegionTypeId;
                regionService.ServiceTypeId = context.ServiceTypes.Where(p => p.Name == "LTACH").Single().ServiceTypeId;


                context.RegionServiceTypes.Add(regionService);

            }

            if (context.RegionServiceTypes.Where(p => p.ServiceType.Name == "Behavioral Health" && p.RegionType.Name == "BH-1" && p.Deleted == false).Count() <= 0)
            {
                RegionServiceType regionService = new RegionServiceType();
                regionService.RegionTypeId = context.RegionTypes.Where(p => p.Name == "BH-1").Single().RegionTypeId;
                regionService.ServiceTypeId = context.ServiceTypes.Where(p => p.Name == "Behavioral Health").Single().ServiceTypeId;

                context.RegionServiceTypes.Add(regionService);

            }


            if (context.RegionServiceTypes.Where(p => p.ServiceType.Name == "Senior Living" && p.RegionType.Name == "SL-1" && p.Deleted == false).Count() <= 0)
            {
                RegionServiceType regionService = new RegionServiceType();
                regionService.RegionTypeId = context.RegionTypes.Where(p => p.Name == "SL-1").Single().RegionTypeId;
                regionService.ServiceTypeId = context.ServiceTypes.Where(p => p.Name == "Senior Living").Single().ServiceTypeId;

                context.RegionServiceTypes.Add(regionService);

            }

            if (context.RegionServiceTypes.Where(p => p.ServiceType.Name == "SNF" && p.RegionType.Name == "SNF-1" && p.Deleted == false).Count() <= 0)
            {
                RegionServiceType regionService = new RegionServiceType();
                regionService.RegionTypeId = context.RegionTypes.Where(p => p.Name == "SNF-1").Single().RegionTypeId;
                regionService.ServiceTypeId = context.ServiceTypes.Where(p => p.Name == "SNF").Single().ServiceTypeId;

                context.RegionServiceTypes.Add(regionService);

            }

        }
        private void SeedPreScreenUpdateTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "CIHQ",
                "TJC"
            };

            foreach (string item in items)
            {
                var entity = context.PreScreenUpdateTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new PreScreenUpdateType();
                    entity.Name = item;
                    context.PreScreenUpdateTypes.Add(entity);
                }
            }

        }
        private void SeedReferralSourceTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "STACH",
                "Physician offices",
                "Non-CHG SNFs",
                "Non-CHG Assisted Living",
                "Non-CHG Behavorial health",
                "Clinic",
                "Any CHG facility",
                "Non-CHG LTACH",
                "Other"
            };

            foreach (string item in items)
            {
                var entity = context.ReferralSourceTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new ReferralSourceType();
                    entity.Name = item;
                    context.ReferralSourceTypes.Add(entity);
                }
            }

        }
        private void SeedPreScreenTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "LTACH Pre-Screen",
                "SSSN Pre-Screen",
                "SNF Pre-Screen",
                "BH Pre-Screen"
            };

            foreach (string item in items)
            {
                var entity = context.PreScreenTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new PreScreenType();
                    entity.Name = item;
                    context.PreScreenTypes.Add(entity);
                }
            }

        }
        private void SeedCategoryTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "Referring",
            "Influencer",
                "Challenge",

        "New"
            };

            foreach (string item in items)
            {
                var entity = context.CategoryTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new CategoryType();
                    entity.Name = item;
                    context.CategoryTypes.Add(entity);
                }
            }

        }
        private void SeedSpecialityTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "Pulmonary",
                     "Critical Care",
                     "Orthopedic",
                     "Interventional Radiology",
                     "Cardiology",
                     "Infectious Disease",
                     "Nephrology",
                     "Surgery",
                     "Psychiatry",
                     "Neurology",
                     "Gastroenterology",
                     "Urology",
                     "Oncology/Hemotology",
                     "Internal Medicine",
                     "ENT Consult",
                     "Physiatrist",
                     "Wound",
                     "Other"
            };

            foreach (string item in items)
            {
                var entity = context.SpecialityTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new SpecialityType();
                    entity.Name = item;
                    context.SpecialityTypes.Add(entity);
                }
            }

        }
        private void SeedContactRoleType(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "Physician",
                "Nurse Practitioner",
                "Case Manager",
                "Social Worker",
                "Other",
                "PT",
                "CEO",
                "CFO",
                "ICU Director ",
                "OT Director",
                "RT Director",
                "RT Supervisor",
                "Discharge Expeditor",
                "Director of Med or Surg floor",
                "Clinical Supervisor of Med or Surg floor",
                "Director of Telemetry floor",
                "Clinical Supervisor of Telemetry floor ",
                "Director of Emergency Dept",
                "ICU Clinical Supervisor",
                "OT",
                "RN",
                "Rehab Tech",
                "Wound Care",
                "RT  Days",
                "WCC Manager",
                "Director of Revenue Analysis",
                "ER Department",
                "Medical Staff Assistant",
                "Infection Prevention Manager",
                "Patient Safety Coordinator",
                "VP of Performance Improvement",
                "Quality Manager",
                "Stroke Program Coordinator",
                "Quality Analyst",
                "Administrative Assistant",
                "AMP Pharmacist"

            };

            foreach (string item in items)
            {
                var entity = context.ContactRoleTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new ContactRoleType();
                    entity.Name = item;
                    context.ContactRoleTypes.Add(entity);
                }
            }

        }
        private void SeedUserRoleType(DataContext context)
        {
            List<string> items = new List<string>()
            {
               "CRO",
                "AVP",
                "CEO",
                "DBD",
                "CL",
                "CAC",
                "PU",
                "MC",
                "SLH",
                "CU",
                "CBO",
                "NCM",
                "IT"
            };

            foreach (string item in items)
            {
                var entity = context.UserRoleTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new UserRoleType();
                    entity.Name = item;
                    context.UserRoleTypes.Add(entity);
                }
            }

        }
     
        private void SeedEmailStatusType(DataContext context)
        {
            List<string> items = new List<string>()
            {

                            "Created",
                            "Processing",
                            "Completed",
                             "Error"
            };

            foreach (string item in items)
            {
                var entity = context.EmailStatusTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new EmailStatusType();
                    entity.Name = item;
                    context.EmailStatusTypes.Add(entity);
                }
            }

        }
      
        private void SeedElectronicReferralType(DataContext context)
        {
            List<string> items = new List<string>()
            {

                            "Curaspan",
                            "NaviHealth Quick Case",
                            "Allscripts",
                            "None",
                            "Midas",
                            "EPIC",
                            "Sorian",
                            "Meditech",
                            "Yes",
                            "UHS Rightfax",
                            "ECIN",
                            "extendedcare"
            };

            foreach (string item in items)
            {
                var entity = context.ElectronicReferralTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new ElectronicReferralType();
                    entity.Name = item;
                    context.ElectronicReferralTypes.Add(entity);
                }
            }

        }
        private void SeedCommercialPayorTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {


                                                        "Community Care",
                                                        "Blue Cross",
                                                        "UHC",
                                                        "Aetna",
                                                        "Cigna",
                                                        "BCBS",
                                                       "HUMANA",
                                                        "KELSEY",
                                                       "MULTIPLAN",
                                                        "TEXAN PLUS",
                                                       "UNITED HEALTH",
                                                        "VA",
                                                        "Worker's Comp",
                                                       "Sendero",
                                                       "Champ VA",
                                                       "TriCare",
                                                        "Superior Ambetter",
                                                       "Medicare",
                                                   "Medicaid",
                                                       "Scott & White",
                                                        "Oscar Insurance",
                                                        "PPO",
                                                       "Community Health Choice",
                                                        "Molina exchange",
                                                       "ALL COMMERCIAL PLANS",
                                                        "Ambetter",
                                                        "TML",
                                                       "Vantage",
                                                        "UMR",
                                                        "TRICARE Prime",
                                                      "Tricare Standard",
                                                        "US Healthcare",
                                                        "Amerigroup",
                                                      "AmeriAdvantage",
                                                     "CIDC",
                                                        "CIGNA Healthcare",
                                                       "CIGNA Healthspring ",
                                                       "WellMed"

            };

            foreach (string item in items)
            {
                var entity = context.CommercialPayorTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new CommercialPayorType();
                    entity.Name = item;
                    context.CommercialPayorTypes.Add(entity);
                }
            }

        }
        private void SeedManagedMedicarePayorTypes(DataContext context)
        {
            List<string> items = new List<string>()
            {
                                                        "Community Care",
                                                        "Humana",
                                                        "UHC",
                                                        "Coventry",
                                                        "Blue Advantage",
                                                        "Aetna",
                                                        "BCBS",
                                                        "CIGNA",
                                                        "KELSEY",
                                                        "TEXAN PLUS",
                                                        "Wellmed",
                                                        "Cigna Healthsprings",
                                                        "Allegian",
                                                         "Ambetter MKT",
                                                         "Superior",
                                                         "Scott & White",
                                                        "AARP",
                                                       "Molina",
                                                        "Amerigroup",

                                                        "Multiplan",
                                                       "Wellcare",
                                                        "Cigna Vantage Local",
                                                        "Galaxy Health Network",
                                                        "Selectcare of Texas",
                                                        "Humana MCR",
                                                            "Community Health Choice",
                                                        "Care Improvement Plus",
                                                        "Vantage",
                                                            "Coventry(WC)",
                                                            "Coventry(PPO)",
                                                            "HMO Blue Texas",
                                                        "AMERIVANTAGE",
                                                            "Texas Medicaid",

            };

            foreach (string item in items)
            {
                var entity = context.ManagedMedicarePayorTypes.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new ManagedMedicarePayorType();
                    entity.Name = item;
                    context.ManagedMedicarePayorTypes.Add(entity);
                }
            }

        }
     
     
      
        private void SeedOrganization(DataContext context)
        {
            List<string> items = new List<string>()
            {
                "Cornerstone Healthcare Group"

            };

            foreach (string item in items)
            {
                var entity = context.Organizations.Where(p => p.Name == item && p.Deleted == false).SingleOrDefault();

                if (entity == null)
                {
                    entity = new Organization();
                    entity.Name = item;
                    context.Organizations.Add(entity);
                }
            }


        }
        private void SeedOrganizationServiceTypes(DataContext context)
        {

            var organizationId = context.Organizations.Take(1).SingleOrDefault().OrganizationId;

            foreach (var service in context.ServiceTypes)
            {
                if (context.OrganizationServiceTypes.Where(p => p.OrganizationId == organizationId && p.ServiceTypeId == service.ServiceTypeId && p.Deleted == false).Count() <= 0)
                {
                    var organizationServiceType = new OrganizationServiceType()
                    {
                        ServiceTypeId = service.ServiceTypeId,
                        OrganizationId = organizationId
                    };

                    context.OrganizationServiceTypes.Add(organizationServiceType);
                }
            }


        }




    }
}
