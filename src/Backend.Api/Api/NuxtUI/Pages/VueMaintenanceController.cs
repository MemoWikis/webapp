using Microsoft.AspNetCore.Antiforgery;
using System.Security;
public class VueMaintenanceController(
    SessionUser _sessionUser,
    UpdateWishcount _updateWishCount,
    IAntiforgery _antiforgery,
    IHttpContextAccessor _httpContextAccessor,
    RelationErrors _relationErrors,
    MeilisearchReIndexAllQuestions _meilisearchReIndexAllQuestions,
    MeilisearchReIndexPages _meilisearchReIndexPages,
    MeilisearchReIndexUser _meilisearchReIndexUser,
    UserWritingRepo _userWritingRepo,
    AnswerRepo _answerRepo,
    MmapCacheStatusService _mmapCacheStatusService) : ApiBaseController
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
        var jobId = JobTracking.CreateJob("RecalculateKnowledgeItems");

        JobScheduler.StartImmediately_RecalculateKnowledgeItems(jobId);

        return new VueMaintenanceResult
        {
            Success = true,
            Data = jobId
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult CalcAggregatedValuesQuestions()
    {
        var jobId = JobTracking.CreateJob("CalcAggregatedValues");

        JobScheduler.StartImmediately_CalcAggregatedValues(jobId);

        return new VueMaintenanceResult
        {
            Success = true,
            Data = jobId
        };
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult UpdateUserReputationAndRankings()
    {
        var jobId = JobTracking.CreateJob("UpdateUserReputation");

        JobScheduler.StartImmediately_UpdateUserReputation(jobId);

        return new VueMaintenanceResult
        {
            Success = true,
            Data = jobId
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
    public VueMaintenanceResult MeiliReIndexAllQuestions()
    {
        var jobId = JobTracking.CreateJob("MeiliReIndexQuestions");

        JobScheduler.StartImmediately_ReindexAllQuestions(jobId);

        return new VueMaintenanceResult
        {
            Success = true,
            Data = jobId
        };
    }


    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> MeiliReIndexAllQuestionsCache()
    {
        try
        {
            await _meilisearchReIndexAllQuestions.RunCache();

            return new VueMaintenanceResult
            {
                Success = true,
                Data = "Questions cache has been re-indexed successfully."
            };
        }
        catch (Exception ex)
        {
            return new VueMaintenanceResult
            {
                Success = false,
                Data = $"Error: {ex.Message}"
            };
        }
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult MeiliReIndexAllPages()
    {
        var jobId = JobTracking.CreateJob("MeiliReIndexPages");

        JobScheduler.StartImmediately_MeiliReIndexPages(jobId);

        return new VueMaintenanceResult
        {
            Success = true,
            Data = jobId
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> MeiliReIndexAllPagesCache()
    {
        try
        {
            await _meilisearchReIndexPages.RunCache();

            return new VueMaintenanceResult
            {
                Success = true,
                Data = "Pages cache has been re-indexed successfully."
            };
        }
        catch (Exception ex)
        {
            return new VueMaintenanceResult
            {
                Success = false,
                Data = $"Error: {ex.Message}"
            };
        }
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult MeiliReIndexAllUsers()
    {
        var jobId = JobTracking.CreateJob("MeiliReIndexUsers");

        JobScheduler.StartImmediately_MeiliReIndexUsers(jobId);

        return new VueMaintenanceResult
        {
            Success = true,
            Data = jobId
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<VueMaintenanceResult> MeiliReIndexAllUsersCache()
    {
        try
        {
            await _meilisearchReIndexUser.RunAllCache();

            return new VueMaintenanceResult
            {
                Success = true,
                Data = "Users cache has been re-indexed successfully."
            };
        }
        catch (Exception ex)
        {
            return new VueMaintenanceResult
            {
                Success = false,
                Data = $"Error: {ex.Message}"
            };
        }
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
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult PollingTest5s()
    {
        var jobId = JobTracking.CreateJob("PollingTest5s");

        _ = Task.Run(async () =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Starting 5-second polling test...", "Polling Test 5s");
                await Task.Delay(1000);

                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Test running (20% complete)...", "Polling Test 5s");
                await Task.Delay(1000);

                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Test running (40% complete)...", "Polling Test 5s");
                await Task.Delay(1000);

                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Test running (60% complete)...", "Polling Test 5s");
                await Task.Delay(1000);

                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Test running (80% complete)...", "Polling Test 5s");
                await Task.Delay(1000);

                JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "5-second polling test completed successfully!", "Polling Test 5s");
            }
            catch (Exception ex)
            {
                JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Test failed: {ex.Message}", "Polling Test 5s");
            }
        });

        return new VueMaintenanceResult
        {
            Success = true,
            Data = jobId
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult PollingTest30s()
    {
        var jobId = JobTracking.CreateJob("PollingTest30s");

        _ = Task.Run(async () =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Starting 30-second polling test...", "Polling Test 30s");

                for (int i = 1; i <= 30; i++)
                {
                    JobTracking.UpdateJobStatus(jobId, JobStatus.Running, $"Test running ({i}/30 seconds)...", "Polling Test 30s");
                    await Task.Delay(1000);
                }

                JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "30-second polling test completed successfully!", "Polling Test 30s");
            }
            catch (Exception ex)
            {
                JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Test failed: {ex.Message}", "Polling Test 30s");
            }
        });

        return new VueMaintenanceResult
        {
            Success = true,
            Data = jobId
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult PollingTest120s()
    {
        var jobId = JobTracking.CreateJob("PollingTest120s");

        _ = Task.Run(async () =>
        {
            try
            {
                JobTracking.UpdateJobStatus(jobId, JobStatus.Running, "Starting 120-second polling test...", "Polling Test 120s");

                for (int i = 1; i <= 120; i++)
                {
                    var phase = i <= 40 ? "Phase 1" : i <= 80 ? "Phase 2" : "Phase 3";
                    JobTracking.UpdateJobStatus(jobId, JobStatus.Running, $"{phase}: Test running ({i}/120 seconds)...", "Polling Test 120s");
                    await Task.Delay(1000);
                }

                JobTracking.UpdateJobStatus(jobId, JobStatus.Completed, "120-second polling test completed successfully!", "Polling Test 120s");
            }
            catch (Exception ex)
            {
                JobTracking.UpdateJobStatus(jobId, JobStatus.Failed, $"Test failed: {ex.Message}", "Polling Test 120s");
            }
        });

        return new VueMaintenanceResult
        {
            Success = true,
            Data = jobId
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public JobStatusResponse GetJobStatus(string jobId)
    {
        return JobTracking.GetJobStatus(jobId);
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IEnumerable<JobStatusResponse> GetAllRunningJobs()
    {
        return JobTracking.GetAllActiveJobs();
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult ClearJob([FromForm] string jobId)
    {
        var success = JobTracking.ClearJob(jobId);

        return new VueMaintenanceResult
        {
            Success = success,
            Data = success ? "Job cleared successfully." : "Job not found or could not be cleared."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult ClearAllJobs()
    {
        var clearedCount = JobTracking.ClearAllJobs();

        return new VueMaintenanceResult
        {
            Success = true,
            Data = $"Cleared {clearedCount} job(s)."
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
    [ValidateAntiForgeryToken]
    [HttpPost]
    public RelationErrorsResult GetRelationErrors()
    {
        // First check if we have cached results from async analysis
        if (RelationErrorsCache.HasCachedResults())
        {
            var cachedResults = RelationErrorsCache.GetCachedResults();
            if (cachedResults.HasValue)
            {
                var cacheTime = RelationErrorsCache.GetCacheTimestamp();
                Log.Information("Returning cached relation errors from {CacheTime}", cacheTime);
                return cachedResults.Value;
            }
        }

        // Fall back to immediate analysis
        Log.Information("No cached results available, performing immediate relation error analysis");
        return _relationErrors.GetErrors();
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult StartRelationAnalysis()
    {
        var jobId = JobTracking.CreateJob("RelationErrorAnalysis");

        JobScheduler.StartImmediately_RelationErrorAnalysis(jobId);

        return new VueMaintenanceResult
        {
            Success = true,
            Data = jobId
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult ClearRelationErrorsCache()
    {
        RelationErrorsCache.ClearCache();
        Log.Information("Relation errors cache cleared");

        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Relation errors cache has been cleared."
        };
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
    public VueMaintenanceResult RefreshMmapCaches()
    {
        var jobId = JobTracking.CreateJob("RefreshMmapCaches");

        JobScheduler.StartImmediately_MmapCacheRefresh(jobId);

        return new VueMaintenanceResult
        {
            Success = true,
            Data = jobId
        };
    }

    [AccessOnlyAsAdmin]
    [HttpPost]
    public VueMaintenanceResult GetMmapCacheStatus()
    {
        return new VueMaintenanceResult
        {
            Success = true,
            Data = _mmapCacheStatusService.GetCacheStatusAsJson()
        };
    }
}