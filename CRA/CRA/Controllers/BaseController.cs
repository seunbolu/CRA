using CRA.Authorization;
using CRA.Data;
using CRA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace CRA.Controllers
{

    [AppConnectAuthorize]
    public class BaseController : Controller
    {
        protected DataContext _dataContext = null;

        protected AuthorizedUserModel userSecurityContext;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            userSecurityContext = (AuthorizedUserModel)filterContext.HttpContext.Items["User"];
            _dataContext = new DataContext(userSecurityContext.UserId);
            base.OnActionExecuting(filterContext);
        }

        public string GetPatientData(long patientId, string sectionCode, string itemCode, List<PatientData> items)
        {

            var item = items.Where(p => p.PatientId == patientId && p.SectionCode == sectionCode && p.ItemCode == itemCode).Take(1).SingleOrDefault();

            if (item != null)
            {
                return item.Value;
            }

            return null;
        }

        public string GetPatientData(long patientId, string sectionCode, string itemCode)
        {

            var item = _dataContext.PatientData.Where(p => p.PatientId == patientId && p.Deleted == false && p.SectionCode == sectionCode && p.ItemCode == itemCode).Take(1).SingleOrDefault();

            if (item != null)
            {
                return item.Value;
            }

            return null;
        }


        public PatientData GetNewPatientDataEntity(string sectionCode, string itemCode, string value, string type, string label, long patientId)
        {
            PatientData patientData = new PatientData();
            patientData.SectionCode = sectionCode;
            patientData.ItemCode = itemCode;
            patientData.Value = value;
            patientData.Type = type;
            patientData.Label = label;
            patientData.PatientId = patientId;

            return patientData;
        }


        public PreScreenData GetNewPreScreenDataEntity(string sectionCode, string itemCode, string value, string type, string label, long preScreenId)
        {
            PreScreenData preScreenData = new PreScreenData();
            preScreenData.SectionCode = sectionCode;
            preScreenData.ItemCode = itemCode;
            preScreenData.Value = value;
            preScreenData.Type = type;
            preScreenData.Label = label;
            preScreenData.PreScreenId = preScreenId;

            return preScreenData;
        }

        public bool HasContextRole(string role)
        {
            if (userSecurityContext != null)
            {
                if (userSecurityContext.Roles.Contains(role))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasContextRole(string [] roles)
        {
            foreach(var role in roles)
            {
                if (HasContextRole(role))
                {
                    return true;
                }
            }

            return false;
        }

        public ActionResult RedirectToUnauthorized()
        {
            return new RedirectToRouteResult(
                           new RouteValueDictionary(
                               new
                               {
                                   controller = "Status",
                                   action = "Unauthorized"
                               })
                           );
        }
        
        public JsonResult ServiceTypes()
        {
            var items = _dataContext
                .ServiceTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.ServiceTypeId).ToList();

            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RegionTypes(long ServiceTypeId)
        {
            var items = _dataContext
                .RegionServiceTypes
                .Where(p => p.ServiceTypeId == ServiceTypeId && p.Deleted == false)
                .Select(p => p.RegionType)
                .OrderBy(p => p.RegionTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegionTypesAll()
        {
            var items = _dataContext
                .RegionServiceTypes
                .Where(p => p.Deleted == false)
                 .OrderBy(p => p.RegionTypeId)
                .Select(p => new { Name = p.RegionType.Name, RegionTypeId = p.RegionType.RegionTypeId, ServiceTypeId = p.ServiceTypeId })
               .ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PreScreenUpdateTypes()
        {
            var items = _dataContext
                .PreScreenUpdateTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.PreScreenUpdateTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReferralSourceTypes()
        {
            var items = _dataContext
                .ReferralSourceTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.ReferralSourceTypeId).ToList();

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserRoleTypes()
        {
            var items = _dataContext
                .UserRoleTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.UserRoleTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegionServiceTypes()
        {
            var items = _dataContext
                .RegionServiceTypes
                .Where(p => p.Deleted == false)
                .Select(p => new
                {
                    RegionServiceTypeId = p.RegionServiceId,
                    ServiceTypeId = p.ServiceTypeId,
                    RegionTypeId = p.RegionTypeId,
                    ServiceName = p.ServiceType.Name,
                    RegionName = p.RegionType.Name
                })
                .OrderBy(p => p.ServiceTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Sites()
        {
            var items = _dataContext
                .GetUserSites(_dataContext.UserId.Value)
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.ServiceTypeId)
                .OrderBy(p => p.RegionTypeId)
                .Select(p => new { CHGSiteId = p.CHGSiteId, ServiceName = p.ServiceType.Name, RegionName = p.RegionType.Name, FullName = p.FullName, ServiceTypeId = p.ServiceTypeId, RegionTypeId = p.RegionTypeId })
                .ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PreScreenTypes()
        {
            var items = _dataContext
                .PreScreenTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.PreScreenTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CategoryTypes()
        {
            var items = _dataContext
                .CategoryTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.CategoryTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ContactRoleTypes()
        {
            var items = _dataContext
                .ContactRoleTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.ContactRoleTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SpecialityTypes()
        {
            var items = _dataContext
                .SpecialityTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.SpecialityTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ElectronicReferralTypes()
        {
            var items = _dataContext
                .ElectronicReferralTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.ElectronicReferralTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CommercialPayorTypes()
        {
            var items = _dataContext
                .CommercialPayorTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.CommercialPayorTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ManagedMedicarePayorTypes()
        {
            var items = _dataContext
                .ManagedMedicarePayorTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.ManagedMedicarePayorTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

       

        public JsonResult GenderTypes()
        {
            var items = _dataContext
                .GenderTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.GenderTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult EthnicityTypes()
        {
            var items = _dataContext
                .EthnicityTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.EthnicityTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult MaritalStatusTypes()
        {
            var items = _dataContext
                .MaritalStatusTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.MaritalStatusTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LanguageTypes()
        {
            var items = _dataContext
                .LanguageTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.LanguageTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RelationshipTypes()
        {
            var items = _dataContext
                .RelationshipTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.RelationshipTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CountryTypes()
        {
            var items = _dataContext
                .CountryTypes
                .Where(p => p.Deleted == false)
                .OrderBy(p => p.CountryTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StateTypes(long countryTypeId)
        {
            var items = _dataContext
                .StateTypes
                .Where(p => p.Deleted == false)
                .Where(p => p.CountryTypeId == countryTypeId)
                .OrderBy(p => p.StateTypeId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCurrentUserNotifcations()
        {
            if(_dataContext.UserId==null || _dataContext.UserId.Value <= 0)
            {
                Json(null, JsonRequestBehavior.AllowGet);
            }
            var userId = _dataContext.UserId;
            List<Notification> notifications = new List<Notification>();
            if (userId != null && userId > 0)
            {

                long lastNotificationId = 0;

                var entity = _dataContext.UserNotifications.Where(p => p.UserId == userId && p.Deleted == false).SingleOrDefault();
                if (entity == null)
                {
                    entity = new UserNotification();
                    _dataContext.UserNotifications.Add(entity);
                    entity.UserId = userId.Value;
                    entity.LastNotificationId = null;
                    lastNotificationId = 0;
                }
                else
                {
                    lastNotificationId = entity.LastNotificationId ?? 0;
                }

                notifications = _dataContext.Notifications.Where(p => p.UserId == userId.Value && p.Deleted == false && p.NotificationId > lastNotificationId).OrderByDescending(p => p.NotificationId).ToList();

                if (notifications.Count() > 0)
                {
                    lastNotificationId = notifications.Max(p => p.NotificationId);
                    entity.LastNotificationId = lastNotificationId;
                }

                _dataContext.SaveChanges();

            }

            return Json(notifications, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContextUserSites()
        {
            var items = _dataContext.GetUserSites(_dataContext.UserId.Value);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}