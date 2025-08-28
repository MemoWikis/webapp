public class PublishPageStoreController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    PageRepository pageRepository,
    QuestionReadingRepo _questionReadingRepo,
    QuestionWritingRepo _questionWritingRepo,
    ExtendedUserCache _extendedUserCache,
    SharesRepository _sharesRepository) : ApiBaseController
{
    public readonly record struct PublishPageRequest(int id);

    public readonly record struct PublishPageResponse(
        bool Success,
        string MessageKey,
        List<int> Data);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public PublishPageResponse PublishPage([FromBody] PublishPageRequest request)
    {
        var pageCacheItem = EntityCache.GetPage(request.id);

        if (pageCacheItem != null)
        {
            if (pageCacheItem.HasPublicParent() ||
                pageCacheItem.IsWiki)
            {
                if (pageCacheItem.Parents().Any(c => c.Id == 1) &&
                    !_sessionUser.IsInstallationAdmin)
                    return new PublishPageResponse
                    {
                        Success = false,
                        MessageKey = FrontendMessageKeys.Error.Page.ParentIsRoot
                    };

                pageCacheItem.Visibility = PageVisibility.Public;
                var page = pageRepository.GetById(request.id);
                page.Visibility = PageVisibility.Public;
                pageRepository.Update(page, _sessionUser.UserId,
                    type: PageChangeType.Published);

                SharesService.RemoveAllSharesForPage(pageCacheItem.Id, _sharesRepository);

                return new PublishPageResponse { Success = true, };
            }

            return new PublishPageResponse
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.ParentIsPrivate,
                Data = pageCacheItem.Parents().Select(c => c.Id).ToList()
            };
        }

        return new PublishPageResponse { Success = false, MessageKey = FrontendMessageKeys.Error.Default };
    }

    public readonly record struct PublishQuestionsRequest(List<int> questionIds);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public void PublishQuestions([FromBody] PublishQuestionsRequest request)
    {
        foreach (var questionId in request.questionIds)
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

    public readonly record struct GetResponse(
        bool Success,
        string Name,
        List<int> QuestionIds,
        int QuestionCount);

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public GetResponse Get([FromRoute] int id)
    {
        var pageCacheItem = EntityCache.GetPage(id);
        var userCacheItem = _extendedUserCache.GetItem(_sessionUser.UserId);

        if (pageCacheItem.Creator == null || pageCacheItem.Creator.Id != userCacheItem.Id)
            return new GetResponse { Success = false, };

        var filteredAggregatedQuestions = pageCacheItem
            .GetAggregatedQuestions(_sessionUser.UserId, permissionCheck: _permissionCheck)
            .Where(q =>
                q.Creator != null &&
                q.Creator.Id == userCacheItem.Id &&
                q.IsPrivate() &&
                _permissionCheck.CanEdit(q))
            .Select(q => q.Id).ToList();

        return new GetResponse
        {
            Success = true,
            Name = pageCacheItem.Name,
            QuestionIds = filteredAggregatedQuestions,
            QuestionCount = filteredAggregatedQuestions.Count
        };
    }
}