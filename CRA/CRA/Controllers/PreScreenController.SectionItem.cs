using CRA.Data;
using CRA.Data.Entities;
using CRA.Helper;
using CRA.Models.CHGSite;
using CRA.Models.PreScreen;
using CRA.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace CRA.Controllers
{
    public partial class PreScreenController : BaseController
    {
        private void PopulateItems(PreScreenModel model)
        {
            



            //Primary Payor All
            var section = model.Tabs
                .Where(p => p.Label == "Payor Info").Single()
                .Accordions[0].Sections.Where(p => p.Label == "Primary").Single().Accordions[0]
                .Sections.Where(p => p.Label == "All").Single();


            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, IsIVFField = true, Label = "Social Security Number", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ssn", ItemType = "SSN" });
            section.Items.Add(new ItemModel() { SNF = true, Label = "Payor type", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payortype", ItemType = "Dropdown", LabelList = Constant.PrimaryPayorType.ToArray(), ValueList = Constant.PrimaryPayorType.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Label = "Additional Payor Information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "additionalpayorinfo", DependsOnCode = "payortype", DependsOnAssertValue = "Other", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Label = "Payor Name", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payorname", ItemType = "Text", DependsOnCode = "payortype", DependsOnAssertValue = "Other" });
            section.Items.Add(new ItemModel() { Label = "Payor Category", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payorcategory", ItemType = "Dropdown", LabelList = Constant.PayorCategory.ToArray(), ValueList = Constant.PayorCategory.ToArray() });
            section.Items.Add(new ItemModel() { Label = "Plan Type", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "plantype", ItemType = "Dropdown", LabelList = Constant.PlanType.ToArray(), ValueList = Constant.PlanType.ToArray() });
            section.Items.Add(new ItemModel() { Label = "STACH Case Manager/Social Worker", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "stachcasemanagerorsocialworker" });
            section.Items.Add(new ItemModel() { Label = "STACH Case Manager/Social Worker Phone Number", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "stachcasemanagerorsocialworkerphonenumber", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { Label = "MVA Exclusions", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mvaexclusions", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Label = "MVA Exclusions Additional Information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mvaexclusionsadditionalinfo", DependsOnCode = "mvaexclusions", DependsOnAssertValue = "Yes", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { Label = "Other exclusion(s)", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherexclusions", ItemType = "Option", LabelList = Constant.YesNo.ToArray(), ValueList = Constant.YesNo.ToArray() });
            section.Items.Add(new ItemModel() { Label = "Other exclusion(s) Additional Information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherexclusionsadditionalinfo", DependsOnCode = "otherexclusions", DependsOnAssertValue = "Yes", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { Label = "Patient is in ESRD", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "patientinesrd", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Label = "ESRD start date", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "esrdstartdate", DependsOnCode = "patientinesrd", DependsOnAssertValue = "Yes", ItemType = "Date" }); //
            section.Items.Add(new ItemModel() { Label = "Type of policy/product", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "typeofpolicyorproduct", ItemType = "Dropdown", LabelList = Constant.TypeOfPolicyProduct.ToArray(), ValueList = Constant.TypeOfPolicyProduct.ToArray() });
            section.Items.Add(new ItemModel() { Label = "Type of policy/product additional information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "typeofpolicyorproductadditionalinfo", DependsOnCode = "typeofpolicyorproduct", DependsOnAssertValue = "Other", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { Label = "Policy is secondary to ONLY Medicare", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "policyissecondaryonlytomedicare", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Label = "Plan will act as primary when Medicare exhausts", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "planwillactasprimarywhenmedicareexhausts", DependsOnCode = "policyissecondaryonlytomedicare", DependsOnAssertValue = "Yes", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() }); //
            section.Items.Add(new ItemModel() { Label = "CEO approval required", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ceoapprovalrequired", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            section = model.Tabs
                        .Where(p => p.Label == "Payor Info").Single()
                        .Accordions[0].Sections.Where(p => p.Label == "Secondary").Single().Accordions[0]
                        .Sections.Where(p => p.Label == "All").Single();




            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, Label = "Payor type", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payortype", ItemType = "Dropdown", LabelList = Constant.PrimaryPayorType.ToArray(), ValueList = Constant.PrimaryPayorType.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Label = "Additional Payor Information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "additionalpayorinfo", DependsOnCode = "payortype", DependsOnAssertValue = "Other", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, Label = "Payor Name", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payorname", ItemType = "Text", DependsOnCode = "payortype", DependsOnAssertValue = "Other" });
            section.Items.Add(new ItemModel() { Label = "Payor Category", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payorcategory", ItemType = "Dropdown", LabelList = Constant.PayorCategory.ToArray(), ValueList = Constant.PayorCategory.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Plan Type", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "plantype", ItemType = "Dropdown", LabelList = Constant.PlanType.ToArray(), ValueList = Constant.PlanType.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "MVA Exclusions", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mvaexclusions", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "MVA Exclusions Additional Information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mvaexclusionsadditionalinfo", DependsOnCode = "mvaexclusions", DependsOnAssertValue = "Yes", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Other exclusion(s)", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherexclusions", ItemType = "Option", LabelList = Constant.YesNo.ToArray(), ValueList = Constant.YesNo.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Other exclusion(s) Additional Information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherexclusionsadditionalinfo", DependsOnCode = "otherexclusions", DependsOnAssertValue = "Yes", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Patient is in ESRD", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "patientinesrd", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "ESRD start date", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "esrdstartdate", DependsOnCode = "patientinesrd", DependsOnAssertValue = "Yes", ItemType = "Date" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Type of policy/product", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "typeofpolicyorproduct", ItemType = "Dropdown", LabelList = Constant.TypeOfPolicyProduct.ToArray(), ValueList = Constant.TypeOfPolicyProduct.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Type of policy/product additional information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "typeofpolicyorproductadditionalinfo", DependsOnCode = "typeofpolicyorproduct", DependsOnAssertValue = "Other", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Policy is secondary to ONLY Medicare", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "policyissecondaryonlytomedicare", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Label = "CEO approval required", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ceoapprovalrequired", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            section = model.Tabs
                       .Where(p => p.Label == "Payor Info").Single()
                       .Accordions[0].Sections.Where(p => p.Label == "Tertiary").Single().Accordions[0]
                       .Sections.Where(p => p.Label == "All").Single();

           
                


            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, Label = "Payor type", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payortype", ItemType = "Dropdown", LabelList = Constant.PrimaryPayorType.ToArray(), ValueList = Constant.PrimaryPayorType.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Label = "Additional Payor Information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "additionalpayorinfo", DependsOnCode = "payortype", DependsOnAssertValue = "Other", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, Label = "Payor Name", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payorname", ItemType = "Text", DependsOnCode = "payortype", DependsOnAssertValue = "Other" });
            section.Items.Add(new ItemModel() { Label = "Payor Category", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payorcategory", ItemType = "Dropdown", LabelList = Constant.PayorCategory.ToArray(), ValueList = Constant.PayorCategory.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Plan Type", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "plantype", ItemType = "Dropdown", LabelList = Constant.PlanType.ToArray(), ValueList = Constant.PlanType.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "MVA Exclusions", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mvaexclusions", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "MVA Exclusions Additional Information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mvaexclusionsadditionalinfo", DependsOnCode = "mvaexclusions", DependsOnAssertValue = "Yes", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Other exclusion(s)", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherexclusions", ItemType = "Option", LabelList = Constant.YesNo.ToArray(), ValueList = Constant.YesNo.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Other exclusion(s) Additional Information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherexclusionsadditionalinfo", DependsOnCode = "otherexclusions", DependsOnAssertValue = "Yes", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Patient is in ESRD", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "patientinesrd", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "ESRD start date", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "esrdstartdate", DependsOnCode = "patientinesrd", DependsOnAssertValue = "Yes", ItemType = "Date" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Type of policy/product", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "typeofpolicyorproduct", ItemType = "Dropdown", LabelList = Constant.TypeOfPolicyProduct.ToArray(), ValueList = Constant.TypeOfPolicyProduct.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Type of policy/product additional information", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "typeofpolicyorproductadditionalinfo", DependsOnCode = "typeofpolicyorproduct", DependsOnAssertValue = "Other", ItemType = "TextArea" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, Label = "Policy is secondary to ONLY Medicare", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "policyissecondaryonlytomedicare", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Label = "CEO approval required", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ceoapprovalrequired", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            //Primary Payor General
            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.payorinfo.primary.general").Single();



            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pists", Label = "Patient is the Subscriber", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "subscribername", Label = "Subscriber name", DependsOnCode = "pists", DependsOnAssertValue = "No" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "subscriberdob", Label = "Subscriber DOB if not patient", ItemType = "Date", DependsOnCode = "pists", DependsOnAssertValue = "No" });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "srthp", Label = "Subscribers relationship to the patient" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "phpdtchg", Label = "Patient has previously admitted to CHG", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", DependsOnCode = "phpdtchg", DependsOnAssertValue = "Yes" });
            //section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ivffieldscompleted", Label = "IVF Fields Completed", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "chgnetworkstatus", Label = "CHG Network Status", ItemType = "Dropdown", LabelList = Constant.CHGNetworkStatus.ToArray(), ValueList = Constant.CHGNetworkStatus.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "othernetworkstatus", Label = "Other", DependsOnCode = "chgnetworkstatus", DependsOnAssertValue = "Other" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pilihms", Label = "Payor is listed in HMS", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "policynumber", Label = "Policy Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "policyphonenumber", Label = "Phone Number", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "groupnumber", Label = "Group Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "groupname", Label = "Group Name", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dwhcwtp", Label = "Do we have a contract with this payor", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ivn", Label = "Insurance Verification Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "sctic", Label = "Sent Clinical to Insurance Company", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ipmtm", Label = "Is plan month to month?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "whopayspremium", Label = "Who pays the premium?" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wwtlpp", Label = "When was the last premium paid?" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pisingp", Label = "Plan is in grace period", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dwtgpe", Label = "Date when grace period expires", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wdtpen", Label = "When does the policy expire next?", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bvpn", Label = "Benefit Verification Phone Number", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "effectivedate", Label = "Effective Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "praisrfta", Label = "Pre-cert/auth is required for this admission", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pcicn", Label = "Pre-cert Insurance Contact Name" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pcipn", Label = "Pre-cert Insurance Phone Number", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cma", Label = "Company managing authorization" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationinitiation", Label = "Authorization Initiation", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pphlc", Label = "Plan/policy has LTACH coverage", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dutd", Label = "Days used to date", ItemType = "Number" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dtro", Label = "Days to recuperate on", ItemType = "Number" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pphsblfl", Label = "Plan/policy has specified benefits/limitations for LTACH", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "limitations", Label = "Limitations", ItemType = "Dropdown", LabelList = Constant.Limitations.ToArray(), ValueList = Constant.Limitations.ToArray(), DependsOnCode = "pphsblfl", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "limitationsbasis", Label = "Limitation basis", ItemType = "Dropdown", LabelList = Constant.LimitationBasis.ToArray(), ValueList = Constant.LimitationBasis.ToArray(), DependsOnCode = "pphsblfl", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ipltachdl", Label = "Inpatient/LTACH day limitation", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "daylimitation", Label = "Day limitation", ItemType = "Dropdown", LabelList = Constant.DayLimitation.ToArray(), ValueList = Constant.DayLimitation.ToArray(), DependsOnCode = "ipltachdl", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitmaximumcap", Label = "Benefit maximum cap", ItemType = "Dropdown", LabelList = Constant.BenefitMaximumCap.ToArray(), ValueList = Constant.BenefitMaximumCap.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "gbbse", Label = "Gastric bypass/bariatric surgery exclusions", ItemType = "Option", LabelList = Constant.YesNoInformationNotAvailable.ToArray(), ValueList = Constant.YesNoInformationNotAvailable.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitspaidt", Label = "Benefits paid at", ItemType = "Dropdown", LabelList = Constant.BenefitsPaitAt.ToArray(), ValueList = Constant.BenefitsPaitAt.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitspaidtrate", Label = "Benefits paid at value", ItemType = "Text" }); //Revisit type
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "outofpocket", Label = "Out of pocket", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "outofpocketmettodate", Label = "Out of pocket met to date", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ramoutofpocket", Label = "Reimbursement after maximum out of pocket", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "liftimebenefitmaximum", Label = "Lifetime benefit maximum", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "howmuchhasbeenpaidinclaimstodate", Label = "How much has been paid in claims to date", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "premiumpaidthroughdate", Label = "Premium paid through", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "preexistingclause", Label = "Pre-existing clause", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "waitingperioddate", Label = "Waiting period date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "preexistingcondition", Label = "Pre-existing condition", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitsquoted", Label = "Benefits Quoted", ItemType = "Dropdown", LabelList = Constant.BenefitsQuoted.ToArray(), ValueList = Constant.BenefitsQuoted.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "attempttogetauthorization", Label = "Attempt to get authorization", ItemType = "Table", LabelList = new string[] { "Date", "Time" }, CodeList = new string[] { "date", "time" }, TypeList = new string[] { "Date", "Time" } });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationnumber", Label = "Authorization Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationnotes", Label = "Authorization notes", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "claimsmailingaddressconfirmed", Label = "Claims mailing address confirmed", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mailingaddress", Label = "Mailing Address", ItemType = "TextArea", DependsOnCode = "claimsmailingaddressconfirmed", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "additionalverificationcomments", Label = "Additional Verification Comments", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodsusedtoattainauthorization", Label = "Method used to attain authorization", ItemType = "Dropdown", LabelList = Constant.AuthorizationMethods.ToArray(), ValueList = Constant.AuthorizationMethods.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "levelofcareauthorized", Label = "Level of Care Authorized", ItemType = "Dropdown", LabelList = Constant.LevelOfCareAuthorized.ToArray(), ValueList = Constant.LevelOfCareAuthorized.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationdenialapproved", Label = "Authorization Denial/Approved", ItemType = "Dropdown", LabelList = Constant.ApprovalStatusNA.ToArray(), ValueList = Constant.ApprovalStatusNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "numberofdaysauthorized", Label = "Number of Days Authorized", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationexpirationdate", Label = "Authorization expiration date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "planhasloapolicy", Label = "Plan has an LOA policy", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "whodeniedapproved", Label = "Who Denied/Approved", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "titleofthepersonwhodenied", Label = "Title of person who denied", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "denialreason", Label = "Denial Reason", ItemType = "Dropdown", LabelList = Constant.DenialReason.ToArray(), ValueList = Constant.DenialReason.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "clinformedreferralsourcephysicianofinsdenial", Label = "CL informed Referral Source/Physician of Ins. denial", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.payorinfo.secondary.general").Single();

            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pists", Label = "Patient is the Subscriber", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "subscribername", Label = "Subscriber name", DependsOnCode = "pists", DependsOnAssertValue = "No" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "subscriberdob", Label = "Subscriber DOB if not patient", ItemType = "Date", DependsOnCode = "pists", DependsOnAssertValue = "No" });
            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "srthp", Label = "Subscribers relationship to the patient" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "chgnetworkstatus", Label = "CHG Network Status", ItemType = "Dropdown", LabelList = Constant.CHGNetworkStatus.ToArray(), ValueList = Constant.CHGNetworkStatus.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "othernetworkstatus", Label = "Other", DependsOnCode = "chgnetworkstatus", DependsOnAssertValue = "Other" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "policynumber", Label = "Policy Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "policyphonenumber", Label = "Phone Number", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "groupnumber", Label = "Group Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ivn", Label = "Insurance Verification Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ipmtm", Label = "Is plan month to month?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "whopayspremium", Label = "Who pays the premium?" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wwtlpp", Label = "When was the last premium paid?" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pisingp", Label = "Plan is in grace period", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dwtgpe", Label = "Date when grace period expires", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wdtpen", Label = "When does the policy expire next?", ItemType = "Date" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bvpn", Label = "Benefit Verification Phone Number", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "effectivedate", Label = "Effective Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "praisrfta", Label = "Pre-cert/auth is required for this admission", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pcicn", Label = "Pre-cert Insurance Contact Name" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pcipn", Label = "Pre-cert Insurance Phone Number", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cma", Label = "Company managing authorization" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationinitiation", Label = "Authorization Initiation", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pphlc", Label = "Plan/policy has LTACH coverage", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dutd", Label = "Days used to date", ItemType = "Number" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dtro", Label = "Days to recuperate on", ItemType = "Number" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pphsblfl", Label = "Plan/policy has specified benefits/limitations for LTACH", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "limitations", Label = "Limitations", ItemType = "Dropdown", LabelList = Constant.Limitations.ToArray(), ValueList = Constant.Limitations.ToArray(), DependsOnCode = "pphsblfl", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "limitationsbasis", Label = "Limitation basis", ItemType = "Dropdown", LabelList = Constant.LimitationBasis.ToArray(), ValueList = Constant.LimitationBasis.ToArray(), DependsOnCode = "pphsblfl", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ipltachdl", Label = "Inpatient/LTACH day limitation", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "daylimitation", Label = "Day limitation", ItemType = "Dropdown", LabelList = Constant.DayLimitation.ToArray(), ValueList = Constant.DayLimitation.ToArray(), DependsOnCode = "ipltachdl", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitmaximumcap", Label = "Benefit maximum cap", ItemType = "Dropdown", LabelList = Constant.BenefitMaximumCap.ToArray(), ValueList = Constant.BenefitMaximumCap.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "gbbse", Label = "Gastric bypass/bariatric surgery exclusions", ItemType = "Option", LabelList = Constant.YesNoInformationNotAvailable.ToArray(), ValueList = Constant.YesNoInformationNotAvailable.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitspaidt", Label = "Benefits paid at", ItemType = "Dropdown", LabelList = Constant.BenefitsPaitAt.ToArray(), ValueList = Constant.BenefitsPaitAt.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitspaidtrate", Label = "Benefits paid at value", ItemType = "Text" }); //Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "outofpocket", Label = "Out of pocket", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "outofpocketmettodate", Label = "Out of pocket met to date", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ramoutofpocket", Label = "Reimbursement after maximum out of pocket", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "liftimebenefitmaximum", Label = "Lifetime benefit maximum", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "howmuchhasbeenpaidinclaimstodate", Label = "How much has been paid in claims to date", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "premiumpaidthroughdate", Label = "Premium paid through", ItemType = "Date" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "preexistingclause", Label = "Pre-existing clause", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "waitingperioddate", Label = "Waiting period date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "preexistingcondition", Label = "Pre-existing condition", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitsquoted", Label = "Benefits Quoted", ItemType = "Dropdown", LabelList = Constant.BenefitsQuoted.ToArray(), ValueList = Constant.BenefitsQuoted.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationnumber", Label = "Authorization Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationnotes", Label = "Authorization notes", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "claimsmailingaddressconfirmed", Label = "Claims mailing address confirmed", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "additionalverificationcomments", Label = "Additional Verification Comments", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "levelofcareauthorized", Label = "Level of Care Authorized", ItemType = "Dropdown", LabelList = Constant.LevelOfCareAuthorized.ToArray(), ValueList = Constant.LevelOfCareAuthorized.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationdenialapproved", Label = "Authorization Denial/Approved", ItemType = "Dropdown", LabelList = Constant.ApprovalStatusNA.ToArray(), ValueList = Constant.ApprovalStatusNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "numberofdaysauthorized", Label = "Number of Days Authorized", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationexpirationdate", Label = "Authorization expiration date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "denialreason", Label = "Denial Reason", ItemType = "Dropdown", LabelList = Constant.DenialReason.ToArray(), ValueList = Constant.DenialReason.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "clinformedreferralsourcephysicianofinsdenial", Label = "CL informed Referral Source/Physician of Ins. denial", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });



            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.payorinfo.tertiary.general").Single();

            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pists", Label = "Patient is the Subscriber", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "subscribername", Label = "Subscriber name", DependsOnCode = "pists", DependsOnAssertValue = "No" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "subscriberdob", Label = "Subscriber DOB if not patient", ItemType = "Date", DependsOnCode = "pists", DependsOnAssertValue = "No" });
            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "srthp", Label = "Subscribers relationship to the patient" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "chgnetworkstatus", Label = "CHG Network Status", ItemType = "Dropdown", LabelList = Constant.CHGNetworkStatus.ToArray(), ValueList = Constant.CHGNetworkStatus.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "othernetworkstatus", Label = "Other", DependsOnCode = "chgnetworkstatus", DependsOnAssertValue = "Other" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "policynumber", Label = "Policy Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "policyphonenumber", Label = "Phone Number", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "groupnumber", Label = "Group Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ivn", Label = "Insurance Verification Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ipmtm", Label = "Is plan month to month?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "whopayspremium", Label = "Who pays the premium?" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wwtlpp", Label = "When was the last premium paid?" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pisingp", Label = "Plan is in grace period", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dwtgpe", Label = "Date when grace period expires", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wdtpen", Label = "When does the policy expire next?", ItemType = "Date" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bvpn", Label = "Benefit Verification Phone Number", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "effectivedate", Label = "Effective Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { SNF = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "praisrfta", Label = "Pre-cert/auth is required for this admission", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pcicn", Label = "Pre-cert Insurance Contact Name" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pcipn", Label = "Pre-cert Insurance Phone Number", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cma", Label = "Company managing authorization" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationinitiation", Label = "Authorization Initiation", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pphlc", Label = "Plan/policy has LTACH coverage", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dutd", Label = "Days used to date", ItemType = "Number" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dtro", Label = "Days to recuperate on", ItemType = "Number" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pphsblfl", Label = "Plan/policy has specified benefits/limitations for LTACH", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "limitations", Label = "Limitations", ItemType = "Dropdown", LabelList = Constant.Limitations.ToArray(), ValueList = Constant.Limitations.ToArray(), DependsOnCode = "pphsblfl", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "limitationsbasis", Label = "Limitation basis", ItemType = "Dropdown", LabelList = Constant.LimitationBasis.ToArray(), ValueList = Constant.LimitationBasis.ToArray(), DependsOnCode = "pphsblfl", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ipltachdl", Label = "Inpatient/LTACH day limitation", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "daylimitation", Label = "Day limitation", ItemType = "Dropdown", LabelList = Constant.DayLimitation.ToArray(), ValueList = Constant.DayLimitation.ToArray(), DependsOnCode = "ipltachdl", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitmaximumcap", Label = "Benefit maximum cap", ItemType = "Dropdown", LabelList = Constant.BenefitMaximumCap.ToArray(), ValueList = Constant.BenefitMaximumCap.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "gbbse", Label = "Gastric bypass/bariatric surgery exclusions", ItemType = "Option", LabelList = Constant.YesNoInformationNotAvailable.ToArray(), ValueList = Constant.YesNoInformationNotAvailable.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitspaidt", Label = "Benefits paid at", ItemType = "Dropdown", LabelList = Constant.BenefitsPaitAt.ToArray(), ValueList = Constant.BenefitsPaitAt.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitspaidtrate", Label = "Benefits paid at value", ItemType = "Text" }); //Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "outofpocket", Label = "Out of pocket", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "outofpocketmettodate", Label = "Out of pocket met to date", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ramoutofpocket", Label = "Reimbursement after maximum out of pocket", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "liftimebenefitmaximum", Label = "Lifetime benefit maximum", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "howmuchhasbeenpaidinclaimstodate", Label = "How much has been paid in claims to date", ItemType = "Text" });//Revisit type
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "premiumpaidthroughdate", Label = "Premium paid through", ItemType = "Date" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "preexistingclause", Label = "Pre-existing clause", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "waitingperioddate", Label = "Waiting period date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "preexistingcondition", Label = "Pre-existing condition", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "benefitsquoted", Label = "Benefits Quoted", ItemType = "Dropdown", LabelList = Constant.BenefitsQuoted.ToArray(), ValueList = Constant.BenefitsQuoted.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationnumber", Label = "Authorization Number", ItemType = "Text" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationnotes", Label = "Authorization notes", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "claimsmailingaddressconfirmed", Label = "Claims mailing address confirmed", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "additionalverificationcomments", Label = "Additional Verification Comments", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "levelofcareauthorized", Label = "Level of Care Authorized", ItemType = "Dropdown", LabelList = Constant.LevelOfCareAuthorized.ToArray(), ValueList = Constant.LevelOfCareAuthorized.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationdenialapproved", Label = "Authorization Denial/Approved", ItemType = "Dropdown", LabelList = Constant.ApprovalStatusNA.ToArray(), ValueList = Constant.ApprovalStatusNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "numberofdaysauthorized", Label = "Number of Days Authorized", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationexpirationdate", Label = "Authorization expiration date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "denialreason", Label = "Denial Reason", ItemType = "Dropdown", LabelList = Constant.DenialReason.ToArray(), ValueList = Constant.DenialReason.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "clinformedreferralsourcephysicianofinsdenial", Label = "CL informed Referral Source/Physician of Ins. denial", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });






            //Primary Payor Medicare
            section = model.Tabs
                .Where(p => p.Label == "Payor Info").Single()
                .Accordions[0].Sections.Where(p => p.Label == "Primary").Single().Accordions[0]
                .Sections.Where(p => p.Label == "Medicare").Single();

            section.Items.Add(new ItemModel() { SNF = true, SSSN=true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "inpatientdaysorsnfdays", Label = "Inpatient days/SNF Days", ItemType = "Number", Step = 1 });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "medicarepartaifapplicable", Label = "Medicare Part A (if applicable)", ItemType = "Dropdown", LabelList = Constant.MedicarePartA.ToArray(), ValueList = Constant.MedicarePartA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "medicarepartbifapplicable", Label = "Medicare Part B (if applicable)", ItemType = "Dropdown", LabelList = Constant.MedicarePartB.ToArray(), ValueList = Constant.MedicarePartB.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "precertorauthisrequiredifmedicareexhausts", Label = "Pre-cert/auth is required if/when Medicare exhausts", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "medicaredaysleftstartdate", Label = "Medicare Days Left Start Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "medicaredaysleftenddate", Label = "Medicare Days Left End Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "medicaredaysleft", Label = "Medicare Days Left", ItemType = "Text", Disabled = true, IsCalculated = true });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dateofmedicaredatecalculation", Label = "Date of Medicare Date Calculation", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "patientmedicatefulldays", Label = "Patient Medicare Full Days", ItemType = "Number", Step = 1 });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "patientmedicarecodays", Label = "Patient Medicare Co-Days", ItemType = "Number", Step = 1 });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "patientmedicareltrdays", Label = "Patient Medicare LTR Days", ItemType = "Number", Step = 1 });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "secondreviewcompleted", Label = "Second review completed", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherexclusionsadditionalinfo", Label = "CAC team member who completed Second review", ItemType = "Dropdown", LabelList = _contextLists["CAC_USERS"].LabelList, ValueList = _contextLists["CAC_USERS"].ValueList });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cacteammemberwhocompletedsecondreview", Label = "Method of verification", ItemType = "Dropdown", LabelList = Constant.MethodOfVerification.ToArray(), ValueList = Constant.MethodOfVerification.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformation", Label = "Method Of Verification Additional Information", ItemType = "TextArea", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Other" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationname", Label = "Name", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationtitle", Label = "Title", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationreference", Label = "Reference", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationdate", Label = "Date", ItemType = "Date", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationtime", Label = "Time", ItemType = "Time", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "senttomaximus", Label = "Sent to Maximus", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "datetimesenttomaximus", ItemType = "Date", Label = "Date/Time sent to Maximus" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "resultofmaximus", Label = "Result of Maximus", ItemType = "Dropdown", LabelList = Constant.ResultOfMaximus.ToArray(), ValueList = Constant.ResultOfMaximus.ToArray() });




            //Primary Payor Medicare
            section = model.Tabs
                .Where(p => p.Label == "Payor Info").Single()
                .Accordions[0].Sections.Where(p => p.Label == "Secondary").Single().Accordions[0]
                .Sections.Where(p => p.Label == "Medicare").Single();

            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "inpatientdaysorsnfdays", Label = "Inpatient days/SNF Days", ItemType = "Number", Step = 1 });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "medicarepartaifapplicable", Label = "Medicare Part A (if applicable)", ItemType = "Dropdown", LabelList = Constant.MedicarePartA.ToArray(), ValueList = Constant.MedicarePartA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "medicarepartbifapplicable", Label = "Medicare Part B (if applicable)", ItemType = "Dropdown", LabelList = Constant.MedicarePartB.ToArray(), ValueList = Constant.MedicarePartB.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "precertorauthisrequiredifmedicareexhausts", Label = "Pre-cert/auth is required if/when Medicare exhausts", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "secondreviewcompleted", Label = "Second review completed", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherexclusionsadditionalinfo", Label = "CAC team member who completed Second review", ItemType = "Dropdown", LabelList = _contextLists["CAC_USERS"].LabelList, ValueList = _contextLists["CAC_USERS"].ValueList });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cacteammemberwhocompletedsecondreview", Label = "Method of verification", ItemType = "Dropdown", LabelList = Constant.MethodOfVerification.ToArray(), ValueList = Constant.MethodOfVerification.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformation", Label = "Method Of Verification Additional Information", ItemType = "TextArea", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Other" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationname", Label = "Name", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationtitle", Label = "Title", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationreference", Label = "Reference", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationdate", Label = "Date", ItemType = "Date", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationtime", Label = "Time", ItemType = "Time", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //



            //Primary Payor Medicare
            section = model.Tabs
                .Where(p => p.Label == "Payor Info").Single()
                .Accordions[0].Sections.Where(p => p.Label == "Tertiary").Single().Accordions[0]
                .Sections.Where(p => p.Label == "Medicare").Single();

            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "inpatientdaysorsnfdays", Label = "Inpatient days/SNF Days", ItemType = "Number", Step = 1 });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "medicarepartaifapplicable", Label = "Medicare Part A (if applicable)", ItemType = "Dropdown", LabelList = Constant.MedicarePartA.ToArray(), ValueList = Constant.MedicarePartA.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "medicarepartbifapplicable", Label = "Medicare Part B (if applicable)", ItemType = "Dropdown", LabelList = Constant.MedicarePartB.ToArray(), ValueList = Constant.MedicarePartB.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "precertorauthisrequiredifmedicareexhausts", Label = "Pre-cert/auth is required if/when Medicare exhausts", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "secondreviewcompleted", Label = "Second review completed", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherexclusionsadditionalinfo", Label = "CAC team member who completed Second review", ItemType = "Dropdown", LabelList = _contextLists["CAC_USERS"].LabelList, ValueList = _contextLists["CAC_USERS"].ValueList });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cacteammemberwhocompletedsecondreview", Label = "Method of verification", ItemType = "Dropdown", LabelList = Constant.MethodOfVerification.ToArray(), ValueList = Constant.MethodOfVerification.ToArray() });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformation", Label = "Method Of Verification Additional Information", ItemType = "TextArea", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Other" });
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationname", Label = "Name", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationtitle", Label = "Title", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationreference", Label = "Reference", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationdate", Label = "Date", ItemType = "Date", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //
            section.Items.Add(new ItemModel() { IsIVFField = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofverificationadditionalinformationtime", Label = "Time", ItemType = "Time", DependsOnCode = "cacteammemberwhocompletedsecondreview", DependsOnAssertValue = "Phone" }); //







            //Primary Payor SCA
            section = model.Tabs
                .Where(p => p.Label == "Payor Info").Single()
                .Accordions[0].Sections.Where(p => p.Label == "Primary").Single().Accordions[0]
                .Sections.Where(p => p.Label == "SCA").Single();

            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "scarequested", Label = "SCA Requested", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payorwillengageinsca", Label = "Payor will engage in SCA", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mcteammemberwhowillcomplete", Label = "Managed Care Team member who will complete the SCA", ItemType = "Dropdown", LabelList = _contextLists["MC_USERS"].LabelList, ValueList = _contextLists["MC_USERS"].ValueList });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "scapayor", Label = "SCA Payor", ItemType = "Dropdown", LabelList = Constant.PayorNames.ToArray(), ValueList = Constant.PayorNames.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "scapayorother", Label = "SCA Payor Additional Information", ItemType = "TextArea", DependsOnCode = "scapayor", DependsOnAssertValue = "Other" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payorscapoc", Label = "Payor SCA Point of Contact", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "payorscapocphone", Label = "Payor SCA Point of Contact Phone #", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "timedateofcontactwithpayorsca", Label = "Time/Date of contact with Payor SCA Point of Contact", ItemType = "Table", LabelList = new string[] { "Date", "Time" }, CodeList = new string[] { "date", "time" }, TypeList = new string[] { "Date", "Time" } });


            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "scasubmittedtopayor", Label = "SCA Submitted to Payor", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "scaapprovaldenial", Label = "SCA Approval/Denial", ItemType = "Dropdown", LabelList = Constant.ApprovalStatus.ToArray(), ValueList = Constant.ApprovalStatus.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "whofrompayporapproveddeniedsca", Label = "Who from Payor denied/approved SCA", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "titleofwhoapproveddeniedsca", Label = "Title of who from Payer denied/approved SCA", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "scadate", Label = "SCA Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "scadays", Label = "SCA Days", ItemType = "Number", Step = 1 });



            //Primary Payor Peer-to-Peer
            section = model.Tabs
                .Where(p => p.Label == "Payor Info").Single()
                .Accordions[0].Sections.Where(p => p.Label == "Primary").Single().Accordions[0]
                .Sections.Where(p => p.Label == "Peer-to-Peer").Single();

            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "initiatingpeertopeer", Label = "Initiating Peer-to-Peer", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "stachphysicianmakingpeertopeercall", Label = "STACH Physician Making Peer-to-Peer call (or Physician making call on their behalf)", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS"].ValueList });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "phonenumberofstachp2pcall", Label = "Phone Number of STACH Physician making P2P call", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "insurancepayer", Label = "Insurance (Payer) Physician receiving the Peer-to-Peer call", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_CASE_MANAGER"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_CASE_MANAGER"].ValueList });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "phonenumberofinsurancepayer", Label = "Phone Number of Insurance Physician receiving P2P call", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bestdatep2p", Label = "Best date for Peer-to-Peer call", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "besttimep2p", Label = "Best time for Peer-to-Peer call", ItemType = "Time" });

            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "whosetupp2pcall", Label = "Who set up the Peer to Peer call", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "p2pdatecompleted", Label = "Peer-to-Peer call date completed", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "p2ptimecompleted", Label = "Peer-to-Peer call time completed", ItemType = "Time" });

            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "p2presult", Label = "Peer to Peer - Result", ItemType = "Dropdown", LabelList = Constant.ApprovalStatus.ToArray(), ValueList = Constant.ApprovalStatus.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "denialreasonp2p", Label = "Denial Reason for peer to peer", ItemType = "TextArea" });




            //Primary Payor Expedited Appeal
            section = model.Tabs
                .Where(p => p.Label == "Payor Info").Single()
                .Accordions[0].Sections.Where(p => p.Label == "Primary").Single().Accordions[0]
                .Sections.Where(p => p.Label == "Expedited Appeal").Single();

            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "initiatingexpeditedappeal", Label = "Initiating Expedited Appeal", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "datesenttoea", Label = "Date sent to EA (expedited appeal)", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "timesenttoea", Label = "Time sent to EA (expedited appeal)", ItemType = "Time" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "attempttogetea", Label = "Attempt to get Expedited Appeal approved", ItemType = "Table", LabelList = new string[] { "Date", "Time" }, CodeList = new string[] { "date", "time" }, TypeList = new string[] { "Date", "Time" } });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "earesult", Label = "Expedited Appeal - Result", ItemType = "Dropdown", LabelList = Constant.ApprovalStatus.ToArray(), ValueList = Constant.ApprovalStatus.ToArray() });


            //Primary Payor Disposition
            section = model.Tabs
                .Where(p => p.Label == "Payor Info").Single()
                .Accordions[0].Sections.Where(p => p.Label == "Primary").Single().Accordions[0]
                .Sections.Where(p => p.Label == "Disposition").Single();

            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "preadmissionconfirmation", Label = "Pre-Admission Confirmation", ItemType = "Dropdown", LabelList = Constant.DispositionTypes.ToArray(), ValueList = Constant.DispositionTypes.ToArray() });
             


            //Admission Info

            section = model.Tabs
                .Where(p => p.Label == "Admission Info").Single()
                .Accordions[0].Sections.Where(p => p.Label == "General").Single();

            #region "Admission Info"
         
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dateofadmission", Label = "Date of Admission", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "timeofadmission", Label = "Time of Admission", ItemType = "Time" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hmsencounter", Label = "HMS Encounter #" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cmgadmittingmd", Label = "CHG Admitting MD" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "inpatientdayssnf", Label = "Inpatient days/SNF Days", ItemType = "Number", Step = 1 });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "roomnumber", Label = "Room Number" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wgmot", Label = "Who gave the MOT" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wacitm", Label = "Who at CHG issued the MOT", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_MOT"].LabelList, ValueList = _contextLists["CONTEXT_MOT"].ValueList });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dwtmwg", Label = "Date when the MOT was given", ItemType = "Date" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "denial", Label = "Denial", ItemType = "Dropdown", LabelList = Constant.Denial.ToArray(), ValueList = Constant.Denial.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nonadmit", Label = "Non Admit", ItemType = "Dropdown", LabelList = Constant.NoAdmit.ToArray(), ValueList = Constant.NoAdmit.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "sentrigthfax", Label = "Sent rightfax to insurance CM with POC info for our CM", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            #endregion

            section = model.Tabs
            .Where(p => p.Label == "PS Referral Source Info").Single()
            .Accordions[0].Sections.Where(p => p.Label == "General").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "chgdestination", Label = "CHG Destination", ItemType = "Dropdown", LabelList = _contextLists["USER_SITES"].LabelList, ValueList = _contextLists["USER_SITES"].ValueList, Disabled = true });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dateofreferral", Label = "Date of Referral", ItemType = "Date", Disabled = true });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "timeofprescreen", Label = "Time pre-screening tool was complete", ItemType = "Text", Disabled = true });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "timeprescreenremaining", Label = "Time remaining until Pre-screen expiration", ItemType = "Text", Disabled = true, IsCalculated = true });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "liaison", Label = "Name of Liaison", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_LIASONS"].LabelList, ValueList = _contextLists["CONTEXT_LIASONS"].ValueList, Disabled = true });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "admissioncontroller", Label = "Name of Admissions Coordinator", ItemType = "Dropdown", LabelList = _contextLists["CAC_USERS"].LabelList, ValueList = _contextLists["CAC_USERS"].ValueList });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "authorizationspecialist", Label = "Name of Authorization Specialist", ItemType = "Dropdown", LabelList = _contextLists["CAC_USERS"].LabelList, ValueList = _contextLists["CAC_USERS"].ValueList });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "stachadmissiondate", Label = "STACH Admission Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "inquiryrequest", Label = "Inquiry Request", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "jocscreeningrequest", Label = "JOC Screening Request", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nonjocscreeningrequest", Label = "Non-JOC Screening Request", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "stachmrnrequired", Label = "STACH MRN Required", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "stachmrn", Label = "STACH MRN", DependsOnCode = "stachmrnrequired", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "referralstatus", Label = "Referral Status", ItemType = "Text", Disabled = true, IsCalculated = true });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "clinicalstatus", Label = "Clinical Status", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "referringsourcetype", Label = "Referring Source Type", ItemType = "Dropdown", LabelList = Constant.ReferringSourceTypes.ToArray(), ValueList = Constant.ReferringSourceTypes.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "referralsourcename", Label = "Referral Source Name", IsCalculated = true, Disabled = true });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "referringfacilityunit", Label = "Referring Facility Unit", ItemType = "Text" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "clinicalisattached", Label = "Clinical is Attached", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "marisattached", Label = "MAR is Attached", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofreceivingbusinessrequest", Label = "Method of receiving the business request", ItemType = "Dropdown", LabelList = Constant.MethodOfBusinessRequest.ToArray(), ValueList = Constant.MethodOfBusinessRequest.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofreceivingbusinessrequestother", Label = "Other", ItemType = "TextArea", DependsOnCode = "methodofreceivingbusinessrequest", DependsOnAssertValue = "Other" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wgytb", Label = "Who gave you the business (Green)", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_REFERRING"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_REFERRING"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "witb", Label = "Who influenced the business (Yellow)", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_INFLUENCER"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_INFLUENCER"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pulmonaryconsult", Label = "Pulmonary Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_PULMONARY"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_PULMONARY"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "criticalcareconsult", Label = "Critical Care Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_CRITICAL_CARE"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_CRITICAL_CARE"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "woundconsult", Label = "Wound Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_WOUND"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_WOUND"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "orthopedicconsult", Label = "Orthopedic Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_ORTHOPEDIC"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_ORTHOPEDIC"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "interradiologyconsult", Label = "Interventional Radiology Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_INTERVENTIONAL_RADIOLOGY"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_INTERVENTIONAL_RADIOLOGY"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cardiologyconsult", Label = "Cardiology Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_CARDIOLOGY"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_CARDIOLOGY"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "idc", Label = "Infectious Disease Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_INFECTIOUS_DISEASE"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_INFECTIOUS_DISEASE"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nephrologyconsuly", Label = "Nephrology Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_NEPHROLOGY"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_NEPHROLOGY"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "surgeryconsult", Label = "Surgery Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_SURGERY"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_SURGERY"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "psychiatryconsult", Label = "Psychiatry Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_PSYCHIATRY"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_PSYCHIATRY"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "neurologyconsult", Label = "Neurology Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_NEUROLOGY"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_NEUROLOGY"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "gastroenterolgyconsult", Label = "Gastroenterology Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_GASTROENTEROLOGY"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_GASTROENTEROLOGY"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "urologyconsult", Label = "Urology Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_UROLOGY"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_UROLOGY"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "oncologyconsult", Label = "Oncology / Hemotology Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_ONCOLOGY"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_ONCOLOGY"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "internalmedicineconsult", Label = "Internal Medicine Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_INTERNAL_MEDICINE"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_INTERNAL_MEDICINE"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "entconsult", Label = "ENT Consult", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_ENT"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_ENT"].ValueList });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "associatedother", Label = "Associated Other", ItemType = "Dropdown", LabelList = _contextLists["CONTEXT_RS_CONTACTS_OTH"].LabelList, ValueList = _contextLists["CONTEXT_RS_CONTACTS_OTH"].ValueList });
            //section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "othercontactperson", Label = "Other Contact Person" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "phonenumber", Label = "Phone Number", ItemType = "Phone" });
            //section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "lisaoniscompleting", Label = "Liaision is completing the pre-screen, assigned to this referral source?" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dateadmittedtofacility", Label = "Date Admitted to Facility for this stay", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wasthepatientinicu", Label = "Was the patient in the ICU/CCU/Stepdown during stay?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wasthereferralgivenfromicu", Label = "Was the referral given from the ICU/CCU/Stepdown?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            //section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "datesinicuccu", Label = "Dates in the ICU/CCU/Stepdown at midnight", ItemType = "Table", LabelList = new string[] { "Date" }, CodeList = new string[] { "date" }, TypeList = new string[] { "Date" } });
            //section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "totalmidnights", Label = "Total midnights in ICU/CCU/Stepdown" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "midnightdates", Label = "Total midnights in ICU/CCU/Stepdown", ItemType = "Table", LabelList = new string[] { "Date", "Floor" }, CodeList = new string[] { "date", "floor" }, TypeList = new string[] { "Date", "Text" } });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "qualifiesforltcduetomidnight", Label = "Qualifies for LTC due to midnights", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "qualifiesforltcdueto200", Label = "Qualifies for LTC due to 200 rev codes", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "qualifiesforltcduetopayor", Label = "Qualifies for LTC due to Payor", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ltc", Label = "LTC", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "methodofconfirmation", Label = "Method of confirmation", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "liasonmetinpersonwithpatient", Label = "Liaison met in person with patient", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "liasonmetinpersonwithpatientandfamily", Label = "Liaison met in person with patient and family during referral (decision makers)", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "notesfrommeeting", Label = "Notes from meeting with patient and/or family", ItemType = "TextArea" });


            section = model.Tabs
       .Where(p => p.Label == "PS Demographics").Single()
       .Accordions[0].Sections.Where(p => p.Label == "General").Single();

            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "firstname", Label = "First Name" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "lastname", Label = "Last Name" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "middleinitial", Label = "Middle Initial" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "address", Label = "Address" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "city", Label = "City" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "state", Label = "State" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "zip", Label = "Zip Code" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "phone", Label = "Phone Number", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dob", Label = "Date of Birth", ItemType = "Date" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "gender", Label = "Gender", ItemType = "Dropdown", LabelList = Constant.Gender.ToArray(), ValueList = Constant.Gender.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "race", Label = "Race", ItemType = "Dropdown", LabelList = Constant.Race.ToArray(), ValueList = Constant.Race.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ethnicity", Label = "Ethnicity", ItemType = "Dropdown", LabelList = Constant.Ethnicity.ToArray(), ValueList = Constant.Ethnicity.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "maritalstatus", Label = "Marital Status", ItemType = "Dropdown", LabelList = Constant.MaritalStatus.ToArray(), ValueList = Constant.MaritalStatus.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "religiousconsideration", Label = "Religious/Cultural Consideration", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "primarylanguage", Label = "Primary Language", ItemType = "Dropdown", LabelList = Constant.PrimaryLanguage.ToArray(), ValueList = Constant.PrimaryLanguage.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "secondarylanguage", Label = "Secondary Language", ItemType = "Dropdown", LabelList = Constant.SecondaryLanguage.ToArray(), ValueList = Constant.SecondaryLanguage.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nextofkin", Label = "Next of Kin" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "relationship", Label = "Relationship", ItemType = "Dropdown", LabelList = Constant.Relationship.ToArray(), ValueList = Constant.Relationship.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "contactnumber", Label = "Contact Number", ItemType = "Phone" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "advancedirectives", Label = "Advance Directives", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "codestatus", Label = "Code Status" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "poa", Label = "Power of Attorney", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "poaname", Label = "Name", DependsOnCode = "poa", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "poarelationship", Label = "Relationship", DependsOnCode = "poa", DependsOnAssertValue = "Yes", ItemType = "Dropdown", LabelList = Constant.Relationship.ToArray(), ValueList = Constant.Relationship.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "poaphonenumber", Label = "Phone Number", ItemType = "Phone", DependsOnCode = "poa", DependsOnAssertValue = "Yes" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.prehospitalliving").Single();
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "phl", Label = "Pre-Hospital Living", ItemType = "Dropdown", LabelList = Constant.PreHospitalLiving.ToArray(), ValueList = Constant.PreHospitalLiving.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherInfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "phl", DependsOnAssertValue = "Other" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.languagecommunicationneeds").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "phl", Label = "Preferred Language", ItemType = "Dropdown", LabelList = Constant.PrimaryLanguage.ToArray(), ValueList = Constant.PrimaryLanguage.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dtpse", Label = "Does the patient speak English?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dtpni", Label = "Does the patient need an interpreter?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.preadmission").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cmsophopi", Label = "Current Medical Status of Patient - History of present illness", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "primarydiagnosis", Label = "Primary Diagnosis", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cmsopfa", Label = "Current Medical Status of Patient -For Admission", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "secondarydiagnosis", Label = "Secondary Diagnosis", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "acmeoamaiomp", Label = "Additional Co - morbidities (expand on active management and impact of medical progress)", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "prlof", Label = "Patient Prior Level of Function", ItemType = "Text" });
            section.Items.Add(new ItemModel() { SSSN = true, AcuityICU = true, AcuityMed = true, AcuityINT = true, AcuityAsserValue = "Yes", KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pr24hcoan", Label = "Patient requires 24 hour care of a nurse", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "prdpv", Label = "Patient Requires Daily Physician Visits", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ptpcras", Label = "Primary Treatment Plan - Clinical Rationale and Summary", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pracsb", Label = "Potential risks and complications (skin breakdown: RN to assess and follow / DVT / PE risk / Seizures)", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ploi", Label = "Planned Level of Improvement", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "elos", Label = "Expected Length of Stay", ItemType = "Number" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "apdsatin", Label = "Anticipated post-discharge setting and treatment if needed", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "panaoetcbhaalloc", Label = "Patient acute needs and / or expensive treatments cannot be handled at a lesser level of care", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SSSN = true, KeyClinicalIndicator = true, KeyClinicalIndicatorAssertValue = "Yes", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pfaalloc", Label = "Patient failed at a lesser level of care ", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, KeyClinicalIndicatorAssertValue = "No|N/A", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "itataalloc", Label = "Is there appropriate treatment at a lower level of care", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pmh", Label = "Past Medical History", ItemType = "TextArea" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.listallacute").Single();
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "laachdtl60d", Label = "Admits", ItemType = "Table", LabelList = new string[] { "Facility", "Reason for Admit", "Admit Date", "Discharge Date" }, CodeList = new string[] { "facility", "reasonforadmit", "admitdate", "dischargedate" }, TypeList = new string[] { "Text", "Text", "Date", "Date" } });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.surgery").Single();
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "surgeries", Label = "Surgeries", ItemType = "Table", LabelList = new string[] { "Surgery", "Date" }, CodeList = new string[] { "surgery", "date" }, TypeList = new string[] { "Text", "Date" } });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.socialhistory").Single();
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "alcoholhistory", Label = "Alcohol History", ItemType = "Option", LabelList = Constant.YesNoUnspecified.ToArray(), ValueList = Constant.YesNoUnspecified.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "tobaccohistory", Label = "Tobacco History", ItemType = "Option", LabelList = Constant.YesNoUnspecified.ToArray(), ValueList = Constant.YesNoUnspecified.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "druguse", Label = "Drug Use", ItemType = "Option", LabelList = Constant.YesNoUnspecified.ToArray(), ValueList = Constant.YesNoUnspecified.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "suicidalideation", Label = "Suicidal Ideation", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.ivaccess").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "peripheral", Label = "Peripheral", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "picc", Label = "PICC", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "centralline", Label = "Central Line", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherInfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "other", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ivl", Label = "IV Location", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ivd", Label = "IV Insertion Date", ItemType = "Date" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.ivfluids").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "notf", Label = "Name of Fluid / Medication(Vanco)", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "duration", Label = "Duration", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "frequency", Label = "Frequency", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pmwivmp", Label = "Pain management with IV medications / PCA", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.infection").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "none", Label = "None", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mrsa", Label = "MRSA", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cdiff", Label = "C - Diff", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "vre", Label = "VRE", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "acinetobacter", Label = "Acinetobacter", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mrdo", Label = "MRDO", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherInfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "other", DependsOnAssertValue = "Yes" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.isolationtype").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "contact", Label = "Contact", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "droplet", Label = "Droplet", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "airborne", Label = "Airborne", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ifmvciihaon", Label = "Isolation (for MRSA, VRE, CNS infection, immunocompromised host, AFB + or neutropenia)", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherInfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "other", DependsOnAssertValue = "Yes" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.diet").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "regulardiet", Label = "Regular Diet", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, AcuityICU = true, AcuityMed = true, AcuityINT = true, AcuityAsserValue = "Yes", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "specialdiet", Label = "Special Diet", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "diabetic", Label = "Diabetic", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "renal", Label = "Renal", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cardiac", Label = "Cardiac", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherInfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "other", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mds", Label = "Modified Diet/ Supervision", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "f11", Label = "Feeder(1:1 with feeding)", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mbs", Label = "MBS", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mbsdate", Label = "MBS Date", ItemType = "Date", DependsOnCode = "mbs", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, AcuityICU = true, AcuityMed = true, AcuityINT = true, AcuityAsserValue = "Yes", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "tpf", Label = "Tube / Parenteral Feeding", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, AcuityICU = true, AcuityMed = true, AcuityINT = true, AcuityAsserValue = "Yes", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "peg", Label = "PEG", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SSSN = true, AcuityICU = true, AcuityMed = true, AcuityINT = true, AcuityAsserValue = "Yes", KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ngtube", Label = "NG Tube", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, AcuityMed = true, AcuityAsserValue = "Yes", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dobhoff", Label = "Dobhoff", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "aam", Label = "Actively address malnutrition(24 hour calorie count, PEG placement etc)", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "albumin", Label = "Albumin / Pre - Albumin", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "formula", Label = "Formula", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "rate", Label = "Rate", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "waterflush", Label = "Water Flush", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, AcuityICU = true, AcuityAsserValue = "Yes", KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ppntpn", Label = "PPN / TPN", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "rate2", Label = "Rate", ItemType = "TextArea" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.bladder").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "continent", Label = "Continent", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "incontinent", Label = "Incontinent", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "anuric", Label = "Anuric", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "urostomy", Label = "Urostomy", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "unspecified", Label = "Unspecified", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "biwa", Label = "Bladder irrigation with antibiotic solution", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cbi", Label = "Continuous Balls irrigation", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.bladderappliance").Single();

            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "catheter", Label = "Catheter", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "catheterdate", Label = "Date", ItemType = "Date", DependsOnCode = "catheter", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ic", Label = "Intermittent catheter", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherInfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "other", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "u24ho", Label = "24 hour urine output", ItemType = "Number" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "notes", Label = "Notes", ItemType = "TextArea" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.bowel").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bowel", Label = "Bowel", ItemType = "Option", LabelList = Constant.Bowel.ToArray(), ValueList = Constant.Bowel.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.bowelappliance").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "colostomy", Label = "Colostomy", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ileostomy", Label = "Ileostomy", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "type", Label = "Type", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dateoflbm", Label = "Date of LBM", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "notes", Label = "Notes", ItemType = "TextArea" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.vitals").Single();

            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "datemeasured", Label = "Date Measured", ItemType = "Date" });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "height", Label = "Height", ItemType = "Text" });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "weight", Label = "Weight", ItemType = "Text" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bp", Label = "BP", ItemType = "Text" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "temp", Label = "Temp", ItemType = "Text" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pulse", Label = "Pulse", ItemType = "Text" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "resp", Label = "Resp", ItemType = "Text" });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "o2sat", Label = "O2 Saturation %", ItemType = "Text" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.painscale").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "scale", Label = "Pain Scale", ItemType = "Dropdown", LabelList = Constant.PainScale.ToArray(), ValueList = Constant.PainScale.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "lop", Label = "Location of Pain", ItemType = "Text" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "painmanagement", Label = "Pain Management", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.vaccinations").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "fluvaccination", Label = "Flu Vaccination", ItemType = "Option", LabelList = Constant.YesNoUnspecified.ToArray(), ValueList = Constant.YesNoUnspecified.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "fluvaccinationdate", Label = "Date", ItemType = "Date", DependsOnCode = "fluvaccination", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ppvvaccination", Label = "PPV Vaccination", ItemType = "Option", LabelList = Constant.YesNoUnspecified.ToArray(), ValueList = Constant.YesNoUnspecified.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ppvvaccinationdate", Label = "Date", ItemType = "Date", DependsOnCode = "ppvvaccination", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hepbvaccination", Label = "Hep B Vaccination", ItemType = "Option", LabelList = Constant.YesNoUnspecified.ToArray(), ValueList = Constant.YesNoUnspecified.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hepbvaccinationdate", Label = "Date", ItemType = "Date", DependsOnCode = "hepbvaccination", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hepbscreening", Label = "Hep B Screening", ItemType = "Option", LabelList = Constant.YesNoUnspecified.ToArray(), ValueList = Constant.YesNoUnspecified.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hepbscreeningdate", Label = "Date", ItemType = "Date", DependsOnCode = "hepbscreening", DependsOnAssertValue = "Yes" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psstatus.psychsocial").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "depressions", Label = "Depressions", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "severeanxiety", Label = "Severe Anxiety", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherInfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "other", DependsOnAssertValue = "Yes" });



            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.specialprecautionsisolation").Single();
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "aspiration", Label = "Aspiration", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "np", Label = "Negative Pressure", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "seizures", Label = "Seizures", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ci", Label = "Contact Isolation", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherInfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "other", DependsOnAssertValue = "Yes" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.allergies").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nka", Label = "No Known Allergies", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "au", Label = "Allergy Unspecified", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dta", Label = "Desensitization to Allergies", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nda", Label = "Non - Drug Allergy", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "da", Label = "Drug Allergy ", ItemType = "TextArea" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.neuroassessment").Single();
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "msa", Label = "Mental status assessment > _ 2 x / 24 h AND psych meds being adjusted for active symptoms", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "na", Label = "Neurological assessment > _ 2 x / 24 h", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bs", Label = "Behavior Symptoms, new onset or increasing", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.cognitive").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "alert", Label = "Alert", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "combative", Label = "Combative", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "confused", Label = "Confused", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "lethargic", Label = "Lethargic", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nonresponsive", Label = "Nonresponsive", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "stmd", Label = "Short Term Memory Deficit", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "vc", Label = "Vegatative / Comatose", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.vision").Single();
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "visionchange", Label = "Vision Change", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "glasses", Label = "Glasses", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "contacts", Label = "Contacts", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherInfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "other", DependsOnAssertValue = "Yes" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.hearing").Single();
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hearingaid", Label = "Hearing Aid", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.cardiovascular").Single();
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "orthopnea", Label = "Orthopnea", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "chestpain", Label = "Chest Pain", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "doe", Label = "Dyspnea on Exertion ", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "edema", Label = "Edema", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "palpitation", Label = "Palpitation", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "arrhythmia", Label = "Arrhythmia", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pacemaker", Label = "Pacemaker", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { AcuityICU = true, AcuityAsserValue = "Yes", KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "telemetry", Label = "Telemetry", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "rhythm", Label = "Rhythm", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { AcuityICU = true, AcuityAsserValue = "Yes", KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mamopab", Label = "Maintenance and monitoring of percutaneous arterial BP", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ioaoatd", Label = "Initiation of/ and / or adjustments to dysrhythmia", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "efp", Label = "EF %", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "echo", Label = "Echo", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "echodate", Label = "Echo Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "lvad", Label = "LVAD", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.gastrointestinal").Single();
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nausea", Label = "Nausea", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "vomitting", Label = "Vomitting", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "diarrhea", Label = "Diarrhea ", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "gis", Label = "GI Suctioning", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherInfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "other", DependsOnAssertValue = "Yes" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.musculoskeletal").Single();
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wnl", Label = "WNL", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nwb", Label = "Non - weight bearing", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pwb", Label = "Partial weight bearing ", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "fwb", Label = "Full weight bearing", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "assistivedevices", Label = "Assistive Devices", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "TextArea" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.skin").Single();
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "rash", Label = "Rash", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wnl", Label = "WNL", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bruising", Label = "Bruising ", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cellulitis", Label = "Cellulitis", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "TextArea" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.wounds").Single();
            section.Items.Add(new ItemModel() { AcuityICU = true, AcuityAsserValue = "Yes", Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "woundmanagement", Label = "Wound Management", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wounds", Label = "Wounds", ItemType = "Table", CodeList = new string[] { "type", "location", "size", "depth" }, TypeList = new string[] { "Text", "Text", "Number", "Number" }, LabelList = new string[] { "Type", "Location", "Size", "Depth" } });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "fcdc", Label = "Frequent(> _ 2x / 24h) &/ or complex dressing changes", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "fwdoi", Label = "Frequent wound debridements or I & D", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "tlcd", Label = "Triple layer compression dressings at least 3x / week(for venous ulcers)", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dpl", Label = "Daily pulse lavage(at least 5 days / wk)", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hbo", Label = "HBO", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "rivimpm", Label = "Requries IV / IM pain medication with each wound treatment", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { AcuityICU = true, AcuityAsserValue = "Yes", Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cfas", Label = "Care for a stage III or IV wound", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { AcuityICU = true, AcuityMed = true, AcuityINT = true, AcuityAsserValue = "Yes", Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "stw", Label = "Stage II wound care while in ICU", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wctp", Label = "Wound Care Treatment Plan", ItemType = "TextArea" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.endocrine").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "diabetes", Label = "Diabetes", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hypothyroid", Label = "Hypothyroid", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hyperthyroid", Label = "Hyperthyroid", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.renalfunction").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cri", Label = "Chronic Renal Insufficiency", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "arf", Label = "Acute Renal Failure", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "crf", Label = "Chronic Renal Failure", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "esrd", Label = "ESRD", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, AcuityICU = true, AcuityINT = true, AcuityAsserValue = "Yes", KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "riesrd", Label = "Renal insufficiency/ ESRD requiring dialysis or peritoneal dialysis > _ 3x's per week", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.dialysis").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "none", Label = "None", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "new", Label = "New", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, AcuityICU = true, AcuityINT = true, AcuityMed = true, AcuityAsserValue = "Yes", Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "chronic", Label = "Chronic", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hemodialysis", Label = "Hemodialysis", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pd", Label = "Peritoneal Dialysis", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "sadm", Label = "Shunt / abscess drain management", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mwf", Label = "MWF", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "tts", Label = "TTS", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pssystems.dialysisaccess").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "avf", Label = "AV Fistula", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "qc", Label = "Quinton Catheter", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "tc", Label = "Temporary Catheter", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "graft", Label = "Graft", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "eopcn", Label = "Established OP Clinic Name", ItemType = "TextArea" });






            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psfunctions.ptotsummary").Single();
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "attacheddocuments", Label = "Attached Documents", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ptotsummarydescribe", Label = "PT / OT Summary - Describe", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "priorleveloffunction", Label = "Prior Level of Function", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "transferstatus", Label = "Transfer Status(Min, Mod, Max, Total)", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "weightbearingstatus", Label = "Weight Bearing Status", ItemType = "Dropdown", LabelList = Constant.WeightBearingStatus.ToArray(), ValueList = Constant.WeightBearingStatus.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mt1hoar", Label = "More than 1h/d of all rehab services combined at least 5 days/wk", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { AcuityICU = true, AcuityINT = true, AcuityMed = true, AcuityAsserValue = "Yes", ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mo3hpd", Label = "minimum of 3 hours / day of multidisciplinary therapy(includes physical, occupational, speech and cardio - pulmonary therapies)", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psfunctions.motor").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "followcommands", Label = "Follow Commands", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "spontaneousmovement", Label = "Spontaneous Movement", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "withdrawsfrompain", Label = "Withdraws from pain", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "posturing", Label = "Posturing", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psfunctions.verbal").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "app", Label = "Appropriate", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "inapp", Label = "Inappropriate", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "words", Label = "Words", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "sounds", Label = "Sounds", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nonverbal", Label = "Nonverbal", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "aphasia", Label = "Aphasia", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pointake", Label = "PO Intake", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "consistencyoffood", Label = "Consistency of Food", ItemType = "TextArea" });



            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psfunctions.safety").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "safetyrisk", Label = "Safety Risk", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "fallrisk", Label = "Fall Risk", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bedalarm", Label = "Bed Alarm", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "fallhistory", Label = "Fall History", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "othersafetyissues", Label = "Other Safety Issues", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "need11", Label = "Need 1:1", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "implemented11", Label = "1:1 Implemented", ItemType = "Date" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "discontinued11", Label = "1:1 Discontinued", ItemType = "Date" });





            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psfunctions.restraints").Single();
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "type", Label = "Type", ItemType = "TextArea", });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "reason", Label = "Reason", ItemType = "TextArea", });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psfunctions.specialequipment").Single();
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "vent", Label = "Vent", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hbo", Label = "HBO", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "telemetry", Label = "Telemetry", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "woundvac", Label = "Wound Vac", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pca", Label = "PCA", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bipapcpap", Label = "BIPAP / CPAP", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bedsidecommode", Label = "Bedside Commode", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "specialitybed", Label = "Specialty Bed", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, AcuityICU = true, AcuityINT = true, AcuityAsserValue = "Yes", KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "smbpsfpwmts", Label = "Special mattress/bed post skin flap/patient with minimal turning surface", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "trapeze", Label = "Trapeze", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ivpump", Label = "IV Pump", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "specialitywalker", Label = "Specialty Walker", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hoyerlift", Label = "Hoyer Lift", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "otherInfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "other", DependsOnAssertValue = "Yes" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psrespiratory.respiratorystatus").Single();
            section.Items.Add(new ItemModel() { AcuityICU = true, AcuityAsserValue = "Yes", Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nrt", Label = "Needs respiratory therapy", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "lsw", Label = "Lung sounds WNL", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "npc", Label = "Non - productive Cough", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pc", Label = "Productive Cough", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "whz", Label = "Wheezing", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "oth", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "othinfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "oth", DependsOnAssertValue = "Yes" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psrespiratory.treatments").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nbl", Label = "Nebulizer", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ntat6", Label = "Nebulizer treatment at least q6h", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cpt", Label = "CPT", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ippb", Label = "IPPB", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "oth", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "othinfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "oth", DependsOnAssertValue = "Yes" });



            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psrespiratory.oxygenrequired").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, AcuityICU = true, AcuityINT = true, AcuityMed = true, AcuityAsserValue = "Yes", Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "or", Label = "Oxygen Required", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "lilpm", Label = "Liters in LPM", ItemType = "Number" });
            section.Items.Add(new ItemModel() { SNF = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "o2sat", Label = "O2 Sat %", ItemType = "Number" });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "no2dar", Label = "New O2 delivery at a rate of 3 liters or greater / two liters or less baseline usage", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cpo", Label = "Continuous pulse oximetry", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "chestpt", Label = "Chest PT > _ 3 x's in 24 hours", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hvt", Label = "Home ventilator training to meet the patient and/ or caregivers ability to exhibit care for the need", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psrespiratory.mode").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mask", Label = "Mask", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nc", Label = "Nasal Cannula", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "tc", Label = "Trach Collar", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "oth", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "othinfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "oth", DependsOnAssertValue = "Yes" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psrespiratory.ventilation").Single();
            section.Items.Add(new ItemModel() { AcuityINT = true, AcuityAsserValue = "Yes", KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mv", Label = "Mechanical Ventilation", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { AcuityICU = true, AcuityAsserValue = "Yes", KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mvaw", Label = "Mechanical Ventilation / actively weaning", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "iponiv", Label = "Is patient on Non - Invasive Ventilator?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "sa6tpd", Label = "Suctioning at least 6 times per day", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bipap", Label = "BiPAP / IPAP", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "bipapinfo", Label = "BiPAP / IPAP Other Info", ItemType = "TextArea", DependsOnCode = "bipap", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cpap", Label = "CPAP", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cpapinfo", Label = "CPAP Other Info", ItemType = "TextArea", DependsOnCode = "cpap", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "epap", Label = "EPAP", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "epapinfo", Label = "EPAP Other Info", ItemType = "TextArea", DependsOnCode = "epap", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nippv", Label = "NIPPV(BIPAP, CPAP etc) actively weaning", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psrespiratory.frequency").Single();
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "nocturnaluse", Label = "Nocturnal Use", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "prn", Label = "PRN", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "oth", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "othinfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "oth", DependsOnAssertValue = "Yes" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psrespiratory.mode2").Single();
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dpou", Label = "Does patient own a unit?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ipov", Label = "Is the patient on a ventilator?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cmv", Label = "CMV", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ac", Label = "AC", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "simv", Label = "SIMV", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "prvc", Label = "PRVC", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "oth", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "othinfo", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "oth", DependsOnAssertValue = "Yes" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "rate", Label = "Rate", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "fi02", Label = "FIO2", ItemType = "Number" });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "fi02g28", Label = "FI02 > _ 28 % or nasal oxygen > _ 2 lit / min", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "tv", Label = "TV", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "peep", Label = "PEEP", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pressuresupport", Label = "Pressure Support", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wa", Label = "Weening Attempted", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "wasd", Label = "Weening Attempted Start Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mode", Label = "Mode", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "method", Label = "Method", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "tov", Label = "Time off Vent", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "weaningcomp", Label = "Weaning Complications", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "chf", Label = "Congestive Heart Failure", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ihr", Label = "Increased Heart Rate", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "drr", Label = "Decreased RR", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "do2sat", Label = "Decreased O2 Sat", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "oth2", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "othinfo2", Label = "Other Info", ItemType = "TextArea", DependsOnCode = "oth2", DependsOnAssertValue = "Yes" });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psrespiratory.arterialbloodgases").Single();
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ph", Label = "PH", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pc02", Label = "pCO2", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "pao2", Label = "PaO2", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "hc03", Label = "HCO3", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "be", Label = "BE", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "o2sat", Label = "O2Sat", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "o2g40", Label = "O2 > 40", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "o2toms", Label = "O2 Tritration to maintain sats > _ 90 %", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "sa02m", Label = " SaO2 monitoring > _ 4x / 24 hours", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psrespiratory.trach").Single();
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dpht", Label = "Does patient have a Trach?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "date", Label = "Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "tmsfbo", Label = "Type (Metal, Shiley, Fenestrated, Bivona, Other)", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "innerdimension", Label = "Inner Dimension", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "cocl", Label = "Cuff or Cuffless", ItemType = "Option", LabelList = Constant.CuffCuffless.ToArray(), ValueList = Constant.CuffCuffless.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "tc", Label = "Trach changes", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "spv", Label = "Speaking Valve", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { AcuityICU = true, AcuityAsserValue = "Yes", KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "tw", Label = "Tracheotomy weaning < _ 2 week", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psrespiratory.ettube").Single();
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dphett", Label = "Does patient have ET Tube?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "date", Label = "Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "size", Label = "Size", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "length", Label = "Length", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "lal", Label = "Length at lip", ItemType = "Number" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dextubated", Label = "Date Extubated", ItemType = "Date" });

            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.psrespiratory.chesttube").Single();
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dphct", Label = "Does patient have a Chest Tube?", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "leftlocation", Label = "Left location", ItemType = "Check" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ltype", Label = "Type", ItemType = "Text", DependsOnCode = "leftlocation", DependsOnAssertValue = "true", ComparePrimitive = true });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "l24ho", Label = "24 hr output", ItemType = "TextArea", DependsOnCode = "leftlocation", DependsOnAssertValue = "true", ComparePrimitive = true });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "lseal", Label = "Seal", ItemType = "TextArea", DependsOnCode = "leftlocation", DependsOnAssertValue = "true", ComparePrimitive = true });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "lairleak", Label = "Air Leak", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray(), DependsOnCode = "leftlocation", DependsOnAssertValue = "true", ComparePrimitive = true });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "rightlocation", Label = "Right location", ItemType = "Check" });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "rtype", Label = "Type", ItemType = "Text", DependsOnCode = "rightlocation", DependsOnAssertValue = "true", ComparePrimitive = true });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "r24ho", Label = "24 hr output", ItemType = "TextArea", DependsOnCode = "rightlocation", DependsOnAssertValue = "true", ComparePrimitive = true });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "rseal", Label = "Seal", ItemType = "TextArea", DependsOnCode = "rightlocation", DependsOnAssertValue = "true", ComparePrimitive = true });
            section.Items.Add(new ItemModel() { Required = false, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "rairleak", Label = "Air Leak", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray(), DependsOnCode = "rightlocation", DependsOnAssertValue = "true", ComparePrimitive = true });



            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pslabs.medicationlist").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "seemedicationadministrationrecord", Label = "See Medication Administration Record", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "medications", Label = "Medications", ItemType = "Table", LabelList = new string[] { "Medication", "Status" }, CodeList = new string[] { "medication", "status" }, TypeList = new string[] { "Text", "Text" } });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "poimmuno", Label = "PO immunosuppressant's (including steroids)", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "fma", Label = "Frequent Medication adjustment >_ 2x in 24 hours (Frequent accu-checks/insulin adjustment, anticoagulation and electrolyte, etc.", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pslabs.bloodproducts").Single();
            section.Items.Add(new ItemModel() { SNF = true, SSSN = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "seettachedlabmicrobiologyreports", Label = "See Attached Lab / Microbiology Reports", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "date", Label = "Date", ItemType = "Date" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "prbc", Label = "PRBC", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ffp", Label = "FFP", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "plazma", Label = "Plazma", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ohorot", Label = "OH or OT", ItemType = "Dropdown", LabelList = Constant.OHOT.ToArray(), ValueList = Constant.OHOT.ToArray() });


            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pslabs.bloodworklabs").Single();
            section.Items.Add(new ItemModel() { KeyClinicalIndicator = true, ParentClientCode = section.GetUniqueClientCode(), ClientCode = "frequentlabassessmentsatleasteveryotherday", Label = "Frequent Lab assessments at least every other day", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "urninalysis", Label = "Urinalysis", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "sp", Label = "SP", ItemType = "TextArea" });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ph", Label = "PH", ItemType = "TextArea" });

            //PS Labs microbiology
            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pslabs.microbiology").Single();
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "organisms", Label = "Organisms", ItemType = "Table", LabelList = new string[] { "Organism", "Source", "Result", "Date" }, CodeList = new string[] { "organism", "source", "result", "date" }, TypeList = new string[] { "Text", "Text", "Text", "Date" } });


            //PS Labs studies.
            section = GetSections().Where(p => p.GetUniqueClientCode() == "model.data.pslabs.studies").Single();
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mri", Label = "MRI", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "mra", Label = "MRA", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "ct", Label = "CT", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "echo", Label = "ECHO", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "xray", Label = "XRAY", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "eeg", Label = "EEG", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "dopplers", Label = "Dopplers", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "other", Label = "Other", ItemType = "Option", LabelList = Constant.YesNoNA.ToArray(), ValueList = Constant.YesNoNA.ToArray() });
            section.Items.Add(new ItemModel() { ParentClientCode = section.GetUniqueClientCode(), ClientCode = "results", Label = "Results", ItemType = "TextArea" });



        }

    }

}






