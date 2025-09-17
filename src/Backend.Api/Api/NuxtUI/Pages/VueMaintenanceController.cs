using Microsoft.AspNetCore.Antiforgery;
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
    RelationErrors _relationErrors) : ApiBaseController
{
    public readonly record struct VueMaintenanceResult(bool Success, string Data);

    public readonly record struct ActiveSessionsResponse(int LoggedInUserCount, int AnonymousUserCount);

    public readonly record struct JobStatusResponse(string JobId, string Status, string Message, int Progress, string OperationName);

    private static readonly Dictionary<string, JobStatusResponse> _jobStatuses = new();

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
        var jobId = Guid.NewGuid().ToString();
        
        _ = Task.Run(() =>
        {
            try
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Starting knowledge items recalculation...", 0, "RecalculateAllKnowledgeItems");
                
                _probabilityUpdateValuationAll.Run();
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Updating question probabilities...", 25, "RecalculateAllKnowledgeItems");
                
                _probabilityUpdateQuestion.Run();
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Updating page probabilities...", 50, "RecalculateAllKnowledgeItems");

                new ProbabilityUpdate_Page(
                        pageRepository,
                        _answerRepo)
                    .Run();
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Initializing user probability updates...", 75, "RecalculateAllKnowledgeItems");

                ProbabilityUpdate_User.Initialize(
                    _userReadingRepo,
                    _userWritingRepo,
                    _answerRepo);

                ProbabilityUpdate_User.Instance.Run();
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "completed", "Answer probabilities have been recalculated.", 100, "RecalculateAllKnowledgeItems");
            }
            catch (Exception ex)
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "failed", $"Error: {ex.Message}", 0, "RecalculateAllKnowledgeItems");
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
    public VueMaintenanceResult CalcAggregatedValuesQuestions()
    {
        var jobId = Guid.NewGuid().ToString();
        
        _ = Task.Run(() =>
        {
            try
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Calculating aggregated values for questions...", 0, "CalcAggregatedValuesQuestions");
                
                _updateQuestionAnswerCounts.Run();
                
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "completed", "Aggregated values have been updated.", 100, "CalcAggregatedValuesQuestions");
            }
            catch (Exception ex)
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "failed", $"Error: {ex.Message}", 0, "CalcAggregatedValuesQuestions");
            }
        });

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
        var jobId = Guid.NewGuid().ToString();
        
        _ = Task.Run(() =>
        {
            try
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Updating user reputation and rankings...", 0, "UpdateUserReputationAndRankings");
                
                _userWritingRepo.ReputationUpdateForAll();
                
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "completed", "Reputation and rankings have been updated.", 100, "UpdateUserReputationAndRankings");
            }
            catch (Exception ex)
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "failed", $"Error: {ex.Message}", 0, "UpdateUserReputationAndRankings");
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
        var jobId = Guid.NewGuid().ToString();
        
        _ = Task.Run(async () =>
        {
            try
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Re-indexing all questions...", 0, "MeiliReIndexAllQuestions");
                
                await _meilisearchReIndexAllQuestions.Run();
                
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "completed", "Questions have been re-indexed.", 100, "MeiliReIndexAllQuestions");
            }
            catch (Exception ex)
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "failed", $"Error: {ex.Message}", 0, "MeiliReIndexAllQuestions");
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
    public VueMaintenanceResult MeiliReIndexAllQuestionsCache()
    {
        var jobId = Guid.NewGuid().ToString();
        
        _ = Task.Run(async () =>
        {
            try
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Re-indexing all questions cache...", 0, "Reindex Questions Cache");
                
                await _meilisearchReIndexAllQuestions.RunCache();
                
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "completed", "Questions have been re-indexed.", 100, "Reindex Questions Cache");
            }
            catch (Exception ex)
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "failed", $"Error: {ex.Message}", 0, "Reindex Questions Cache");
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
    public VueMaintenanceResult MeiliReIndexAllPages()
    {
        var jobId = Guid.NewGuid().ToString();
        
        _ = Task.Run(async () =>
        {
            try
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Re-indexing all pages...", 0, "Reindex Pages");
                
                await _meilisearchReIndexPages.Run();
                
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "completed", "Pages have been re-indexed.", 100, "Reindex Pages");
            }
            catch (Exception ex)
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "failed", $"Error: {ex.Message}", 0, "Reindex Pages");
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
    public VueMaintenanceResult MeiliReIndexAllPagesCache()
    {
        var jobId = Guid.NewGuid().ToString();
        
        _ = Task.Run(async () =>
        {
            try
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Re-indexing all pages cache...", 0, "Reindex Pages Cache");
                
                await _meilisearchReIndexPages.RunCache();
                
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "completed", "Pages have been re-indexed.", 100, "Reindex Pages Cache");
            }
            catch (Exception ex)
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "failed", $"Error: {ex.Message}", 0, "Reindex Pages Cache");
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
    public VueMaintenanceResult MeiliReIndexAllUsers()
    {
        var jobId = Guid.NewGuid().ToString();
        
        _ = Task.Run(async () =>
        {
            try
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Re-indexing all users...", 0, "Reindex Users");
                
                await _meilisearchReIndexUser.RunAll();
                
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "completed", "Users have been re-indexed.", 100, "Reindex Users");
            }
            catch (Exception ex)
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "failed", $"Error: {ex.Message}", 0, "Reindex Users");
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
    public VueMaintenanceResult MeiliReIndexAllUsersCache()
    {
        var jobId = Guid.NewGuid().ToString();
        
        _ = Task.Run(async () =>
        {
            try
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "running", "Re-indexing all users cache...", 0, "Reindex Users Cache");
                
                await _meilisearchReIndexUser.RunAllCache();
                
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "completed", "Users have been re-indexed.", 100, "Reindex Users Cache");
            }
            catch (Exception ex)
            {
                _jobStatuses[jobId] = new JobStatusResponse(jobId, "failed", $"Error: {ex.Message}", 0, "Reindex Users Cache");
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
    public JobStatusResponse GetJobStatus(string jobId)
    {
        if (_jobStatuses.TryGetValue(jobId, out var status))
        {
            // Clean up completed jobs after retrieval
            if (status.Status == "completed" || status.Status == "failed")
            {
                _jobStatuses.Remove(jobId);
            }
            return status;
        }
        
        return new JobStatusResponse(jobId, "not_found", "Job not found", 0, "Unknown");
    }

    [AccessOnlyAsAdmin]
    [HttpGet]
    public IEnumerable<JobStatusResponse> GetAllRunningJobs()
    {
        // Return only running jobs, clean up completed/failed ones
        var runningJobs = new List<JobStatusResponse>();
        var jobsToRemove = new List<string>();
        
        foreach (var kvp in _jobStatuses)
        {
            if (kvp.Value.Status == "completed" || kvp.Value.Status == "failed")
            {
                jobsToRemove.Add(kvp.Key);
            }
            else
            {
                runningJobs.Add(kvp.Value);
            }
        }
        
        // Clean up completed/failed jobs
        foreach (var jobId in jobsToRemove)
        {
            _jobStatuses.Remove(jobId);
        }
        
        return runningJobs;
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
}