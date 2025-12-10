using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
#nullable enable

public class PageDataManager
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public PageDataManager(
        SessionUser sessionUser,
        PermissionCheck permissionCheck,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        QuestionReadingRepo questionReadingRepo)
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _questionReadingRepo = questionReadingRepo;
    }

    public PageDataResult GetPageData(int id, [CanBeNull] string token = null, [CanBeNull] int? userId = null)
    {
        var page = EntityCache.GetPage(id);
        if (page == null)
            return new PageDataResult
            {
                ErrorCode = NuxtErrorPageType.NotFound,
                MessageKey = FrontendMessageKeys.Error.Page.NotFound
            };

        var canView = userId != null
            ? _permissionCheck.CanView((int)userId, page, token)
            : _permissionCheck.CanView(page, token);

        if (canView)
        {
            var imageMetaData = _imageMetaDataReadingRepo.GetBy(id, ImageType.Page);
            var knowledgeSummary = _knowledgeSummaryLoader.RunFromCache(id, _sessionUser.UserId);

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
            QuestionCount = page.GetCountQuestionsAggregated(_sessionUser.UserId, permissionCheck: _permissionCheck),
            ImageUrl = new PageImageSettings(page.Id, _httpContextAccessor)
                .GetUrl_128px(true)
                .Url,
            MiniImageUrl = new ImageFrontendData(
                    _imageMetaDataReadingRepo.GetBy(page.Id, ImageType.Page),
                    _httpContextAccessor,
                    _questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Page).Url,
            Visibility = (int)page.Visibility,
            LanguageCode = page.Language
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
            Views = page.TotalViews,
            SubpageViews = GraphService
                .VisibleDescendants(id, _permissionCheck, _sessionUser.UserId)
                .Sum(p => p.TotalViews),
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
            IsWiki = page.IsWiki,
            CurrentUserIsCreator = CurrentUserIsCreator(page),
            CanBeDeleted = _permissionCheck.CanDelete(page).Allowed,
            QuestionCount = page.GetCountQuestionsAggregated(_sessionUser.UserId, permissionCheck: _permissionCheck),
            DirectQuestionCount = page.GetCountQuestionsAggregated(_sessionUser.UserId, true, page.Id, permissionCheck: _permissionCheck),
            ImageId = imageMetaData != null ? imageMetaData.Id : 0,
            PageItem = FillMiniPageItem(page),
            MetaDescription = SeoUtils.ReplaceDoubleQuotes(page.Content == null
                    ? null
                    : Regex.Replace(page.Content, "<.*?>", ""))
                .Truncate(250, true),
            KnowledgeSummary = new KnowledgeSummaryResponse(knowledgeSummary),
            GridItems = new PageGridManager(
                _permissionCheck,
                _sessionUser,
                _imageMetaDataReadingRepo,
                _httpContextAccessor,
                _knowledgeSummaryLoader,
                _questionReadingRepo).GetChildren(id),
            IsChildOfPersonalWiki = _sessionUser.IsLoggedIn && EntityCache
                .GetPage(_sessionUser.User.FirstWikiId)
                .ChildRelations
                .Any(r => r.ChildId == page.Id),
            TextIsHidden = page.TextIsHidden,
            MessageKey = "",
            Language = page.Language,
            DirectQuestionViews = page.GetQuestionViewCount(_sessionUser.UserId, fullList: false, permissionCheck: _permissionCheck),
            TotalQuestionViews = page.GetQuestionViewCount(_sessionUser.UserId, permissionCheck: _permissionCheck)
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

    public record struct KnowledgeStatusCountsResponse(
        int NotLearned,
        int NotLearnedPercentage,
        int NeedsLearning,
        int NeedsLearningPercentage,
        int NeedsConsolidation,
        int NeedsConsolidationPercentage,
        int Solid,
        int SolidPercentage,
        int NotLearnedPercentageOfTotal,
        int NeedsLearningPercentageOfTotal,
        int NeedsConsolidationPercentageOfTotal,
        int SolidPercentageOfTotal,
        int? NotInWishKnowledgeCount,
        int? NotInWishKnowledgePercentage)
    {
        public KnowledgeStatusCountsResponse(KnowledgeStatusCounts k) : this(
            k.NotLearned,
            k.NotLearnedPercentage,
            k.NeedsLearning,
            k.NeedsLearningPercentage,
            k.NeedsConsolidation,
            k.NeedsConsolidationPercentage,
            k.Solid,
            k.SolidPercentage,
            k.NotLearnedPercentageOfTotal,
            k.NeedsLearningPercentageOfTotal,
            k.NeedsConsolidationPercentageOfTotal,
            k.SolidPercentageOfTotal,
            k.NotInWishKnowledgeCount,
            k.NotInWishKnowledgePercentage)
        {
        }
    }

    public record struct KnowledgeSummaryResponse(
        int TotalCount = 0,
        double KnowledgeStatusPoints = 0.0,
        double KnowledgeStatusPointsTotal = 0.0,
        KnowledgeStatusCountsResponse InWishKnowledge = new KnowledgeStatusCountsResponse(),
        KnowledgeStatusCountsResponse NotInWishKnowledge = new KnowledgeStatusCountsResponse(),
        KnowledgeStatusCountsResponse Total = new KnowledgeStatusCountsResponse())
    {
        public KnowledgeSummaryResponse(KnowledgeSummary k)
            : this(
                TotalCount: k.TotalCount,
                KnowledgeStatusPoints: k.KnowledgeStatusPoints,
                KnowledgeStatusPointsTotal: k.KnowledgeStatusPointsTotal,
                InWishKnowledge: new KnowledgeStatusCountsResponse(k.InWishKnowledge),
                NotInWishKnowledge: new KnowledgeStatusCountsResponse(k.NotInWishKnowledge),
                Total: new KnowledgeStatusCountsResponse(k.Total))
        {
        }
    }

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
        int SubpageViews,
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
        KnowledgeSummaryResponse KnowledgeSummary,
        PageGridManager.GridPageItem[] GridItems,
        bool IsChildOfPersonalWiki,
        bool TextIsHidden,
        string? MessageKey,
        NuxtErrorPageType? ErrorCode,
        int TodayViews,
        List<DailyViews> ViewsLast30DaysAggregatedPage,
        List<DailyViews> ViewsLast30DaysPage,
        List<DailyViews> ViewsLast30DaysAggregatedQuestions,
        List<DailyViews> viewsLast30DaysQuestions,
        string Language,
        int DirectQuestionViews,
        int TotalQuestionViews
    );
}