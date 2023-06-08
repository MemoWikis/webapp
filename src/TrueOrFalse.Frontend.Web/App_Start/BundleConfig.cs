using System.Web.Optimization;

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
            .Include("~/Scripts/npm/d3/d3.js")); ;
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