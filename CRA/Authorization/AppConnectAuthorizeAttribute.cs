using CRA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CRA.Authorization
{
    public class AppConnectAuthorizeAttribute : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
 
            string userName = httpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }

            string[] tokens = userName.Split('\\');
            if (tokens.Length < 2)
            {
                return false;
            }

            string domainName = tokens[0];
            userName = tokens[1];
            
            DataContext _dataContext = new DataContext();

            var user = _dataContext.Users.Where(p => p.DomainName == domainName && p.UserName == userName && p.Deleted == false && p.Enabled == true).SingleOrDefault();
            if (user != null)
            {
                AuthorizedUserModel userModel = new AuthorizedUserModel();

                userModel.UserId = user.UserId;
                userModel.Domain = user.DomainName;
                userModel.UserName = user.UserName;
 
                foreach(var role in _dataContext.GetUserRoles(user.UserId))
                {
                    userModel.Roles.Add(role.Name);
                }

                httpContext.Items["User"] = userModel;

                return true;
            }

            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary(
                               new
                               {
                                   controller = "Status",
                                   action = "Unauthorized"
                               })
                           );
        }
    }
}