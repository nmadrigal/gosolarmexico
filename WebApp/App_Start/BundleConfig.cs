using System.Web;
using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
               "~/Content/bootstrap/bootstrap.css",
               "~/Content/bootstrap/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/foundationcss").Include(
               "~/Content/foundation/normalize.css",
               "~/Content/foundation/foundation.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Styles/base.css",
                "~/Content/Styles/entities.css",
                "~/Content/Styles/overrides.css"
            ));

            /**** for mobile app *****/
            bundles.Add(new StyleBundle("~/Content/mobileCss").Include(
                "~/fonts/fontawesome-webfont.woff",
                "~/Content/Styles/Mobile/mobile-base.css",
                "~/Content/Styles/Mobile/mobile-entities.css",
                "~/Content/Styles/Mobile/mobile-overrides.css"
            ));

            bundles.Add(new StyleBundle("~/Content/Styles/mobileangularcss").Include(
                "~/Content/Styles/Framework/mobile-angular-ui-base.css",
                "~/Content/Styles/Framework/mobile-angular-ui-desktop.css",
                "~/Content/Styles/Framework/mobile-angular-ui-hover.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
           
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap*"));

            bundles.Add(new ScriptBundle("~/bundles/foundationscript").Include(
                "~/Scripts/Framework/foundation/foundation.js",
                "~/Scripts/Framework/foundation/foundation.offcanvas.js",
                "~/Scripts/Framework/foundation/foundation.orbit.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/Framework/angular.min.js",
                "~/Scripts/Framework/angular-route.js",
                "~/Scripts/Framework/mobile-angular-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularcore").Include(
                "~/Scripts/Framework/components/activeLinks.js",
                "~/Scripts/Framework/components/capture.js",
                "~/Scripts/Framework/components/fastclick.js",
                "~/Scripts/Framework/components/outerclick.js",
                "~/Scripts/Framework/components/sharedState.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularcomponents").Include(
                "~/Scripts/Framework/components/modals.js",
                "~/Scripts/Framework/components/navbars.js",
                "~/Scripts/Framework/components/scrollable.js",
                "~/Scripts/Framework/components/sidebars.js",
                "~/Scripts/Framework/components/switch.js"));

            bundles.Add(new ScriptBundle("~/bundles/gosolar").Include(
                "~/Scripts/gosolar/WebHome.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/gosolarMobile").Include(         
                "~/Scripts/gosolar/Mobile/MobileApp.js"
            ));
        }
    }
}