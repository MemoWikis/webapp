using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class ImageLicenseStoreController : BaseController
{
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public ImageLicenseStoreController(SessionUser sessionUser,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        QuestionReadingRepo questionReadingRepo) : base(sessionUser)
    {
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _questionReadingRepo = questionReadingRepo;
    }

    public readonly record struct LicenseInfoJson(
        bool ImageCanBeDisplayed,
        string Url,
        string Alt,
        string Description,
        string AttributionHtmlString);

    [HttpGet]
    public LicenseInfoJson GetLicenseInfo([FromRoute] int id)
    {
        var imageFrontendData = new ImageFrontendData(_imageMetaDataReadingRepo.GetById(id),
            _httpContextAccessor,
            _questionReadingRepo);
        try
        {
            var imageUrl = imageFrontendData.GetImageUrl(1000, false, false,
                imageFrontendData.ImageMetaData.Type);

            if (imageFrontendData.ImageMetaDataExists && imageUrl.HasUploadedImage ||
                imageFrontendData.ImageMetaData != null &&
                imageFrontendData.ImageMetaData.IsYoutubePreviewImage)
            {
                if (!imageFrontendData.ImageCanBeDisplayed)
                    return new LicenseInfoJson
                    (
                        ImageCanBeDisplayed: false,
                        AttributionHtmlString: imageFrontendData.AttributionHtmlString,
                        Url: "",
                        Alt: "",
                        Description: ""
                    );

                var dataIsYoutubeVideo = "";
                if (imageFrontendData.ImageMetaData.IsYoutubePreviewImage)
                    dataIsYoutubeVideo =
                        $" data-is-youtube-video='{imageFrontendData.ImageMetaData.YoutubeKey}' ";

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

                return new LicenseInfoJson
                (
                    ImageCanBeDisplayed: true,
                    Url: imageUrl.Url,
                    Alt: altDescription,
                    Description: imageFrontendData.Description,
                    AttributionHtmlString: imageFrontendData.AttributionHtmlString
                );
            }

            return new LicenseInfoJson
            (
                Url: "",
                Alt: "",
                Description: "",
                ImageCanBeDisplayed: false,
                AttributionHtmlString: imageFrontendData.AttributionHtmlString
            );
        }
        catch (Exception e)
        {
            Logg.Error(e);
            return new LicenseInfoJson
            (
                ImageCanBeDisplayed: false,
                AttributionHtmlString: "",
                Url: "",
                Alt: "",
                Description: ""
            );
        }
    }
}