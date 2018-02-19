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
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/lightbox.js"));

            bundles.Add(new StyleBundle("~/bundles/appcss").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/bootstrap-datepicker.min.css",
                      "~/Content/css/mainstyle.css",
                      "~/Content/css/lightbox.css"));

            bundles.Add(new ScriptBundle("~/bundles/appjs").Include(
                      "~/Scripts/main.js"));
        }
    }
}
