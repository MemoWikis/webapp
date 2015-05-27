using System.Web.Optimization;

namespace TrueOrFalse.View
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* CSS */
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                  "~/Style/bootstrap/bootstrap.css",
                  "~/Style/includes/shared.css",
                  "~/Style/*.css",
                  "~/Style/jquery-ui/jquery-ui.structure.css", 
                  "~/Style/jquery-ui/jquery-ui.theme.css", 
                  "~/Style/zocial/css/zocial.css"));

            bundles.Add(new StyleBundle("~/bundles/markdownCss")
                .Include("~/Style/markdown-editor.css"));

            bundles.Add(new StyleBundle("~/bundles/message")
                .Include("~/Views/Messages/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/shared")
                .IncludeDirectory("~/Scripts/", "*.js")
                .IncludeDirectory("~/Scripts/vendor", "*.js")
                .IncludeDirectory("~/Views/Images", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/fileUploader")
                .Include("~/Scripts/vendor.file-uploader/header.js")
                .Include("~/Scripts/vendor.file-uploader/util.js")
                .Include("~/Scripts/vendor.file-uploader/button.js")
                .Include("~/Scripts/vendor.file-uploader/handler.base.js")
                .Include("~/Scripts/vendor.file-uploader/handler.form.js")
                .Include("~/Scripts/vendor.file-uploader/handler.xhr.js")
                .Include("~/Scripts/vendor.file-uploader/uploader.basic.js")
                .Include("~/Scripts/vendor.file-uploader/dnd.js")
                .Include("~/Scripts/vendor.file-uploader/uploader.js")
                .Include("~/Scripts/vendor.file-uploader/jquery-plugin.js"));

            bundles.Add(new ScriptBundle("~/bundles/questions")
                .IncludeDirectory("~/Views/Questions/Js/", "*.js")
                .Include("~/Scripts/ValuationPerRow.js")
                .Include("~/Views/Categories/Edit/AutocompleteCategories.js"));

            bundles.Add(new ScriptBundle("~/bundles/questionEdit")
                .Include("~/Views/Images/ImageUpload/ImageUpload.js")
                .IncludeDirectory("~/Views/Questions/Edit/Js/", "*.js")
                .Include("~/Views/Categories/Edit/AutocompleteCategories.js"));

            bundles.Add(new ScriptBundle("~/bundles/Beta")
                .IncludeDirectory("~/Views/Beta/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/Sets")
                .IncludeDirectory("~/Views/Sets/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/Categories")
                .IncludeDirectory("~/Views/Categories/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/CategoryEdit")
                .IncludeDirectory("~/Views/Categories/Edit/Js/", "*.js")
                .Include("~/Views/Images/ImageUpload/ImageUpload.js")
                .Include("~/Views/Categories/Edit/AutocompleteCategories.js"));

            bundles.Add(new ScriptBundle("~/bundles/Users")
                .IncludeDirectory("~/Views/Users/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/AnswerQuestion")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Views/Questions/Answer/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/SetEdit")
                .IncludeDirectory("~/Views/Sets/Edit/Js/" ,"*.js")
                .Include("~/Views/Images/ImageUpload/ImageUpload.js")
                .Include("~/Views/Categories/Edit/AutocompleteCategories.js"));

            bundles.Add(new ScriptBundle("~/bundles/Set")
                .IncludeDirectory("~/Views/Sets/Detail/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/Messages")
                .IncludeDirectory("~/Views/Messages/Js/", "*.js"));

            //Games
            bundles.Add(new ScriptBundle("~/bundles/js/Games")
                .IncludeDirectory("~/Views/Games/Js/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/Games")
                .Include("~/Views/Games/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/Game")
                .IncludeDirectory("~/Views/Games/Edit/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/GamePlay")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .IncludeDirectory("~/Views/Games/Play/Js/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/GamePlay")
                .Include("~/Views/Games/Play/*.css")
                .Include("~/Views/Questions/Answer/AnswerQuestionSolution.css"));

            //Markdown
            bundles.Add(new ScriptBundle("~/bundles/markdown")
                .Include("~/Scripts/Markdown.Converter.js",
                         "~/Scripts/Markdown.Sanitizer.js", 
                         "~/Scripts/Markdown.Editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/Maintenance")
               .Include("~/Views/Maintenance/ImageMaintenance.js")
               .Include("~/Views/Maintenance/ImageDetail.js")
               .Include("~/Scripts/BootstrapCustomUtils.js"));

            bundles.Add(new ScriptBundle("~/bundles/MaintenanceTools")
                .Include("~/Views/Maintenance/ToolsBrainWaveHub.js"));

#if RELEASE
                BundleTable.EnableOptimizations = true;
#endif
        }
    }
}