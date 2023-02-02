using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NHibernate.Util;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Tools;
using TrueOrFalse.Tools.Cache;
using TrueOrFalse.Utilities.ScheduledJobs;
using TrueOrFalse.Web;
using static System.String;

[AccessOnlyAsAdmin]
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
[SetUserMenu(UserMenuEntry.None)]
public class MaintenanceController : BaseController
{
    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult Maintenance()
    {
        return View(new MaintenanceModel());
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult Messages()
    {
        return View(new MessagesModel());
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult CMS()
    {
        return View(new CMSModel().Init());
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult CMS(CMSModel cmsModel)
    {
        cmsModel.Message = new SuccessMessage("CMS Werte gespeichert");

        ModelState.Clear();

        return View(cmsModel);
    }

    public string CmsRenderCategoryNetworkNavigation(int id)
    {
        return ViewRenderer.RenderPartialView(
            "~/Views/Categories/Navigation/CategoryNetworkNavigation.ascx",
            new CategoryNetworkNavigationModel(id),
            ControllerContext);
    }

    [HttpPost]
    public string CmsRenderLooseCategories()
    {
        var looseCategories = GetAllCategoriesUnconnectedToRootCategories.Run()
            .OrderByDescending(c => c.CountQuestionsAggregated);
        var result = looseCategories.Count() +
                     " categories found (ordered by aggregated question count descending):<br/>";
        foreach (var category in looseCategories)
        {
            result += ViewRenderer.RenderPartialView("~/Views/Shared/CategoryLabel.ascx", category, ControllerContext);
        }

        return result;
    }

    [HttpPost]
    public string CmsRenderCategoriesInSeveralRootCategories()
    {
        var doubleRootedCategories = GetAllCategoriesInSeveralRootCategories.Run()
            .OrderByDescending(c => c.CountQuestionsAggregated);
        var result = doubleRootedCategories.Count() +
                     " categories found (ordered by aggregated question count descending):<br/>";
        foreach (var category in doubleRootedCategories)
        {
            result += ViewRenderer.RenderPartialView("~/Views/Shared/CategoryLabel.ascx", category, ControllerContext);
        }

        return result;
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult ContentCreatedReport()
    {
        return View(new ContentCreatedReportModel());
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult Statistics()
    {
        return View(new StatisticsModel());
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult RecalculateAllKnowledgeItems()
    {
        ProbabilityUpdate_ValuationAll.Run();
        ProbabilityUpdate_Question.Run();
        ProbabilityUpdate_Category.Run();
        ProbabilityUpdate_User.Run();

        return View("Maintenance", new MaintenanceModel
        {
            Message = new SuccessMessage("Antwortwahrscheinlichkeiten wurden neu berechnet.")
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult CalcAggregatedValuesQuestions()
    {
        Resolve<UpdateQuestionAnswerCounts>().Run();
        return View("Maintenance",
            new MaintenanceModel { Message = new SuccessMessage("Aggregierte Werte wurden aktualisiert.") });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult UpdateFieldQuestionCountForCategories()
    {
        Resolve<UpdateQuestionCountForCategory>().All();
        return View("Maintenance",
            new MaintenanceModel { Message = new SuccessMessage("Feld: AnzahlFragen für Themen wurde aktualisiert.") });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult UpdateUserReputationAndRankings()
    {
        Resolve<ReputationUpdate>().RunForAll();
        return View("Maintenance",
            new MaintenanceModel { Message = new SuccessMessage("Reputation and Rankings wurden aktualisiert.") });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult UpdateUserWishCount()
    {
        Resolve<UpdateWishcount>().Run();
        return View("Maintenance",
            new MaintenanceModel
            { Message = new SuccessMessage("Wunschwissen-Antwortwahrscheinlichkeit wurde aktualisiert.") });
    }
    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult ReIndexAllQuestions()
    {
        Resolve<ReIndexAllQuestions>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Fragen wurden neu indiziert.") });
    }
    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public ViewResult ReIndexAllCategories()
    {
        Resolve<SolrReIndexAllCategories>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Themen wurden neu indiziert.") });
    }
    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult ReIndexAllUsers()
    {
        Resolve<ReIndexAllUsers>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Nutzer wurden neu indiziert.") });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<ViewResult> MeiliReIndexAllQuestions()
    {
       await Resolve<MeiliSearchReIndexAllQuestions>().Go();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Fragen wurden neu indiziert.") });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<ViewResult> MeiliReIndexAllCategories()
    {
        await Resolve<MeiliSearchReIndexCategories>().Go();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Themen wurden neu indiziert.") });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<ViewResult> MeiliReIndexAllUsers()
    {
        await Resolve<MeiliSearchReIndexAllUsers>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Nutzer wurden neu indiziert.") });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult SendMessage(MessagesModel model)
    {
        CustomMsg.Send(
            model.TestMsgReceiverId,
            model.TestMsgSubject,
            model.TestMsgBody);

        model.Message = new SuccessMessage("Message was sent");
        return View("Messages", model);
    }

    [AccessOnlyAsLoggedIn]
    [ValidateAntiForgeryToken]
    [HttpPost]
    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult SendKnowledgeReportMessage(MessagesModel model)
    {
        var sessionUser = Sl.UserRepo.GetById(SessionUser.UserId);
        KnowledgeReportMsg.SendHtmlMail(sessionUser);

        model.Message = new SuccessMessage("KnowledgeReport was sent to user <em>" + SessionUser.User.Name +
                                           "</em> with email address <em>" + SessionUser.User.EmailAddress + "</em>.");
        return View("Messages", model);
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult Tools()
    {
        return View(new ToolsModel());
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult Throw500()
    {
        throw new Exception("Some random exception");
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult CleanUpWorkInProgressQuestions()
    {
        JobScheduler.StartImmediately_CleanUpWorkInProgressQuestions();
        return View("Tools",
            new ToolsModel { Message = new SuccessMessage("Job: 'Cleanup work in progress' wird ausgeführt.") });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult Start100TestJobs()
    {
        for (var i = 0; i < 1000; i++)
            JobScheduler.StartImmediately<TestJob1>();

        for (var i = 0; i < 1000; i++)
            JobScheduler.StartImmediately<TestJob2>();

        return View("Tools", new ToolsModel { Message = new SuccessMessage("Started 100 test jobs.") });

    }

    [HttpPost]
    public ActionResult CheckForDuplicateInteractionNumbers()
    {
        var duplicates = Sl.R<AnswerRepo>().GetAll()
            .Where(a => a.QuestionViewGuid != Guid.Empty)
            .GroupBy(a => new { a.QuestionViewGuid, a.InteractionNumber })
            .Where(g => g.Skip(1).Any())
            .SelectMany(g => g)
            .ToList();

        var message = duplicates.Any() ? "Es gibt Dubletten." : "Es gibt keine Dubletten.";

        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage(message) });
    }

    [HttpPost]
    public ActionResult ClearMigratedData()
    {
        var questionViewRepo = Sl.R<QuestionViewRepository>();

        questionViewRepo.GetAll()
            .Where(v => v.Migrated)
            .ForEach(v =>
            {
                v.GuidString = null;
                v.Milliseconds = 0;
                questionViewRepo.Update(v);
            });

        var answerRepo = Sl.R<AnswerRepo>();

        answerRepo.GetAll()
            .Where(a => a.Migrated)
            .ForEach(a =>
            {
                a.QuestionViewGuidString = null;
                a.InteractionNumber = 0;
                a.MillisecondsSinceQuestionView = 0;
                answerRepo.Update(a);
            });

        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Cleared") });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult CheckForCategoriesWithIncorrectQuestionCount()
    {
        var list = new List<CategoryCacheItem>();

        var cats = EntityCache.GetAllCategories();

        foreach (var cat in cats)
        {
            if (cat.GetCountQuestionsAggregated() != cat.CountQuestionsAggregated)
                list.Add(cat);
        }

        return View("Maintenance", new MaintenanceModel { });
    }

    [HttpPost]
    public ActionResult MigrateDefaultTemplates()
    {
        var allCategories = Sl.CategoryRepo.GetAll();

        foreach (var category in allCategories)
        {
            if (IsNullOrWhiteSpace(category.TopicMarkdown))
            {
                var topicMarkdownBeforeUpdate = category.TopicMarkdown;
                var categoryId = category.Id;

                if (R<CategoryRepository>().GetChildren(category.Id)
                    .Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard))
                    category.TopicMarkdown = "[[{\"TemplateName\":\"TopicNavigation\",\"Title\":\"Unterthemen\"}]]\r\n";

                if (R<CategoryRepository>().GetChildren(category.Id)
                    .Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education))
                    category.TopicMarkdown = category.TopicMarkdown + "[[{\"TemplateName\":\"EducationOfferList\"}]]" +
                                             Environment.NewLine;

                if (R<CategoryRepository>().GetChildren(category.Id)
                    .Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Media))
                    category.TopicMarkdown = category.TopicMarkdown + "[[{\"TemplateName\":\"MediaList\"}]]" +
                                             Environment.NewLine;

                category.TopicMarkdown = category.TopicMarkdown +
                                         "[[{\"TemplateName\":\"ContentLists\"}]]" + Environment.NewLine +
                                         "[[{\"TemplateName\":\"RelatedContentLists\"}]]" + Environment.NewLine +
                                         "[[{\"TemplateName\":\"CategoryNetwork\"}]]" + Environment.NewLine;

                Sl.CategoryRepo.Update(category);

                Logg.r().Information("{categoryId} {beforeMarkdown} {afterMarkdown}", categoryId,
                    topicMarkdownBeforeUpdate, category.TopicMarkdown);
            }
        }

        return View("Maintenance",
            new MaintenanceModel { Message = new SuccessMessage("Default Templates wurden migriert") });
    }

    [HttpPost]
    public ActionResult UserDelete(ToolsModel toolsModel)
    {
        Sl.UserRepo.DeleteFromAllTables(toolsModel.UserId);

        return View("Tools",
            new ToolsModel { Message = new SuccessMessage("Der User wurde gelöscht") });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult ReloadListFromIgnoreCrawlers()
    {
        if (Request.IsLocal)
        {
            IgnoreLog.LoadNewList();
            return View("Tools",
                new ToolsModel { Message = new SuccessMessage("Die Liste wird neu geladen.") });
        }

        return View("Tools",
            new ToolsModel { Message = new ErrorMessage("Sie sind nicht berechtigt die Liste neu zu laden.") });
    }

    public ActionResult ClearCache()
    {
        CacheClearer.Run();
        Session.Abandon();

        return View("Maintenance",
            new MaintenanceModel { Message = new SuccessMessage("Der Cache wurde geleert.") });
    }
    public ActionResult UpdateCategoryAuthors()
    {
        CategoryAuthorUpdater.UpdateAll();
        return View("Maintenance",
            new MaintenanceModel { Message = new SuccessMessage("Autoren von Kategorien wurden aktualisiert.") });
    }
}