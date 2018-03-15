using System.Web;
using System.Web.Optimization;

namespace ProjectManagementApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.2.1.min.js",
                        "~/Scripts/jquery-ui-1.12.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/respond.min.js",
                      "~/Scripts/lightbox.js"));

            bundles.Add(new StyleBundle("~/bundles/appcss").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/bootstrap-datepicker.min.css",
                      "~/Content/css/mainstyle.css",
                      "~/Content/css/lightbox.css"));

            bundles.Add(new ScriptBundle("~/bundles/appjs").Include(
                      "~/Scripts/main.js"));
        }
    }
}
