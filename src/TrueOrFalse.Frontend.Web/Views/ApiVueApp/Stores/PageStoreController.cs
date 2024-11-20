using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VueApp;

public class PageStoreController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    PageRepository pageRepository,
    IHttpContextAccessor _httpContextAccessor,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    PageUpdater pageUpdater,
    ImageStore _imageStore) : Controller
{
    public readonly record struct SaveContentRequest(
        int Id,
        string Content);

    public readonly record struct SaveResult(bool Success, string MessageKey);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public SaveResult SaveContent([FromBody] SaveContentRequest req)
    {
        var pageCacheItem = EntityCache.GetPage(req.Id);

        if (pageCacheItem == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        if (pageCacheItem.Content.Trim() == req.Content.Trim())
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Page.NoChange };

        if (!_permissionCheck.CanEdit(pageCacheItem))
            return new SaveResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.MissingRights
            };

        var page = pageRepository.GetByIdEager(req.Id);

        if (page == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        pageCacheItem.Content = req.Content;
        page.Content = req.Content;
        pageRepository.Update(page, _sessionUser.UserId, type: PageChangeType.Text);

        EntityCache.AddOrUpdate(pageCacheItem);

        return new SaveResult
        {
            Success = true
        };
    }

    public readonly record struct SaveNameRequest(
        int Id,
        string Name);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public SaveResult SaveName([FromBody] SaveNameRequest req)
    {
        var pageCacheItem = EntityCache.GetPage(req.Id);

        if (pageCacheItem == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        if (pageCacheItem.Name.Trim() == req.Name.Trim())
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Page.NoChange };

        if (!_permissionCheck.CanEdit(pageCacheItem))
            return new SaveResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.MissingRights
            };

        var page = pageRepository.GetByIdEager(req.Id);

        if (page == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        pageCacheItem.Name = req.Name.Trim();
        page.Name = req.Name.Trim();
        pageRepository.Update(page, _sessionUser.UserId, type: PageChangeType.Renamed);

        EntityCache.AddOrUpdate(pageCacheItem);

        return new SaveResult
        {
            Success = true
        };
    }

    [HttpGet]
    public string GetPageImageUrl([FromRoute] int id)
    {
        if (_permissionCheck.CanViewPage(id))
            return new PageImageSettings(id, _httpContextAccessor).GetUrl_128px(asSquare: true)
                .Url;

        return "";
    }

    public readonly record struct KnowledgeSummaryResult(
        int NotLearned,
        int NeedsLearning,
        int NeedsConsolidation,
        int Solid);

    [HttpGet]
    public KnowledgeSummaryResult GetUpdatedKnowledgeSummary([FromRoute] int id)
    {
        var sessionUserId = _sessionUser?.UserId ?? -1;
        var knowledgeSummary = _knowledgeSummaryLoader.RunFromMemoryCache(id, sessionUserId);

        return new KnowledgeSummaryResult
        {
            NotLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
            NeedsLearning = knowledgeSummary.NeedsLearning,
            NeedsConsolidation = knowledgeSummary.NeedsConsolidation,
            Solid = knowledgeSummary.Solid,
        };
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
        pageUpdater.HideOrShowPageText(hideOrShowItem.hideText, hideOrShowItem.pageId);

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

        Logg.r.Information("UploadContentImage {id}, {file}", form.PageId, form.File);

        var url = _imageStore.RunPageContentUploadAndGetPath(
            form.File,
            form.PageId,
            _sessionUser.UserId,
            _sessionUser.User.Name);

        return url;
    }

    public record struct DeleteContentImagesRequest(int id, string[] imageUrls);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public void DeleteContentImages([FromBody] DeleteContentImagesRequest req)
    {
        var imageSettings = new PageContentImageSettings(req.id, _httpContextAccessor);
        var deleteImage = new DeleteImage();

        var filenames = new List<string>();

        foreach (var path in req.imageUrls)
            filenames.Add(Path.GetFileName(path));

        deleteImage.Run(imageSettings.BasePath, filenames);
    }

    public record struct PageAnalyticsResponse(
        List<DailyViews> ViewsPast90DaysAggregatedPages,
        List<DailyViews> ViewsPast90DaysPage,
        List<DailyViews> ViewsPast90DaysAggregatedQuestions,
        List<DailyViews> ViewsPast90DaysDirectQuestions);

    [HttpGet]
    public PageAnalyticsResponse GetPageAnalytics([FromRoute] int id)
    {
        var topic = EntityCache.GetPage(id);

        var viewsPast90DaysPages = topic.GetViewsOfPast90Days();
        var viewsPast90DaysAggregatedPages = GetAggregatedPageViewsOfPast90Days(id, viewsPast90DaysPages);
        var viewsPast90DaysAggregatedQuestions = GetAggregatedQuestionViewsOfPast90Days(topic);
        var viewsPast90DaysQuestion = GetQuestionViewsOfPast90Days(topic);

        return new PageAnalyticsResponse(
            viewsPast90DaysAggregatedPages,
            viewsPast90DaysPages,
            viewsPast90DaysAggregatedQuestions,
            viewsPast90DaysQuestion
);
    }

    private List<DailyViews> GetAggregatedPageViewsOfPast90Days(int id, List<DailyViews> topicViews)
    {
        var descendants = GraphService.VisibleDescendants(id, _permissionCheck, _sessionUser.UserId);

        var aggregatedViewsOfPast90Days = descendants
            .SelectMany(descendant => descendant.GetViewsOfPast90Days())
            .Concat(topicViews)
            .GroupBy(view => view.Date)
            .Select(group => new DailyViews
            {
                Date = group.Key,
                Count = group.Sum(view => view.Count)
            })
            .OrderBy(view => view.Date)
            .ToList();

        return aggregatedViewsOfPast90Days;
    }

    private List<DailyViews> GetQuestionViews(IList<QuestionCacheItem> questions)
    {
        var result = questions
            .SelectMany(q => q.GetViewsOfPast90Days())
            .GroupBy(view => view.Date)
            .Select(group => new DailyViews
            {
                Date = group.Key,
                Count = group.Sum(view => view.Count)
            })
            .OrderBy(view => view.Date)
            .ToList();

        return result;
    }

    private List<DailyViews> GetQuestionViewsOfPast90Days(PageCacheItem topic)
    {
        var questions = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId, onlyVisible: true, fullList: false, pageId: topic.Id);
        return GetQuestionViews(questions);
    }

    private List<DailyViews> GetAggregatedQuestionViewsOfPast90Days(PageCacheItem topic)
    {
        var questions = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId, onlyVisible: true, fullList: true, pageId: topic.Id);
        return GetQuestionViews(questions);
    }
}