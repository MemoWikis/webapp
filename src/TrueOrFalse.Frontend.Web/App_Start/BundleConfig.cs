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
                  "~/Style/jquery-ui/jquery-ui.theme.css"));

            bundles.Add(new StyleBundle("~/bundles/markdownCss")
                .Include("~/Style/markdown-editor.css"));

            bundles.Add(new StyleBundle("~/bundles/message")
                .Include("~/Views/Messages/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/shared")
                .IncludeDirectory("~/Scripts/", "*.js")
                .IncludeDirectory("~/Scripts/vendor", "*.js")
                .IncludeDirectory("~/Scripts/hubs", "*.js")
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

            bundles.Add(new ScriptBundle("~/bundles/guidedTourScript")
                .IncludeDirectory("~/Scripts/guidedTour/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/guidedTourStyle")
                .Include("~/Style/guidedTour/bootstrap-tour.css"));

            bundles.Add(new ScriptBundle("~/bundles/Welcome")
                .IncludeDirectory("~/Views/Welcome/Js", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/Registration")
                .Include("~/Views/Welcome/Registration/Register.css"));

            bundles.Add(new StyleBundle("~/bundles/Knowledge")
                .IncludeDirectory("~/Views/Knowledge/", "*.css"));

            bundles.Add(new StyleBundle("~/bundles/AlgoInsight")
                .IncludeDirectory("~/Views/AlgoInsight/", "*.css"));

            bundles.Add(new ScriptBundle("~/bundles/questions")
                .IncludeDirectory("~/Views/Questions/Js/", "*.js")
                .Include("~/Scripts/ValuationPerRow.js")
                .Include("~/Scripts/autocompletes/AutocompleteCategories.js"));

            bundles.Add(new ScriptBundle("~/bundles/questionEdit")
                .Include("~/Views/Images/ImageUpload/ImageUpload.js")
                .IncludeDirectory("~/Views/Questions/Edit/Js/", "*.js")
                .Include("~/Scripts/autocompletes/AutocompleteCategories.js"));

            bundles.Add(new ScriptBundle("~/bundles/Beta")
                .IncludeDirectory("~/Views/Beta/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/Sets")
                .IncludeDirectory("~/Views/Sets/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/Categories")
                .IncludeDirectory("~/Views/Categories/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/CategoryEdit")
                .IncludeDirectory("~/Views/Categories/Edit/Js/", "*.js")
                .Include("~/Views/Images/ImageUpload/ImageUpload.js")
                .Include("~/Scripts/autocompletes/AutocompleteCategories.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/Users")
                .IncludeDirectory("~/Views/Users/Js/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/Users")
                .IncludeDirectory("~/Views/Users/", "*.css"));

            bundles.Add(new StyleBundle("~/bundles/User")
                .IncludeDirectory("~/Views/Users/Detail/", "*.css"));

            bundles.Add(new ScriptBundle("~/bundles/Js/User")
                .IncludeDirectory("~/Views/Users/Detail/Js", "*.js")
                .Include("~/Views/Users/Js/UserRowFollow.js").
                Include("~/Views/Users/Js/UserRow.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/AnswerQuestion")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Views/Questions/Answer/Js/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/AnswerQuestion")
                .Include("~/Views/Questions/Answer/*.css"));

            bundles.Add(new StyleBundle("~/bundles/js/DeleteQuestion")
                .Include("~/Views/Questions/Js/QuestionRowDelete.js"));

            bundles.Add(new ScriptBundle("~/bundles/SetEdit")
                .IncludeDirectory("~/Views/Sets/Edit/Js/" ,"*.js")
                .Include("~/Views/Images/ImageUpload/ImageUpload.js")
                .Include("~/Scripts/autocompletes/AutocompleteCategories.js"));

            bundles.Add(new ScriptBundle("~/bundles/Set")
                .IncludeDirectory("~/Views/Sets/Detail/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/Messages")
                .IncludeDirectory("~/Views/Messages/Js/", "*.js"));

            //Dates
            bundles.Add(new StyleBundle("~/bundles/Dates")
                .Include("~/Views/Dates/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/Dates")
                .Include("~/Views/Dates/Js/*.js"));

            bundles.Add(new StyleBundle("~/bundles/EditDate")
                .Include("~/Views/Dates/Edit/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/EditDate")
                .IncludeDirectory("~/Views/Dates/Edit/Js/", "*.js")
                .Include("~/Scripts/autocompletes/AutocompleteSets.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/LearningSessionResult")
                .IncludeDirectory("~/Views/Questions/Answer/LearningSession/Js/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/TestSessionResult")
                .IncludeDirectory("~/Views/Questions/Answer/TestSession/Js/", "*.js"));

            //Games
            bundles.Add(new ScriptBundle("~/bundles/js/Games")
                .IncludeDirectory("~/Views/Games/Js/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/Games")
                .Include("~/Views/Games/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/Game")
                .IncludeDirectory("~/Views/Games/Edit/Js/", "*.js")
                .Include("~/Scripts/autocompletes/AutocompleteSets.js"));

            bundles.Add(new StyleBundle("~/bundles/Game")
                .Include("~/Views/Games/Edit/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/GamePlay")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .IncludeDirectory("~/Views/Games/Play/Js/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/GamePlay")
                .Include("~/Views/Games/Play/*.css")
                .Include("~/Views/Questions/Answer/AnswerQuestionSolution.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/Widget")
                .IncludeDirectory("~/Views/Questions/Answer/AnswerControls/", "*.js")
                .IncludeDirectory("~/Scripts/answerQuestion/", "*.js")
                .Include("~/Views/Widgets/WidgetQuestion.js")
                .Include("~/Views/Widgets/AwesomeIframe.js"));

            //Markdown
            bundles.Add(new ScriptBundle("~/bundles/markdown")
                .Include("~/Scripts/Markdown.Converter.js",
                         "~/Scripts/Markdown.Sanitizer.js", 
                         "~/Scripts/Markdown.Editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/Maintenance")
               .Include("~/Views/Maintenance/ImageMaintenance.js")
               .Include("~/Views/Maintenance/ImageDetail.js")
               .Include("~/Scripts/BootstrapCustomUtils.js"));

            bundles.Add(new StyleBundle("~/bundles/Maintenance")
                .Include("~/Views/Maintenance/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/MaintenanceTools")
                .Include("~/Views/Maintenance/ToolsBrainWaveHub.js"));

#if RELEASE
                BundleTable.EnableOptimizations = true;
#endif
        }
    }
}