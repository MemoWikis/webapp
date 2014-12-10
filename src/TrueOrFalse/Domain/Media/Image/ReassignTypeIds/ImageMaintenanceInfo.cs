using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using EasyNetQ.Events;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using NHibernate.Loader.Custom;
using NHibernate.Mapping;

namespace TrueOrFalse
{
    public class ImageMaintenanceInfo
    {
        public string Test = "";
        
        public int ImageId;
        public int TypeId;

        public bool InQuestionFolder;
        public bool InCategoryFolder;
        public bool InSetFolder;

        public ImageMetaData MetaData;
        public ManualImageData ManualImageData;

        public string Url_128;

        //new
        public string Author;
        public string Description;

        public License MainLicense;
        public List<License> AllLicenses;
        public ImageDeployability ImageDeployability;
        public string GlobalLicenseStateMessage;
        public string LicenseStateCssClass;
        public string LicenseStateHtmlList;


        public List<string> PossibleLicenseStrings;

        public ImageMaintenanceInfo(int typeId, ImageType imageType)
            : this(ServiceLocator.Resolve<ImageMetaDataRepository>().GetBy(typeId, imageType))
        {
        }

        public ImageMaintenanceInfo(ImageMetaData imageMetaData)
        {
            var categoryImgBasePath = new CategoryImageSettings().BasePath;
            var questionImgBasePath = new QuestionImageSettings().BasePath;
            var setImgBasePath = new QuestionSetImageSettings().BasePath;

            ImageId = imageMetaData.Id;
            MetaData = imageMetaData;
            TypeId = imageMetaData.TypeId;
            ManualImageData = ManualImageData.FromJson(MetaData.ManualEntries);
            
            //new
            Author = !String.IsNullOrEmpty(ManualImageData.AuthorManuallyAdded)
                        ? ManualImageData.AuthorManuallyAdded
                        : (MetaData.AuthorParsed);
            Description = !String.IsNullOrEmpty(MetaData.SourceUrl)
                            ? "Datei: " + Regex.Split(MetaData.SourceUrl, "/").Last() + "<br/>"
                            : "";
            Description += !String.IsNullOrEmpty(ManualImageData.DescriptionManuallyAdded)
                           ? ManualImageData.DescriptionManuallyAdded
                           : (MetaData.DescriptionParsed);

            TempHelperLicenseInfoFromDbOrSetNew.Run(MetaData);
            MainLicense = MainLicenseInfo.FromJson(MetaData.MainLicenseInfo).GetMainLicense();
            AllLicenses = License.FromLicenseIdList(MetaData.AllRegisteredLicenses);
            LicenseStateHtmlList = !String.IsNullOrEmpty(ToLicenseStateHtmlList()) ?
                                    ToLicenseStateHtmlList() : 
                                    "";
            EvaluateImageDeployability();
            SetLicenseStateCssClass();

            InCategoryFolder = File.Exists(HttpContext.Current.Server.MapPath(
                categoryImgBasePath + imageMetaData.TypeId + ".jpg"));
            InQuestionFolder = File.Exists(HttpContext.Current.Server.MapPath(
                questionImgBasePath + imageMetaData.TypeId + ".jpg"));
            InSetFolder = File.Exists(HttpContext.Current.Server.MapPath(
                setImgBasePath + imageMetaData.TypeId + ".jpg"));

            if (MetaData.Type == ImageType.Category)
                Url_128 = new CategoryImageSettings(MetaData.TypeId).GetUrl_128px(asSquare: true).Url;
            
            if (MetaData.Type == ImageType.Question)
                Url_128 = new QuestionImageSettings(MetaData.TypeId).GetUrl_128px_square().Url;

            if (MetaData.Type == ImageType.QuestionSet)
                Url_128 = QuestionSetImageSettings.Create(MetaData.TypeId).GetUrl_128px_square().Url;
        }

        public void EvaluateImageDeployability()
        {
            if (ManualImageData.ManualImageEvaluation == ManualImageEvaluation.ImageManuallyRuledOut)
            {
                ImageDeployability = ImageDeployability.ImageCurrentlyNotDeployable;
                GlobalLicenseStateMessage = "Bild wurde manuell von der Nutzung ausgeschlossen.";
                return;
            }
            
            if (MainLicense != null)
            {
                GlobalLicenseStateMessage += "Lizenzangaben vollst�ndig. ";

                if (ManualImageData.ManualImageEvaluation ==
                    ManualImageEvaluation.ImageCheckedForCustomAttributionAndAuthorized)
                {
                    ImageDeployability = ImageDeployability.ImageIsReadyToUse;
                    GlobalLicenseStateMessage += "Auf spezielle Attributierungsanforderungen �berpr�ft und zugelassen.";
                    return;
                }
                if (ManualImageData.ManualImageEvaluation == ManualImageEvaluation.NotAllRequirementsMetYet)
                {
                    ImageDeployability = ImageDeployability.ImageCurrentlyNotDeployable;
                    GlobalLicenseStateMessage += "Manuell festgestellt: derzeit nicht alle Attributierungsanforderungen erf�llt.";
                    return;
                }
                if (ManualImageData.ManualImageEvaluation == ManualImageEvaluation.ImageNotEvaluated)
                {
                    ImageDeployability = ImageDeployability.ImageCurrentlyNotDeployable;
                    GlobalLicenseStateMessage += "Bitte auf spezielle Attributierungsanforderungen �berpr�fen und wenn m�glich freigeben.";
                    return;
                }
            }

            if (AllLicenses.Any(license =>
                        LicenseParser.CheckImageLicenseState(license, MetaData) ==
                        ImageLicenseState.LicenseAuthorizedButInfoMissing))
            {
                ImageDeployability = ImageDeployability.ImageCurrentlyNotDeployable;
                GlobalLicenseStateMessage = "Autorisierte Lizenz vorhanden, aber Angaben fehlen. Bitte erg�nzen. ";

                if (ManualImageData.ManualImageEvaluation == ManualImageEvaluation.NotAllRequirementsMetYet)
                {
                    ImageDeployability = ImageDeployability.ImageCurrentlyNotDeployable;
                    GlobalLicenseStateMessage += "Manuell festgestellt: derzeit nicht alle Attributierungsanforderungen erf�llt.";
                    return;
                }
                if (ManualImageData.ManualImageEvaluation == ManualImageEvaluation.ImageNotEvaluated)
                {
                    ImageDeployability = ImageDeployability.ImageCurrentlyNotDeployable;
                    GlobalLicenseStateMessage += "Bitte auf spezielle Attributierungsanforderungen �berpr�fen und wenn m�glich freigeben.";
                    return;
                }

                return;
            }

            ImageDeployability = ImageDeployability.ImageCurrentlyNotDeployable;
            GlobalLicenseStateMessage = "Keine (autorisierte) Lizenz automatisch ermittelt.";
        }

        public void SetLicenseStateCssClass()
        {
            switch (ImageDeployability)
            {
                case ImageDeployability.ImageIsReadyToUse:
                    LicenseStateCssClass = "success";
                    break;

                case ImageDeployability.FurtherActionRequired:
                    LicenseStateCssClass = "warning";
                    break;

                case ImageDeployability.ImageCurrentlyNotDeployable:
                    LicenseStateCssClass = "warning";
                    break;

                case ImageDeployability.ImageRuledOutManually:
                    LicenseStateCssClass = "danger";
                    break;
            }
        }

        private int GetAmountMatches()
        {
            int amountTrues = 0;
            if (InQuestionFolder) amountTrues++;
            if (InCategoryFolder) amountTrues++;
            if (InSetFolder) amountTrues++;
            return amountTrues;
        }

        public bool IsNothing()
        {
            return !InQuestionFolder && !InCategoryFolder && !InSetFolder;
        }

        public bool IsClear()
        {
            return GetAmountMatches() == 1;
        }

        public bool IsNotClear()
        {
            return GetAmountMatches() > 1;
        }

        public string GetCssClass()
        {
            if (IsClear())
                return "success";

            if (IsNothing())
                return "warning";

            if (IsNotClear())
                return "danger";

            return "";
        }

        public ImageType GetImageType()
        {
            if (IsClear())
            {
                if (InQuestionFolder)
                    return ImageType.Question;

                if (InCategoryFolder)
                    return ImageType.Category;

                if (InSetFolder)
                    return ImageType.QuestionSet;
            }

            throw new Exception("no clear type");
        }

        public string ToLicenseStateHtmlList()
        {
            return AllLicenses.Count > 0
                ? "<ul>" + 
                    AllLicenses
                        .Aggregate("",
                            (current, license) =>
                                current + "<li>" +
                                (!String.IsNullOrEmpty(license.LicenseShortName)
                                    ? license.LicenseShortName
                                    : license.WikiSearchString) + " (" +
                                GetSingleLicenseStateMessage(license) + ")</li>")
                    + "</ul>"
                : "";
        }

        public string GetSingleLicenseStateMessage(License license)
        {
            switch (LicenseParser.CheckImageLicenseState(license, MetaData))
            {
                case ImageLicenseState.LicenseIsApplicableForImage:
                    return "verwendbar";

                case ImageLicenseState.LicenseAuthorizedButInfoMissing:
                    return "zugelassen, aber ben�tigte Angaben unvollst�ndig";

                case ImageLicenseState.LicenseIsNotAuthorized:
                    return "nicht zugelassen";
            }

            return "unbekannt";
        }
    }
}