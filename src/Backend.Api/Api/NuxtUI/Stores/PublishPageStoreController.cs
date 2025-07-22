public class PublishPageStoreController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    PageRepository pageRepository,
    QuestionReadingRepo _questionReadingRepo,
    QuestionWritingRepo _questionWritingRepo,
    LoggedInUserCache _loggedInUserCache,
    SharesRepository _sharesRepository) : ApiBaseController
{
    public readonly record struct PublishPageJson(int id);

    public readonly record struct PublishPageResult(
        bool Success,
        string MessageKey,
        List<int> Data);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public PublishPageResult PublishPage([FromBody] PublishPageJson json)
    {
        var pageCacheItem = EntityCache.GetPage(json.id);

        if (pageCacheItem != null)
        {
            if (pageCacheItem.HasPublicParent() ||
                pageCacheItem.Creator.FirstWikiId == json.id)
            {
                if (pageCacheItem.Parents().Any(c => c.Id == 1) &&
                    !_sessionUser.IsInstallationAdmin)
                    return new PublishPageResult
                    {
                        Success = false,
                        MessageKey = FrontendMessageKeys.Error.Page.ParentIsRoot
                    };

                pageCacheItem.Visibility = PageVisibility.Public;
                var page = pageRepository.GetById(json.id);
                page.Visibility = PageVisibility.Public;
                pageRepository.Update(page, _sessionUser.UserId,
                    type: PageChangeType.Published);

                SharesService.RemoveAllSharesForPage(pageCacheItem.Id, _sharesRepository);

                return new PublishPageResult
                {
                    Success = true,
                };
            }

            return new PublishPageResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.ParentIsPrivate,
                Data = pageCacheItem.Parents().Select(c => c.Id).ToList()
            };
        }

        return new PublishPageResult
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.Default
        };
    }

    public readonly record struct PublishQuestionsJson(List<int> questionIds);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public void PublishQuestions([FromBody] PublishQuestionsJson json)
    {
        foreach (var questionId in json.questionIds)
        {
            var questionCacheItem =
                EntityCache.GetQuestionById(questionId);
            if (questionCacheItem.Creator.Id == _sessionUser.User.Id)
            {
                questionCacheItem.Visibility = QuestionVisibility.Public;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = _questionReadingRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.Public;
                _questionWritingRepo.UpdateOrMerge(question, false);
            }
        }
    }

    public readonly record struct TinyPage(
        bool Success,
        string Name,
        List<int> QuestionIds,
        int QuestionCount);

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public TinyPage Get([FromRoute] int id)
    {
        var pageCacheItem = EntityCache.GetPage(id);
        var userCacheItem = _loggedInUserCache.GetItem(_sessionUser.UserId);

        if (pageCacheItem.Creator == null || pageCacheItem.Creator.Id != userCacheItem.Id)
            return new TinyPage
            {
                Success = false,
            };

        var filteredAggregatedQuestions = pageCacheItem
            .GetAggregatedQuestions(_sessionUser.UserId, permissionCheck: _permissionCheck)
            .Where(q =>
                q.Creator != null &&
                q.Creator.Id == userCacheItem.Id &&
                q.IsPrivate() &&
                _permissionCheck.CanEdit(q))
            .Select(q => q.Id).ToList();

        return new TinyPage
        {
            Success = true,
            Name = pageCacheItem.Name,
            QuestionIds = filteredAggregatedQuestions,
            QuestionCount = filteredAggregatedQuestions.Count
        };
    }
}