using System.Web;
using System.Web.Optimization;

namespace ProjectManagmentApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/bundles/appcss").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/bootstrap-datepicker.min.css",
                      "~/Content/css/mainstyle.css"));

            bundles.Add(new ScriptBundle("~/bundles/appjs").Include(
                      "~/Scripts/main.js"));
        }
    }
}
