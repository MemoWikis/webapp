using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Tools;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace VueApp;

public class VueMaintenanceController : BaseController
{
    public VueMaintenanceController(SessionUser sessionUser):base(sessionUser)
    {
        
    }
    [AccessOnlyAsLoggedIn]
    [AccessOnlyAsAdmin]
    [HttpGet]
    public JsonResult Get()
    {
        if (_sessionUser.IsInstallationAdmin)
        {
            AntiForgery.GetTokens(null, out string cookieToken, out string formToken);
            HttpCookie antiForgeryCookie = new HttpCookie("__RequestVerificationToken");
            antiForgeryCookie.Value = cookieToken;
            antiForgeryCookie.HttpOnly = true;

            Response.Cookies.Add(antiForgeryCookie);
            return Fetch.Success(formToken, true);
        }

        return Fetch.Error("notAllowed", true);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult RecalculateAllKnowledgeItems()
    {
        ProbabilityUpdate_ValuationAll.Run();
        ProbabilityUpdate_Question.Run();
        ProbabilityUpdate_Category.Run();
        ProbabilityUpdate_User.Run();

        return Json(new
            {
                success = true,
                data = "Antwortwahrscheinlichkeiten wurden neu berechnet."
            });
    }


    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult CalcAggregatedValuesQuestions()
    {
        Resolve<UpdateQuestionAnswerCounts>().Run();


        return Json(new
        {
            success = true,
            data = "Aggregierte Werte wurden aktualisiert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult UpdateFieldQuestionCountForTopics()
    {
        Resolve<UpdateQuestionCountForCategory>().All();

        return Json(new
        {
            success = true,
            data = "Feld: AnzahlFragen für Themen wurde aktualisiert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult UpdateUserReputationAndRankings()
    {
        Resolve<ReputationUpdate>().RunForAll();

        return Json(new
        {
            success = true,
            data = "Reputation and Rankings wurden aktualisiert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult UpdateUserWishCount()
    {
        Resolve<UpdateWishcount>().Run();

        return Json(new
        {
            success = true,
            data = "Wunschwissen-Antwortwahrscheinlichkeit wurde aktualisiert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult DeleteUser(int userId)
    {
        Sl.UserRepo.DeleteFromAllTables(userId);

        return Json(new
        {
            success = true,
            data = "Der User wurde gelöscht"
        });
    }




    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult ReIndexAllQuestions()
    {
        Resolve<ReIndexAllQuestions>().Run();

        return Json(new
        {
            success = true,
            data = "Fragen wurden neu indiziert."
        });
    }
    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult ReIndexAllTopics()
    {
        Resolve<SolrReIndexAllCategories>().Run();

        return Json(new
        {
            success = true,
            data = "Themen wurden neu indiziert."
        });
    }
    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult ReIndexAllUsers()
    {
        Resolve<ReIndexAllUsers>().Run();

        return Json(new
        {
            success = true,
            data = "Nutzer wurden neu indiziert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<JsonResult> MeiliReIndexAllQuestions()
    {
        await Resolve<MeiliSearchReIndexAllQuestions>().Go();

        return Json(new
        {
            success = true,
            data = "Fragen wurden neu indiziert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<JsonResult> MeiliReIndexAllTopics()
    {
        await Resolve<MeiliSearchReIndexCategories>().Go();

        return Json(new
        {
            success = true,
            data = "Themen wurden neu indiziert.."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<JsonResult> MeiliReIndexAllUsers()
    {
        await Resolve<MeiliSearchReIndexAllUsers>().Run();

        return Json(new
        {
            success = true,
            data = "Nutzer wurden neu indiziert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult CheckForDuplicateInteractionNumbers()
    {
        var duplicates = Sl.R<AnswerRepo>().GetAll()
            .Where(a => a.QuestionViewGuid != Guid.Empty)
            .GroupBy(a => new { a.QuestionViewGuid, a.InteractionNumber })
            .Where(g => g.Skip(1).Any())
            .SelectMany(g => g)
            .ToList();

        var message = duplicates.Any() ? "Es gibt Dubletten." : "Es gibt keine Dubletten.";

        return Json(new
        {
            success = true,
            data = message
        });
    }

    //Tools

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult Throw500()
    {
        throw new Exception("Some random exception");
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult CleanUpWorkInProgressQuestions()
    {
        return Json(new
        {
            success = true,
            data = "Job: 'Cleanup work in progress' wird ausgeführt."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult ReloadListFromIgnoreCrawlers()
    {
        if (Request.IsLocal)
        {
            IgnoreLog.LoadNewList();

            return Json(new
            {
                success = true,
                data = "Die Liste wird neu geladen."
            });
        }

        return Json(new
        {
            success = true,
            data = "Sie sind nicht berechtigt die Liste neu zu laden."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult Start100TestJobs()
    {
        //for (var i = 0; i < 1000; i++)
        //    JobScheduler.StartImmediately<TestJob1>();

        //for (var i = 0; i < 1000; i++)
        //    JobScheduler.StartImmediately<TestJob2>();

        JobScheduler.StartImmediately<TestJobCacheInitializer>();

        return Json(new
        {
            success = true,
            data = "Started 100 test jobs."
        });

    }



    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult RemoveAdminRights()
    {
        _sessionUser.IsInstallationAdmin = false;

        return Json(new
        {
            success = true,
            data = ""
        });
    }
}