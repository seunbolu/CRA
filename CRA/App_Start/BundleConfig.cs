using System.Web;
using System.Web.Optimization;

namespace CRA
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.3.1.min.js", 
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                             "~/Scripts/jquery.maskedinput.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui-1.12.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/themes/base/jquery-ui.min.css", "~/Content/toastr.css"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
               "~/Scripts/knockout-3.4.2.js"));

            bundles.Add(new StyleBundle("~/Content/font-awesome").Include(
                   "~/Content/font-awesome.min.css"));


            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
           "~/Content/themes/base/jquery-ui.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
               "~/Scripts/toastr.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                       "~/Scripts/jszip.js", //used for kendo grid export - has to be included before kendo scripts
                       "~/Scripts/kendo/kendo.all.min.js",
                       "~/Scripts/kendo/kendo.aspnetmvc.min.js"
                       ));

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
                             "~/Content/kendo/kendo.common.min.css",
                             "~/Content/kendo/kendo.common-bootstrap.min.css",
                             "~/Content/kendo/kendo.bootstrap.min.css",
                             "~/Content/kendo/kendo.bootstrap.mobile.min.css"
                             ));


        }
    }
}
