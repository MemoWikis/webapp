using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Maintenance;
using TrueOrFalse.Web;

[AccessOnlyAsAdmin]
public class MaintenanceImagesController : BaseController
{
    private const string _viewLocation = "~/Views/Maintenance/Images/Images.aspx";
    private const string _viewLocationMarkup = "~/Views/Maintenance/Images/Markup.ascx";
    private const string _viewLocationModal = "~/Views/Maintenance/Images/ImageMaintenanceModal.ascx";
    private const string _viewLocationRow = "~/Views/Maintenance/Images/ImageMaintenanceRow.ascx";

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult Images(int? page)
    {
        var model = new ImagesModel();
        if (!page.HasValue)
        {
            model.CkbOpen = true;
            model.CkbExcluded = false;
            model.CkbApproved = false;
        }
        else
        {
            var searchSpec = _sessionUiData.ImageMetaDataSearchSpec;

            model.CkbOpen = searchSpec.LicenseStates.Any(x => x == ImageLicenseState.Unknown);
            model.CkbExcluded = searchSpec.LicenseStates.Any(x => x == ImageLicenseState.NotApproved);
            model.CkbApproved = searchSpec.LicenseStates.Any(x => x == ImageLicenseState.Approved);
        }

        model.Init(page);
        return View(_viewLocation, model);
    }

    [HttpPost]
    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult Images(int? page, ImagesModel imageModel)
    {
        imageModel.Init(1);
        return View(_viewLocation, imageModel);
    }

    public ActionResult ReparseMarkup_OfNoneAuthorized(int? page)
    {
        Resolve<LoadImageMarkups>().UpdateAllWithoutAuthorizedMainLicense(loadMarkupFromWikipedia: false);
        return View(_viewLocation, new ImagesModel(page) { Message = new SuccessMessage("License data has been updated") });
    }

    public ActionResult ReparseMarkup_OfNoneAuthorized_AndLoadFromWikipedia(int? page)
    {
        Resolve<LoadImageMarkups>().UpdateAllWithoutAuthorizedMainLicense();
        return View(_viewLocation, new ImagesModel(page) { Message = new SuccessMessage("License data has been updated") });
    }

    public ActionResult ReparseMarkup_All_AndLoadFromWikipedia(int? page)
    {
        Resolve<LoadImageMarkups>().UpdateAll();
        return View(_viewLocation, new ImagesModel(page) { Message = new SuccessMessage("License data has been updated") });
    }

    public ActionResult SetAllImageLicenseStati()
    {
        SetImageLicenseStatus.RunForAll();
        return View(_viewLocation, new ImagesModel(null) { Message = new SuccessMessage("License stati have been set") });
    }

    public JsonResult ImageReload(int imageMetaDataId)
    {
        var imageMetaData = R<ImageMetaDataRepo>().GetById(imageMetaDataId);
        R<ImageStore>().ReloadWikipediaImage(imageMetaData);

        return new JsonResult
        {
            Data = new
            {
                Url = new ImageFrontendData(imageMetaData).GetImageUrl(350).Url
            }
        };
    }

    public EmptyResult ImageDelete(int imageMetaDataId)
    {
        var imageMetaData = R<ImageMetaDataRepo>().GetById(imageMetaDataId);
        R<ImageStore>().Delete(imageMetaData);

        return new EmptyResult();
    }

    public ActionResult ParseMarkupFromDb(int? page)
    {
        Resolve<ParseMarkupFromDb>().Run();
        return View(_viewLocation, new ImagesModel(page) { Message = new SuccessMessage("License data has been updated") });
    }

    public ActionResult ImageMarkup(int imgId)
    {
        var imageMaintenanceInfo = 
                Resolve<GetImageMaintenanceInfos>()
                .Run().FirstOrDefault(imageInfo => imageInfo.ImageId == imgId);
        return View(_viewLocationMarkup, imageMaintenanceInfo);
    }

    public string ImageMaintenanceModal(int imgId)
    {
        var imageMaintenanceInfo = new ImageMaintenanceInfo(Resolve<ImageMetaDataRepo>().GetById(imgId));
        return ViewRenderer.RenderPartialView(_viewLocationModal, imageMaintenanceInfo, ControllerContext);
    }

    [HttpPost]
    public string UpdateImage(
        int id,
        string authorManuallyAdded,
        string descriptionManuallyAdded,
        string manualImageEvaluation,
        string remarks,
        int selectedMainLicenseId)
    {
        var imageMetaData = Resolve<ImageMetaDataRepo>().GetById(id);
        var manualEntries = imageMetaData.ManualEntriesFromJson();
        var selectedEvaluation = (ManualImageEvaluation)Enum.Parse(typeof(ManualImageEvaluation), manualImageEvaluation);

        manualEntries.AuthorManuallyAdded = authorManuallyAdded;
        manualEntries.DescriptionManuallyAdded = descriptionManuallyAdded;
        manualEntries.ManualRemarks = remarks;

        var message = "";

        if (selectedMainLicenseId == -1)
        {
            if (selectedEvaluation == ManualImageEvaluation.ImageManuallyRuledOut)
            {
                manualEntries.ManualImageEvaluation = selectedEvaluation;
                imageMetaData.MainLicenseInfo = null;
            }
            else
            {
                message += "Um Hauptlizenz zu löschen, bitte gleichzeitig unter Freigabe Bild ausschließen. Freigabe wurde nicht geändert.";
            }
        }

        else if (selectedEvaluation == ManualImageEvaluation.ImageCheckedForCustomAttributionAndAuthorized)
        {
            if (selectedMainLicenseId < -1)
            {
                message += "Die gewählte Lizenz ist nicht gültig. Hauptlizenz und Freigabe wurden nicht geändert. ";
            }
            else
            {
                ImageMetaDataRepo.SetMainLicenseInfo(imageMetaData, selectedMainLicenseId);
                manualEntries.ManualImageEvaluation = selectedEvaluation;
            }
        }

        else //If not authorized
        {
            message += "Das Bild wurde nicht freigegeben. Die Hauptlizenz wurde nicht geändert. ";
            manualEntries.ManualImageEvaluation = selectedEvaluation;
        }

        imageMetaData.ManualEntries = manualEntries.ToJson();

        Resolve<ImageMetaDataRepo>().Update(imageMetaData);

        var imageMaintenanceInfo = new ImageMaintenanceInfo(imageMetaData);
        imageMaintenanceInfo.MaintenanceRowMessage = message;

        return ViewRenderer.RenderPartialView(_viewLocationRow, imageMaintenanceInfo, ControllerContext);
    }
}