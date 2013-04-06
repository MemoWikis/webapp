using System.Reflection.Emit;
using System.Web;
using System.Web.Optimization;

namespace TrueOrFalse.View
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* CSS */
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                  "~/Style/*.css",
                  "~/Style/smoothness/jquery-ui.css", 
                  "~/Style/zocial/css/zocial.css"));

            bundles.Add(new StyleBundle("~/bundles/markdownCss")
                .Include("~/Style/markdown-editor.css"));

            bundles.Add(new StyleBundle("~/bundles/category")
                .Include("~/Views/Categories/Detail/*.css"));

            /* JS */
            bundles.Add(new ScriptBundle("~/bundles/shared")
                .Include("~/Scripts/html5shiv.js")
                .Include("~/Scripts/jquery-1.9.0.min.js")
                .Include("~/Scripts/jquery-1.9.0.min.js")
                .Include("~/Scripts/jquery-ui-1.10.0.min.js")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/underscore-1.4.3.min.js")
                .Include("~/Scripts/lib.js")
                .Include("~/Scripts/jquery.sparkline.min.js")
                .Include("~/Scripts/modernizr-2.6.2.js")
                .Include("~/Scripts/google-code-prettify/prettify.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/json2.js"));

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

            bundles.Add(new ScriptBundle("~/bundles/questionEdit")
                .IncludeDirectory("~/Views/Questions/Edit/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/questionSetEdit")
                .Include("~/Views/QuestionSets/Edit/Js/ImageUpload.js")
                .Include("~/Views/QuestionSets/Edit/Js/RemoveQuestionFromSet.js")
                .Include("~/Views/QuestionSets/Edit/Js/SetSortable.js")
                .Include("~/Views/Shared/ImageUpload/ImageUpload.js")
                .Include("~/Scripts/jquery.scrollTo-1.4.3.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/markdown")
                .Include("~/Scripts/Markdown.Converter.js",
                         "~/Scripts/Markdown.Sanitizer.js", 
                         "~/Scripts/Markdown.Editor.js"));

            //BundleTable.EnableOptimizations = false;
        }
    }
}