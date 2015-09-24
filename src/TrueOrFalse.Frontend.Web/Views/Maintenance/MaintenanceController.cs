﻿using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Maintenance;
using TrueOrFalse.Search;
using TrueOrFalse.Utilities.ScheduledJobs;
using TrueOrFalse.Web;

public class MaintenanceController : BaseController
{
    [AccessOnlyAsAdmin]
    public ActionResult Maintenance(){
        return View(new MaintenanceModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Images(int? page)
    {
        var model = new MaintenanceImagesModel();
        if (!page.HasValue)
        {
            model.CkbOpen = true;
            model.CkbExcluded = true;
            model.CkbApproved = true;
        }
        else
        {
            var searchSpec = Sl.R<SessionUiData>().ImageMetaDataSearchSpec;
            model.CkbOpen = searchSpec.LicenseStates.Any(x => x == ImageLicenseState.Unknown);
            model.CkbExcluded = searchSpec.LicenseStates.Any(x => x == ImageLicenseState.NotApproved);
            model.CkbApproved = searchSpec.LicenseStates.Any(x => x == ImageLicenseState.Approved);            
        }

        model.Init(page);
        return View(model);
    }

    [AccessOnlyAsAdmin]
    [HttpPost]
    public ActionResult Images(int? page, MaintenanceImagesModel imageModel)
    {
        imageModel.Init(null);
        return View(imageModel);
    }

    public ActionResult LoadMarkupAndParse(int? page)
    {
        Resolve<LoadImageMarkups>().UpdateAllWithoutAuthorizedMainLicense();
        return View("Images", new MaintenanceImagesModel(page) { Message = new SuccessMessage("License data has been updated") });
    }

    public ActionResult LoadMarkupAndParseAll(int? page)
    {
        Resolve<LoadImageMarkups>().UpdateAll();
        return View("Images", new MaintenanceImagesModel(page) { Message = new SuccessMessage("License data has been updated") });
    }

    public ActionResult SetAllImageLicenseStati()
    {
        SetImageLicenseStatus.RunForAll();
        return View("Images", new MaintenanceImagesModel(null) { Message = new SuccessMessage("License stati have been set") });
    }

    public JsonResult ImageReload(int imageMetaDataId)
    {
        var imageMetaData = R<ImageMetaDataRepo>().GetById(imageMetaDataId);
        R<ImageStore>().ReloadWikipediaImage(imageMetaData);

        return new JsonResult
        {
            Data = new {
                Url = new ImageFrontendData(imageMetaData).GetImageUrl(350).Url
            }
        };
    }

    public ActionResult ParseMarkupFromDb(int? page)
    {
        Resolve<ParseMarkupFromDb>().Run();
        return View("Images", new MaintenanceImagesModel(page) { Message = new SuccessMessage("License data has been updated") });
    }

    [AccessOnlyAsAdmin]
    public ActionResult Messages()
    {
        return View(new MaintenanceMessagesModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult RecalculateAllKnowledgeItems()
    {
        R<AddProbabilitiesEntries_ForSetsAndDates>().RunForAllUsers();

        ProbabilityUpdate_ValuationAll.Run();
        ProbabilityUpdate_Question.Run();
        ProbabilityUpdate_Category.Run();
        ProbabilityUpdate_User.Run();

        return View("Maintenance", new MaintenanceModel{
            Message = new SuccessMessage("Antwortwahrscheinlichkeiten wurden neu berechnet.")
        });
    }

    [AccessOnlyAsAdmin]
    public ActionResult CalcAggregatedValuesQuestions()
    {
        Resolve<UpdateQuestionAnswerCounts>().Run();
        return View("Maintenance", new MaintenanceModel{Message = new SuccessMessage("Aggregierte Werte wurden aktualisiert.")});
    }

    [AccessOnlyAsAdmin]
    public ActionResult CalcAggregatedValuesSets()
    {
        Resolve<UpdateSetDataForQuestion>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Aggregierte Werte wurden aktualisiert.") });
    }

    [AccessOnlyAsAdmin]
    public ActionResult DeleteValuationsForRemovedSets()
    {
        Resolve<DeleteValuationsForNonExisitingSets>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Valuations for deleted sets are removed.") });
    }

    [AccessOnlyAsAdmin]
    public ActionResult UpdateFieldQuestionCountForCategories()
    {
        Resolve<UpdateQuestionCountForCategory>().All();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Feld: AnzahlFragen für Kategorien wurde aktualisiert.") });
    }

    public ActionResult UpdateUserReputationAndRankings()
    {
        Resolve<UpdateReputationsAndRank>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Reputation and Rankings wurden aktualisiert.") });
    }

    public ActionResult UpdateUserWishCount()
    {
        Resolve<UpdateWishcount>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Wunschwissen-Antwortwahrscheinlichkeit wurde aktualisiert.") });
    }

    [AccessOnlyAsAdmin]
    public ActionResult ReIndexAllQuestions()
    {
        Resolve<ReIndexAllQuestions>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Fragen wurden neu indiziert.") });
    }

    [AccessOnlyAsAdmin]
    public ActionResult ReIndexAllSets()
    {
        Resolve<ReIndexAllSets>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Fragesätze wurden neu indiziert.") });
    }

    [AccessOnlyAsAdmin]
    public ActionResult ReIndexAllCategories()
    {
        Resolve<ReIndexAllCategories>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Kategorien wurden neu indiziert.") });
    }

    [AccessOnlyAsAdmin]
    public ActionResult ReIndexAllUsers(){
        Resolve<ReIndexAllUsers>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Nutzer wurden neu indiziert.") });
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public ActionResult SendMessage(MaintenanceMessagesModel model)
    {
        Resolve<SendCustomMsg>().Run(
            model.TestMsgReceiverId, 
            model.TestMsgSubject,
            model.TestMsgBody);

        model.Message = new SuccessMessage("Message was sent");
        return View("Messages", model);
    }

    [AccessOnlyAsAdmin]
    public ActionResult Tools()
    {
        return View(new MaintenanceToolsModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Throw500()
    {
        throw new Exception("Some random exception");
    }

    [AccessOnlyAsAdmin]
    public ActionResult CleanUpWorkInProgressQuestions()
    {
        JobScheduler.StartCleanupWorkInProgressJob();
        return View("Tools", new MaintenanceToolsModel { Message = new SuccessMessage("Job: 'Cleanup work in progress' wird ausgeführt.") });
    }

    [AccessOnlyAsAdmin]
    public ActionResult ImageMarkup(int imgId)
    {
        var imageMaintenanceInfo =
            Resolve<GetImageMaintenanceInfos>()
                .Run().FirstOrDefault(imageInfo => imageInfo.ImageId == imgId);
        return View("Markup", imageMaintenanceInfo);
    }

    [AccessOnlyAsAdmin]
    public string ImageMaintenanceModal(int imgId)
    {
        var imageMaintenanceInfo = new ImageMaintenanceInfo(Resolve<ImageMetaDataRepo>().GetById(imgId));
        return ViewRenderer.RenderPartialView("ImageMaintenanceModal", imageMaintenanceInfo, ControllerContext);
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
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

        return ViewRenderer.RenderPartialView("ImageMaintenanceRow", imageMaintenanceInfo, ControllerContext);
    }
}