﻿using Microsoft.AspNetCore.Http;
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
    CategoryRepository _categoryRepository,
    IHttpContextAccessor _httpContextAccessor,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    CategoryUpdater _categoryUpdater,
    ImageStore _imageStore,
    CategoryViewRepo _categoryViewRepo,
    QuestionViewRepository _questionViewRepository) : Controller
{
    public readonly record struct SaveTopicParam(
        int id,
        string name,
        bool saveName,
        string content,
        bool saveContent);

    public readonly record struct SaveTopicResult(bool Success, string MessageKey);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public SaveTopicResult SaveTopic([FromBody] SaveTopicParam param)
    {
        if (!_permissionCheck.CanEditCategory(param.id))
            return new SaveTopicResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.MissingRights
            };

        var categoryCacheItem = EntityCache.GetCategory(param.id);
        var category = _categoryRepository.GetById(param.id);

        if (categoryCacheItem == null || category == null)
            return new SaveTopicResult { Success = false };

        if (param.saveName)
        {
            categoryCacheItem.Name = param.name;
            category.Name = param.name;
            _categoryRepository.Update(category, _sessionUser.UserId, type: CategoryChangeType.Renamed);
        }

        if (param.saveContent)
        {
            categoryCacheItem.Content = param.content;
            category.Content = param.content;
            _categoryRepository.Update(category, _sessionUser.UserId, type: CategoryChangeType.Text);
        }

        EntityCache.AddOrUpdate(categoryCacheItem);

        return new SaveTopicResult
        {
            Success = true
        };
    }

    [HttpGet]
    public string GetTopicImageUrl([FromRoute] int id)
    {
        if (_permissionCheck.CanViewCategory(id))
            return new CategoryImageSettings(id, _httpContextAccessor).GetUrl_128px(asSquare: true)
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
        CategoryVisibility Visibility,
        TopicGridManager.TinyTopicModel[] Parents,
        TopicGridManager.KnowledgebarData KnowledgebarData,
        bool IsChildOfPersonalWiki,
        int CreatorId,
        bool CanDelete
    );

    public readonly record struct HideOrShowItem(bool hideText, int topicId);

    [HttpPost]
    public bool HideOrShowText([FromBody] HideOrShowItem hideOrShowItem) =>
        _categoryUpdater.HideOrShowTopicText(hideOrShowItem.hideText, hideOrShowItem.topicId);

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
        var topic = EntityCache.GetCategory(id);

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

    private List<DailyViews> GetQuestionViewsOfPast90Days(CategoryCacheItem topic)
    {
        var questions = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId, onlyVisible: true, fullList: false, categoryId: topic.Id);
        return GetQuestionViews(questions);
    }

    private List<DailyViews> GetAggregatedQuestionViewsOfPast90Days(CategoryCacheItem topic)
    {
        var questions = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId, onlyVisible: true, fullList: true, categoryId: topic.Id);
        return GetQuestionViews(questions);
    }

    public readonly record struct GetFeedResponse(IList<FeedItem> feedItems, int maxCount);
    public readonly record struct FeedItem(DateTime Date, CategoryChangeType Type, int CategoryChangeId, int TopicId, CategoryVisibility Visibility);

    public readonly record struct GetFeedRequest(int TopicId, int Page, int PageSize);
    [HttpPost]
    public GetFeedResponse GetFeed([FromBody] GetFeedRequest req)
    {
        var topic = EntityCache.GetCategory(req.TopicId);
        var (feedItems, maxCount) = topic.GetVisibleFeedItemsByPage(_sessionUser.UserId, req.Page, req.PageSize);

        return new GetFeedResponse(
            feedItems: feedItems.Select(c =>
                new FeedItem(c.Date, c.Type, c.CategoryChangeCacheItem.Id, c.TopicId, c.Visibility)).ToList(),
            maxCount: maxCount);
    }

    [HttpPost]
    public GetFeedResponse GetFeedWithDescendants([FromBody] GetFeedRequest req)
    {
        var topic = EntityCache.GetCategory(req.TopicId);
        var (feedItems, maxCount) = topic.GetAllVisibleFeedItemsByPage(_permissionCheck, _sessionUser.UserId, req.Page, req.PageSize);

        return new GetFeedResponse(
            feedItems: feedItems.Select(c =>
                new FeedItem(c.Date, c.Type, c.CategoryChangeCacheItem.Id, c.TopicId, c.Visibility)).ToList(),
            maxCount: maxCount);
    }
}