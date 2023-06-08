﻿using System.Web.Optimization;

namespace TrueOrFalse.View;

public class BundleConfig
{
    public static void RegisterBundles(BundleCollection bundles)
    {
        BundleTable.EnableOptimizations = Settings.Environment() != "develop";
        
        /* CSS */
        bundles.Add(new StyleBundle("~/bundles/css").Include(
            "~/Style/bootstrap/bootstrap.css",
            "~/Style/includes/shared.css",
            "~/Style/*.css",
            "~/Fonts/googleFonts.css",
            "~/Fonts/font-awesome-5.7.2/css/all.css",
            "~/Fonts/font-awesome-5.7.2/css/v4-shims.css",
            "~/Views/Shared/CategoryLabel.css"));

        bundles.Add(new ScriptBundle("~/bundles/js/stageOverlay")
            .Include("~/Scripts/header/StageOverlay.js")
        );

        bundles.Add(new StyleBundle("~/bundles/jqueryUi")
            .IncludeDirectory("~/Style/jquery-ui/", "*.css"));

        bundles.Add(new ScriptBundle("~/bundles/js/jqueryUI")
            .Include("~/Scripts/vendor/jqueryUi/jquery-ui.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/bootstrapTooltip")
            .Include("~/Scripts/vendor/bootstrapTooltip/bootstrap-tooltip.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/lazy")
            .Include("~/Views/Shared/Lazy/LazyComponent.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/searchTemplate")
            .Include("~/Views/Shared/Search/SearchComponent.js"));
        bundles.Add(new StyleBundle("~/bundles/searchTemplate")
            .Include("~/Views/Shared/Search/Search.css"));

        bundles.Add(new ScriptBundle("~/bundles/js/headerSearch")
            .Include("~/Views/Shared/Search/HeaderSearch.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/stickySearch")
            .Include("~/Views/Shared/Search/StickySearch.js"));

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

        bundles.Add(new ScriptBundle("~/bundles/LoginModalComponent")
            .Include("~/Views/Welcome/Login/LoginModalComponent.js"));

        bundles.Add(new StyleBundle("~/bundles/Registration")
            .Include("~/Views/Welcome/Registration/Register.css"));

        bundles.Add(new StyleBundle("~/bundles/Category")
            .Include("~/Views/Shared/Spinner/Spinner.css")
            .Include("~/Views/Categories/Detail/Category.css")
            .Include("~/Views/Shared/Delete.css"));


        bundles.Add(new StyleBundle("~/bundles/Segmentation")
            .Include("~/Views/Categories/Detail/Partials/Segmentation/Segmentation.css"));


        bundles.Add(new ScriptBundle("~/bundles/js/d3")
            .Include("~/Scripts/npm/d3/d3.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/alertModal")
            .IncludeDirectory("~/Views/Shared/Modals/AlertModal/", "*.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/defaultModal")
            .IncludeDirectory("~/Views/Shared/Modals/DefaultModal/", "*.js"));

        bundles.Add(new Bundle("~/bundles/js/tiptap")
            .IncludeDirectory("~/Scripts/npm/tiptap-build/", "*.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/Editor")
            .Include("~/Views/Shared/Editor/EditorMenuBarComponent.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/EditQuestionLoader")
            .Include("~//Views/Questions/Edit/EditComponents/EditQuestionLoader.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/FloatingActionButton")
            .Include("~/Scripts/npm/vue-material-design-ripple-build/vue-material-design-ripple-build.js")
            .Include("~/Views/Categories/Detail/FloatingActionButton/FloatingActionButton.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/TopicTabFABLoader")
            .Include("~/Views/Categories/Detail/FloatingActionButton/TopicTabFabLoader.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/EditQuestionModalLoader")
            .Include("~/Views/Questions/Js/EditQuestionModalLoader/EditQuestionModalLoader.js"));

        bundles.Add(new StyleBundle("~/bundles/Login")
            .Include("~/Views/Welcome/Registration/Register.css"));

        bundles.Add(new ScriptBundle("~/bundles/questions")
            .IncludeDirectory("~/Views/Questions/Js/", "*.js")
            .Include("~/Scripts/ValuationPerRow.js"));

        bundles.Add(new ScriptBundle("~/bundles/questionEdit")
            .Include("~/Views/Images/ImageUpload/ImageUpload.js")
            .IncludeDirectory("~/Views/Questions/Edit/Js/", "*.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/CommentsSection")
            .Include("~/Views/Questions/Modals/QuestionCommentSectionModalComponentLoader.js")
            .IncludeDirectory("~/Views/Questions/Answer/Comments/", "*.js")
        );

        bundles.Add(new StyleBundle("~/bundles/AnswerQuestion")
            .Include("~/Views/Questions/Answer/*.css"));

        bundles.Add(new StyleBundle("~/bundles/SessionConfig")
            .Include("~/Views/Questions/SessionConfig/*.css"));

        bundles.Add(new ScriptBundle("~/bundles/js/QuestionDetailsApp")
            .Include("~/Views/Questions/Answer/Js/QuestionDetails/AnswerQuestionDetailsApp.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/QuestionDetailsComponent")
            .Include("~/Views/Questions/Answer/Js/QuestionDetails/AnswerQuestionDetailsComponent.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/QuestionListApp")
            .Include("~/Views/Questions/Modals/QuestionCommentSectionModalComponentLoader.js")
            .Include("~/Views/Questions/Js/QuestionList/QuestionListApp.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/QuestionListComponents")
            .Include("~/Views/Questions/Js/AddQuestion/AddQuestion.js")
            .Include("~/Scripts/npm/vue-slider-component/vue-slider-component.umd.js")
            .Include("~/Views/Questions/Js/QuestionList/QuestionListComponent.js")
            .Include("~/Views/Questions/Js/QuestionList/QuestionComponent.js")
            .Include("~/Views/Shared/PinComponentVue/PinComponent.vue.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/SessionConfigComponent")
            .Include("~/Views/Questions/Js/SessionConfig/SessionProgressBarComponent.js")
            .Include("~/Views/Questions/Js/SessionConfig/SessionConfigComponent.js"));

        bundles.Add(new StyleBundle("~/bundles/QuestionList")
            .Include("~/Views/Questions/QuestionList/QuestionList.css"));

        bundles.Add(new StyleBundle("~/bundles/QuestionHistory")
            .Include("~/Views/Questions/History/*.css"));

        bundles.Add(new StyleBundle("~/bundles/QuestionHistoryDetail")
            .Include("~/Views/Questions/History/Detail/*.css"));

        bundles.Add(new ScriptBundle("~/bundles/js/QuestionHistoryDetail")
            .Include("~/Views/Questions/History/Detail/Js/*.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/DeleteQuestion")
            .Include("~/Views/Questions/Js/DeleteQuestionComponent.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/Maintenance")
            .Include("~/Scripts/BootstrapCustomUtils.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/diff2html")
            .Include("~/Scripts/vendor/diff2html/*.js"));

        bundles.Add(new ScriptBundle("~/bundles/mailto")
            .Include("~/Scripts/various/mailto.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/Knowledge")
            .Include("~/Views/Knowledge/Js/Page.js")
            .Include("~/Views/Knowledge/Js/WishKnowledgeContent.js"));

        bundles.Add(new ScriptBundle("~/bundles/js/_dashboard")
            .Include("~/Views/Knowledge/Js/_dashboard.js"));

        bundles.Add(new StyleBundle("~/bundles/_dashboard")
            .Include("~/Views/Knowledge/Css/_dashBoard.css"));

        bundles.Add(new StyleBundle("~/bundles/KnowledgeTopics")
            .Include("~/Views/Knowledge/Css/KnowledgeTopics.css"));

        bundles.Add(new ScriptBundle("~/bundles/js/KnowledgeQuestions")
            .Include("~/Views/Knowledge/Js/KnowledgeQuestions.js"));

        bundles.Add(new StyleBundle("~/bundles/KnowledgeQuestions")
            .Include("~/Views/Knowledge/Css/KnowledgeQuestions.css"));

        //------------------------ END KNOWLEDGECENTRAL------------------------------------------------------------

        bundles.Add(new StyleBundle("~/bundles/Promoter")
            .Include("~/Views/Welcome/Promoter.css"));

        bundles.Add(new ScriptBundle("~/bundles/AboutMemucho")
            .Include("~/Views/About/Js/Page.js"));

        bundles.Add(new StyleBundle("~/bundles/Team")
            .Include("~/Views/Welcome/Team.css"));

        bundles.Add(new StyleBundle("~/bundles/switch")
            .Include("~/Views/Shared/Switch/Switch.css"));
#if RELEASE
            BundleTable.EnableOptimizations = true;
#endif

        SetIgnorePatterns(bundles.IgnoreList);
    }

    public static void SetIgnorePatterns(IgnoreList ignoreList)
    {
        ignoreList.Ignore("*SetVideoPlayer.js");
    }
}