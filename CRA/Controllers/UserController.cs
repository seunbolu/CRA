using CRA.Data;
using CRA.Data.Entities;
using CRA.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{
    public class UserController : BaseController
    {

        public JsonResult Users(string keyword, long? userRoleTypeId)
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized" }, JsonRequestBehavior.AllowGet);
            }

            List<UserModel> model = new List<UserModel>();

            var query = (from c in _dataContext.Users.Include("Contact") where c.Deleted == false select c);

            query = query.Where(p => p.Deleted == false && p.Enabled == true);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.Contact.FirstName.StartsWith(keyword));
            }

            if (userRoleTypeId != null && userRoleTypeId != 0)
            {
                query = query.Join(_dataContext.UserRoles.Where(p => p.UserRoleTypeId == userRoleTypeId && p.Deleted == false), s => s.UserId, t => t.UserId, (s, t) => new { User = s }).Select(p => p.User);
            }

            foreach (var entity in query.ToList())
            {
                model.Add(new UserModel()
                {
                    UserId = entity.UserId,
                    DomainName = entity.DomainName,
                    UserName = entity.UserName,
                    FirstName = entity.Contact.FirstName,
                    LastName = entity.Contact.LastName,
                    Mobile = entity.Contact.Mobile,
                    Phone = entity.Contact.Phone,
                    Email = entity.Contact.Email
                });
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteUsers(string term)
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized" }, JsonRequestBehavior.AllowGet);
            }

            var query = (from c in _dataContext.Users.Include("Contact") where c.Deleted == false select c);

            query = query.Where(p => p.Deleted == false && p.Enabled == false);

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(p => p.Contact.FirstName.StartsWith(term));
            }

            List<object> items = new List<object>();
            foreach (var entity in query.ToList())
            {

                items.Add(new
                {
                    label = string.Format(@"{0}\{1}, {2} {3}, {4}", entity.DomainName, entity.UserName, entity.Contact.FirstName, entity.Contact.LastName, entity.Contact.Email),
                    value = entity.UserId,
                    userName = entity.UserName,
                    userId = entity.UserId
                });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return RedirectToUnauthorized();
            }
            return View();
        }

        public ActionResult Detail()
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return RedirectToUnauthorized();
            }
            return View();
        }

        public ActionResult Edit()
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
            return View();
        }

        [HttpPost]
        public JsonResult Edit(long? id, UserModel model)
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }

            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." });
            }


            if (ModelState.IsValid)
            {
                using (var transaction = _dataContext.Database.BeginTransaction())
                {

                    var entity = _dataContext.Users.Where(p => p.UserId == id && p.Deleted == false).SingleOrDefault();
                    if (entity == null)
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "User not found. Invalid User." });
                    }


                    entity.Contact.Phone = model.Phone;
                    entity.Contact.Mobile = model.Mobile;

                    _dataContext.SaveChanges();


                    long userId = entity.UserId;
                    long userRoleTypeId = _dataContext.UserRoleTypes.Where(p => p.Name == model.UserRole.RoleName && p.Deleted == false).Single().UserRoleTypeId;
                    var userRole = _dataContext.UserRoles.Where(p => p.UserId == userId && p.Deleted == false).Take(1).SingleOrDefault();

                    ClearRoles(userId);
                    _dataContext.SaveChanges();


                    if (userRole == null)
                    {
                        userRole = new UserRole();
                        userRole.UserRoleTypeId = userRoleTypeId;
                        userRole.UserId = userId;
                        _dataContext.UserRoles.Add(userRole);
                    }
                    else
                    {
                        userRole.UserRoleTypeId = userRoleTypeId;
                    }

                    _dataContext.SaveChanges();

                    if (model.UserRole.RoleName == "SLH")
                    {
                        if (model.UserRole.Services == null || model.UserRole.Services.Where(p => p.Selected == true).Count() <= 0)
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "At least one service must be selected for the service line head." });
                        }

                        foreach (var service in model.UserRole.Services.Where(p => p.Selected == true))
                        {
                            UserService userService = new UserService();
                            userService.UserId = userId;
                            userService.ServiceTypeId = service.ServiceTypeId;
                            _dataContext.UserServices.Add(userService);

                        }
                    }
                    _dataContext.SaveChanges();


                    if (model.UserRole.RoleName == "AVP")
                    {
                        if (model.UserRole.RegionServices == null || model.UserRole.RegionServices.Where(p => p.Selected == true).Count() <= 0)
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "At least one service region must be selected for the AVP." });
                        }

                        foreach (var service in model.UserRole.RegionServices.Where(p => p.Selected == true).Select(p => p.ServiceTypeId).Distinct())
                        {
                            UserService userService = new UserService();
                            userService.UserId = userId;
                            userService.ServiceTypeId = service;
                            _dataContext.UserServices.Add(userService);

                        }

                        foreach (var region in model.UserRole.RegionServices.Where(p => p.Selected == true).Select(p => p.RegionTypeId).Distinct())
                        {
                            UserRegion userRegion = new UserRegion();
                            userRegion.UserId = userId;
                            userRegion.RegionTypeId = region;
                            _dataContext.UserRegions.Add(userRegion);

                        }

                    }

                    _dataContext.SaveChanges();

                    if (model.UserRole.RoleName == "CEO" || model.UserRole.RoleName == "DBD" || model.UserRole.RoleName == "CL" || model.UserRole.RoleName == "NCM")
                    {
                        if (model.UserRole.Sites == null || model.UserRole.Sites.Where(p => p.Selected == true).Count() <= 0)
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "At least one site must be selected for a " + model.UserRole.RoleName + "." });
                        }

                        foreach (var service in model.UserRole.Sites.Where(p => p.Selected == true).Select(p => p.ServiceTypeId).Distinct())
                        {
                            UserService userService = new UserService();
                            userService.UserId = userId;
                            userService.ServiceTypeId = service;
                            _dataContext.UserServices.Add(userService);

                        }


                        foreach (var region in model.UserRole.Sites.Where(p => p.Selected == true).Select(p => p.RegionTypeId).Distinct())
                        {
                            UserRegion userRegion = new UserRegion();
                            userRegion.UserId = userId;
                            userRegion.RegionTypeId = region;
                            _dataContext.UserRegions.Add(userRegion);

                        }


                        foreach (var site in model.UserRole.Sites.Where(p => p.Selected == true).Select(p => p.CHGSiteId).Distinct())
                        {
                            UserCHGSite userCHGSite = new UserCHGSite();
                            userCHGSite.UserId = userId;
                            userCHGSite.CHGSiteId = site;
                            _dataContext.UserCHGSites.Add(userCHGSite);

                        }

                    }

                    _dataContext.SaveChanges();


                    if (model.UserRole.RoleName == "CL")
                    {
                        if (model.UserRole.PreScreenTypeId == null)
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Pre-screen type must be selected for the liaison." });
                        }


                        UserPreScreen userPreScreeen = new UserPreScreen();
                        userPreScreeen.UserId = userId;
                        userPreScreeen.PreScreenTypeId = model.UserRole.PreScreenTypeId.Value;
                        _dataContext.UserPreScreens.Add(userPreScreeen);
                    }
                    _dataContext.SaveChanges();

                    transaction.Commit();

                }

                return Json(new { Status = Constant.RESPONSE_OK, Description = "User saved successfully." });

            }

            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "One or more fields failed validation." });

        }


        [HttpPost]
        public JsonResult Create(long? id, UserModel model)
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." });
            }
            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." });
            }


            if (ModelState.IsValid)
            {
                using (var transaction = _dataContext.Database.BeginTransaction())
                {

                    var entity = _dataContext.Users.Where(p => p.UserId == id && p.Deleted == false).SingleOrDefault();
                    if (entity == null)
                    {
                        return Json(new { Status = Constant.RESPONSE_ERROR, Description = "User not found. Invalid User." });
                    }

                    entity.Contact.Phone = model.Phone;
                    entity.Contact.Mobile = model.Mobile;
                    entity.Enabled = true;
                    _dataContext.SaveChanges();

                    long userId = id.Value;
                    long userRoleTypeId = _dataContext.UserRoleTypes.Where(p => p.Name == model.UserRole.RoleName && p.Deleted == false).Single().UserRoleTypeId;
                    var userRole = _dataContext.UserRoles.Where(p => p.UserId == userId && p.Deleted == false).Take(1).SingleOrDefault();

                    ClearRoles(userId);
                    _dataContext.SaveChanges();

                    if (userRole == null)
                    {
                        userRole = new UserRole();
                        userRole.UserRoleTypeId = userRoleTypeId;
                        userRole.UserId = userId;
                        _dataContext.UserRoles.Add(userRole);
                    }
                    else
                    {
                        userRole.UserRoleTypeId = userRoleTypeId;
                    }

                    _dataContext.SaveChanges();

                    if (model.UserRole.RoleName == "SLH")
                    {
                        if (model.UserRole.Services == null || model.UserRole.Services.Where(p => p.Selected == true).Count() <= 0)
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "At least one service must be selected for the service line head." });
                        }

                        foreach (var service in model.UserRole.Services.Where(p => p.Selected == true))
                        {
                            UserService userService = new UserService();
                            userService.UserId = userId;
                            userService.ServiceTypeId = service.ServiceTypeId;
                            _dataContext.UserServices.Add(userService);

                        }
                    }
                    _dataContext.SaveChanges();


                    if (model.UserRole.RoleName == "AVP")
                    {
                        if (model.UserRole.RegionServices == null || model.UserRole.RegionServices.Where(p => p.Selected == true).Count() <= 0)
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "At least one service region must be selected for the AVP." });
                        }

                        foreach (var service in model.UserRole.RegionServices.Where(p => p.Selected == true).Select(p => p.ServiceTypeId).Distinct())
                        {
                            UserService userService = new UserService();
                            userService.UserId = userId;
                            userService.ServiceTypeId = service;
                            _dataContext.UserServices.Add(userService);

                        }

                        foreach (var region in model.UserRole.RegionServices.Where(p => p.Selected == true).Select(p => p.RegionTypeId).Distinct())
                        {
                            UserRegion userRegion = new UserRegion();
                            userRegion.UserId = userId;
                            userRegion.RegionTypeId = region;
                            _dataContext.UserRegions.Add(userRegion);

                        }

                    }

                    _dataContext.SaveChanges();


                    if (model.UserRole.RoleName == "CEO" || model.UserRole.RoleName == "DBD" || model.UserRole.RoleName == "CL" || model.UserRole.RoleName == "NCM")
                    {
                        if (model.UserRole.Sites == null || model.UserRole.Sites.Where(p => p.Selected == true).Count() <= 0)
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "At least one site must be selected for a " + model.UserRole.RoleName + "." });
                        }

                        foreach (var service in model.UserRole.Sites.Where(p => p.Selected == true).Select(p => p.ServiceTypeId).Distinct())
                        {
                            UserService userService = new UserService();
                            userService.UserId = userId;
                            userService.ServiceTypeId = service;
                            _dataContext.UserServices.Add(userService);

                        }


                        foreach (var region in model.UserRole.Sites.Where(p => p.Selected == true).Select(p => p.RegionTypeId).Distinct())
                        {
                            UserRegion userRegion = new UserRegion();
                            userRegion.UserId = userId;
                            userRegion.RegionTypeId = region;
                            _dataContext.UserRegions.Add(userRegion);

                        }


                        foreach (var site in model.UserRole.Sites.Where(p => p.Selected == true).Select(p => p.CHGSiteId).Distinct())
                        {
                            UserCHGSite userCHGSite = new UserCHGSite();
                            userCHGSite.UserId = userId;
                            userCHGSite.CHGSiteId = site;
                            _dataContext.UserCHGSites.Add(userCHGSite);

                        }

                    }

                    _dataContext.SaveChanges();


                    if (model.UserRole.RoleName == "CL")
                    {
                        if (model.UserRole.PreScreenTypeId == null)
                        {
                            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Pre-screen type must be selected for the liaison." });
                        }


                        UserPreScreen userPreScreeen = new UserPreScreen();
                        userPreScreeen.UserId = userId;
                        userPreScreeen.PreScreenTypeId = model.UserRole.PreScreenTypeId.Value;
                        _dataContext.UserPreScreens.Add(userPreScreeen);


                    }
                    _dataContext.SaveChanges();

                    transaction.Commit();

                    return Json(new { Status = Constant.RESPONSE_OK, Description = "User saved successfully.", ContextId = entity.UserId });
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

            var entity = _dataContext.Users.Where(p => p.UserId == id).SingleOrDefault();
            if (entity != null)
            {
                entity.Enabled = false;
                ClearRoles(id.Value);
                _dataContext.UserRoles.RemoveRange(_dataContext.UserRoles.Where(p => p.UserId == id.Value));
                _dataContext.SaveChanges();

                return Json(new { Status = Constant.RESPONSE_OK });
            }
            return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Entity not found!" });
        }

        public JsonResult DetailEntity(long? id)
        {
            if (!HasContextRole(new string[] { "CRO" }))
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Unauthorized." }, JsonRequestBehavior.AllowGet);
            }

            if (id == null || id == 0)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "Invalid ID Specification." });
            }

            var entity = _dataContext.Users.Where(p => p.UserId == id && p.Deleted == false).SingleOrDefault();
            if (entity == null)
            {
                return Json(new { Status = Constant.RESPONSE_ERROR, Description = "User not found. Invalid User." });
            }

            UserRoleModel userRoleModel = new UserRoleModel();

            UserRole userRole = _dataContext.UserRoles.Where(p => p.UserId == id && p.Deleted == false).SingleOrDefault();
            if (userRole != null)
            {
                userRoleModel.RoleName = userRole.UserRoleType.Name;
                userRoleModel.Services = _dataContext.UserServices.Where(p => p.UserId == id && p.Deleted == false).Select(p => new UserServiceModel()
                {
                    Selected = true,
                    ServiceTypeId = p.ServiceTypeId
                }).ToList();

                userRoleModel.RegionServices = _dataContext.UserRegions.Where(p => p.UserId == id && p.Deleted == false).Select(p => new UserRegionServiceModel()
                {
                    Selected = true,
                    RegionTypeId = p.RegionTypeId
                }).ToList();

                userRoleModel.Sites = _dataContext.UserCHGSites.Where(p => p.UserId == id && p.Deleted == false).Select(p => new UserCHGSiteModel()
                {
                    Selected = true,
                    CHGSiteId = p.CHGSiteId
                }).ToList();

                userRoleModel.PreScreens = _dataContext.UserPreScreens.Where(p => p.UserId == id && p.Deleted == false).Select(p => new UserPreScreenModel()
                {
                    Selected = true,
                    PreScreenTypeId = p.PreScreenTypeId
                }).ToList();
            }

            UserModel model = new UserModel()
            {
                UserId = entity.UserId,
                DomainName = entity.DomainName,
                Email = entity.Contact.Email,
                FirstName = entity.Contact.FirstName,
                LastName = entity.Contact.LastName,
                Mobile = entity.Contact.Mobile,
                Phone = entity.Contact.Phone,
                UserName = entity.UserName,
                UserRole = userRoleModel
            };

            return Json(new { Status = Constant.RESPONSE_OK, Data = model }, JsonRequestBehavior.AllowGet);
        }


        private void ClearRoles(long userId)
        {
            _dataContext.UserServices.RemoveRange(_dataContext.UserServices.Where(p => p.UserId == userId).ToList());
            _dataContext.UserRegions.RemoveRange(_dataContext.UserRegions.Where(p => p.UserId == userId).ToList());
            _dataContext.UserCHGSites.RemoveRange(_dataContext.UserCHGSites.Where(p => p.UserId == userId).ToList());
            _dataContext.UserPreScreens.RemoveRange(_dataContext.UserPreScreens.Where(p => p.UserId == userId).ToList());

        }


    }

}