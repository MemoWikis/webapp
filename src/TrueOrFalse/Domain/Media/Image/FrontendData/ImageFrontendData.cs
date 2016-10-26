using System;

public class ImageFrontendData
{ 
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

    public ImageFrontendData(int typeId, ImageType imageType) : this(PrepareConstructorArguments(typeId, imageType))
    {
    }
        
    public ImageFrontendData(ImageMetaData imageMetaData)
    {
        if (imageMetaData != null)
        {
            ImageMetaDataExists = true;
            ImageMetaData = imageMetaData;
            MainLicenseInfo = !String.IsNullOrEmpty(ImageMetaData.MainLicenseInfo)
                                ? MainLicenseInfo.FromJson(ImageMetaData.MainLicenseInfo)
                                : null;

            if (ImageMetaData.Source == ImageSource.WikiMedia)
            {
                OriginalFileLink = LicenseParser.GetWikiDetailsPageFromSourceUrl(ImageMetaData.SourceUrl);
            }
        
            ManualImageEvaluation = imageMetaData.ManualEntriesFromJson().ManualImageEvaluation;
            ImageCanBeDisplayed = ManualImageEvaluation != ManualImageEvaluation.ImageManuallyRuledOut;

            Description = !String.IsNullOrEmpty(ImageMetaData.ManualEntriesFromJson().DescriptionManuallyAdded)
                    ? ImageMetaData.ManualEntriesFromJson().DescriptionManuallyAdded
                    : ImageMetaData.DescriptionParsed;
            

            if (!ImageCanBeDisplayed)
            {
                AttributionHtmlString = "Das Bild kann aus lizenzrechtlichen Gründen leider zur Zeit nicht angezeigt werden. ";
                if (!String.IsNullOrEmpty(OriginalFileLink))
                {
                    AttributionHtmlString += "Hier findest du die <a href='" + OriginalFileLink + "' target='_blank'>Originaldatei</a>.";
                }
            }

            else { //Image can be displayed

                if (MainLicenseInfo != null &&
                    ManualImageEvaluation == ManualImageEvaluation.ImageCheckedForCustomAttributionAndAuthorized)
                {
                    HasAuthorizedMainLicense = true;
                    MainLicense = LicenseImageRepo.GetById(MainLicenseInfo.MainLicenseId);
                    if (MainLicense != null)
                    {
                        LicenseName = !String.IsNullOrEmpty(MainLicense.LicenseShortName)
                                        ? MainLicense.LicenseShortName
                                        : MainLicense.LicenseLongName;
                        LicenseLink = MainLicense.LicenseLink;
                        LicenseShortDescriptionLink = MainLicense.LicenseShortDescriptionLink;
                    }
           
                    if(!String.IsNullOrEmpty(MainLicenseInfo.Author))
                    {
                        Author = MainLicenseInfo.Author;
                    }
                    else
                    {
                        LicenseDataIncomplete = true;
                        Author = !String.IsNullOrEmpty(ImageMetaData.ManualEntriesFromJson().AuthorManuallyAdded)
                            ? ImageMetaData.ManualEntriesFromJson().AuthorManuallyAdded
                            : ImageMetaData.AuthorParsed;
                    }

                    AttributionHtmlString = "<span class='InfoLabel'>Bild</span>: " + (!String.IsNullOrEmpty(Author) ? Author + ", " : "");

                    if (ImageMetaData.Source == ImageSource.WikiMedia)
                    {
                        AttributionHtmlString +=
                            "<a href='http://commons.wikimedia.org/wiki/Main_Page?uselang=de' target='_blank'>Wikimedia Commons</a>";
                    }

                    if (!String.IsNullOrEmpty(LicenseName))
                    {
                        if (!String.IsNullOrEmpty(LicenseLink))
                        {
                            AttributionHtmlString += ", Lizenz: " + "<a href='" + LicenseLink + "' target='_blank'>" + LicenseName + "</a>";
                        
                            if (!String.IsNullOrEmpty(LicenseShortDescriptionLink))
                            {
                                AttributionHtmlString += " (<a href='" + LicenseShortDescriptionLink + "' target='_blank'>Kurzfassung</a>)";
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
                    else //No license name
                    {
                        LicenseDataIncomplete = true;
                    }
                }
                else //No authorized main license
                {
                    Author = ImageMetaData.AuthorParsed;
                    AttributionHtmlString = "<span class='InfoLabel'>Bild:</span> " + (!String.IsNullOrEmpty(Author) ? Author + ", " : "");

                    if (ImageMetaData.Source == ImageSource.WikiMedia)
                    {
                        AttributionHtmlString +=
                            "<a href='http://commons.wikimedia.org/wiki/Main_Page?uselang=de' target='_blank'>Wikimedia Commons</a>. ";
                    }

                    AttributionHtmlString +=
                        "Die Lizenzangaben werden zur Zeit aufbereitet und erscheinen innerhalb der nächsten Tage. ";

                    if (!String.IsNullOrEmpty(OriginalFileLink))
                    {
                        AttributionHtmlString += "Hier findest du die <a href='" + OriginalFileLink + "' target='_blank'>Originaldatei</a>.";
                    }
                }
            }

            ImageParsingNotifications = ImageParsingNotifications.FromJson(ImageMetaData.Notifications);
        }
    }

    public static ImageFrontendData Create(Question question)
    {
        return new ImageFrontendData(question.Id, ImageType.Question);
    }

    public static ImageFrontendData Create(Set set)
    {
        return new ImageFrontendData(set.Id, ImageType.QuestionSet);
    }

    public ImageUrl GetImageUrl(int width, bool asSquare = false, bool getDummy = false, ImageType imageTypeForDummy = ImageType.Question)
    {
        IImageSettings imageSettings;
        var typeId = getDummy ? -1 : (ImageMetaDataExists ? ImageMetaData.TypeId : -1);
        var imageType = ImageMetaDataExists ? ImageMetaData.Type : imageTypeForDummy;

        if (imageType == ImageType.Category)
        {
            imageSettings = new CategoryImageSettings(typeId);
        }

        else if (imageType == ImageType.User)
        {
            imageSettings = new UserImageSettings(typeId);
        }

        else if (imageType == ImageType.QuestionSet)
        {
            imageSettings = SetImageSettings.Create(typeId);
        }

        else //Default: question
        {
            imageSettings = new QuestionImageSettings(typeId);
        }

        return ImageUrl.Get(imageSettings, width, asSquare, arg => ImageUrl.GetFallbackImageUrl(imageSettings, width));
    }

    public string RenderHtmlImageBasis(
        int width, 
        bool asSquare, 
        ImageType imageTypeForDummies, 
        string insertLicenseLinkAfterAncestorOfClass = "ImageContainer", 
        string additionalCssClasses = "",
        string linkToItem = "",
        bool noFollow = false)
    {
        var imageUrl = GetImageUrl(width, asSquare, false, imageTypeForDummies);
        if(additionalCssClasses != "")
            additionalCssClasses = " " + additionalCssClasses;

        if (ImageMetaDataExists && imageUrl.HasUploadedImage)
        {
            if (!ImageCanBeDisplayed)
            {
                additionalCssClasses += " JS-CantBeDisplayed";
                //return "<img src='" + GetImageUrl(width, asSquare, true, imageTypeForDummies).Url + "' class='ItemImage JS-InitImage CantBeDisplayed" + additionalCssClasses + "' />";
            }

            var altDescription = String.IsNullOrEmpty(this.Description) ?
                "" : 
                this.Description.Replace("\"", "'")
                    .Replace("„", "'")
                    .Replace("“", "'")
                    .StripHTMLTags()
                    .Truncate(120, true);
            return AddLink(
                    "<img src='" + GetImageUrl(width, asSquare, true, imageTypeForDummies).Url //Dummy url gets replaced by javascript (look for class: LicensedImage) to avoid displaying images without license in case of no javascript
                    + "' class='ItemImage LicensedImage JS-InitImage" + additionalCssClasses
                    + "' data-image-id='" + ImageMetaData.Id + "' data-image-url='" + imageUrl.Url
                    + "' data-append-image-link-to='" + insertLicenseLinkAfterAncestorOfClass
                    + "' alt='" + altDescription + "'/>",
                    linkToItem, noFollow);
        }
        
        return AddLink( //if no image, then display dummy picture
            "<img src='" + GetImageUrl(width, asSquare, true, imageTypeForDummies).Url 
            + "' class='ItemImage JS-InitImage" + additionalCssClasses
            + "' data-append-image-link-to='" + insertLicenseLinkAfterAncestorOfClass 
            + "' alt=''/>",
            linkToItem, noFollow);
    }

    private static string AddLink(string html, string link, bool noFollow = false)
    {
        if(link == "")
            return html;

        var noFollowString = noFollow ? " rel='nofollow'" : "";
        return $"<a href='{link}'{noFollowString}>{html}</a>";

    }

    private static ImageMetaData PrepareConstructorArguments(int typeId, ImageType imageType)
    {
        return ServiceLocator.Resolve<ImageMetaDataRepo>().GetBy(typeId, imageType);
    }
}
