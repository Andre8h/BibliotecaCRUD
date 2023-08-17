using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.Optimization;
using System.Web.UI.WebControls;

namespace EJERCICIOMVC
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios.  De esta manera estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //Plugins
            bundles.Add(new StyleBundle("~/Content/Plugin/css").Include(
            "~/Content/Datatables/css/jquery.dataTables.min.css",
            "~/Content/fontawesome/css/all.css",
            "~/Content/Datatables/css/buttons.dataTables.min.css"

                       ));

            bundles.Add(new StyleBundle("~/Content/Plugin/js").Include(
            "~/Content/Datatables/js/jquery.dataTables.min.js",
            "~/Content/fontawesome/js/all.js",
            "~/Content/Datatables/js/dataTables.buttons.min.js",
            "~/Content/Moment/moment.min.js"

                        ));



        }
    }
}
