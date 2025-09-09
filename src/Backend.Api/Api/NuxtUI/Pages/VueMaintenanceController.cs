using Microsoft.AspNetCore.Antiforgery;
using System.Security;
using System.Text.Json;
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
    RelationErrors _relationErrors,
    MmapCacheRefreshService _mmapCacheRefreshService,
    PageViewMmapCache _pageViewMmapCache,
    QuestionViewMmapCache _questionViewMmapCache) : ApiBaseController
{
    public readonly record struct VueMaintenanceResult(bool Success, string Data);

    public readonly record struct ActiveSessionsResponse(int LoggedInUserCount, int AnonymousUserCount);

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
            Data = "Answer probabilities have been recalculated."
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
            Data = "Aggregated values have been updated."
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
            Data = "Reputation and rankings have been updated."
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
            Data = "Wish knowledge answer probability has been updated."
        };
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult DeleteUser([FromForm] int userId)
    {
        _userWritingRepo.DeleteFromAllTables(userId);

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "The user has been deleted."
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
            Data = "Questions have been re-indexed."
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
            Data = "Questions have been re-indexed."
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
            Data = "Pages have been re-indexed."
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
            Data = "Pages have been re-indexed."
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
            Data = "Users have been re-indexed."
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
            Data = "Users have been re-indexed."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult CheckForDuplicateInteractionNumbers()
    {
        var duplicates = _answerRepo.GetAll()
            .Where(answer => answer.QuestionViewGuid != Guid.Empty)
            .GroupBy(answer => new { answer.QuestionViewGuid, answer.InteractionNumber })
            .Where(group => group.Skip(1).Any())
            .SelectMany(group => group)
            .ToList();

        var message = duplicates.Any() ? "There are duplicates." : "There are no duplicates.";

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
            Data = "Job: 'Cleanup work in progress' is being executed."
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
                Data = "The list is being reloaded."
            };
        }

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "You are not authorized to reload the list."
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
    [HttpGet]
    public ActiveSessionsResponse GetActiveSessions()
    {
        var loggedInUserCount = LoggedInSessionStore.GetLoggedInUsersActiveWithin(TimeSpan.FromMinutes(5));
        var anonymousCount = LoggedInSessionStore.GetAnonymousActiveWithin(TimeSpan.FromMinutes(1));
        return new ActiveSessionsResponse(loggedInUserCount, anonymousCount);
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

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult ClearCache()
    {
        // Clear various caches
        EntityCache.Clear();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Cache has been cleared."
        };
    }

    [AccessOnlyAsAdmin]
    [HttpGet]
    public RelationErrorsResult ShowRelationErrors()
    {
        return _relationErrors.GetErrors();
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult HealRelations([FromForm] int pageId)
    {
        var result = _relationErrors.HealErrors(pageId);

        return new VueMaintenanceResult
        {
            Success = result.Success,
            Data = result.Message
        };
    }

    // Mmap Cache Methods
    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> RefreshMmapCaches()
    {
        await _mmapCacheRefreshService.TriggerManualRefresh();
        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Mmap caches refreshed successfully"
        };
    }

    [AccessOnlyAsAdmin]
    [HttpPost]
    public VueMaintenanceResult GetMmapCacheStatus()
    {
        var cacheDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "viewcache");
        var pageViewsFile = Path.Combine(cacheDirectory, "pageviews.mmap");
        var questionViewsFile = Path.Combine(cacheDirectory, "questionviews.mmap");

        var status = new
        {
            pageViewsCache = new
            {
                exists = System.IO.File.Exists(pageViewsFile),
                lastModified = System.IO.File.Exists(pageViewsFile) ? System.IO.File.GetLastWriteTime(pageViewsFile) : (DateTime?)null,
                sizeBytes = System.IO.File.Exists(pageViewsFile) ? new FileInfo(pageViewsFile).Length : 0
            },
            questionViewsCache = new
            {
                exists = System.IO.File.Exists(questionViewsFile),
                lastModified = System.IO.File.Exists(questionViewsFile) ? System.IO.File.GetLastWriteTime(questionViewsFile) : (DateTime?)null,
                sizeBytes = System.IO.File.Exists(questionViewsFile) ? new FileInfo(questionViewsFile).Length : 0
            }
        };

        return new VueMaintenanceResult
        {
            Success = true,
            Data = JsonSerializer.Serialize(status)
        };
    }
}