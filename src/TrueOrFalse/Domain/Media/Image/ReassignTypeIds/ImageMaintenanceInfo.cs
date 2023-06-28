using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class ImageMaintenanceInfo
{
    public int ImageId;
    public int TypeId;
    public bool TypeNotFound;
    public object Type;
    public string TypeUrl;

    public bool InQuestionFolder;
    public bool InCategoryFolder;
    public bool InSetFolder;

    public ImageMetaData MetaData;
    public ManualImageData ManualImageData;

    public string Url_128;

    public string FileName;
    public string Description;
    public string Author;

    public LicenseImage MainLicenseAuthorized;
    public LicenseImage SuggestedMainLicense;
    public List<LicenseImage> AllRegisteredLicenses;
    public List<LicenseImage> AllAuthorizedLicenses;
    public ImageLicenseState LicenseState;
    public string GlobalLicenseStateMessage;
    public string LicenseStateCssClass;
    public string LicenseStateHtmlList;
    public ImageFrontendData FrontendData;
    private readonly List<LicenseImage> _offeredLicenses;
    public int SelectedMainLicenseId { get; set; }


    public ImageMaintenanceInfo(int typeId, ImageType imageType)
        : this(ServiceLocator.Resolve<ImageMetaDataRepo>().GetBy(typeId, imageType))
    {
    }

    public ImageMaintenanceInfo(ImageMetaData imageMetaData)
    {
        var categoryImgBasePath = new CategoryImageSettings().BasePath;
        var questionImgBasePath = new QuestionImageSettings().BasePath;
        var setImgBasePath = new SetImageSettings().BasePath;

        ImageId = imageMetaData.Id;
        MetaData = imageMetaData;
        TypeId = imageMetaData.TypeId;
        TypeNotFound = false;
        
        switch (MetaData.Type)
        {
            case ImageType.Category:
                Type = Sl.R<CategoryRepository>().GetById(MetaData.TypeId);
                TypeUrl = Links.GetUrl(Type);
                break;
            case ImageType.QuestionSet:
                Type = Sl.R<SetRepo>().GetById(MetaData.TypeId);
                TypeUrl = Links.GetUrl(Type);
                break;
            case ImageType.Question:
                Type = Sl.R<QuestionRepo>().GetById(MetaData.TypeId);
                TypeUrl = Links.GetUrl(Type);
                break;
            default:
                throw new Exception("invalid type");
        }

        if (Type == null)
            TypeNotFound = true;
       
        ManualImageData = ManualImageData.FromJson(MetaData.ManualEntries);
            
        //new
        FileName = !String.IsNullOrEmpty(MetaData.SourceUrl)
                        ? Regex.Split(MetaData.SourceUrl, "/").Last()
                        : "";
        Description = !String.IsNullOrEmpty(ManualImageData.DescriptionManuallyAdded)
                        ? ManualImageData.DescriptionManuallyAdded
                        : MetaData.DescriptionParsed;
        Author = !String.IsNullOrEmpty(ManualImageData.AuthorManuallyAdded)
                    ? ManualImageData.AuthorManuallyAdded
                    : MetaData.AuthorParsed;

        _offeredLicenses = new List<LicenseImage> {new LicenseImage { Id = -2, WikiSearchString = "Hauptlizenz wählen" } }
            .Concat(new List<LicenseImage> { new LicenseImage { Id = -1, WikiSearchString = "Hauptlizenz löschen" } })
            .ToList();
            
        if (LicenseImage.FromLicenseIdList(MetaData.AllRegisteredLicenses).Any(x => LicenseImageRepo.GetAllAuthorizedLicenses().Any(y => x.Id == y.Id)))
        {
            _offeredLicenses = _offeredLicenses
                .Concat(new List<LicenseImage>{new LicenseImage { Id = -3, WikiSearchString = "Geparste autorisierte Lizenzen" } })
                .Concat(
                    LicenseImage.FromLicenseIdList(MetaData.AllRegisteredLicenses)
                        .Where(x => 
                            LicenseImageRepo.GetAllAuthorizedLicenses()
                            .Any(y => x.Id == y.Id)
                        )
                )
                .ToList();
        }

        if (
            LicenseImageRepo.GetAllAuthorizedLicenses()
                .Any(x => LicenseImage.FromLicenseIdList(MetaData.AllRegisteredLicenses).All(y => x.Id != y.Id)))
        {
            _offeredLicenses = _offeredLicenses.Concat(new List<LicenseImage>{ new LicenseImage { Id = -4, WikiSearchString = "Sonstige autorisierte Lizenzen (ACHTUNG: Nur verwenden, wenn beim Bild gefunden!)" } })
                                                .Concat(LicenseImageRepo.GetAllAuthorizedLicenses().Where(x => LicenseImage.FromLicenseIdList(MetaData.AllRegisteredLicenses).All(y => x.Id != y.Id)))
                                                .ToList();
        }
                
        MainLicenseAuthorized = MainLicenseInfo.FromJson(MetaData.MainLicenseInfo).GetMainLicense();
        AllRegisteredLicenses = LicenseImage.FromLicenseIdList(MetaData.AllRegisteredLicenses);
        AllAuthorizedLicenses = AllRegisteredLicenses
                                .Where(x => LicenseImageRepo.GetAllAuthorizedLicenses().Any(y => x.Id == y.Id))
                                .ToList();
        SuggestedMainLicense = LicenseParser.SuggestMainLicenseFromParsedList(imageMetaData) ?? //Checked for requirements
                                AllAuthorizedLicenses.FirstOrDefault(); //not checked
        SelectedMainLicenseId = (MetaData.MainLicenseInfo != null 
                                    && MainLicenseInfo.FromJson(MetaData.MainLicenseInfo) != null)
                                    ? MainLicenseInfo.FromJson(MetaData.MainLicenseInfo).MainLicenseId
                                    : (SuggestedMainLicense != null ? SuggestedMainLicense.Id : -2);
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
            Url_128 = new SetImageSettings(MetaData.TypeId).GetUrl_128px_square().Url;

        FrontendData = new ImageFrontendData(MetaData);
    }

    public void EvaluateImageDeployability()
    {
        LicenseState = ImageLicenseState.Unknown;

        if (ManualImageData.ManualImageEvaluation == ManualImageEvaluation.ImageManuallyRuledOut)
        {
            LicenseState = ImageLicenseState.NotApproved;
            GlobalLicenseStateMessage = "Bild wurde manuell von der Nutzung ausgeschlossen.";
            return;
        }

        if (ManualImageData.ManualImageEvaluation == ManualImageEvaluation.NotAllRequirementsMetYet)
        {
            LicenseState = ImageLicenseState.Unknown;
            GlobalLicenseStateMessage += "Manuell festgestellt: derzeit nicht alle Attributierungsanforderungen erfüllt.";
            return;
        }

        if (EvaluateMainLicensePresence() &&
            EvaluateLicenseRequirements(MainLicenseAuthorized) &&
            EvaluateManualApproval())
        {
            LicenseState = ImageLicenseState.Approved;
            GlobalLicenseStateMessage = "Alles klar (Hauptlizenz vorhanden, Angaben vollständig, Bild freigegeben).";
            return;
        }

        if (EvaluateMainLicensePresence())
        {
            GlobalLicenseStateMessage = "Hauptlizenz vorhanden. ";
            EvaluateLicenseRequirements(MainLicenseAuthorized);
            EvaluateManualApproval();
            return;
        }

        if (LicenseParser.SuggestMainLicenseFromParsedList(MetaData) != null) {

                GlobalLicenseStateMessage = String.Format(
                    "Keine Hauptlizenz festgelegt, aber verwendbare geparste Lizenz ({0}) vorhanden. Bitte überprüfen und freigeben.",
                    LicenseParser.SuggestMainLicenseFromParsedList(MetaData).WikiSearchString);
                return;
        }

        if (AllAuthorizedLicenses.Any())
        {
            GlobalLicenseStateMessage =
                String.Format("Keine Hauptlizenz festgelegt. Geparste Lizenzen vorhanden. Vorschlag: {0}. ",
                    AllAuthorizedLicenses.First().WikiSearchString);
            EvaluateLicenseRequirements(AllAuthorizedLicenses.First());
            return;
        }

        GlobalLicenseStateMessage = "Keine (autorisierte) Lizenz automatisch ermittelt.";
    }

    private bool EvaluateLicenseRequirements(LicenseImage license)
    {
        var requirementsCheck = LicenseParser.CheckLicenseRequirementsWithDb(license, MetaData);
        if (requirementsCheck.AllRequirementsMet)
            return true;

        var missingDataList = new List<string>
        {
            requirementsCheck.AuthorIsMissing ? "Autor" : "",
            requirementsCheck.LicenseLinkIsMissing ? "Lizenzlink" : "",
            requirementsCheck.LocalCopyOfLicenseUrlMissing ? "Lizenzkopie" : ""
        };

        LicenseState = ImageLicenseState.Unknown;
        GlobalLicenseStateMessage += String.Format("Angaben fehlen ({0}). ",
            missingDataList.Where(x => x != "").Aggregate((a, b) => a + ", " + b));

        return false;
    }

    private bool EvaluateManualApproval()
    {
        if(ManualImageData.ManualImageEvaluation == ManualImageEvaluation.ImageCheckedForCustomAttributionAndAuthorized)
            return true;

        LicenseState = ImageLicenseState.Unknown;
        GlobalLicenseStateMessage += "Bild wurde (noch) nicht zugelassen. ";

        return false;
    }

    public bool EvaluateMainLicensePresence()
    {
        if (MainLicenseAuthorized != null)
            return true;
        LicenseState = ImageLicenseState.Unknown;
        GlobalLicenseStateMessage += "Keine Hauptlizenz vorhanden. ";
        return false;
    }

    public void SetLicenseStateCssClass()
    {
        switch (LicenseState)
        {
            case ImageLicenseState.Approved:
                LicenseStateCssClass = "success";
                break;

            case ImageLicenseState.Unknown:
                LicenseStateCssClass = "warning";
                break;

            case ImageLicenseState.NotApproved:
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
        return AllRegisteredLicenses.Count > 0
            ? "<ul>" + 
                AllRegisteredLicenses
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

    public string GetSingleLicenseStateMessage(LicenseImage license)
    {
        switch (LicenseParser.CheckImageLicenseState(license, MetaData))
        {
            case global::LicenseState.IsApplicableForImage:
                return "verwendbar";

            case global::LicenseState.AuthorizedButInfoMissing:
                return "zugelassen, aber benötigte Angaben unvollständig";

            case global::LicenseState.IsNotAuthorized:
                return "nicht zugelassen";
        }

        return "unbekannt";
    }
}