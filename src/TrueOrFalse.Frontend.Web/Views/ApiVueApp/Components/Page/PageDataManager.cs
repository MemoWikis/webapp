﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class PageDataManager(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    PageViewRepo pageViewRepo,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo)
{
    public PageDataResult GetPageData(int id)
    {
        var page = EntityCache.GetPage(id);
        if (page == null)
            return new PageDataResult
            {
                ErrorCode = NuxtErrorPageType.NotFound,
                MessageKey = FrontendMessageKeys.Error.Page.NotFound
            };

        if (_permissionCheck.CanView(_sessionUser.UserId, page))
        {
            var imageMetaData = _imageMetaDataReadingRepo.GetBy(id, ImageType.Page);
            var knowledgeSummary =
                _knowledgeSummaryLoader.RunFromMemoryCache(id, _sessionUser.UserId);

            return CreatePageDataObject(id, page, imageMetaData, knowledgeSummary);
        }

        if (_sessionUser.IsLoggedIn)
            return new PageDataResult
            {
                ErrorCode = NuxtErrorPageType.Unauthorized,
                MessageKey = FrontendMessageKeys.Error.Page.NoRights
            };

        return new PageDataResult
        {
            ErrorCode = NuxtErrorPageType.Unauthorized,
            MessageKey = FrontendMessageKeys.Error.Page.Unauthorized
        };
    }

    private SearchPageItem FillMiniPageItem(PageCacheItem page)
    {
        var miniPageItem = new SearchPageItem
        {
            Id = page.Id,
            Name = page.Name,
            QuestionCount = page.GetCountQuestionsAggregated(_sessionUser.UserId),
            ImageUrl = new PageImageSettings(page.Id,
                    _httpContextAccessor)
                .GetUrl_128px(true)
                .Url,
            MiniImageUrl = new ImageFrontendData(
                    _imageMetaDataReadingRepo.GetBy(page.Id, ImageType.Page),
                    _httpContextAccessor,
                    _questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Page).Url,
            Visibility = (int)page.Visibility
        };

        return miniPageItem;
    }

    private PageDataResult CreatePageDataObject(
        int id,
        PageCacheItem page,
        ImageMetaData imageMetaData,
        KnowledgeSummary knowledgeSummary)
    {
        var authorIds = page.AuthorIds.Distinct();
        return new PageDataResult
        {
            CanAccess = true,
            Id = id,
            Name = page.Name,
            ImageUrl = new PageImageSettings(id, _httpContextAccessor).GetUrl_128px(true).Url,
            Content = page.Content,
            ParentPageCount = page.Parents()
                .Where(_permissionCheck.CanView)
                .ToList()
                .Count,
            Parents = page.Parents()
                .Where(_permissionCheck.CanView)
                .Select(p =>
                    new Parent
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ImgUrl = new PageImageSettings(p.Id, _httpContextAccessor)
                            .GetUrl(50, true)
                            .Url
                    })
                .ToArray(),
            ChildPageCount = GraphService
                .VisibleDescendants(page.Id, _permissionCheck, _sessionUser.UserId).Count,
            DirectVisibleChildPageCount = GraphService
                .VisibleChildren(page.Id, _permissionCheck, _sessionUser.UserId).Count,
            Views = pageViewRepo.GetViewCount(id),
            Visibility = page.Visibility,
            AuthorIds = authorIds.ToArray(),
            Authors = authorIds.Select(authorId =>
            {
                var author = EntityCache.GetUserById(authorId);
                return new Author(
                    authorId,
                    author.Name,
                    new UserImageSettings(author.Id, _httpContextAccessor)
                        .GetUrl_20px_square(author).Url,
                    author.Reputation,
                    author.ReputationPos
                );
            }).ToArray(),
            IsWiki = page.IsWikiType(),
            CurrentUserIsCreator = CurrentUserIsCreator(page),
            CanBeDeleted = _permissionCheck.CanDelete(page),
            QuestionCount = page.GetCountQuestionsAggregated(_sessionUser.UserId),
            DirectQuestionCount = page.GetCountQuestionsAggregated(_sessionUser.UserId, true,
                page.Id),
            ImageId = imageMetaData != null ? imageMetaData.Id : 0,
            PageItem = FillMiniPageItem(page),
            MetaDescription = SeoUtils.ReplaceDoubleQuotes(page.Content == null
                    ? null
                    : Regex.Replace(page.Content, "<.*?>", ""))
                .Truncate(250, true),
            KnowledgeSummary = new KnowledgeSummarySlim(
                NotLearned: knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
                NeedsLearning: knowledgeSummary.NeedsLearning,
                NeedsConsolidation: knowledgeSummary.NeedsConsolidation,
                Solid: knowledgeSummary.Solid
            ),
            GridItems = new PageGridManager(
                _permissionCheck,
                _sessionUser,
                _imageMetaDataReadingRepo,
                _httpContextAccessor,
                _knowledgeSummaryLoader,
                _questionReadingRepo).GetChildren(id),
            IsChildOfPersonalWiki = _sessionUser.IsLoggedIn && EntityCache
                .GetPage(_sessionUser.User.StartPageId)
                .ChildRelations
                .Any(r => r.ChildId == page.Id),
            TextIsHidden = page.TextIsHidden,
            MessageKey = ""
        };
    }

    private bool CurrentUserIsCreator(PageCacheItem page)
    {
        if (_sessionUser.IsLoggedIn == false)
            return false;

        return _sessionUser.UserId == page.Creator?.Id;
    }

    public record Author(int Id, string Name, string ImgUrl, int Reputation, int ReputationPos);

    public class Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
    }

    public record struct KnowledgeSummarySlim(
        int NotLearned,
        int NeedsLearning,
        int NeedsConsolidation,
        int Solid);

    public record struct PageDataResult(
        bool CanAccess,
        int Id,
        string Name,
        string ImageUrl,
        string Content,
        int ParentPageCount,
        Parent[] Parents,
        int ChildPageCount,
        int DirectVisibleChildPageCount,
        int Views,
        PageVisibility Visibility,
        int[] AuthorIds,
        Author[] Authors,
        bool IsWiki,
        bool CurrentUserIsCreator,
        bool CanBeDeleted,
        int QuestionCount,
        int DirectQuestionCount,
        int ImageId,
        SearchPageItem PageItem,
        string MetaDescription,
        KnowledgeSummarySlim KnowledgeSummary,
        PageGridManager.GridPageItem[] GridItems,
        bool IsChildOfPersonalWiki,
        bool TextIsHidden,
        string? MessageKey,
        NuxtErrorPageType? ErrorCode,
        int TodayViews,
        List<DailyViews> ViewsLast30DaysAggregatedPage,
        List<DailyViews> ViewsLast30DaysPage,
        List<DailyViews> ViewsLast30DaysAggregatedQuestions,
        List<DailyViews> viewsLast30DaysQuestions
    );
}