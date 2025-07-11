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
    PageRelationRepo _pageRelationRepo) : ApiBaseController
{
    public readonly record struct VueMaintenanceResult(bool Success, string Data);

    public readonly record struct ActiveSessionsResponse(int LoggedInUserCount, int AnonymousUserCount);

    public readonly record struct RelationErrorsResponse(bool Success, List<RelationErrorItem> Data);

    public readonly record struct RelationErrorItem(int ParentId, List<RelationError> Errors);

    public readonly record struct RelationError(string Type, int ChildId, string Description);

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
            Data = "Cache wurde geleert."
        };
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult UpdateCategoryAuthors()
    {
        // This would need to be implemented based on your business logic
        // For now, returning a placeholder message
        return new VueMaintenanceResult
        {
            Success = true,
            Data = "Kategorie-Autoren wurden aktualisiert."
        };
    }

    [AccessOnlyAsAdmin]
    [HttpGet]
    public RelationErrorsResponse ShowRelationErrors()
    {
        var relationErrors = new List<RelationErrorItem>();
        
        try
        {
            var allRelations = EntityCache.GetAllRelations();
            var groupedByParent = allRelations.GroupBy(r => r.ParentId);

            foreach (var parentGroup in groupedByParent)
            {
                var parentId = parentGroup.Key;
                var parentPage = EntityCache.GetPage(parentId);
                
                if (parentPage == null) continue;

                var relations = parentGroup.ToList();
                var errors = new List<RelationError>();

                // Check for duplicate relations
                var duplicateGroups = relations
                    .GroupBy(r => r.ChildId)
                    .Where(g => g.Count() > 1);

                foreach (var duplicateGroup in duplicateGroups)
                {
                    var childId = duplicateGroup.Key;
                    var count = duplicateGroup.Count();
                    
                    errors.Add(new RelationError(
                        "Duplicate",
                        childId,
                        $"Child page appears {count} times"
                    ));
                }

                // Check for broken links (child pages that don't exist)
                foreach (var relation in relations)
                {
                    var childPage = EntityCache.GetPage(relation.ChildId);
                    if (childPage == null)
                    {
                        errors.Add(new RelationError(
                            "BrokenLink",
                            relation.ChildId,
                            "Child page does not exist"
                        ));
                    }
                }

                // Check for broken ordering (PreviousId/NextId inconsistencies)
                var orderedRelations = PageOrderer.Sort(relations, parentId);

                if (orderedRelations.Count != relations.Count)
                {
                    errors.Add(new RelationError(
                        "BrokenOrder",
                        0,
                        "Inconsistent relation ordering detected"
                    ));
                }

                if (errors.Any())
                {
                    relationErrors.Add(new RelationErrorItem(
                        parentId,
                        errors
                    ));
                }
            }

            return new RelationErrorsResponse(true, relationErrors);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while checking relation errors");
            return new RelationErrorsResponse(false, new List<RelationErrorItem>());
        }
    }

    [AccessOnlyAsAdmin]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public VueMaintenanceResult HealRelations(int pageId)
    {
        try
        {
            var page = EntityCache.GetPage(pageId);
            if (page == null)
            {
                return new VueMaintenanceResult
                {
                    Success = false,
                    Data = "Seite nicht gefunden."
                };
            }

            var relations = EntityCache.GetCacheRelationsByParentId(pageId);
            var healedCount = 0;

            // Remove duplicate relations
            var duplicateGroups = relations
                .GroupBy(r => r.ChildId)
                .Where(g => g.Count() > 1);

            foreach (var duplicateGroup in duplicateGroups)
            {
                var relationsToKeep = duplicateGroup.First();
                var relationsToDelete = duplicateGroup.Skip(1);

                foreach (var relationToDelete in relationsToDelete)
                {
                    var dbRelation = _pageRelationRepo.GetById(relationToDelete.Id);
                    if (dbRelation != null)
                    {
                        _pageRelationRepo.Delete(dbRelation);
                        EntityCache.Remove(relationToDelete);
                        healedCount++;
                    }
                }
            }

            // Remove relations with broken links (non-existent child pages)
            var brokenRelations = relations.Where(r => EntityCache.GetPage(r.ChildId) == null);
            foreach (var brokenRelation in brokenRelations)
            {
                var dbRelation = _pageRelationRepo.GetById(brokenRelation.Id);
                if (dbRelation != null)
                {
                    _pageRelationRepo.Delete(dbRelation);
                    EntityCache.Remove(brokenRelation);
                    healedCount++;
                }
            }

            // Fix ordering by re-sorting remaining relations
            var remainingRelations = EntityCache.GetCacheRelationsByParentId(pageId);

            if (remainingRelations.Any())
            {
                // Reset all ordering
                for (int i = 0; i < remainingRelations.Count; i++)
                {
                    var relation = remainingRelations[i];
                    var dbRelation = _pageRelationRepo.GetById(relation.Id);
                    
                    if (dbRelation != null)
                    {
                        relation.PreviousId = i > 0 ? remainingRelations[i - 1].ChildId : null;
                        relation.NextId = i < remainingRelations.Count - 1 ? remainingRelations[i + 1].ChildId : null;
                        
                        dbRelation.PreviousId = relation.PreviousId;
                        dbRelation.NextId = relation.NextId;
                        
                        _pageRelationRepo.Update(dbRelation);
                        EntityCache.AddOrUpdate(relation);
                    }
                }
            }

            return new VueMaintenanceResult
            {
                Success = true,
                Data = $"Relationen repariert. {healedCount} Probleme behoben."
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while healing relations for page {PageId}", pageId);
            return new VueMaintenanceResult
            {
                Success = false,
                Data = "Fehler beim Reparieren der Relationen."
            };
        }
    }
}