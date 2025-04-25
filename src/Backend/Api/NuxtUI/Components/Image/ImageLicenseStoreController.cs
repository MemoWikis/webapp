using System.Net;

public class ImageLicenseStoreController(
    ImageMetaDataReadingRepo imageMetaDataReadingRepo,
    IHttpContextAccessor httpContextAccessor,
    QuestionReadingRepo questionReadingRepo) : ApiBaseController
{
    public readonly record struct GetLicenseInfoResult(
        bool ImageCanBeDisplayed,
        string Url,
        string Alt,
        string Description,
        string AttributionHtmlString);

    [HttpGet]
    public GetLicenseInfoResult GetLicenseInfo([FromRoute] int id)
    {
        try
        {
            return TryGetLicenseInfo(id);
        }
        catch (Exception e)
        {
            Logg.Error(e);
            return new GetLicenseInfoResult
            (
                ImageCanBeDisplayed: false,
                AttributionHtmlString: "",
                Url: "",
                Alt: "",
                Description: ""
            );
        }
    }

    private GetLicenseInfoResult TryGetLicenseInfo(int id)
    {
        var imageFrontendData = new ImageFrontendData(
            imageMetaDataReadingRepo.GetById(id),
            httpContextAccessor,
            questionReadingRepo);

        var imageUrl = imageFrontendData
            .GetImageUrl(
            1000, 
            false, 
            false,
            imageFrontendData.ImageMetaData.Type);

        if (imageFrontendData.ImageMetaDataExists && imageUrl.HasUploadedImage ||
            imageFrontendData.ImageMetaData != null &&
            imageFrontendData.ImageMetaData.IsYoutubePreviewImage)
        {
            if (!imageFrontendData.ImageCanBeDisplayed)
                return new GetLicenseInfoResult
                (
                    ImageCanBeDisplayed: false,
                    AttributionHtmlString: imageFrontendData.AttributionHtmlString,
                    Url: "",
                    Alt: "",
                    Description: ""
                );

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

            return new GetLicenseInfoResult
            (
                ImageCanBeDisplayed: true,
                Url: imageUrl.Url,
                Alt: altDescription,
                Description: imageFrontendData.Description,
                AttributionHtmlString: imageFrontendData.AttributionHtmlString
            );
        }

        return new GetLicenseInfoResult
        (
            Url: "",
            Alt: "",
            Description: "",
            ImageCanBeDisplayed: false,
            AttributionHtmlString: imageFrontendData.AttributionHtmlString
        );
    }
}