using System.Web.Optimization;

namespace Odin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/Scripts/app/controllers/userManagementController.js"));

            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/datatables/jquery.datatables.js",
                        "~/Scripts/datatables/datatables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-paper.css",
                      "~/Content/site.css",
                      "~/Content/datatables/css/datatables.bootstrap.css"));

            // Page Styling
            bundles.Add(new StyleBundle("~/Styling/orders").Include("~/Content/Orders/orders.css"));
            bundles.Add(new StyleBundle("~/Styling/login").Include("~/Content/Login/login.css"));
            bundles.Add(new StyleBundle("~/Styling/forgotpw").Include("~/Content/ForgotPassword/forgotpassword.css"));

            // Page scripts
            bundles.Add(new ScriptBundle("~/Scripts/orders").Include("~/Scripts/app/views/orders.js"));
        }
    }
}
