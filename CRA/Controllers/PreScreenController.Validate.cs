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

        private List<ValidationModel> ValidatePreScreenSection(long preScreenId, string sectionCode)
        {
            
            List<ValidationModel> items = new List<ValidationModel>();
            var values = _dataContext.PreScreenData.Where(p => p.PreScreenId == preScreenId && p.Deleted == false && p.SectionCode == sectionCode.Replace(_prefix, "")).ToList();
            var sections = GetSections();
            var section = sections.Where(p => p.GetUniqueClientCode() == sectionCode).Single();

            foreach (var item in section.Items)
            {
                if (item.DependsOnCode != null)
                {
                    var depValue = values.Where(p => p.ItemCode == item.DependsOnCode).SingleOrDefault();
                    if (depValue != null)
                    {
                        if (depValue.Value != item.DependsOnAssertValue)
                        {
                            item.Required = false;
                        }
                    }
                }

                if (item.IsCalculated == true || item.Disabled == true)
                {
                    item.Required = false;
                }

                if (item.Required)
                {
                    var value = values.Where(p => p.ItemCode == item.ClientCode).Take(1).SingleOrDefault();
                    if (value == null || string.IsNullOrEmpty(value.Value))
                    {
                        items.Add(new ValidationModel() {Section=section.Label, Label = item.Label, Error = $"Required {item.Label}" });
                    }



                }
            }

            return items;
        }


        private List<ValidationModel> ValidatePatientSection(long patientId, string sectionCode)
        {

            List<ValidationModel> items = new List<ValidationModel>();
            var values = _dataContext.PatientData.Where(p => p.PatientId == patientId && p.Deleted == false && p.SectionCode == sectionCode.Replace(_prefix, "")).ToList();
            var sections = GetSections();
            var section = sections.Where(p => p.GetUniqueClientCode() == sectionCode).Single();

            foreach (var item in section.Items)
            {
                if (item.DependsOnCode != null)
                {
                    var depValue = values.Where(p => p.ItemCode == item.DependsOnCode).SingleOrDefault();
                    if (depValue != null)
                    {
                        if (depValue.Value != item.DependsOnAssertValue)
                        {
                            item.Required = false;
                        }
                    }
                }

                if (item.IsCalculated == true || item.Disabled == true)
                {
                    item.Required = false;
                }

                if (item.Required)
                {
                    var value = values.Where(p => p.ItemCode == item.ClientCode).Take(1).SingleOrDefault();
                    if (value == null || string.IsNullOrEmpty(value.Value))
                    {
                        items.Add(new ValidationModel() { Section = section.Label, Label = item.Label, Error = $"Required {item.Label}" });
                    }



                }
            }

            return items;
        }


        private List<ValidationModel> ValidateTab(long preSecreenId, TabModel model)
        {

            var patientId = _dataContext.PreScreens.Find(preSecreenId).PatientId;

            List<ValidationModel> items = new List<ValidationModel>();

            foreach (var acc in model.Accordions)
            {
                foreach (var section in acc.Sections)
                {
                    List<ValidationModel> sectionItems;

                    if (section.Deleted == true)
                    {
                        return items;
                    }

                    if (section.Context == "PreScreen")
                    {
                        sectionItems = ValidatePreScreenSection(preSecreenId, section.GetUniqueClientCode());
                    }
                    else
                    {
                        sectionItems = ValidatePatientSection(patientId, section.GetUniqueClientCode());
                    }

                    items.AddRange(sectionItems);

                    foreach (var acc1 in section.Accordions)
                    {
                        foreach (var section1 in acc1.Sections)
                        {

                            List<ValidationModel> sectionItems1;

                            if (section1.Deleted == true)
                            {
                                return items;
                            }

                            if (section.Context == "PreScreen")
                            {
                                sectionItems1 = ValidatePreScreenSection(preSecreenId, section1.GetUniqueClientCode());
                            }
                            else
                            {
                                sectionItems1 = ValidatePatientSection(patientId, section1.GetUniqueClientCode());
                            }


                            items.AddRange(sectionItems1);
                        }
                    }
                }
            }


            return items;
        }

        public void ApplyFilter(string type)
        {
            var sections = GetSections();

            foreach (var section in sections)
            {
                switch (type)
                {
                    case "LTACH Pre-Screen":
                        section.Items = section.Items.Where(p => p.LTACH == true).ToList();
                        break;
                    case "SSSN Pre-Screen":
                        section.Items = section.Items.Where(p => p.SSSN == true).ToList();
                        break;
                    case "SNF Pre-Screen":
                        section.Items = section.Items.Where(p => p.SNF == true).ToList();
                        break;
                    case "BH Pre-Screen":
                        section.Items = section.Items.Where(p => p.LTACH == true).ToList();
                        break;
                }

                if (section.Items.Count() <= 0)
                {
                    section.Deleted = true;
                }

            }

            
             
        }      


        public JsonResult ValidateTabs(long id)
        {
          
            List<KeyValuePairModel> items = ValidatePreScreen(id);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        private List<KeyValuePairModel> ValidatePreScreen(long id)
        {
            _model = new PreScreenModel();

            PopulateAccordions(_model);
            SetContextLists();

            PopulateItems(_model);

            var preScreen = _dataContext.PreScreens.Find(id);
            ApplyFilter(preScreen.PreScreenType.Name);

            Dictionary<string, bool> results = new Dictionary<string, bool>();

            foreach (var tab in _model.Tabs)
            {
                var validationItems = ValidateTab(id, tab);
                results.Add(tab.ClientCode, validationItems.Count() <= 0);
            }

            List<KeyValuePairModel> items = new List<KeyValuePairModel>();
            foreach (var key in results.Keys)
            {
                items.Add(new KeyValuePairModel() { Key = key, Value = results[key].ToString() });
            }

            return items;
        }


        public JsonResult ValidationResult(long id)
        {
            _model = new PreScreenModel();

            PopulateAccordions(_model);
            SetContextLists();

            PopulateItems(_model);

            var preScreen = _dataContext.PreScreens.Find(id);
            ApplyFilter(preScreen.PreScreenType.Name);

            List<ValidationModel> items = new List<ValidationModel>();
            foreach (var tab in _model.Tabs)
            {
                var validationItems = ValidateTab(id, tab);
                items.AddRange(validationItems);

            }

            return Json(items, JsonRequestBehavior.AllowGet);

        }


       
    }

}






