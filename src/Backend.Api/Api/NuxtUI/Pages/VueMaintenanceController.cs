using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using System.Security;

public class VueMaintenanceController(
    SessionUser _sessionUser,
    ProbabilityUpdate_ValuationAll _probabilityUpdateValuationAll,
    ProbabilityUpdate_Question _probabilityUpdateQuestion,
    MeilisearchReIndexAllQuestions _meilisearchReIndexAllQuestions,
    UpdateQuestionAnswerCounts _updateQuestionAnswerCounts,
    UpdateWishcount _updateWishCount,
    MeilisearchReIndexPages _meilisearchReIndexPages,
    MeilisearchReIndexUser _meilisearchReIndexUser,
    PageRepository pageRepository,
    AnswerRepo _answerRepo,
    UserReadingRepo _userReadingRepo,
    UserWritingRepo _userWritingRepo,
    IAntiforgery _antiforgery,
    IHttpContextAccessor _httpContextAccessor,
    IWebHostEnvironment _webHostEnvironment) : ApiBaseController
{
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

        new ProbabilityUpdate_Page(
                pageRepository,
                _answerRepo)
            .Run();

        ProbabilityUpdate_User.Initialize(
            _userReadingRepo,
            _userWritingRepo,
            _answerRepo);

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
        _updateWishCount.Run();

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

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> MeiliReIndexAllQuestions()
    {
        await _meilisearchReIndexAllQuestions.Run();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Fragen wurden neu indiziert."
        };
    }


    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> MeiliReIndexAllQuestionsCache()
    {
        await _meilisearchReIndexAllQuestions.RunCache();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Fragen wurden neu indiziert."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> MeiliReIndexAllPages()
    {
        await _meilisearchReIndexPages.Run();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Themen wurden neu indiziert."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> MeiliReIndexAllPagesCache()
    {
        await _meilisearchReIndexPages.RunCache();

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
        await _meilisearchReIndexUser.RunAll();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Nutzer wurden neu indiziert."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> MeiliReIndexAllUsersCache()
    {
        await _meilisearchReIndexUser.RunAllCache();

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