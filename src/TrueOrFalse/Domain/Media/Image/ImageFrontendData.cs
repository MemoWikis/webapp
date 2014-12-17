using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse;

//$temp: stattdessen ImageMaintenanceInfo?
public class ImageFrontendData
{ 
    public ImageMetaData ImageMetaData;
    public MainLicenseInfo MainLicenseInfo;
    public ManualImageEvaluation ManualImageEvaluation;
    public bool ImageCanBeDisplayed;
    public bool HasAuthorizedMainLicense;
    public bool LicenseDataIncomplete;
    public License MainLicense;
    public string Author;
    public string LicenseName;
    public string LicenseLink;
    public string LicenseShortDescriptionLink;
    public string AttributionHtmlString;


    public ImageFrontendData(int typeId, ImageType imageType) : this(PrepareConstructorArguments(typeId, imageType))
    {
    }
        
    public ImageFrontendData(ImageMetaData imageMetaData)
    {
        ImageMetaData = imageMetaData;
        MainLicenseInfo = !String.IsNullOrEmpty(ImageMetaData.MainLicenseInfo)
                            ? MainLicenseInfo.FromJson(ImageMetaData.MainLicenseInfo)
                            : null;
        
        ManualImageEvaluation = imageMetaData.ManualEntriesFromJson().ManualImageEvaluation;
        ImageCanBeDisplayed = ManualImageEvaluation != ManualImageEvaluation.ImageManuallyRuledOut;

        if (!ImageCanBeDisplayed)
        {
            AttributionHtmlString = "Das Bild kann aus lizenzrechtlichen Gründen leider zur Zeit nicht angezeigt werden.";
        }

        else { //Image can be displayed

            if (MainLicenseInfo != null &&
                ManualImageEvaluation == ManualImageEvaluation.ImageCheckedForCustomAttributionAndAuthorized)
            {
                HasAuthorizedMainLicense = true;
                MainLicense = LicenseRepository.GetById(MainLicenseInfo.MainLicenseId);
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

                AttributionHtmlString = "Bild: " + Author;

                if (ImageMetaData.Source == ImageSource.WikiMedia)
                {
                    AttributionHtmlString +=
                        ", <a href='http://commons.wikimedia.org/wiki/Main_Page?uselang=de' target='_blank'>Wikimedia Commons</a>";
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
                AttributionHtmlString = "Bild: " + Author;

                if (ImageMetaData.Source == ImageSource.WikiMedia)
                {
                    AttributionHtmlString +=
                        ", <a href='http://commons.wikimedia.org/wiki/Main_Page?uselang=de' target='_blank'>Wikimedia Commons</a>";
                }

                AttributionHtmlString +=
                    ". Die Lizenzangaben werden zur Zeit aufbereitet und erscheinen innerhalb der nächsten Tage.";
            }
        }
    }

    private static ImageMetaData PrepareConstructorArguments(int typeId, ImageType imageType)
    {
        return ServiceLocator.Resolve<ImageMetaDataRepository>().GetBy(typeId, imageType);
    }
}
