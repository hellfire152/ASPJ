using System.Web;
using System.Web.Optimization;

namespace ASPJ_Project
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/game/logic").Include(
                        "~/Scripts/TofuUniverse/big.min.js",
                        "~/Scripts/TofuUniverse/tofu-universe-items.js",
                        "~/Scripts/TofuUniverse/tofu-universe-upgrades.js",
                        "~/Scripts/TofuUniverse/tofu-universe.js"));

            bundles.Add(new ScriptBundle("~/signalr").Include(
                        "~/Scripts/jquery.signalR-2.2.2.js"
                ));

            bundles.Add(new StyleBundle("~/Shop/css").Include(
                        "~/Content/PremiumShop/assets/css/main.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/game/css").Include(
                        "~/Content/Main.css",
                        "~/Content/bootstrap.css"));
        }
    }
}
