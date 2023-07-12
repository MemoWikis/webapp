﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class SegmentationLogic : IRegisterAsInstancePerLifetime
{
    private ControllerContext _controllerContext;
    private readonly PermissionCheck _permissionCheck;
    private readonly int _sessionUserId;
    private readonly SessionUser _sessionUser;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;
    private readonly UserRepo _userRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;

    public SegmentationLogic(ControllerContext controllerContext,
        PermissionCheck permissionCheck, 
        SessionUser sessionUser,
        CategoryValuationRepo categoryValuationRepo,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        ImageMetaDataRepo imageMetaDataRepo, 
        UserRepo userRepo,
        QuestionValuationRepo questionValuationRepo)
    {
        _controllerContext = controllerContext;
        _permissionCheck = permissionCheck;
        _sessionUserId = sessionUser.UserId;
        _sessionUser = sessionUser;
        _categoryValuationRepo = categoryValuationRepo;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _imageMetaDataRepo = imageMetaDataRepo;
        _userRepo = userRepo;
        _questionValuationRepo = questionValuationRepo;
    }
    public dynamic GetSegmentation(int id)
    {
        var category = EntityCache.GetCategory(id);
        var s = new SegmentationModel(category,_permissionCheck);
        return new
        {
            childCategoryIds = s.NotInSegmentCategoryIds,
            segmentJson = s.SegmentJson
        };
    }
    public dynamic GetSegment(SegmentJson json)
    {
        var categoryId = json.CategoryId;
        var segment = new Segment();
        segment.Item = EntityCache.GetCategory(categoryId);

        if (json.ChildCategoryIds != null)
            segment.ChildCategories = EntityCache.GetCategories(json.ChildCategoryIds).ToList();
        else
            segment.ChildCategories = EntityCache.GetChildren(categoryId);

        return new
        {
            CategoryId = segment.Item.Id,
            Title = json.Title,
            ChildCategoryIds = "[" + String.Join(", ", segment.ChildCategories.Select(c => c.Id).ToList()) + "]",
        };
    }

    public dynamic GetCategoriesData(int[] categoryIds)
    {
        var ids = categoryIds;
        ConcurrentDictionary<int, CategoryValuation> userValuation = null;
        var startTopicId = 1;

        if (_sessionUser.IsLoggedIn)
        {
            userValuation = SessionUserCache.GetItem(_sessionUser.UserId, _categoryValuationRepo, _userRepo, _questionValuationRepo).CategoryValuations;
            startTopicId = SessionUserCache.GetUser(_sessionUser.UserId, _categoryValuationRepo, _userRepo, _questionValuationRepo).StartTopicId;
        }

        var categoryDataList = categoryIds.Select(
            categoryId => _sessionUser.IsLoggedIn ? GetCategoryCardData(categoryId, userValuation, startTopicId)
                : GetCategoryCardData(categoryId)).Where(categoryCardData => categoryCardData != null)
            .ToList();

        return categoryDataList;
    }

    public dynamic GetCategoryData(int categoryId)
    {
        var categoryCardData = _sessionUser.IsLoggedIn
            ? GetCategoryCardData(categoryId, SessionUserCache.GetItem(_sessionUserId, _categoryValuationRepo, _userRepo, _questionValuationRepo).CategoryValuations,
                SessionUserCache.GetUser(_sessionUser.UserId, _categoryValuationRepo, _userRepo, _questionValuationRepo).StartTopicId)
            : GetCategoryCardData(categoryId);
        return categoryCardData != null ? categoryCardData : "";
    }

    private CategoryCardData GetCategoryCardData(int categoryId, ConcurrentDictionary<int, CategoryValuation> userValuation = null, int? startTopicId = null)
    {
        var categoryCacheItem = EntityCache.GetCategory(categoryId);
        if (!_permissionCheck.CanView(categoryCacheItem)) return null;

        var linkToCategory = Links.CategoryDetail(categoryCacheItem);

        var imageMetaData = _imageMetaDataRepo.GetBy(categoryId, ImageType.Category);
        var imageFrontendData = new ImageFrontendData(imageMetaData);
        var imgHtml = imageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category);
        var imgUrl = imageFrontendData.GetImageUrl(128, true, false, ImageType.Category).Url;

        var childCategoryCount = EntityCache.GetChildren(categoryId).Where(_permissionCheck.CanView).Distinct().Count();
        var questionCount = categoryCacheItem.GetAggregatedQuestionsFromMemoryCache(_sessionUserId).Count;

        var knowledgeBarSummary = new CategoryKnowledgeBarModel(categoryCacheItem,_sessionUserId, _knowledgeSummaryLoader).CategoryKnowledgeSummary;

        var isInWishknowledge = false;
        var isPersonalHomepage = false;
        if (_sessionUser.IsLoggedIn)
        {
            if (userValuation != null && userValuation.ContainsKey(categoryId))
                isInWishknowledge = userValuation[categoryId].IsInWishKnowledge();
            isPersonalHomepage = categoryCacheItem.Id == startTopicId;
        }

        return new CategoryCardData
        {
            Id = categoryCacheItem.Id,
            Name = categoryCacheItem.Name,
            Visibility = (int)categoryCacheItem.Visibility,
            LinkToCategory = linkToCategory,
            ImgUrl = imgUrl,
            ImgHtml = imgHtml,
            KnowledgeBarData =
            new {
                Total = knowledgeBarSummary.Total,
                NeedsLearning = knowledgeBarSummary.NeedsLearning,
                NeedsLearningPercentage = knowledgeBarSummary.NeedsLearningPercentage,
                NeedsConsolidation = knowledgeBarSummary.NeedsConsolidation,
                NeedsConsolidationPercentage = knowledgeBarSummary.NeedsConsolidationPercentage,
                Solid = knowledgeBarSummary.Solid,
                SolidPercentage = knowledgeBarSummary.SolidPercentage,
                NotLearned = knowledgeBarSummary.NotLearned,
                NotLearnedPercentage = knowledgeBarSummary.NotLearnedPercentage
            },
            ChildCategoryCount = childCategoryCount,
            QuestionCount = questionCount,
            IsInWishknowledge = isInWishknowledge,
            IsPersonalHomepage = isPersonalHomepage
        };

    }

    private class CategoryCardData
    {
        public int Id;
        public string Name;
        public int Visibility;
        public string LinkToCategory;
        public string ImgHtml;
        public dynamic KnowledgeBarData;
        public int ChildCategoryCount;
        public int QuestionCount;
        public bool IsInWishknowledge = false;
        public bool IsPersonalHomepage = false;
        public string ImgUrl;
    }

    public dynamic GetSegmentData(int categoryId)
    {
        var categoryCacheItem = EntityCache.GetCategory(categoryId);
        var linkToCategory = Links.CategoryDetail(categoryCacheItem);

        var questionCount = categoryCacheItem.GetAggregatedQuestionsFromMemoryCache(_sessionUserId).Count;
        var knowledgeBarHtml = "";
        if (questionCount > 0)
            knowledgeBarHtml = ViewRenderer.RenderPartialView("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(categoryCacheItem, _sessionUserId, _knowledgeSummaryLoader), _controllerContext);
        return new
        {
            categoryId = categoryCacheItem.Id,
            categoryName = categoryCacheItem.Name,
            visibility = categoryCacheItem.Visibility,
            linkToCategory,
            knowledgeBarHtml
        };
    }
}






