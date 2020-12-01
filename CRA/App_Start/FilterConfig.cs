using CRA.Authorization;
using System.Web;
using System.Web.Mvc;

namespace CRA
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AppConnectAuthorizeAttribute());
        }
    }
}
