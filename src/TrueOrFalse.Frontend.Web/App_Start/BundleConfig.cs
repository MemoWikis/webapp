using System.Reflection.Emit;
using System.Web;
using System.Web.Optimization;

namespace TrueOrFalse.View
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Views/QuestionSets").Include("~/Views/QuestionSets/QuestionSets.js"));
            bundles.Add(new StyleBundle("~/Views/QuestionSets").Include("~/Views/QuestionSets/QuestionSets.css"));

            bundles.Add(new ScriptBundle("~/bundles/fileUploader")
                .Include("~/Scripts/file-uploader/header.js")
                .Include("~/Scripts/file-uploader/util.js")
                .Include("~/Scripts/file-uploader/button.js")
                .Include("~/Scripts/file-uploader/handler.base.js")
                .Include("~/Scripts/file-uploader/handler.form.js")
                .Include("~/Scripts/file-uploader/handler.xhr.js")
                .Include("~/Scripts/file-uploader/uploader.basic.js")
                .Include("~/Scripts/file-uploader/dnd.js")
                .Include("~/Scripts/file-uploader/uploader.js")
                .Include("~/Scripts/file-uploader/jquery-plugin.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}