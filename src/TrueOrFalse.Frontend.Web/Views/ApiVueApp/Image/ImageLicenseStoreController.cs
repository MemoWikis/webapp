using System;
using System.Net;
using System.Web.Mvc;

namespace VueApp;

public class ImageLicenseStoreController : BaseController
{
    [HttpGet]
    public JsonResult GetLicenseInfo(int id)
    {

        var imageFrontendData = new ImageFrontendData(Resolve<ImageMetaDataRepo>().GetById(id));
        try
        {
            var imageUrl = imageFrontendData.GetImageUrl(1000, false, false, imageFrontendData.ImageMetaData.Type);

            if (imageFrontendData.ImageMetaDataExists && imageUrl.HasUploadedImage ||
                imageFrontendData.ImageMetaData != null && imageFrontendData.ImageMetaData.IsYoutubePreviewImage)
            {
                if (!imageFrontendData.ImageCanBeDisplayed)
                    return Json(new
                    {
                        imageCanBeDisplayed = false,
                        attributionHtmlString = imageFrontendData.AttributionHtmlString
                    }, JsonRequestBehavior.AllowGet);

                var dataIsYoutubeVideo = "";
                if (imageFrontendData.ImageMetaData.IsYoutubePreviewImage)
                    dataIsYoutubeVideo = $" data-is-youtube-video='{imageFrontendData.ImageMetaData.YoutubeKey}' ";

                var altDescription = String.IsNullOrEmpty(imageFrontendData.Description)
                    ? ""
                    : WebUtility.HtmlEncode(imageFrontendData.Description)
                        .Replace("\"", "'")
                        .Replace("„", "")
                        .Replace("“", "")
                        .Replace("{", "")
                        .Replace("}", "")
                        .StripHTMLTags()
                        .Truncate(120, true);

                return Json(new
                {
                    imageCanBeDisplayed = true,
                    url = imageUrl.Url,
                    alt = altDescription,
                    description = imageFrontendData.Description,
                    attributionHtmlString = imageFrontendData.AttributionHtmlString
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                    imageCanBeDisplayed = false,
                    attributionHtmlString = imageFrontendData.AttributionHtmlString

            }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
            Logg.Error(e);
            return Json(new
            {
                imageCanBeDisplayed = false
            }, JsonRequestBehavior.AllowGet);
        }
    }

}