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
        private void PopulateAccordions(PreScreenModel model)
        {
            _patientTabs = new TabModel() { ClientId = "tab_payorinfo", ParentClientCode = "model.data", ClientCode = "payorinfo", Label = "Payor Info", Active = true };
            model.Tabs.Add(_patientTabs);
            var accordion = new AccordionModel() { ClientId = "acc_payorinfo" };
            _patientTabs.Accordions.Add(accordion);
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_primary", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "primary", ParentClientId = accordion.ClientId, Label = "Primary", Active = true });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_secondary", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "secondary", ParentClientId = accordion.ClientId, Label = "Secondary" });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_tertiary", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "tertiary", ParentClientId = accordion.ClientId, Label = "Tertiary" });

            SectionModel primary;
            SectionModel secondary;
            SectionModel tertiary;
            primary = accordion.Sections.Where(p => p.Label == "Primary").SingleOrDefault();
            secondary = accordion.Sections.Where(p => p.Label == "Secondary").SingleOrDefault();
            tertiary = accordion.Sections.Where(p => p.Label == "Tertiary").SingleOrDefault();


            accordion = new AccordionModel() { ClientId = $"acc_{primary.ClientId}" };
            primary.Accordions.Add(accordion);
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_all", ParentClientCode = primary.GetUniqueClientCode(), ClientCode = "all", ParentClientId = accordion.ClientId, Label = "All", Active = true });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_medicare", ParentClientCode = primary.GetUniqueClientCode(), ClientCode = "medicare", ParentClientId = accordion.ClientId, Label = "Medicare", Active = false, ClientCallBack = "setMedicareDaysLeft()" });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_general", ParentClientCode = primary.GetUniqueClientCode(), ClientCode = "general", ParentClientId = accordion.ClientId, Label = "General", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_sca", ParentClientCode = primary.GetUniqueClientCode(), ClientCode = "sca", ParentClientId = accordion.ClientId, Label = "SCA", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_peer_to_peer", ParentClientCode = primary.GetUniqueClientCode(), ClientCode = "peertopeer", ParentClientId = accordion.ClientId, Label = "Peer-to-Peer", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_expedited_appeal", ParentClientCode = primary.GetUniqueClientCode(), ClientCode = "expeditedappeal", ParentClientId = accordion.ClientId, Label = "Expedited Appeal", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_disposition", ParentClientCode = primary.GetUniqueClientCode(), ClientCode = "disposition", ParentClientId = accordion.ClientId, Label = "Disposition", Active = false });



            accordion = new AccordionModel() { ClientId = $"acc_{secondary.ClientId}" };
            secondary.Accordions.Add(accordion);
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_all", ParentClientCode = secondary.GetUniqueClientCode(), ClientCode = "all", ParentClientId = accordion.ClientId, Label = "All", Active = true });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_medicare", ParentClientCode = secondary.GetUniqueClientCode(), ClientCode = "medicare", ParentClientId = accordion.ClientId, Label = "Medicare", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_general", ParentClientCode = secondary.GetUniqueClientCode(), ClientCode = "general", ParentClientId = accordion.ClientId, Label = "General", Active = false });

            accordion = new AccordionModel() { ClientId = $"acc_{tertiary.ClientId}" };
            tertiary.Accordions.Add(accordion);
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_all", ParentClientCode = tertiary.GetUniqueClientCode(), ClientCode = "all", ParentClientId = accordion.ClientId, Label = "All", Active = true });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_medicare", ParentClientCode = tertiary.GetUniqueClientCode(), ClientCode = "medicare", ParentClientId = accordion.ClientId, Label = "Medicare", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_general", ParentClientCode = tertiary.GetUniqueClientCode(), ClientCode = "general", ParentClientId = accordion.ClientId, Label = "General", Active = false });


            primary.SaveButtonVisible = false;
            secondary.SaveButtonVisible = false;
            tertiary.SaveButtonVisible = false;

            _patientTabs = new TabModel() { ClientId = "tab_admissioninfo", ParentClientCode = "model.data", ClientCode = "admissioninfo", Label = "Admission Info" };
            model.Tabs.Add(_patientTabs);
            accordion = new AccordionModel() { ClientId = "acc_admissioninfo" };
            _patientTabs.Accordions.Add(accordion);

            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_general", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "general", ParentClientId = accordion.ClientId, Label = "General", Active = true });


            _patientTabs = new TabModel() { ClientId = "tab_referralsourceinfo", ParentClientCode = "model.data", ClientCode = "referralsourceinfo", Label = "PS Referral Source Info" };
            model.Tabs.Add(_patientTabs);
            accordion = new AccordionModel() { ClientId = "acc_referralsourceinfo" };
            _patientTabs.Accordions.Add(accordion);

            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_general", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "general", ParentClientId = accordion.ClientId, Label = "General", Active = true });


            _patientTabs = new TabModel() { ClientId = "tab_demographics", ParentClientCode = "model.data", ClientCode = "demographics", Label = "PS Demographics" };
            model.Tabs.Add(_patientTabs);
            accordion = new AccordionModel() { ClientId = "acc_demographics" };
            _patientTabs.Accordions.Add(accordion);
            accordion.Sections.Add(new SectionModel() { Context = "Patient", ClientId = $"{accordion.ClientId}_general", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "general", ParentClientId = accordion.ClientId, Label = "General", Active = true });


            _patientTabs = new TabModel() { ClientId = "tab_status", ParentClientCode = "model.data", ClientCode = "psstatus", Label = "PS Pt. Status" };
            model.Tabs.Add(_patientTabs);
            accordion = new AccordionModel() { ClientId = "acc_status" };
            _patientTabs.Accordions.Add(accordion);

            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_prehospitalliving", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "prehospitalliving", ParentClientId = accordion.ClientId, Label = "Pre-Hospital Living", Active = true });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_languagecommunicationneeds", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "languagecommunicationneeds", ParentClientId = accordion.ClientId, Label = "Language Communication Needs", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_preadmission", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "preadmission", ParentClientId = accordion.ClientId, Label = "Pre-Admission", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_listallacute", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "listallacute", ParentClientId = accordion.ClientId, Label = "List All Acute Care Hospitalizations During the Last 60 Days", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_surgery", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "surgery", ParentClientId = accordion.ClientId, Label = "Surgery", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_socialhistory", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "socialhistory", ParentClientId = accordion.ClientId, Label = "Social History", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_ivaccess", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "ivaccess", ParentClientId = accordion.ClientId, Label = "IV Access", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_ivfluids", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "ivfluids", ParentClientId = accordion.ClientId, Label = "IV Fluids or IV Meds(e.g., IV Antibiotics, IV Lasix etc.) > _ 1x daily", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_infection", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "infection", ParentClientId = accordion.ClientId, Label = "Infection", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_isolationtype", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "isolationtype", ParentClientId = accordion.ClientId, Label = "Isolation Type", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_diet", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "diet", ParentClientId = accordion.ClientId, Label = "Diet", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_bladder", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "bladder", ParentClientId = accordion.ClientId, Label = "Bladder", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_bladderappliance", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "bladderappliance", ParentClientId = accordion.ClientId, Label = "Bladder Appliance", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_bowel", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "bowel", ParentClientId = accordion.ClientId, Label = "Bowel", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_bowelappliance", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "bowelappliance", ParentClientId = accordion.ClientId, Label = "Bowel Appliance", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_vitals", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "vitals", ParentClientId = accordion.ClientId, Label = "Vitals", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_painscale", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "painscale", ParentClientId = accordion.ClientId, Label = "Pain Scale", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_vaccinations", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "vaccinations", ParentClientId = accordion.ClientId, Label = "Vaccinations", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_psychsocial", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "psychsocial", ParentClientId = accordion.ClientId, Label = "Psych-Social", Active = false });


            _patientTabs = new TabModel() { ClientId = "tab_systems", ParentClientCode = "model.data", ClientCode = "pssystems", Label = "PS Pt. Systems" };
            model.Tabs.Add(_patientTabs);
            accordion = new AccordionModel() { ClientId = "acc_systems" };
            _patientTabs.Accordions.Add(accordion);
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_specialprecautionsisolation", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "specialprecautionsisolation", ParentClientId = accordion.ClientId, Label = "Special Precautions / Isolation", Active = true });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_allergies", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "allergies", ParentClientId = accordion.ClientId, Label = "Allergies", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_neuroassessment", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "neuroassessment", ParentClientId = accordion.ClientId, Label = "Neuro Assessment", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_cognitive", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "cognitive", ParentClientId = accordion.ClientId, Label = "Cognitive", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_vision", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "vision", ParentClientId = accordion.ClientId, Label = "Vision", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_hearing", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "hearing", ParentClientId = accordion.ClientId, Label = "Hearing", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_cardiovascular", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "cardiovascular", ParentClientId = accordion.ClientId, Label = "Cardiovascular", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_gastrointestinal", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "gastrointestinal", ParentClientId = accordion.ClientId, Label = "Gastrointestinal", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_musculoskeletal", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "musculoskeletal", ParentClientId = accordion.ClientId, Label = "Musculoskeletal", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_skin", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "skin", ParentClientId = accordion.ClientId, Label = "Skin", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_wounds", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "wounds", ParentClientId = accordion.ClientId, Label = "Wounds", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_endocrine", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "endocrine", ParentClientId = accordion.ClientId, Label = "Endocrine", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_renalfunction", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "renalfunction", ParentClientId = accordion.ClientId, Label = "Renal Function", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_dialysis", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "dialysis", ParentClientId = accordion.ClientId, Label = "Dialysis", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_dialysisaccess", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "dialysisaccess", ParentClientId = accordion.ClientId, Label = "Dialysis Access", Active = false });

            _patientTabs = new TabModel() { ClientId = "tab_functionequip", ParentClientCode = "model.data", ClientCode = "psfunctions", Label = "PS Pt. Function & Equip." };
            model.Tabs.Add(_patientTabs);
            accordion = new AccordionModel() { ClientId = "acc_functionequip" };
            _patientTabs.Accordions.Add(accordion);
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_ptotsummary", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "ptotsummary", ParentClientId = accordion.ClientId, Label = "PT / OT Summary", Active = true });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_motor", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "motor", ParentClientId = accordion.ClientId, Label = "Motor", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_verbal", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "verbal", ParentClientId = accordion.ClientId, Label = "Verbal", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_safety", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "safety", ParentClientId = accordion.ClientId, Label = "Safety", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_restraints", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "restraints", ParentClientId = accordion.ClientId, Label = "Restraints", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_specialequipment", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "specialequipment", ParentClientId = accordion.ClientId, Label = "Special Equipment", Active = false });



            _patientTabs = new TabModel() { ClientId = "tab_respiratory", ParentClientCode = "model.data", ClientCode = "psrespiratory", Label = "PS Respiratory" };
            model.Tabs.Add(_patientTabs);
            accordion = new AccordionModel() { ClientId = "acc_respiratory" };
            _patientTabs.Accordions.Add(accordion);
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_respiratorystatus", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "respiratorystatus", ParentClientId = accordion.ClientId, Label = "Respiratory Status", Active = true });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_treatments", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "treatments", ParentClientId = accordion.ClientId, Label = "Treatments", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_oxygenrequired", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "oxygenrequired", ParentClientId = accordion.ClientId, Label = "Oxygen Required", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_mode", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "mode", ParentClientId = accordion.ClientId, Label = "Mode", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_ventilation", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "ventilation", ParentClientId = accordion.ClientId, Label = "Ventilation", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_frequency", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "frequency", ParentClientId = accordion.ClientId, Label = "Frequency", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_mode2", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "mode2", ParentClientId = accordion.ClientId, Label = "Mode", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_arterialbloodgases", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "arterialbloodgases", ParentClientId = accordion.ClientId, Label = "Arterial Blood Gases", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_trach", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "trach", ParentClientId = accordion.ClientId, Label = "Trach", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_ettube", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "ettube", ParentClientId = accordion.ClientId, Label = "ET Tube", Active = false });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_chesttube", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "chesttube", ParentClientId = accordion.ClientId, Label = "Chest Tube", Active = false });


            _patientTabs = new TabModel() { ClientId = "tab_labs", ParentClientCode = "model.data", ClientCode = "pslabs", Label = "PS Labs" };
            model.Tabs.Add(_patientTabs);
            accordion = new AccordionModel() { ClientId = "acc_labs" };
            _patientTabs.Accordions.Add(accordion);
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_medicationlist", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "medicationlist", ParentClientId = accordion.ClientId, Label = "Medication List", Active = true });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_bloodproducts", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "bloodproducts", ParentClientId = accordion.ClientId, Label = "Blood / Blood Products" });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_bloodworklabs", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "bloodworklabs", ParentClientId = accordion.ClientId, Label = "Blood Work / Labs" });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_microbiology", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "microbiology", ParentClientId = accordion.ClientId, Label = "Microbiology" });
            accordion.Sections.Add(new SectionModel() { Context = "PreScreen", ClientId = $"{accordion.ClientId}_studies", ParentClientCode = _patientTabs.GetUniqueClientCode(), ClientCode = "studies", ParentClientId = accordion.ClientId, Label = "Studies" });


        }

    }

}






