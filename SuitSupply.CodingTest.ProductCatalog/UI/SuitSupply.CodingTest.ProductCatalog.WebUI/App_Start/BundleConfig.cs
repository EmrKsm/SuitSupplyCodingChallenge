using System.Web;
using System.Web.Optimization;

public class BundleConfig
{
    public static void RegisterBundles(BundleCollection bundles)
    {
        bundles.Add(new ScriptBundle("~/bundles/scripts")
            .Include(   "~/Content/Scripts/jquery-*",
                        "~/Content/Scripts/modernizr-*",
                        "~/Content/Scripts/bootstrap*"));
        bundles.Add(new StyleBundle("~/Content/css")
            .Include(   "~/Content/css/site.css",
                        "~/Content/css/modern-business.css",
                        "~/Content/css/bootstrap*",
                        "~/Content/css/PagedList.css"));
    }
}