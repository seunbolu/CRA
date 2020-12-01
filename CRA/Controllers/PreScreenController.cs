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

        Dictionary<string, ListModel> _contextLists = new Dictionary<string, ListModel>();

        private void SetContextLists()
        {
            

            var list = new ListModel();

            var userSites = _dataContext.GetUserSites(_dataContext.UserId.Value);
            list.ValueList = userSites.Select(p => p.CHGSiteId.ToString()).ToArray();
            list.LabelList = userSites.Select(p => p.FullName).ToArray();

            AddSite("USER_SITES", list);

            long preScreenId = 0;

            if (Request.QueryString["id"] != null)
            {
                preScreenId = long.Parse(Request.QueryString["id"].ToString());
            }

            if (Request.QueryString["preScreenId"] != null)
            {
                preScreenId = long.Parse(Request.QueryString["preScreenId"].ToString());
            }

            if (this.RouteData.Values["id"] != null)
            {
                preScreenId = long.Parse(this.RouteData.Values["id"].ToString());
            }

            var preScreen = _dataContext.PreScreens.Find(preScreenId);

            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == preScreenId).Take(1).Single();


            list = new ListModel();

            var liasons = (from u in _dataContext.Users join r in _dataContext.Referrals on u.UserId equals r.LiaisonId where r.PreScreenId == preScreenId select new { Contact = u.Contact, User = u }).ToList();

            list.ValueList = liasons.Select(p => p.User.UserId.ToString()).ToArray();
            list.LabelList = liasons.Select(p => $"{p.Contact.FirstName} {p.Contact.LastName}").ToArray();

            AddSite("CONTEXT_LIASONS", list);

            list = new ListModel();
            var cacUsers = _dataContext.GetRoleUsers("CAC");

            list.ValueList = cacUsers.Select(p => p.UserId.ToString()).ToArray();
            list.LabelList = cacUsers.Select(p => $"{p.Contact.FirstName} {p.Contact.LastName}").ToArray();

            AddSite("CAC_USERS", list);


            var mcUsers = _dataContext.GetRoleUsers("MC");
            list = new ListModel();
            list.ValueList = mcUsers.Select(p => p.UserId.ToString()).ToArray();
            list.LabelList = mcUsers.Select(p => $"{p.Contact.FirstName} {p.Contact.LastName}").ToArray();
            AddSite("MC_USERS", list);

            list = new ListModel();

            var motUsers = (from u in _dataContext.UserCHGSites where u.CHGSiteId == referral.CHGSiteId && u.Deleted == false select new { Contact = u.User.Contact, User = u }).ToList();
            list.ValueList = motUsers.Select(p => p.User.UserId.ToString()).ToArray();
            list.LabelList = motUsers.Select(p => $"{p.Contact.FirstName} {p.Contact.LastName}").ToArray();

            AddSite("CONTEXT_MOT", list);


            var rsContacts = (from a in _dataContext.ContactAttributes join c in _dataContext.ContactReferralSources on a.ContactId equals c.ContactId where a.Deleted == false && c.Deleted == false && c.ReferralSourceId == referral.ReferralSourceId select new { Attribute = a.CategoryType, Contact = c.Contact, RoleType = a.ContactRoleType }).ToList();
            list = new ListModel();
            list.ValueList = rsContacts.Select(p => p.Contact.ContactId.ToString()).ToArray();
            list.LabelList = rsContacts.Select(p => $"{p.Contact.FirstName} {p.Contact.LastName}").ToArray();

            AddSite("CONTEXT_RS_CONTACTS", list);

            list = new ListModel();

            list.ValueList = rsContacts.Where(p => p.Attribute.Name == "Referring").Select(p => p.Contact.ContactId.ToString()).ToArray();
            list.LabelList = rsContacts.Where(p => p.Attribute.Name == "Referring").Select(p => $"{p.Contact.FirstName} {p.Contact.LastName}").ToArray();


            AddSite("CONTEXT_RS_CONTACTS_REFERRING", list);

            list = new ListModel();

            list.ValueList = rsContacts.Where(p => p.Attribute.Name == "Influencer").Select(p => p.Contact.ContactId.ToString()).ToArray();
            list.LabelList = rsContacts.Where(p => p.Attribute.Name == "Influencer").Select(p => $"{p.Contact.FirstName} {p.Contact.LastName}").ToArray();

            AddSite("CONTEXT_RS_CONTACTS_INFLUENCER", list);

            AddSite("CONTEXT_RS_CONTACTS_PULMONARY", GetContactForSpeciality(referral.ReferralSourceId, "Pulmonary"));
            AddSite("CONTEXT_RS_CONTACTS_CRITICAL_CARE", GetContactForSpeciality(referral.ReferralSourceId, "Critical Care"));
            AddSite("CONTEXT_RS_CONTACTS_ORTHOPEDIC", GetContactForSpeciality(referral.ReferralSourceId, "Orthopedic"));
            AddSite("CONTEXT_RS_CONTACTS_INTERVENTIONAL_RADIOLOGY", GetContactForSpeciality(referral.ReferralSourceId, "Interventional Radiology"));
            AddSite("CONTEXT_RS_CONTACTS_CARDIOLOGY", GetContactForSpeciality(referral.ReferralSourceId, "Cardiology"));
            AddSite("CONTEXT_RS_CONTACTS_WOUND", GetContactForSpeciality(referral.ReferralSourceId, "Wound"));

            AddSite("CONTEXT_RS_CONTACTS_INFECTIOUS_DISEASE", GetContactForSpeciality(referral.ReferralSourceId, "Infectious Disease"));
            AddSite("CONTEXT_RS_CONTACTS_NEPHROLOGY", GetContactForSpeciality(referral.ReferralSourceId, "Nephrology"));
            AddSite("CONTEXT_RS_CONTACTS_SURGERY", GetContactForSpeciality(referral.ReferralSourceId, "Surgery"));
            AddSite("CONTEXT_RS_CONTACTS_PSYCHIATRY", GetContactForSpeciality(referral.ReferralSourceId, "Psychiatry"));
            AddSite("CONTEXT_RS_CONTACTS_NEUROLOGY", GetContactForSpeciality(referral.ReferralSourceId, "Neurology"));
            AddSite("CONTEXT_RS_CONTACTS_GASTROENTEROLOGY", GetContactForSpeciality(referral.ReferralSourceId, "Gastroenterology"));
            AddSite("CONTEXT_RS_CONTACTS_UROLOGY", GetContactForSpeciality(referral.ReferralSourceId, "Urology"));
            AddSite("CONTEXT_RS_CONTACTS_ONCOLOGY", GetContactForSpeciality(referral.ReferralSourceId, "Oncology/Hemotology"));
            AddSite("CONTEXT_RS_CONTACTS_INTERNAL_MEDICINE", GetContactForSpeciality(referral.ReferralSourceId, "Internal Medicine"));
            AddSite("CONTEXT_RS_CONTACTS_ENT", GetContactForSpeciality(referral.ReferralSourceId, "ENT Consult"));
            AddSite("CONTEXT_RS_CONTACTS_OTH", GetContactForSpeciality(referral.ReferralSourceId, "Other"));


            list = new ListModel();

            list.ValueList = rsContacts.Where(p => p.RoleType.Name == "Case Manager").Select(p => p.Contact.ContactId.ToString()).ToArray();
            list.LabelList = rsContacts.Where(p => p.RoleType.Name == "Case Manager").Select(p => $"{p.Contact.FirstName} {p.Contact.LastName}").ToArray();

            AddSite("CONTEXT_RS_CONTACTS_CASE_MANAGER", list);


        }
        private ListModel GetContactForSpeciality(long referralSourceId, string specialityName)
        {

            var list = new ListModel();

            var rsContacts = (from a in _dataContext.ContactAttributes join c in _dataContext.ContactReferralSources on a.ContactId equals c.ContactId where a.Deleted == false && c.Deleted == false && c.ReferralSourceId == referralSourceId select new { Attribute = a.SpecialityType, Contact = c.Contact }).ToList();
            list.ValueList = rsContacts.Where(p => p.Attribute != null).Where(p => p.Attribute.Name == specialityName).Select(p => p.Contact.ContactId.ToString()).ToArray();
            list.LabelList = rsContacts.Where(p => p.Attribute != null).Where(p => p.Attribute.Name == specialityName).Select(p => $"{p.Contact.FirstName} {p.Contact.LastName}").ToArray();

            return list;
        }
        private void AddSite(string name, ListModel model)
        {
            if (!_contextLists.ContainsKey(name))
            {
                _contextLists.Add(name, model);
            }

            
        }
        public ActionResult Create(long? PatientId)
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetPatientRecord(long patientId)
        {
            var items = _dataContext.PatientData.Where(p => p.PatientId == patientId && p.Deleted == false).ToList();

            var data = new
            {
                PatientId = patientId,
                FirstName = GetPatientData(patientId, "demographics.general", "firstname", items),
                MiddleName = GetPatientData(patientId, "demographics.general", "middleinitial", items),
                LastName = GetPatientData(patientId, "demographics.general", "lastname", items),
                SSN = GetPatientData(patientId, "payorinfo.primary.all", "ssn", items),
                DateOfBirth = GetPatientData(patientId, "demographics.general", "dob", items)
            };

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        private long _preScreenId = 0;
        public ActionResult Edit(long? id)
        {

            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." });
            }


            var preScreen = _dataContext.PreScreens.Find(id.Value);
            var patientId = preScreen.PatientId;

            if (preScreen.Status == "Created")
            {
                preScreen.Status = "Pre-Screen In Progress";
                _dataContext.AddPreScreenStatusLog(preScreen.PreScreenId, "Pre-Screen In Progress", _dataContext.UserId.Value);
            }

            _dataContext.SaveChanges();

            ViewBag.PatientId = patientId;
            _preScreenId = id.Value;
            _model = new PreScreenModel();

            SetContextLists();
            PopulateAccordions(_model);
            PopulateItems(_model);

            ApplyFilter(preScreen.PreScreenType.Name);

            var task = (from t in _dataContext.UserTasks join p in _dataContext.UserTaskParameters on t.UserTaskId equals p.UserTaskId where t.Deleted == false && p.Deleted == false && p.Key == "PRE_SCREEN_ID" && t.UserId == _dataContext.UserId.Value && t.Status == "Created" && p.Value == id.Value.ToString()  select t).Distinct().OrderByDescending(p=>p.UserTaskId).Take(1).SingleOrDefault();
            if (task != null)
            {
                _model.PendingTaskId = task.UserTaskId;

                switch (task.TaskType)
                {
                    case "Pre-Screen Approval":
                        _model.ResolveLocation = "ResolvePreScreenApproval";
                        break;
                    case "Pre-Screen SCA Requested":
                        _model.ResolveLocation = "ResolveSCARequested";
                        break;
                }
            }

            _model.AdmissionStatus = preScreen.AdmissionStatus;
            return View(_model);

        }
        public JsonResult TimeRemainingForPreScreen(long preScreenId)
        {
            int hours = 48;
            string timeRemaining = null;

            string referralSourceName = null;

            string status = null;
            var preScreen = _dataContext.PreScreens.Where(p => p.PreScreenId == preScreenId).Take(1).SingleOrDefault();
            var referral = _dataContext.Referrals.Include("ReferralSource").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).Take(1).SingleOrDefault();

            if (preScreen != null)
            {
                status = preScreen.Status;
                if (preScreen.LastSubmitted != null)
                {
                    var targetDate = preScreen.LastSubmitted.Value.AddHours(hours);
                    var rem = targetDate.Subtract(DateTime.Now);
                    timeRemaining = $"{Math.Floor(rem.TotalHours)} hours, {rem.Minutes} minutes";
                }
            }

            if (referral != null)
            {
                referralSourceName = referral.ReferralSource.FullName;
            }

            return Json(new { TimeRemaining = timeRemaining, Status = status, ReferralSourceName = referralSourceName, AdmissionType = preScreen.AdmissionType, AdmissionStatus = preScreen.AdmissionStatus, AdmissionNotes = preScreen.AdmissionNotes }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Create(CreatePreScreenModel model)
        {
            if (ModelState.IsValid)
            {

                using (var transaction = _dataContext.Database.BeginTransaction())
                {
                    var preScreen = new PreScreen()
                    {
                        PatientId = model.PatientId,
                        PreScreenTypeId = model.PreScreenTypeId,
                        Status = "New"
                    };

                    _dataContext.PreScreens.Add(preScreen);
                    _dataContext.SaveChanges();
                    transaction.Commit();
                    return Json(new { Status = Constant.RESPONSE_OK, Description = "Pre-Screen created successfully.", PreScreenId = preScreen.PreScreenId });
                }



            }

            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });

        }

        [HttpPost]
        public JsonResult Delete(long? id)
        {
            var entity = _dataContext.PreScreens.Where(p => p.PreScreenId == id && p.Deleted == false).SingleOrDefault();
            if (entity != null)
            {
                entity.Deleted = true;
                _dataContext.SaveChanges();

                return Json(new { Status = Constant.RESPONSE_OK });
            }
            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
        }
        public JsonResult PatientPreScreen(long patientId)
        {

            var items = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PatientId == patientId && p.Deleted == false)
                .OrderByDescending(p => p.DateModified)
                .ToList().Select(p => new
                {
                    PatientId = p.Patient.PatientId,
                    //FirstName = p.Patient.FirstName,
                    //MiddleName = p.Patient.MiddleName,
                    //LastName = p.Patient.LastName,
                    //SSN = p.Patient.SSN,
                    //DateOfBirth = p.Patient.DateOfBirth.ToShortDateString(),
                    PreScreenId = p.PreScreenId,
                    PreScreenTypeName = p.PreScreenType.Name,
                    CreatedTimeStamp = p.DateCreated.ToString(),
                    ModifiedTimeStamp = p.DateModified.ToString()
                });

            return Json(items, JsonRequestBehavior.AllowGet);

        }
        public JsonResult PreScreens(long? preScreenTypeId, string keyword)
        {

            var items = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.Deleted == false).AsQueryable();

            if (preScreenTypeId != null && preScreenTypeId > 0)
            {
                items = items.Where(p => p.PreScreenTypeId == preScreenTypeId);
            }

            // items = items.Where(p => p.Patient.FirstName.StartsWith(keyword) || p.Patient.MiddleName.StartsWith(keyword) || p.Patient.LastName.StartsWith(keyword) || p.Patient.SSN.StartsWith(keyword) || string.IsNullOrEmpty(keyword));

            var data = items.ToList().Select(p => new
            {
                PatientId = p.Patient.PatientId,
                //FirstName = p.Patient.FirstName,
                //MiddleName = p.Patient.MiddleName,
                //LastName = p.Patient.LastName,
                //SSN = p.Patient.SSN,
                //DateOfBirth = p.Patient.DateOfBirth.ToShortDateString(),
                PreScreenId = p.PreScreenId,
                PreScreenTypeName = p.PreScreenType.Name,
                CreatedTimeStamp = p.DateCreated.ToString(),
                ModifiedTimeStamp = p.DateModified.ToString(),
                // PatientName = p.Patient.FirstName + " " + p.Patient.LastName
            });

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult PreScreen(long preScreenId)
        {
            var data = _dataContext.PreScreens.Include("Patient").Include("PreScreenType").Where(p => p.PreScreenId == preScreenId && p.Deleted == false).ToList().SingleOrDefault();
            if (data == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
            }

            int hours = 48;
            var referral = _dataContext.Referrals.Where(p => p.PreScreenId == data.PreScreenId).Take(1).SingleOrDefault();
            if (referral != null)
            {
                //Put code here for hours pre-screen will be available.
            }

            double? lastSubmitted = null;
            if (data.LastSubmitted != null)
            {
                lastSubmitted = data.LastSubmitted.Value.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            }

            return Json(new { PatientId = data.PatientId, PreScreenTypeId = data.PreScreenTypeId, LastSubmitted = lastSubmitted, Hours = hours, Status = data.Status, VerificationComplete = data.VerificationComplete }, JsonRequestBehavior.AllowGet);

        }


        private TabModel _patientTabs;

        PreScreenModel _model;

        private List<SectionModel> GetSections()
        {
            if (_model != null)
            {
                List<SectionModel> sections = new List<SectionModel>();
                foreach (var tab in _model.Tabs)
                {
                    foreach (var acc in tab.Accordions)
                    {
                        foreach (var sec in acc.Sections)
                        {
                            sections.Add(sec);
                            foreach (var acc1 in sec.Accordions)
                            {
                                foreach (var sec1 in acc1.Sections)
                                {
                                    sections.Add(sec1);
                                }
                            }
                        }
                    }
                }


                return sections;
            }

            return null;
        }

        string _prefix = "model.data.";
        private void StripModelDataPrefix(PatientDataModel model)
        {
            model.SectionCode = model.SectionCode.Replace(_prefix, "");
        }
        private void SavePatientData(PatientDataModel model)
        {
            _model = new PreScreenModel();

            SetContextLists();
            PopulateAccordions(_model);
            PopulateItems(_model);

            var preScreen = _dataContext.PreScreens.Find(model.PreScreenId);
            ApplyFilter(preScreen.PreScreenType.Name);

            List<PatientData> lstPatientData;
            List<PreScreenData> lstPreScreenData;

            lstPatientData = _dataContext.PatientData.Where(p => p.PatientId == model.ContextId && p.SectionCode == model.SectionCode && p.Deleted == false).ToList();
            lstPreScreenData = _dataContext.PreScreenData.Where(p => p.PreScreenId == model.ContextId && p.SectionCode == model.SectionCode && p.Deleted == false).ToList();

            if (model.Items != null)
            {
                foreach (var item in model.Items)
                {
                    CommonPatientData data;

                    switch (model.Context)
                    {
                        case "Patient":
                            data = lstPatientData.Where(p => p.ItemCode == item.Key).SingleOrDefault();

                            if (data == null)
                            {
                                data = new PatientData() { PatientId = model.ContextId };
                                _dataContext.PatientData.Add((PatientData)data);
                            }

                            break;
                        case "PreScreen":
                            data = lstPreScreenData.Where(p => p.ItemCode == item.Key).SingleOrDefault();

                            if (data == null)
                            {
                                data = new PreScreenData() { PreScreenId = model.ContextId };
                                _dataContext.PreScreenData.Add((PreScreenData)data);
                            }

                            break;
                        default:
                            throw new NotImplementedException();
                    }


                    var section = GetSections().Where(p => p.GetUniqueClientCode() == (_prefix + model.SectionCode)).Single();

                    data.ItemCode = item.Key;
                    data.SectionCode = model.SectionCode;
                    data.Label = section.Items.Where(p => p.ClientCode == item.Key).Single().Label;
                    data.Value = item.Value;
                    data.Type = section.Items.Where(p => p.ClientCode == item.Key).Single().ItemType;
                    data.DependsOnCode = section.Items.Where(p => p.ClientCode == item.Key).Single().DependsOnCode;
                    data.DependsOnAssertValue = section.Items.Where(p => p.ClientCode == item.Key).Single().DependsOnAssertValue;




                }
            }


            if (model.Tables != null)
            {

                foreach (var table in model.Tables)
                {
                    CommonPatientData data;

                    switch (model.Context)
                    {
                        case "Patient":
                            data = lstPatientData.Where(p => p.ItemCode == table.ClientCode).SingleOrDefault();

                            if (data == null)
                            {
                                data = new PatientData() { PatientId = model.ContextId };
                                _dataContext.PatientData.Add((PatientData)data);
                            }

                            break;
                        case "PreScreen":
                            data = lstPreScreenData.Where(p => p.ItemCode == table.ClientCode).SingleOrDefault();

                            if (data == null)
                            {
                                data = new PreScreenData() { PreScreenId = model.ContextId };
                                _dataContext.PreScreenData.Add((PreScreenData)data);
                            }

                            break;
                        default:
                            throw new NotImplementedException();
                    }


                    var section = GetSections().Where(p => p.GetUniqueClientCode() == (_prefix + model.SectionCode)).Single();

                    data.ItemCode = table.ClientCode;
                    data.SectionCode = model.SectionCode;
                    data.Label = section.Items.Where(p => p.ClientCode == table.ClientCode).Single().Label;
                    data.Value = Common.XmlSerializeType(table.GetType(), table);
                    data.Type = section.Items.Where(p => p.ClientCode == table.ClientCode).Single().ItemType;
                    data.DependsOnCode = section.Items.Where(p => p.ClientCode == table.ClientCode).Single().DependsOnCode;
                    data.DependsOnAssertValue = section.Items.Where(p => p.ClientCode == table.ClientCode).Single().DependsOnAssertValue;


                }
            }
        }
        private void SetDependentFields(PatientDataModel model)
        {

            string sectionCode = model.SectionCode;

            if (!sectionCode.StartsWith(_prefix))
            {
                sectionCode = $"{_prefix}{sectionCode}";
            }

            _model = new PreScreenModel();
            PopulateAccordions(_model);
            PopulateItems(_model);
            var preScreen = _dataContext.PreScreens.Find(model.PreScreenId);
            ApplyFilter(preScreen.PreScreenType.Name);

            var sections = GetSections();
            var section = sections.Where(p => p.GetUniqueClientCode() == sectionCode).Single();

            var dependentItems = section.Items.Where(p => p.DependsOnCode != null).ToList();

            foreach (var item in dependentItems)
            {
                var dependentValue = item.DependsOnAssertValue;
                var dependentCode = item.DependsOnCode;

                if (model.Items.Where(p => p.Key == dependentCode).Single().Value != dependentValue)
                {

                    var dependent = model.Items.Where(p => p.Key == item.ClientCode).Single();
                    dependent.Value = null;
                    item.Required = false;


                }

            }

        }

        [HttpPost]
        public JsonResult UpdatePatientData(string id, PatientDataModel model)
        {
            SetContextLists();
            //Filter 1, strip out the model.data prefix.
            StripModelDataPrefix(model);

            model.PreScreenId = long.Parse(id);

            SetDependentFields(model);

            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                //Save the patient data.

                SavePatientData(model);

                _dataContext.SaveChanges();

                //All the alerts and notifications/ tasks go here after saving a section.

                SendSCARequestedAlert(model.PreScreenId);
                SendPlanInGraceAlert(model.PreScreenId);

                _dataContext.SaveChanges();
                transaction.Commit();
            }

            return Json(new { Status = Constant.RESPONSE_OK, Description = "Section saved successfully." });
        }
        public JsonResult PatientData(string Context, string SectionCode, long ContextId)
        {
            PatientDataModel model = new PatientDataModel()
            {
                Context = Context,
                ContextId = ContextId,
                SectionCode = SectionCode
            };

            StripModelDataPrefix(model);
            string code = model.SectionCode;
            switch (Context)
            {
                case "Patient":
                    var lstPatient = _dataContext.PatientData.Where(p => p.PatientId == ContextId && p.SectionCode == code && p.Deleted == false).ToList();
                    model.Items = lstPatient.Where(p => p.Type != "Table").Select(p => new KeyValuePairModel() { Key = p.ItemCode, Value = p.Value }).ToList();
                    model.Tables = new List<TableDataModel>();
                    foreach (var item in lstPatient.Where(p => p.Type == "Table"))
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            model.Tables.Add((TableDataModel)Common.XmlDeserialize(typeof(TableDataModel), item.Value));
                        }

                    }
                    break;
                case "PreScreen":
                    var lstPreScreen = _dataContext.PreScreenData.Where(p => p.PreScreenId == ContextId && p.SectionCode == code && p.Deleted == false).ToList();
                    model.Items = lstPreScreen.Where(p => p.Type != "Table").Select(p => new KeyValuePairModel() { Key = p.ItemCode, Value = p.Value }).ToList();
                    model.Tables = new List<TableDataModel>();
                    foreach (var item in lstPreScreen.Where(p => p.Type == "Table"))
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            model.Tables.Add((TableDataModel)Common.XmlDeserialize(typeof(TableDataModel), item.Value));
                        }

                    }
                    break;
                default:
                    return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Context not implemented" });
            }


            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]

        public ActionResult Report(long id)
        {
            _model = new PreScreenModel();
            SetContextLists();
            PopulateAccordions(_model);
            PopulateItems(_model);
            var preScreen = _dataContext.PreScreens.Find(id);
            ApplyFilter(preScreen.PreScreenType.Name);
            ReportModel model = new ReportModel();

            var section = new ReportSectionModel() { Label = "Patient Demographics" };
            model.Sections.Add(section);

            var patientId = _dataContext.PreScreens.Find(id).PatientId;
            foreach (var item in _dataContext.PatientData.Where(p => p.PatientId == patientId && p.Deleted == false && p.SectionCode == "demographics.general").OrderBy(p => p.PatientDataId).ToList())
            {
                section.Items.Add(new ReportItemModel() { Label = item.Label, Value = item.Value });
            }

            Dictionary<string, string> sections = new Dictionary<string, string>();



            sections.Add("psstatus.prehospitalliving", "PS Pt. Status - Pre - Hospital Living");
            sections.Add("psstatus.languagecommunicationneeds", "PS Pt. Status - Language Communication Needs");
            sections.Add("psstatus.preadmission", "PS Pt. Status - Pre - Admission");

            sections.Add("psstatus.listallacute", "PS Pt. Status - List All Acute Care Hospitalizations During the Last 60 Days");
            sections.Add("psstatus.surgery", "PS Pt. Status - Surgery");
            sections.Add("psstatus.socialhistory", "PS Pt. Status - Social History");
            sections.Add("psstatus.ivaccess", "PS Pt. Status -  IV Access");
            sections.Add("psstatus.ivfluids", "PS Pt. Status - IV Fluids or IV Meds(e.g., IV Antibiotics, IV Lasix etc.) > _ 1x daily");
            sections.Add("psstatus.infection", "PS Pt. Status - Infection");
            sections.Add("psstatus.isolationtype", "PS Pt. Status - Isolation Type");
            sections.Add("psstatus.diet", "PS Pt. Status - Diet");
            sections.Add("psstatus.bladder", "PS Pt. Status - Bladder");
            sections.Add("psstatus.bladderappliance", "PS Pt. Status - Bladder Appliance");
            sections.Add("psstatus.bowel", "PS Pt. Status - Bowel");
            sections.Add("psstatus.bowelappliance", "PS Pt. Status - Bowel Appliance");
            sections.Add("psstatus.vitals", "PS Pt. Status - Vitals");
            sections.Add("psstatus.painscale", "PS Pt. Status - Pain Scale");
            sections.Add("psstatus.vaccinations", "PS Pt. Status - Vaccinations");
            sections.Add("psstatus.psychsocial", "PS Pt. Status - Psych - Social");



            sections.Add("pssystems.specialprecautionsisolation", "PS Pt. Systems - Special Precautions / Isolation");
            sections.Add("pssystems.allergies", "PS Pt. Systems - Allergies");
            sections.Add("pssystems.neuroassessment", "PS Pt. Systems - Neuro Assessment");
            sections.Add("pssystems.cognitive", "PS Pt. Systems - Cognitive");
            sections.Add("pssystems.vision", "PS Pt. Systems - Vision");
            sections.Add("pssystems.hearing", "PS Pt. Systems - Hearing");
            sections.Add("pssystems.cardiovascular", "PS Pt. Systems - Cardiovascular");
            sections.Add("pssystems.gastrointestinal", "PS Pt. Systems - Gastrointestinal");
            sections.Add("pssystems.musculoskeletal", "PS Pt. Systems - Musculoskeletal");
            sections.Add("pssystems.skin", "PS Pt. Systems - Skin");
            sections.Add("pssystems.wounds", "PS Pt. Systems - Wounds");
            sections.Add("pssystems.endocrine", "PS Pt. Systems - Endocrine");
            sections.Add("pssystems.renalfunction", "PS Pt. Systems - Renal Function");
            sections.Add("pssystems.dialysis", "PS Pt. Systems - Dialysis");
            sections.Add("pssystems.dialysisaccess", "PS Pt. Systems - Dialysis Access");



            sections.Add("psfunctions.ptotsummary", "PS Function & Equipment - PT / OT Summary");
            sections.Add("psfunctions.motor", "PS Function & Equipment - Motor");
            sections.Add("psfunctions.verbal", "PS Function & Equipment - Verbal");
            sections.Add("psfunctions.safety", "PS Function & Equipment - Safety");
            sections.Add("psfunctions.restraints", "PS Function & Equipment - Restraints");
            sections.Add("psfunctions.specialequipment", "PS Function & Equipment - Special Equipment");



            sections.Add("psrespiratory.respiratorystatus", "PS Respiratory - Respiratory Status");
            sections.Add("psrespiratory.treatments", "PS Respiratory - Treatments");
            sections.Add("psrespiratory.oxygenrequired", "PS Respiratory - Oxygen Required");
            sections.Add("psrespiratory.mode", "PS Respiratory - Mode");
            sections.Add("psrespiratory.ventilation", "PS Respiratory - Ventilation");
            sections.Add("psrespiratory.frequency", "PS Respiratory - Frequency");
            sections.Add("psrespiratory.mode2", "PS Respiratory - Mode");
            sections.Add("psrespiratory.arterialbloodgases", "PS Respiratory - Arterial Blood Gases");
            sections.Add("psrespiratory.trach", "PS Respiratory - Trach");
            sections.Add("psrespiratory.ettube", "PS Respiratory - ET Tube");
            sections.Add("psrespiratory.chesttube", "PS Respiratory - Chest Tube");

            sections.Add("pslabs.medicationlist", "PS Labs - Medication List");
            sections.Add("pslabs.bloodproducts", "PS Labs - Blood/ Blood Products");
            sections.Add("pslabs.bloodworklabs", "PS Labs - Blood Work/ Labs");
            sections.Add("pslabs.microbiology", "PS Labs - Microbiology");
            sections.Add("pslabs.studies", "PS Labs - Studies");

            foreach (var key in sections.Keys)
            {
                section = new ReportSectionModel() { Label = sections[key] };
                foreach (var item in _dataContext.PreScreenData.Where(p => p.PreScreenId == id && p.Deleted == false && p.SectionCode == key).OrderBy(p => p.PreScreenDataId).ToList())
                {
                    var reportItem = new ReportItemModel() { Label = item.Label, Value = item.Value, Type = item.Type };
                    if (reportItem.Type == "Table")
                    {
                        reportItem.Table = (TableDataModel)Common.XmlDeserialize(typeof(TableDataModel), item.Value);
                        reportItem.HeaderList = GetSections().Where(p => p.GetUniqueClientCode() == $"{_prefix}{key}").Single().Items.Where(p => p.ClientCode == item.ItemCode).Single().LabelList;
                    }
                    section.Items.Add(reportItem);
                }
                model.Sections.Add(section);
            }


            return View(model);
        }
        public ActionResult Acuity(long id)
        {
            _model = new PreScreenModel();
            SetContextLists();
            PopulateAccordions(_model);
            PopulateItems(_model);
            var preScreen = _dataContext.PreScreens.Find(id);
            ApplyFilter(preScreen.PreScreenType.Name);

            AcuityReportModel model = new AcuityReportModel();

            var sections = GetSections();

            foreach (var section in sections)
            {
                foreach (var item in section.Items)
                {

                    if (item.AcuityICU)
                    {
                        model.Items.Add(new AcuityReportItemModel()
                        {
                            Category = "ICU",
                            ItemCode = item.ClientCode,
                            Label = item.Label,
                            Section = section.GetUniqueClientCode(),
                            AssertValue = item.AcuityAsserValue,
                            SectionLabel = section.Label
                        });


                    }

                    if (item.AcuityINT)
                    {
                        model.Items.Add(new AcuityReportItemModel()
                        {
                            Category = "INT",
                            ItemCode = item.ClientCode,
                            Label = item.Label,
                            Section = section.GetUniqueClientCode(),
                            AssertValue = item.AcuityAsserValue,
                            SectionLabel = section.Label
                        });


                    }

                    if (item.AcuityMed)
                    {
                        model.Items.Add(new AcuityReportItemModel()
                        {
                            Category = "MED",
                            ItemCode = item.ClientCode,
                            Label = item.Label,
                            Section = section.GetUniqueClientCode(),
                            AssertValue = item.AcuityAsserValue,
                            SectionLabel = section.Label
                        });


                    }
                }
            }

            var preScreenData = _dataContext.PreScreenData.Where(p => p.PreScreenId == id && p.Deleted == false).OrderBy(p => p.PreScreenDataId).ToList();

            foreach (var acuityItem in model.Items)
            {
                var item = preScreenData.Where(p => $"{_prefix}{p.SectionCode}" == acuityItem.Section && p.ItemCode == acuityItem.ItemCode).Take(1).SingleOrDefault();
                if (item != null)
                {
                    acuityItem.Value = item.Value;
                    if (acuityItem.AssertValue == item.Value)
                    {
                        acuityItem.Positive = true;
                    }
                }

            }

            return View(model);
        }
        public ActionResult IVF(long id)
        {
            _model = new PreScreenModel();
            SetContextLists();
            PopulateAccordions(_model);
            PopulateItems(_model);
            var preScreen = _dataContext.PreScreens.Find(id);
            ApplyFilter(preScreen.PreScreenType.Name);
            ReportModel model = new ReportModel();

            var section = new ReportSectionModel() { Label = "Patient Demographics", DisplaySeparate = true };
            model.Sections.Add(section);



            var patientId = _dataContext.PreScreens.Find(id).PatientId;
            foreach (var item in _dataContext.PatientData.Where(p => p.PatientId == patientId && p.Deleted == false && p.SectionCode == "demographics.general").OrderBy(p => p.PatientDataId).ToList())
            {
                section.Items.Add(new ReportItemModel() { Label = item.Label, Value = item.Value });
            }

            Dictionary<string, string> sections = new Dictionary<string, string>();

            sections.Add("payorinfo.primary.all", "Primary Payor - All");
            sections.Add("payorinfo.primary.general", "Primary Payor - General");
            sections.Add("payorinfo.primary.medicare", "Primary Payor - Medicare");

            sections.Add("payorinfo.secondary.all", "Secondary Payor - All");
            sections.Add("payorinfo.secondary.general", "Secondary Payor - General");
            sections.Add("payorinfo.secondary.medicare", "Secondary Payor - Medicare");

            sections.Add("payorinfo.tertiary.all", "Tertiary Payor - All");
            sections.Add("payorinfo.tertiary.general", "Tertiary Payor - General");
            sections.Add("payorinfo.tertiary.medicare", "Tertiary Payor - Medicare");

            var modelSections = GetSections();

            foreach (var key in sections.Keys)
            {
                section = new ReportSectionModel() { Label = sections[key] };
                foreach (var item in _dataContext.PatientData.Where(p => p.PatientId == patientId && p.Deleted == false && p.SectionCode == key).OrderBy(p => p.PatientDataId).ToList())
                {
                    if (modelSections.Where(p => p.GetUniqueClientCode() == $"{_prefix}{key}").Single().Items.Where(p => p.ClientCode == item.ItemCode && p.IsIVFField == true).Count() <= 0)
                    {
                        continue;
                    }
                    var reportItem = new ReportItemModel() { Label = item.Label, Value = item.Value, Type = item.Type };
                    if (reportItem.Type == "Table")
                    {
                        reportItem.Table = (TableDataModel)Common.XmlDeserialize(typeof(TableDataModel), item.Value);
                        reportItem.HeaderList = GetSections().Where(p => p.GetUniqueClientCode() == $"{_prefix}{key}").Single().Items.Where(p => p.ClientCode == item.ItemCode).Single().LabelList;
                    }
                    section.Items.Add(reportItem);
                }
                model.Sections.Add(section);
            }


            return View(model);
        }
        public ActionResult PatientSummary(long id)
        {
            _model = new PreScreenModel();
            SetContextLists();
            PopulateAccordions(_model);
            PopulateItems(_model);
            var preScreen = _dataContext.PreScreens.Find(id);
            ApplyFilter(preScreen.PreScreenType.Name);

            ReportModel model = new ReportModel();

            var section = new ReportSectionModel() { Label = "Patient Demographics" };
            model.Sections.Add(section);

            var modelSections = GetSections();

            var patientId = _dataContext.PreScreens.Find(id).PatientId;
            foreach (var item in _dataContext.PatientData.Where(p => p.PatientId == patientId && p.Deleted == false && p.SectionCode == "demographics.general").OrderBy(p => p.PatientDataId).ToList())
            {
                section.Items.Add(new ReportItemModel() { Label = item.Label, Value = item.Value });
            }


            Dictionary<string, string> sections = new Dictionary<string, string>();

            sections.Add("psstatus.preadmission", "PS Pt. Status - Pre - Admission");
            sections.Add("psstatus.ivfluids", "PS Pt. Status - IV Fluids or IV Meds(e.g., IV Antibiotics, IV Lasix etc.) > _ 1x daily");
            sections.Add("psstatus.isolationtype", "PS Pt. Status - Isolation Type");
            sections.Add("psstatus.diet", "PS Pt. Status - Diet");
            sections.Add("psstatus.bladder", "PS Pt. Status - Bladder");

            sections.Add("pssystems.neuroassessment", "PS Pt. Systems - Neuro Assessment");
            sections.Add("pssystems.cardiovascular", "PS Pt. Systems - Cardiovascular");
            sections.Add("pssystems.gastrointestinal", "PS Pt. Systems - Gastrointestinal");
            sections.Add("pssystems.wounds", "PS Pt. Systems - Wounds");
            sections.Add("pssystems.renalfunction", "PS Pt. Systems - Renal Function");
            sections.Add("pssystems.dialysis", "PS Pt. Systems - Dialysis");

            sections.Add("psfunctions.ptotsummary", "PS Function & Equipment - PT / OT Summary");
            sections.Add("psfunctions.specialequipment", "PS Function & Equipment - Special Equipment");

            sections.Add("psrespiratory.treatments", "PS Respiratory - Treatments");
            sections.Add("psrespiratory.oxygenrequired", "PS Respiratory - Oxygen Required");
            sections.Add("psrespiratory.ventilation", "PS Respiratory - Ventilation");
            sections.Add("psrespiratory.mode2", "PS Respiratory - Mode");
            sections.Add("psrespiratory.arterialbloodgases", "PS Respiratory - Arterial Blood Gases");
            sections.Add("psrespiratory.trach", "PS Respiratory - Trach");
            sections.Add("psrespiratory.chesttube", "PS Respiratory - Chest Tube");

            sections.Add("pslabs.medicationlist", "PS Labs - Medication List");
            sections.Add("pslabs.bloodworklabs", "PS Labs - Blood Work/ Labs");

            section = new ReportSectionModel() { Label = "Key Clinical Indicators" };
            model.Sections.Add(section);

            foreach (var key in sections.Keys)
            {

                foreach (var item in _dataContext.PreScreenData.Where(p => p.PreScreenId == id && p.Deleted == false && p.SectionCode == key).OrderBy(p => p.PreScreenDataId).ToList())
                {
                    if (modelSections.Where(p => p.GetUniqueClientCode() == $"{_prefix}{key}").Single().Items.Where(p => p.ClientCode == item.ItemCode && p.KeyClinicalIndicator == true).Count() <= 0)
                    {
                        continue;
                    }


                    var reportItem = new ReportItemModel() { Label = item.Label, Value = item.Value, Type = item.Type };
                    if (reportItem.Type == "Table")
                    {
                        reportItem.Table = (TableDataModel)Common.XmlDeserialize(typeof(TableDataModel), item.Value);
                        reportItem.HeaderList = modelSections.Where(p => p.GetUniqueClientCode() == $"{_prefix}{key}").Single().Items.Where(p => p.ClientCode == item.ItemCode).Single().LabelList;
                    }
                    if (reportItem.Type == "Dropdown")
                    {

                        var value = item.Value;
                        var values = modelSections.Where(p => p.GetUniqueClientCode() == $"{_prefix}{key}").Single().Items.Where(p => p.ClientCode == item.ItemCode).Single().ValueList.ToArray();
                        var labels = modelSections.Where(p => p.GetUniqueClientCode() == $"{_prefix}{key}").Single().Items.Where(p => p.ClientCode == item.ItemCode).Single().LabelList.ToArray();
                        var index = Array.IndexOf(values, value);
                        reportItem.Value = labels[index];
                    }


                    var modelItem = modelSections.Where(p => p.GetUniqueClientCode() == $"{_prefix}{key}").Single().Items.Where(p => p.ClientCode == item.ItemCode).Single();

                    if (modelItem.KeyClinicalIndicatorAssertValue != null)
                    {
                        if (modelItem.KeyClinicalIndicatorAssertValue.Split('|').Contains(item.Value))
                        {
                            section.Items.Add(reportItem);
                        }

                    }
                    else
                    {
                        section.Items.Add(reportItem);
                    }


                }

            }



            //Add code for key clinical indicators.

            sections = new Dictionary<string, string>();

            sections.Add("referralsourceinfo.general", "Referral Source Info");



            foreach (var key in sections.Keys)
            {
                section = new ReportSectionModel() { Label = sections[key] };
                foreach (var item in _dataContext.PreScreenData.Where(p => p.PreScreenId == id && p.Deleted == false && p.SectionCode == key).OrderBy(p => p.PreScreenDataId).ToList())
                {

                    var reportItem = new ReportItemModel() { Label = item.Label, Value = item.Value, Type = item.Type };
                    if (reportItem.Type == "Table")
                    {
                        reportItem.Table = (TableDataModel)Common.XmlDeserialize(typeof(TableDataModel), item.Value);
                        reportItem.HeaderList = modelSections.Where(p => p.GetUniqueClientCode() == $"{_prefix}{key}").Single().Items.Where(p => p.ClientCode == item.ItemCode).Single().LabelList;
                    }
                    if (reportItem.Type == "Dropdown")
                    {

                        var value = item.Value;
                        var values = modelSections.Where(p => p.GetUniqueClientCode() == $"{_prefix}{key}").Single().Items.Where(p => p.ClientCode == item.ItemCode).Single().ValueList.ToArray();
                        var labels = modelSections.Where(p => p.GetUniqueClientCode() == $"{_prefix}{key}").Single().Items.Where(p => p.ClientCode == item.ItemCode).Single().LabelList.ToArray();
                        var index = Array.IndexOf(values, value);
                        reportItem.Value = labels[index];
                    }

                    section.Items.Add(reportItem);
                }
                model.Sections.Add(section);
            }


            return View(model);
        }


        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                //TempData["UploadedFiles"] = Basic_Usage_Get_File_Info(files);
            }

            return Content("Ok");
        }



    }

}






