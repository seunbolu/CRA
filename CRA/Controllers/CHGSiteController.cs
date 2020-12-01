using CRA.Data;
using CRA.Data.Entities;
using CRA.Data.Tracking;
using CRA.Models.CHGSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{
    public class CHGSiteController : BaseController
    {


        public JsonResult CHGSites(long? serviceTypeId, string keyword)
        {
            List<CHGSiteModel> model = new List<CHGSiteModel>();

            var query = (from c in _dataContext.CHGSites.Where(p => p.Deleted == false) select c);

            if (serviceTypeId != null && serviceTypeId > 0)
            {
                query = query.Where(p => p.ServiceTypeId == serviceTypeId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.FullName.Contains(keyword));
            }

            foreach (var entity in query.ToList())
            {
                model.Add(new CHGSiteModel()
                {
                    CHGSiteId = entity.CHGSiteId,
                    ShortName = entity.ShortName,
                    FullName = entity.FullName,
                    Address = entity.Address,
                    BedCount = entity.BedCount,
                    ServiceName = entity.ServiceType.Name,
                    RegionName = entity.RegionType.Name,
                    ServiceTypeId = entity.ServiceTypeId,
                    RegionTypeId = entity.RegionTypeId
                });
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return RedirectToUnauthorized();
            }
            return View();
        }

        public ActionResult Create()
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return RedirectToUnauthorized();
            }
            ViewBag.ltachId = _dataContext.ServiceTypes.Where(p => p.Name == "LTACH" && p.Deleted == false).Single().ServiceTypeId;

            return View();
        }

        [HttpPost]
        public JsonResult Create(CHGSiteModel model)
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }


            var ltachId = _dataContext.ServiceTypes.Where(p => p.Name == "LTACH" && p.Deleted == false).Single().ServiceTypeId;


            if (ModelState.IsValid)
            {
                using (var transaction = _dataContext.Database.BeginTransaction())
                {
                    using (var changeSet = new ChangeSetTracking(_dataContext))
                    {
                        if (model.ServiceTypeId == ltachId && (model.PreScreenUpdateTypeId == null || model.PreScreenUpdateTypeId <= 0))
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });
                        }
                        if (model.ServiceTypeId != ltachId)
                        {
                            model.PreScreenUpdateTypeId = null;
                        }

                        if (_dataContext.CHGSites.Where(p => p.FullName == model.FullName && p.Deleted == false).Count() <= 0)
                        {
                            CHGSite entity = new CHGSite()
                            {
                                FullName = model.FullName,
                                RegionTypeId = model.RegionTypeId,
                                ShortName = model.ShortName,
                                Address = model.Address,
                                BedCount = model.BedCount,
                                OperationalICUBedCount = model.OperationalICUBedCount,
                                PreScreenUpdateTypeId = model.PreScreenUpdateTypeId,
                                ServiceTypeId = model.ServiceTypeId
                            };

                            _dataContext.CHGSites.Add(entity);
                            _dataContext.SaveChanges();
                            transaction.Commit();

                            return Json(new { Status = Constant.RESPONSE_OK, Description = "Site created successfully." });
                        }
                        else
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "The site with the same full name already exists. Please enter a different name." });
                        }
                    }  
                }
            }

            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });

        }

        public ActionResult Detail()
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return RedirectToUnauthorized();
            }
            ViewBag.ltachId = _dataContext.ServiceTypes.Where(p => p.Name == "LTACH" && p.Deleted == false).Single().ServiceTypeId;
            return View();
        }

        public JsonResult DetailEntity(long? id)
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }
            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." });
            }

            var entity = _dataContext.CHGSites.Where(p => p.CHGSiteId == id && p.Deleted == false).SingleOrDefault();
            if (entity == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "CHG site not found. Invalid CHG site." });
            }

            CHGSiteModel model = new CHGSiteModel()
            {
                CHGSiteId = entity.CHGSiteId,
                Address = entity.Address,
                BedCount = entity.BedCount,
                FullName = entity.FullName,
                OperationalICUBedCount = entity.OperationalICUBedCount,
                PreScreenUpdateTypeId = entity.PreScreenUpdateTypeId,
                RegionName = entity.RegionType.Name,
                ServiceName = entity.ServiceType.Name,
                RegionTypeId = entity.RegionTypeId,
                ServiceTypeId = entity.ServiceTypeId,
                ShortName = entity.ShortName,
                PreScreenUpdateTypeName = entity.PreScreenUpdateTypeId != null ? entity.PreScreenUpdateType.Name : null
            };

            return Json(new { Status = Constant.RESPONSE_OK, Data = model }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit()
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return RedirectToUnauthorized();
            }
            ViewBag.ltachId = _dataContext.ServiceTypes.Where(p => p.Name == "LTACH" && p.Deleted == false).Single().ServiceTypeId;
            return View();
        }

        [HttpPost]
        public JsonResult Edit(long? id, CHGSiteModel model)
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }
            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." });
            }

            var ltachId = _dataContext.ServiceTypes.Where(p => p.Name == "LTACH" && p.Deleted == false).Single().ServiceTypeId;

            if (ModelState.IsValid)
            {
                using (var transaction = _dataContext.Database.BeginTransaction())
                {

                    using (var changeSetTracking = new ChangeSetTracking(_dataContext))
                    {

                        if (model.ServiceTypeId == ltachId && (model.PreScreenUpdateTypeId == null || model.PreScreenUpdateTypeId <= 0))
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });
                        }

                        if (model.ServiceTypeId != ltachId)
                        {
                            model.PreScreenUpdateTypeId = null;
                        }


                        if (_dataContext.CHGSites.Where(p => p.FullName == model.FullName && p.CHGSiteId != id && p.Deleted == false).Count() <= 0)
                        {
                            var entity = _dataContext.CHGSites.Where(p => p.CHGSiteId == id && p.Deleted == false).SingleOrDefault();
                            if (entity == null)
                            {
                                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "CHG site not found. Invalid CHG site." });
                            }


                            entity.FullName = model.FullName;
                            entity.ShortName = model.ShortName;
                            entity.RegionTypeId = model.RegionTypeId;
                            entity.ServiceTypeId = model.ServiceTypeId;
                            entity.Address = model.Address;
                            entity.OperationalICUBedCount = model.OperationalICUBedCount;
                            entity.BedCount = model.BedCount;
                            entity.PreScreenUpdateTypeId = model.PreScreenUpdateTypeId;
                            _dataContext.SaveChanges();
                            transaction.Commit();
                            return Json(new { Status = Constant.RESPONSE_OK, Description = "Site saved successfully." });
                        }
                        else
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "The site with the same full name already exists. Please enter a different name." });
                        }
                    }

                
                }
            }

            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });

        }


        [HttpPost]
        public JsonResult Delete(long? id)
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                using (var changeSetTracking = new ChangeSetTracking(_dataContext))
                {
                    var entity = _dataContext.CHGSites.Where(p => p.CHGSiteId == id).SingleOrDefault();
                    if (entity != null)
                    {
                        entity.Deleted = true;
                        _dataContext.SaveChanges();
                        transaction.Commit();
                        return Json(new { Status = Constant.RESPONSE_OK });
                    }
                }
            }
         
            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
        }





    }

}