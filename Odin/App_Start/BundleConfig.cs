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
                        "~/Scripts/moment.js",
                        "~/Scripts/bootstrap-datetimepicker.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/jspdf.min.js",
                        "~/Scripts/datatables/jquery.datatables.js",
                        "~/Scripts/datatables/datatables.bootstrap.js",
                        "~/Scripts/app/views/_header.js",
                        "~/Scripts/dw_loader.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-notify").Include(
                        "~/Scripts/bootstrap-notify.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/cssBundle").Include(
                      "~/Content/bootstrap-paper.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/site.css",
                      "~/Content/css/primaryNav.css",
                      "~/Content/datatables/css/datatables.bootstrap.css",
                      "~/Content/animate.css",
                      "~/Content/css/header.css",
                      "~/Content/css/shared/dw_loading.css"));


            // Page Styling
            bundles.Add(new StyleBundle("~/Styling/help").Include("~/Content/css/help.css"));
            bundles.Add(new StyleBundle("~/Styling/orders").Include("~/Content/css/orders.css", "~/Content/typeahead.css"));
            bundles.Add(new StyleBundle("~/Styling/login").Include("~/Content/css/login.css"));
            bundles.Add(new StyleBundle("~/Styling/forgotPassword").Include("~/Content/css/forgotpassword.css",
                "~/Content/animate.css"));
            bundles.Add(new StyleBundle("~/Styling/transferee").Include("~/Content/css/transferee/transferee.css"));
            bundles.Add(new StyleBundle("~/Styling/bookmarklet").Include("~/Content/css/bookmarklet.css", "~/Content/animate.css"));
            bundles.Add(new StyleBundle("~/Styling/selectedProperty").Include("~/Content/css/selectedProperty.css"));
            // Panel Styling
            bundles.Add(new StyleBundle("~/Styling/sideNav").Include("~/Content/css/transferee/sideNav.css"));
            bundles.Add(new StyleBundle("~/Styling/itinerary").Include("~/Content/css/transferee/itinerary.css"));
            bundles.Add(new StyleBundle("~/Styling/details").Include("~/Content/css/transferee/details.css"));
            bundles.Add(new StyleBundle("~/Styling/history").Include("~/Content/css/transferee/history.css"));
            bundles.Add(new StyleBundle("~/Styling/intake").Include("~/Content/css/transferee/intake.css", "~/Content/animate.css"));
            bundles.Add(new StyleBundle("~/Styling/messages").Include("~/Content/css/transferee/messages.css"));
            bundles.Add(new StyleBundle("~/Styling/housing").Include("~/Content/css/transferee/housing.css", "~/Content/leaflet.css"));
            bundles.Add(new StyleBundle("~/Styling/housing").Include("~/Content/css/transferee/housing.css"));
            bundles.Add(new StyleBundle("~/Styling/propertiesPDF").Include("~/Content/css/transferee/HFPropertiesPDF.css"));
            bundles.Add(new StyleBundle("~/Styling/transfereeHelp").Include("~/Content/css/transferee/help.css"));
            bundles.Add(new StyleBundle("~/Styling/propertyMessages").Include("~/Content/css/transferee/propertyMessages.css"));
            bundles.Add(new StyleBundle("~/Styling/itineraryPdf").Include("~/Content/css/transferee/itineraryPdf.css"));
            bundles.Add(new StyleBundle("~/Styling/dashboard").Include("~/Content/css/transferee/dashboard.css"));

            // Page scripts
            bundles.Add(new ScriptBundle("~/Scripts/orders").Include("~/Scripts/app/views/orders.js", "~/Scripts/typeahead.js"));
            bundles.Add(new ScriptBundle("~/Scripts/help").Include("~/Scripts/app/views/help.js"));
            bundles.Add(new ScriptBundle("~/Scripts/transferee").Include(
                "~/Scripts/bootstrap-notify.min.js",
                "~/Scripts/app/views/transferee.js",
                "~/Scripts/app/views/transferee-intake.js",
                "~/Scripts/app/views/transferee-details.js",
                "~/Scripts/app/views/transferee-housing.js",
                "~/Scripts/app/views/transferee-itinerary.js",
                "~/Scripts/app/views/transferee-itin-appntmt.js",
                "~/Scripts/app/views/transferee-hfp-messages.js",
                "~/Scripts/app/views/transferee-selectedProperty.js",
                "~/Scripts/leaflet.js"));

            bundles.Add(new ScriptBundle("~/Scripts/forgotPassword").Include(
                "~/Scripts/bootstrap-notify.min.js",
                "~/Scripts/app/views/forgotPassword.js"));

            //Originally bundled in Scripts folder, but getting 403 error in IFrame.
            bundles.Add(new ScriptBundle("~/bundles/bookmarklet").Include(
                "~/Scripts/bootstrap-notify.min.js",
                "~/Scripts/app/views/bookmarklet.js"));

        }
    }
}
