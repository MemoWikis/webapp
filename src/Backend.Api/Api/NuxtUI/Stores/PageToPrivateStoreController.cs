public class PageToPrivateStoreController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    PageRepository pageRepository,
    QuestionReadingRepo _questionReadingRepo,
    QuestionWritingRepo _questionWritingRepo,
    ExtendedUserCache _extendedUserCache) : ApiBaseController
{
    public readonly record struct Page(
        string Name,
        List<int> PersonalQuestionIds,
        int PersonalQuestionCount,
        List<int> AllQuestionIds,
        int AllQuestionCount
    );

    public readonly record struct GetResult(bool Success, string MessageKey, object Data);

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public GetResult Get([FromRoute] int id)
    {
        var pageCacheItem = EntityCache.GetPage(id);
        var userCacheItem = _extendedUserCache.GetItem(_sessionUser.UserId);
        if (pageCacheItem == null)
            return new GetResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };

        if (!_permissionCheck.CanEdit(pageCacheItem))
            return new GetResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.MissingRights
            };

        var aggregatedPages = pageCacheItem.VisibleAggregatedPages(_permissionCheck)
            .Where(c => c.Value.Visibility == PageVisibility.Public);
        var publicAggregatedQuestions = pageCacheItem
            .GetAggregatedQuestions(_sessionUser.UserId, true, permissionCheck: _permissionCheck)
            .Where(q => q.Visibility == QuestionVisibility.Public).ToList();

        var pinCount = pageCacheItem.TotalRelevancePersonalEntries;

        if (!_sessionUser.IsInstallationAdmin)
        {
            if (id == FeaturedPage.RootPageId)
                return new GetResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.RootPageMustBePublic
                };

            foreach (var c in aggregatedPages)
            {
                var parents = c.Value.Parents();
                bool childHasPublicParent = parents.Any(p =>
                    p.Visibility == PageVisibility.Public && p.Id != id);

                if (!childHasPublicParent && parents.Any(p => p.Id != id))
                    return new GetResult
                    {
                        Success = false,
                        MessageKey = FrontendMessageKeys.Error.Page.PublicChildPages
                    };
            }

            var pinnedQuestionIds = new List<int>();
            foreach (var q in publicAggregatedQuestions)
            {
                bool questionIsPinned = q.TotalRelevanceForAllEntries > 0;
                if (questionIsPinned)
                    pinnedQuestionIds.Add(q.Id);
            }

            if (pinnedQuestionIds.Count > 0)
                return new GetResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.PinnedQuestions,
                    Data = pinnedQuestionIds
                };

            if (pinCount >= 10)
            {
                return new GetResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.TooPopular
                };
            }
        }

        var filteredAggregatedQuestions = publicAggregatedQuestions
            .Where(q => q.Creator != null && q.Creator.Id == userCacheItem.Id)
            .Select(q => q.Id).ToList();

        return new GetResult
        {
            Success = true,
            Data = new Page
            {
                Name = pageCacheItem.Name,
                PersonalQuestionIds = filteredAggregatedQuestions,
                PersonalQuestionCount = filteredAggregatedQuestions.Count,
                AllQuestionIds = publicAggregatedQuestions.Select(q => q.Id).ToList(),
                AllQuestionCount = publicAggregatedQuestions.Count
            }
        };
    }

    public readonly record struct SetResult(bool Success, string MessageKey);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public SetResult Set([FromRoute] int id)
    {
        var pageCacheItem = EntityCache.GetPage(id);
        if (pageCacheItem == null)
            return new SetResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };

        if (!_permissionCheck.CanEdit(pageCacheItem))
            return new SetResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.MissingRights
            };

        var page = pageRepository.GetById(id);
        var pinCount = page.TotalRelevancePersonalEntries;
        if (!_sessionUser.IsInstallationAdmin)
        {
            if (id == FeaturedPage.RootPageId)
                return new SetResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.RootPageMustBePublic
                };

            var aggregatedPages = pageCacheItem.VisibleAggregatedPages(_permissionCheck, false)
                .Where(c => c.Value.Visibility == PageVisibility.Public);

            foreach (var c in aggregatedPages)
            {
                var parents = c.Value.Parents();
                bool childHasPublicParent = parents.Any(p =>
                    p.Visibility == PageVisibility.Public && p.Id != id);

                if (!childHasPublicParent && parents.Any(p => p.Id != id))
                    return new SetResult
                    {
                        Success = false,
                        MessageKey = FrontendMessageKeys.Error.Page.PublicChildPages
                    };
            }

            if (pinCount >= 10)
            {
                return new SetResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.TooPopular
                };
            }
        }

        pageCacheItem.Visibility = PageVisibility.Private;
        page.Visibility = PageVisibility.Private;
        pageRepository.Update(page, _sessionUser.UserId, type: PageChangeType.Privatized);

        return new SetResult
        {
            Success = true,
            MessageKey = FrontendMessageKeys.Success.Page.SetToPrivate
        };
    }

    public readonly record struct SetQuestionsToPrivateJson(List<int> questionIds);

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public void SetQuestionsToPrivate([FromBody] SetQuestionsToPrivateJson json)
    {
        foreach (var questionId in json.questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId);
            var otherUsersHaveQuestionInWishknowledge =
                questionCacheItem.TotalRelevancePersonalEntries >
                (questionCacheItem.IsInWishknowledge(_sessionUser.UserId, _extendedUserCache)
                    ? 1
                    : 0);
            if ((questionCacheItem.Creator.Id == _sessionUser.UserId &&
                 !otherUsersHaveQuestionInWishknowledge) ||
                _sessionUser.IsInstallationAdmin)
            {
                questionCacheItem.Visibility = QuestionVisibility.Private;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = _questionReadingRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.Private;
                _questionWritingRepo.UpdateOrMerge(question, false);
            }
        }
    }
}