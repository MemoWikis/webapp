using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VueApp;

public class TopicStoreController(
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
        var categoryCacheItem = EntityCache.GetPage(req.Id);

        if (categoryCacheItem == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };


        if (categoryCacheItem.Content.Trim() == req.Content.Trim())
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Page.NoChange };

        if (!_permissionCheck.CanEdit(categoryCacheItem))
            return new SaveResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.MissingRights
            };

        var category = pageRepository.GetByIdEager(req.Id);

        if (category == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        categoryCacheItem.Content = req.Content;
        category.Content = req.Content;
        pageRepository.Update(category, _sessionUser.UserId, type: PageChangeType.Text);

        EntityCache.AddOrUpdate(categoryCacheItem);

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
        var categoryCacheItem = EntityCache.GetPage(req.Id);

        if (categoryCacheItem == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        if (categoryCacheItem.Name.Trim() == req.Name.Trim())
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Page.NoChange };

        if (!_permissionCheck.CanEdit(categoryCacheItem))
            return new SaveResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.MissingRights
            };

        var category = pageRepository.GetByIdEager(req.Id);

        if (category == null)
            return new SaveResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default };

        categoryCacheItem.Name = req.Name.Trim();
        category.Name = req.Name.Trim();
        pageRepository.Update(category, _sessionUser.UserId, type: PageChangeType.Renamed);

        EntityCache.AddOrUpdate(categoryCacheItem);

        return new SaveResult
        {
            Success = true
        };
    }

    [HttpGet]
    public string GetTopicImageUrl([FromRoute] int id)
    {
        if (_permissionCheck.CanViewCategory(id))
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
        var sessionuserId = _sessionUser == null ? -1 : _sessionUser.UserId;
        var knowledgeSummary = _knowledgeSummaryLoader.RunFromMemoryCache(id, sessionuserId);

        return new KnowledgeSummaryResult
        {
            NotLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
            NeedsLearning = knowledgeSummary.NeedsLearning,
            NeedsConsolidation = knowledgeSummary.NeedsConsolidation,
            Solid = knowledgeSummary.Solid,
        };
    }

    public readonly record struct GridTopicItem(
        int Id,
        string Name,
        int QuestionCount,
        int ChildrenCount,
        string ImageUrl,
        PageVisibility Visibility,
        TopicGridManager.TinyTopicModel[] Parents,
        TopicGridManager.KnowledgebarData KnowledgebarData,
        bool IsChildOfPersonalWiki,
        int CreatorId,
        bool CanDelete
    );

    public readonly record struct HideOrShowItem(bool hideText, int topicId);

    [HttpPost]
    public bool HideOrShowText([FromBody] HideOrShowItem hideOrShowItem) =>
        pageUpdater.HideOrShowTopicText(hideOrShowItem.hideText, hideOrShowItem.topicId);

    [HttpGet]
    public GridTopicItem[] GetGridTopicItems([FromRoute] int id)
    {
        var data = new TopicGridManager(
            _permissionCheck,
            _sessionUser,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _knowledgeSummaryLoader,
            _questionReadingRepo).GetChildren(id);

        return data.Select(git => new GridTopicItem
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
        public int TopicId { get; set; }
        public IFormFile File { get; set; }
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public string UploadContentImage([FromForm] UploadContentImageRequest form)
    {
        if (!_permissionCheck.CanEditCategory(form.TopicId))
            throw new Exception("No Upload rights");

        Logg.r.Information("UploadContentImage {id}, {file}", form.TopicId, form.File);

        var url = _imageStore.RunTopicContentUploadAndGetPath(
            form.File,
            form.TopicId,
            _sessionUser.UserId,
            _sessionUser.User.Name);

        return url;
    }

    public record struct DeleteContentImagesRequest(int id, string[] imageUrls);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public void DeleteContentImages([FromBody] DeleteContentImagesRequest req)
    {
        var imageSettings = new TopicContentImageSettings(req.id, _httpContextAccessor);
        var deleteImage = new DeleteImage();

        var filenames = new List<string>();

        foreach (var path in req.imageUrls)
            filenames.Add(Path.GetFileName(path));

        deleteImage.Run(imageSettings.BasePath, filenames);
    }

    public record struct TopicAnalyticsResponse(
        List<DailyViews> ViewsPast90DaysAggregatedTopics,
        List<DailyViews> ViewsPast90DaysTopic,
        List<DailyViews> ViewsPast90DaysAggregatedQuestions,
        List<DailyViews> ViewsPast90DaysDirectQuestions);

    [HttpGet]
    public TopicAnalyticsResponse GetTopicAnalytics([FromRoute] int id)
    {
        var topic = EntityCache.GetPage(id);

        var viewsPast90DaysTopics = topic.GetViewsOfPast90Days();
        var viewsPast90DaysAggregatedTopics = GetAggregatedTopicViewsOfPast90Days(id, viewsPast90DaysTopics);
        var viewsPast90DaysAggregatedQuestions = GetAggregatedQuestionViewsOfPast90Days(topic);
        var viewsPast90DaysQuestion = GetQuestionViewsOfPast90Days(topic);

        return new TopicAnalyticsResponse(
            viewsPast90DaysAggregatedTopics,
            viewsPast90DaysTopics,
            viewsPast90DaysAggregatedQuestions,
            viewsPast90DaysQuestion
);
    }

    private List<DailyViews> GetAggregatedTopicViewsOfPast90Days(int id, List<DailyViews> topicViews)
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
        var questions = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId, onlyVisible: true, fullList: false, categoryId: topic.Id);
        return GetQuestionViews(questions);
    }

    private List<DailyViews> GetAggregatedQuestionViewsOfPast90Days(PageCacheItem topic)
    {
        var questions = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId, onlyVisible: true, fullList: true, categoryId: topic.Id);
        return GetQuestionViews(questions);
    }
}