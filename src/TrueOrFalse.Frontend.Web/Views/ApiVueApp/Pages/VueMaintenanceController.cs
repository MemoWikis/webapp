using System;
using System.Linq;
using System.Security;
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

public class VueMaintenanceController : BaseController
{
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

    public VueMaintenanceController(
        SessionUser sessionUser,
        ProbabilityUpdate_ValuationAll probabilityUpdateValuationAll,
        ProbabilityUpdate_Question probabilityUpdateQuestion,
        MeiliSearchReIndexAllQuestions meiliSearchReIndexAllQuestions,
        UpdateQuestionAnswerCounts updateQuestionAnswerCounts,
        UpdateWishcount updateWishcount,
        MeiliSearchReIndexCategories meiliSearchReIndexCategories,
        MeiliSearchReIndexAllUsers meiliSearchReIndexAllUsers,
        CategoryRepository categoryRepository,
        AnswerRepo answerRepo,
        UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo,
        IAntiforgery antiforgery,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment) : base(sessionUser)
    {
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

    public readonly record struct VueMaintenanceResult(bool Success, string Data);

    [AccessOnlyAsAdmin]
    [HttpGet]
    public VueMaintenanceResult Get()
    {
        if (_sessionUser.IsInstallationAdmin)
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("X-CSRF-TOKEN", tokens.RequestToken,
                new CookieOptions { HttpOnly = true });

            return new VueMaintenanceResult
            {
                Success = true,
                Data = tokens.RequestToken
            };
        }

        throw new SecurityException("Not allowed");
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult RecalculateAllKnowledgeItems()
    {
        _probabilityUpdateValuationAll.Run();
        _probabilityUpdateQuestion.Run();

        new ProbabilityUpdate_Category(
                _categoryRepository,
                _answerRepo,
                _httpContextAccessor,
                _webHostEnvironment)
            .Run();

        ProbabilityUpdate_User.Initialize(
            _userReadingRepo,
            _userWritingRepo,
            _answerRepo,
            _httpContextAccessor,
            _webHostEnvironment);

        ProbabilityUpdate_User.Instance.Run();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Antwortwahrscheinlichkeiten wurden neu berechnet."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult CalcAggregatedValuesQuestions()
    {
        _updateQuestionAnswerCounts.Run();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Aggregierte Werte wurden aktualisiert."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult UpdateFieldQuestionCountForTopics()
    {
        _updateQuestionCountForCategory.All(_categoryRepository);

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Feld: AnzahlFragen für Themen wurde aktualisiert."
        };
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult UpdateUserReputationAndRankings()
    {
        _userWritingRepo.ReputationUpdateForAll();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Reputation and Rankings wurden aktualisiert."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult UpdateUserWishCount()
    {
        _updateWishcount.Run();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Wunschwissen-Antwortwahrscheinlichkeit wurde aktualisiert."
        };
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult DeleteUser(int userId)
    {
        _userWritingRepo.DeleteFromAllTables(userId);

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Der User wurde gelöscht"
        };
    }

    //todo: Remove when Meilisearch is active
    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> ReIndexAllQuestions()
    {
        await _meiliSearchReIndexAllQuestions.Go();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Fragen wurden neu indiziert."
        };
    }

    //todo: Remove when Meilisearch is active
    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> ReIndexAllTopics()
    {
        await _meiliSearchReIndexCategories.Go();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Themen wurden neu indiziert."
        };
    }

    //todo: Remove when Meilisearch is active
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> ReIndexAllUsers()
    {
        await _meiliSearchReIndexAllUsers.Run();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Nutzer wurden neu indiziert."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> MeiliReIndexAllQuestions()
    {
        await _meiliSearchReIndexAllQuestions.Go();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Fragen wurden neu indiziert."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> MeiliReIndexAllTopics()
    {
        await _meiliSearchReIndexCategories.Go();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Themen wurden neu indiziert."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> MeiliReIndexAllUsers()
    {
        await _meiliSearchReIndexAllUsers.Run();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Nutzer wurden neu indiziert."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult CheckForDuplicateInteractionNumbers()
    {
        var duplicates = _answerRepo.GetAll()
            .Where(a => a.QuestionViewGuid != Guid.Empty)
            .GroupBy(a => new { a.QuestionViewGuid, a.InteractionNumber })
            .Where(g => g.Skip(1).Any())
            .SelectMany(g => g)
            .ToList();

        var message = duplicates.Any() ? "Es gibt Dubletten." : "Es gibt keine Dubletten.";

        return new VueMaintenanceResult
        {
            Success = true,
            Data = message
        };
    }

    //Tools
    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult Throw500()
    {
        throw new Exception("Some random exception");
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult CleanUpWorkInProgressQuestions()
    {
        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Job: 'Cleanup work in progress' wird ausgeführt."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult ReloadListFromIgnoreCrawlers()
    {
        if (_httpContextAccessor.HttpContext.Request.IsLocal())
        {
            IgnoreLog.LoadNewList();

            return new VueMaintenanceResult
            {
                Success = true,
                Data = "Die Liste wird neu geladen."
            };
        }

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Sie sind nicht berechtigt die Liste neu zu laden."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult Start100TestJobs()
    {
        JobScheduler.StartImmediately<TestJobCacheInitializer>();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Started 100 test jobs."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult RemoveAdminRights()
    {
        _sessionUser.IsInstallationAdmin = false;

        return new VueMaintenanceResult
        {
            Success = true,
            Data = ""
        };
    }
}