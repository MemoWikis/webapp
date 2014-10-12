using System;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Maintenance;
using TrueOrFalse.Search;
using TrueOrFalse.Web;

public class MaintenanceController : BaseController
{
    [AccessOnlyAsAdmin]
    public ActionResult Maintenance(){
        return View(new MaintenanceModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Images(){
        return View(new MaintenanceImagesModel());
    }

    public ActionResult ImageUpdateLicenceData()
    {
        Resolve<LoadImageMarkups>().Run();
        return View("Images", new MaintenanceImagesModel { Message = new SuccessMessage("Licence data has been updated") });
    }

    public ActionResult ImageUpdateMarkupFromDb()
    {
        Resolve<UpdateMarkupFromDb>().Run();
        return View("Images", new MaintenanceImagesModel { Message = new SuccessMessage("Licence data has been updated") });
    }

    [AccessOnlyAsAdmin]
    public ActionResult Messages()
    {
        return View(new MaintenanceMessagesModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult RecalculateAllKnowledgeItems()
    {
        Resolve<ProbabilityForUsersUpdate>().Run();
        Resolve<ProbabilityUpdate_OnQuestion>().Run();

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
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Valutions for deleted sets are removed.") });
    }

    [AccessOnlyAsAdmin]
    public ActionResult UpdateFieldQuestionCountForCategories()
    {
        Resolve<UpdateQuestionCountForAllCategories>().Run();
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
        return View(new MaintenanceModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Throw500()
    {
        throw new Exception("Some random exception");
    }
}