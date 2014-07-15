using System.Web;
using System.Web.Optimization;

namespace vm80q
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/Content/js").Include(
                      "~/Content/js/bootstrap.js",
                      "~/Content/js/bootstrap.min.js",
                      "~/Content/js/classie.js",
                      "~/Content/js/jquery-1.10.2.js",
                      "~/Content/js/label.js",
                      "~/Content/js/modernizr.custom.js",
                      "~/Content/js/uiProressButton.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/index.css",
                      "~/Content/css/main.css"));
        }
    }
}
