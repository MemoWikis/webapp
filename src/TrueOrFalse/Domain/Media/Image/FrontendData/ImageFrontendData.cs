using Microsoft.AspNetCore.Http;
using static System.String;

public class ImageFrontendData
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly QuestionReadingRepo _questionReadingRepo;
    public ImageMetaData ImageMetaData;
    public bool ImageMetaDataExists;
    public MainLicenseInfo MainLicenseInfo;
    public string OriginalFileLink;
    public ManualImageEvaluation ManualImageEvaluation;
    public bool ImageCanBeDisplayed;
    public bool HasAuthorizedMainLicense;
    public bool LicenseDataIncomplete;
    public LicenseImage MainLicense;
    public string Author;
    public string Description;
    public string LicenseName;
    public string LicenseLink;
    public string LicenseShortDescriptionLink;
    public string AttributionHtmlString;
    public ImageParsingNotifications ImageParsingNotifications;

    public ImageFrontendData(
        ImageMetaData imageMetaData,
        IHttpContextAccessor httpContextAccessor,
        QuestionReadingRepo questionReadingRepo)
    {
        _httpContextAccessor = httpContextAccessor;
        _questionReadingRepo = questionReadingRepo;
        if (imageMetaData == null)
            return;

        ImageMetaDataExists = true;
        ImageMetaData = imageMetaData;

        MainLicenseInfo = !IsNullOrEmpty(ImageMetaData.MainLicenseInfo)
            ? MainLicenseInfo.FromJson(ImageMetaData.MainLicenseInfo)
            : null;

        if (ImageMetaData.Source == ImageSource.WikiMedia)
            OriginalFileLink =
                LicenseParser.GetWikiDetailsPageFromSourceUrl(ImageMetaData.SourceUrl);

        ManualImageEvaluation = imageMetaData.ManualEntriesFromJson().ManualImageEvaluation;
        ImageCanBeDisplayed = ManualImageEvaluation != ManualImageEvaluation.ImageManuallyRuledOut;

        Description = !IsNullOrEmpty(ImageMetaData.ManualEntriesFromJson().DescriptionManuallyAdded)
            ? ImageMetaData.ManualEntriesFromJson().DescriptionManuallyAdded
            : ImageMetaData.DescriptionParsed;

        if (!ImageCanBeDisplayed)
        {
            AttributionHtmlString =
                "Das Bild kann aus lizenzrechtlichen Gründen leider zur Zeit nicht angezeigt werden. ";

            if (!IsNullOrEmpty(OriginalFileLink))
                AttributionHtmlString +=
                    $"Hier findest du die <a href='{OriginalFileLink}' target='_blank'>Originaldatei</a>.";
        }
        else //Image can be displayed
        {
            Fill_Author();

            if (IsAuthorizedLicense())
                FillFor_Authorized_License();
            else
                FillFor_NonAuthorized_License();
        }

        ImageParsingNotifications = ImageParsingNotifications.FromJson(ImageMetaData.Notifications);
    }

    private void Fill_Author()
    {
        if (!IsNullOrEmpty(MainLicenseInfo?.Author))
        {
            Author = MainLicenseInfo.Author;
        }
        else
        {
            LicenseDataIncomplete = true;
            Author = !IsNullOrEmpty(ImageMetaData.ManualEntriesFromJson().AuthorManuallyAdded)
                ? ImageMetaData.ManualEntriesFromJson().AuthorManuallyAdded
                : ImageMetaData.AuthorParsed;
        }
    }

    private bool IsAuthorizedLicense()
    {
        return MainLicenseInfo != null && ManualImageEvaluation ==
            ManualImageEvaluation.ImageCheckedForCustomAttributionAndAuthorized;
    }

    private void FillFor_Authorized_License()
    {
        HasAuthorizedMainLicense = true;
        MainLicense = LicenseImageRepo.GetById(MainLicenseInfo.MainLicenseId);
        if (MainLicense != null)
        {
            LicenseName = !IsNullOrEmpty(MainLicense.LicenseShortName)
                ? MainLicense.LicenseShortName
                : MainLicense.LicenseLongName;
            LicenseLink = MainLicense.LicenseLink;
            LicenseShortDescriptionLink = MainLicense.LicenseShortDescriptionLink;
        }

        AttributionHtmlString =
            $"<span class='InfoLabel'>Bild</span>: {(!IsNullOrEmpty(Author) ? Author + ", " : "")}";

        if (ImageMetaData.Source == ImageSource.WikiMedia && !IsNullOrEmpty(OriginalFileLink))
        {
            AttributionHtmlString +=
                $"<a href='{OriginalFileLink}' target='_blank'>Wikimedia Commons</a>";
        }

        if (IsNullOrEmpty(LicenseName))
        {
            LicenseDataIncomplete = true;
        }
        else //LicenseDataIncomplete
        {
            if (!IsNullOrEmpty(LicenseLink))
            {
                AttributionHtmlString +=
                    $", Lizenz: <a href='{LicenseLink}' target='_blank'>{LicenseName}</a>";

                if (!IsNullOrEmpty(LicenseShortDescriptionLink))
                {
                    AttributionHtmlString +=
                        $" (<a href='{LicenseShortDescriptionLink}' target='_blank'>Kurzfassung</a>)";
                }
            }
            else if (MainLicense != null && MainLicense.LicenseLinkRequired == true)
            {
                LicenseDataIncomplete = true;
            }
            else
            {
                AttributionHtmlString += ", Lizenz: " + LicenseName;
            }
        }
    }

    private void FillFor_NonAuthorized_License()
    {
        AttributionHtmlString =
            $"<span class='InfoLabel'>Bild:</span> {(!IsNullOrEmpty(Author) ? Author + ", " : "")}";

        if (!IsNullOrEmpty(OriginalFileLink))
        {
            var source = "";

            if (ImageMetaData.Source == ImageSource.WikiMedia)
                source = "(Wikimedia Commons)";

            AttributionHtmlString +=
                $"<a href='{OriginalFileLink}' target='_blank'>Bildquelle und erweiterte Information {source}</a>.";
        }
        else
        {
            AttributionHtmlString +=
                "Die Lizenzangaben konnten nicht überprüft werden.";
        }
    }

    public ImageUrl GetImageUrl(
        int width,
        bool asSquare = false,
        bool getDummy = false,
        ImageType imageTypeForDummy = ImageType.Question)
    {
        IImageSettings imageSettings;
        var typeId = getDummy ? -1 : (ImageMetaDataExists ? ImageMetaData.TypeId : -1);
        var imageType = ImageMetaDataExists ? ImageMetaData.Type : imageTypeForDummy;

        switch (imageType)
        {
            case ImageType.Page:
                imageSettings = new PageImageSettings(typeId, _httpContextAccessor);
                break;
            case ImageType.User:
                imageSettings = new UserImageSettings(typeId, _httpContextAccessor);
                break;
            default:
                imageSettings =
                    new QuestionImageSettings(typeId, _httpContextAccessor, _questionReadingRepo);
                break;
        }

        var result = new ImageUrl(_httpContextAccessor).Get(imageSettings, width, asSquare, arg =>
            new ImageUrl(_httpContextAccessor).GetFallbackImageUrl(imageSettings, width));

        return result;
    }
}