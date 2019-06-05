/* 
 *  Bundle Configurations.
 */

using System.Web.Optimization;

namespace JTicket
{
    public class BundleConfig
    {
        /* For more information on bundling, 
        visit https://go.microsoft.com/fwlink/?LinkId=301862 */
        public static void RegisterBundles(BundleCollection bundles)
        {

            /* Create a larger bundle with third-party javaScript files.
               The goal is to minimize the amount of HTTP requests needed
               to get the javaScript files. */
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootbox.js",
                        "~/Scripts/datatables/jquery.datatables.js",
                        "~/Scripts/datatables/bootstrap4.datatables.js",
                        "~/Scripts/datatables/enum.js",
                        "~/Scripts/datatables/moment.js",
                        "~/Scripts/datatables/datetime-moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and 
            // learn from. Then, when you're ready for production, use the 
            // build tool at https://modernizr.com to pick only the tests you 
            // need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            /* Bundle containing CSS files. The Bootstrap template is 
             * bootstrap-lumen.css. */ 
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-lumen.css",  // Bootstrap templace    
                      "~/Content/Site.css",
                      "~/Content/datatables/css/datatables.bootstrap4.css"));
        }
    }
}
