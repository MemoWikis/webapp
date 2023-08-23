using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Tools;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace VueApp;

public class VueMaintenanceController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly ProbabilityUpdate_ValuationAll _probabilityUpdateValuationAll;
    private readonly ProbabilityUpdate_Question _probabilityUpdateQuestion;
    private readonly MeiliSearchReIndexAllQuestions _meiliSearchReIndexAllQuestions;
    private readonly UpdateQuestionAnswerCounts _updateQuestionAnswerCounts;
    private readonly UpdateQuestionCountForCategory _updateQuestionCountForCategory;
    private readonly UpdateWishcount _updateWishcount;
    private readonly MeiliSearchReIndexCategories _meiliSearchReIndexCategories;
    private readonly MeiliSearchReIndexAllUsers _meiliSearchReIndexAllUsers;
    private readonly CategoryRepository _categoryRepository;
    private readonly AnswerRepo _answerRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly IAntiforgery _antiforgery;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public VueMaintenanceController(SessionUser sessionUser,
        ProbabilityUpdate_ValuationAll probabilityUpdateValuationAll,
        ProbabilityUpdate_Question probabilityUpdateQuestion,
        MeiliSearchReIndexAllQuestions meiliSearchReIndexAllQuestions,
        UpdateQuestionAnswerCounts updateQuestionAnswerCounts,
        UpdateWishcount updateWishcount,
        MeiliSearchReIndexCategories meiliSearchReIndexCategories,
        MeiliSearchReIndexAllUsers meiliSearchReIndexAllUsers,
        CategoryRepository categoryRepository,
        AnswerRepo answerRepo,
        UserReadingRepo userReadingRepo, UserWritingRepo userWritingRepo,
        IAntiforgery antiforgery,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _sessionUser = sessionUser;
        _probabilityUpdateValuationAll = probabilityUpdateValuationAll;
        _probabilityUpdateQuestion = probabilityUpdateQuestion;
        _meiliSearchReIndexAllQuestions = meiliSearchReIndexAllQuestions;
        _updateQuestionAnswerCounts = updateQuestionAnswerCounts;
        _updateWishcount = updateWishcount;
        _meiliSearchReIndexCategories = meiliSearchReIndexCategories;
        _meiliSearchReIndexAllUsers = meiliSearchReIndexAllUsers;
        _categoryRepository = categoryRepository;
        _answerRepo = answerRepo;
        _userReadingRepo = userReadingRepo;
        _userWritingRepo = userWritingRepo;
        _antiforgery = antiforgery;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    [ValidateAntiForgeryToken]
    [AccessOnlyAsLoggedIn]
    [AccessOnlyAsAdmin]
    [HttpGet]
    public JsonResult Get()
    {
        if (_sessionUser.IsInstallationAdmin)
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("X-CSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = true });

            return new JsonResult(new { token = tokens.RequestToken, success = true });
        }

        return new JsonResult(new { error = "notAllowed", success = false });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public JsonResult RecalculateAllKnowledgeItems()
    {
        _probabilityUpdateValuationAll.Run();
        _probabilityUpdateQuestion.Run();
       new  ProbabilityUpdate_Category(_categoryRepository, _answerRepo, _httpContextAccessor, _webHostEnvironment).Run();
       ProbabilityUpdate_User.Initialize(_userReadingRepo, _userWritingRepo, _answerRepo, _httpContextAccessor,
           _webHostEnvironment);
       ProbabilityUpdate_User.Instance.Run();

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
        _updateQuestionAnswerCounts.Run();


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
        _updateQuestionCountForCategory.All(_categoryRepository);

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
        _userWritingRepo.ReputationUpdateForAll();

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
        _updateWishcount.Run();

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
        _userWritingRepo.DeleteFromAllTables(userId);

        return Json(new
        {
            success = true,
            data = "Der User wurde gelöscht"
        });
    }




    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<JsonResult> ReIndexAllQuestions()
    {
       await _meiliSearchReIndexAllQuestions.Go();

        return Json(new
        {
            success = true,
            data = "Fragen wurden neu indiziert."
        });
    }
    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<JsonResult> ReIndexAllTopics()
    {
        await _meiliSearchReIndexCategories.Go();

        return Json(new
        {
            success = true,
            data = "Themen wurden neu indiziert."
        });
    }
    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<JsonResult> ReIndexAllUsers()
    {
        await _meiliSearchReIndexAllUsers.Run();

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
        await _meiliSearchReIndexAllQuestions.Go();

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
        await _meiliSearchReIndexCategories.Go();

        return Json(new
        {
            success = true,
            data = "Themen wurden neu indiziert."
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<JsonResult> MeiliReIndexAllUsers()
    {
        await _meiliSearchReIndexAllUsers.Run();

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
        var duplicates = _answerRepo.GetAll()
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
        if (_httpContextAccessor.HttpContext.Request.IsLocal())
        {
            IgnoreLog.LoadNewList(_httpContextAccessor, _webHostEnvironment);

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