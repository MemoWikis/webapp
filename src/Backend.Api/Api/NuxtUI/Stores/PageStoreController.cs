using JetBrains.Annotations;


public class PageStoreController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    PageRepository _pageRepository,
    IHttpContextAccessor _httpContextAccessor,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    PageUpdater _pageUpdater,
    ImageStore _imageStore,
    AiUsageLogRepo _aiUsageLogRepo,
    TokenDeductionService _tokenDeductionService) : ApiBaseController
{
    public readonly record struct SaveContentRequest(
        int Id,
        string Content,
        [CanBeNull] string ShareToken,
        string[] CurrentImages);

    public readonly record struct SaveResult(bool Success, string MessageKey);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public SaveResult SaveContent([FromBody] SaveContentRequest request)
    {
        var pageCacheItem = EntityCache.GetPage(request.Id);

        if (pageCacheItem == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        if (pageCacheItem.Content?.Trim() == request.Content.Trim())
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Page.NoChange };

        if (!_permissionCheck.CanEdit(pageCacheItem, request.ShareToken))
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Page.MissingRights };

        var page = _pageRepository.GetByIdEager(request.Id);

        if (page == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        // Store previous images for comparison
        var previousImages = pageCacheItem.CurrentImageUrls ?? Array.Empty<string>();

        pageCacheItem.Content = request.Content;
        pageCacheItem.CurrentImageUrls = request.CurrentImages;
        page.Content = request.Content;

        EntityCache.AddOrUpdate(pageCacheItem);
        LanguageExtensions.SetContentLanguageOnAuthors(pageCacheItem.Id);
        _pageRepository.Update(page, _sessionUser.UserId, type: PageChangeType.Text);

        ImageCleanup.Schedule(request.Id, previousImages, request.CurrentImages);

        return new SaveResult { Success = true };
    }

    public readonly record struct SaveNameRequest(
        int Id,
        string Name,
        [CanBeNull] string ShareToken);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public SaveResult SaveName([FromBody] SaveNameRequest request)
    {
        var pageCacheItem = EntityCache.GetPage(request.Id);

        if (pageCacheItem == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        if (pageCacheItem.Name.Trim() == request.Name.Trim())
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Page.NoChange };

        if (!_permissionCheck.CanEdit(pageCacheItem, request.ShareToken))
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Page.MissingRights };

        var page = _pageRepository.GetByIdEager(request.Id);

        if (page == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        pageCacheItem.Name = request.Name.Trim();
        page.Name = request.Name.Trim();
        EntityCache.AddOrUpdate(pageCacheItem);
        LanguageExtensions.SetContentLanguageOnAuthors(pageCacheItem.Id);
        _pageRepository.Update(page, _sessionUser.UserId, type: PageChangeType.Renamed);

        return new SaveResult { Success = true };
    }

    [HttpGet]
    public string GetPageImageUrl([FromRoute] int id)
    {
        if (_permissionCheck.CanViewPage(id))
            return new PageImageSettings(id, _httpContextAccessor).GetUrl_128px(asSquare: true).Url;

        return "";
    }

    [HttpGet]
    public KnowledgeSummaryResponse GetUpdatedKnowledgeSummary([FromRoute] int id)
    {
        var sessionUserId = _sessionUser?.UserId ?? -1;
        var knowledgeSummary = _knowledgeSummaryLoader.RunFromCache(pageId: id, sessionUserId, maxCacheAgeInMinutes: 0);

        return new KnowledgeSummaryResponse(knowledgeSummary);
    }

    public readonly record struct GridPageItem(
        int Id,
        string Name,
        int QuestionCount,
        int ChildrenCount,
        string ImageUrl,
        PageVisibility Visibility,
        PageGridManager.TinyPageModel[] Parents,
        PageGridManager.KnowledgebarData KnowledgebarData,
        bool IsChildOfPersonalWiki,
        int CreatorId,
        bool CanDelete
    );

    public readonly record struct HideOrShowItem(bool hideText, int pageId);

    [HttpPost]
    public bool HideOrShowText([FromBody] HideOrShowItem hideOrShowItem) =>
        _pageUpdater.HideOrShowPageText(hideOrShowItem.hideText, hideOrShowItem.pageId);

    [HttpGet]
    public GridPageItem[] GetGridPageItems([FromRoute] int id)
    {
        var data = new PageGridManager(
            _permissionCheck,
            _sessionUser,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _knowledgeSummaryLoader,
            _questionReadingRepo).GetChildren(id);

        return data.Select(git => new GridPageItem
        {
            CanDelete = git.CanDelete,
            ChildrenCount = git.ChildrenCount,
            CreatorId = git.CreatorId,
            Id = git.Id,
            ImageUrl = git.ImageUrl,
            IsChildOfPersonalWiki = git.IsChildOfPersonalWiki,
            KnowledgebarData = git.KnowledgebarData,
            Name = git.Name,
            Visibility = git.Visibility,
            Parents = git.Parents,
            QuestionCount = git.QuestionCount
        }).ToArray();
    }

    public class UploadContentImageRequest
    {
        public int PageId { get; set; }
        public IFormFile File { get; set; }
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public string UploadContentImage([FromForm] UploadContentImageRequest form)
    {
        if (!_permissionCheck.CanEditPage(form.PageId))
            throw new Exception("No Upload rights");

        Log.Information("UploadContentImage {id}, {file}", form.PageId, form.File);

        var url = _imageStore.RunPageContentUploadAndGetPath(
            form.File,
            form.PageId,
            _sessionUser.UserId,
            _sessionUser.User.Name);

        return url;
    }

    public record struct PageAnalyticsResponse(
        List<DailyViews> ViewsPast90DaysAggregatedPages,
        List<DailyViews> ViewsPast90DaysPage,
        List<DailyViews> ViewsPast90DaysAggregatedQuestions,
        List<DailyViews> ViewsPast90DaysDirectQuestions);

    [HttpGet]
    public PageAnalyticsResponse GetPageAnalytics([FromRoute] int id)
    {
        var page = EntityCache.GetPage(id);

        var viewsPast90DaysPages = page.GetViewsOfPast90Days();
        var viewsPast90DaysAggregatedPages = GetAggregatedPageViewsOfPast90Days(id, viewsPast90DaysPages);
        var viewsPast90DaysAggregatedQuestions = GetAggregatedQuestionViewsOfPast90Days(page);
        var viewsPast90DaysQuestion = GetQuestionViewsOfPast90Days(page);

        return new PageAnalyticsResponse(
            viewsPast90DaysAggregatedPages,
            viewsPast90DaysPages,
            viewsPast90DaysAggregatedQuestions,
            viewsPast90DaysQuestion
        );
    }

    private List<DailyViews> GetAggregatedPageViewsOfPast90Days(int id, List<DailyViews> pageViews)
    {
        var descendants = GraphService.VisibleDescendants(id, _permissionCheck, _sessionUser.UserId);

        var aggregatedViewsOfPast90Days = descendants
            .SelectMany(descendant => descendant.GetViewsOfPast90Days())
            .Concat(pageViews)
            .GroupBy(view => view.Date)
            .Select(group => new DailyViews { Date = group.Key, Count = group.Sum(view => view.Count) })
            .OrderBy(view => view.Date)
            .ToList();

        return aggregatedViewsOfPast90Days;
    }

    private List<DailyViews> GetQuestionViews(IList<QuestionCacheItem> questions)
    {
        var result = questions
            .SelectMany(q => q.GetViewsOfPast90Days())
            .GroupBy(view => view.Date)
            .Select(group => new DailyViews { Date = group.Key, Count = group.Sum(view => view.Count) })
            .OrderBy(view => view.Date)
            .ToList();

        return result;
    }

    private List<DailyViews> GetQuestionViewsOfPast90Days(PageCacheItem page)
    {
        var questions = page.GetAggregatedQuestions(_sessionUser.UserId, onlyVisible: true, getQuestionsFromChildPages: false,
            pageId: page.Id, permissionCheck: _permissionCheck);
        return GetQuestionViews(questions);
    }

    private List<DailyViews> GetAggregatedQuestionViewsOfPast90Days(PageCacheItem page)
    {
        var questions = page.GetAggregatedQuestions(_sessionUser.UserId, onlyVisible: true, getQuestionsFromChildPages: true,
            pageId: page.Id, permissionCheck: _permissionCheck);
        return GetQuestionViews(questions);
    }

    public readonly record struct GenerateFlashCardRequest(int PageId, string Text, int? Count = 3);

    [HttpPost]
    [ItemCanBeNull]
    public async Task<GenerateFlashCardResponse?> GenerateFlashCard([FromBody] GenerateFlashCardRequest request)
    {
        if (!_permissionCheck.CanViewPage(request.PageId) || !_sessionUser.IsLoggedIn)
            return null;

        // Check if user has enough tokens for flashcard generation
        if (!_tokenDeductionService.CanAffordTokens(_sessionUser.UserId, request.Text, TokenDeductionService.GenerationType.Flashcards))
        {
            return new GenerateFlashCardResponse(new List<AiFlashCard.FlashCard>(), FrontendMessageKeys.Error.Ai.InsufficientTokens);
        }

        var limitCheck = new LimitCheck(_sessionUser);

        string? messageKey = null;

        if (!limitCheck.CanSavePrivateQuestion() &&
            EntityCache.GetPage(request.PageId).Visibility != PageVisibility.Public)
        {
            messageKey = FrontendMessageKeys.Error.Ai.NoFlashcardsCreatedCauseLimitAndPageIsPrivate;
            return new GenerateFlashCardResponse(new List<AiFlashCard.FlashCard>(), messageKey);
        }

        var aiFlashCard = new AiFlashCard(_aiUsageLogRepo);
        var flashcards =
            await aiFlashCard.Generate(request.Text, request.PageId, _sessionUser.UserId, _permissionCheck);

        if (flashcards.Count == 0)
            messageKey = FrontendMessageKeys.Error.Ai.GenerateFlashcards;
        else if (!limitCheck.CanSavePrivateQuestion())
            messageKey = FrontendMessageKeys.Info.Ai.FlashcardsCreatedWillBePublicCauseLimit;
        else if (limitCheck.NewPrivateQuestionsWillExceedLimit(flashcards.Count))
            messageKey = FrontendMessageKeys.Info.Ai.SomeFlashcardsCreatedWillBePublicCauseLimit;

        var response = new GenerateFlashCardResponse(flashcards, messageKey);

        return response;
    }

    public record struct GenerateFlashCardResponse(List<AiFlashCard.FlashCard> Flashcards, string? MessageKey);

    [HttpGet]
    public int GetQuestionCount([FromRoute] int id)
    {
        var page = EntityCache.GetPage(id);
        if (page != null)
            return page.GetCountQuestionsAggregated(_sessionUser.UserId);
        return 0;
    }


    [HttpGet]
    public bool GetIsShared([FromRoute] int id, [CanBeNull] string shareToken)
    {
        var canView = shareToken != null
            ? _permissionCheck.CanViewPage(id, shareToken)
            : _permissionCheck.CanViewPage(id);
        return canView && SharesService.IsShared(id);

    }
}