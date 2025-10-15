public class PageGridManager(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    ImageMetaDataReadingRepo _imageMetaDataReading,
    IHttpContextAccessor _httpContextAccessor,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    QuestionReadingRepo _questionReadingRepo)
    : IRegisterAsInstancePerLifetime
{
    public readonly record struct GridPageItem(
        int Id,
        string Name,
        int QuestionCount,
        int ChildrenCount,
        string ImageUrl,
        PageVisibility Visibility,
        TinyPageModel[] Parents,
        KnowledgebarData KnowledgebarData,
        bool IsChildOfPersonalWiki,
        int CreatorId,
        bool CanDelete,
        bool IsWiki
    );

    public readonly record struct TinyPageModel(
        int Id,
        string Name,
        string ImgUrl
    );

    public readonly record struct KnowledgebarData(
        int Total,
        int NeedsLearning,
        double NeedsLearningPercentage,
        int NeedsConsolidation,
        double NeedsConsolidationPercentage,
        int Solid,
        double SolidPercentage,
        int NotLearned,
        double NotLearnedPercentage,
        int NotInWishknowledge,
        double NotInWishknowledgePercentage
    );

    public GridPageItem[] GetChildren(int id)
    {
        var visibleChildren = GraphService.VisibleChildren(id, _permissionCheck, _sessionUser.UserId);
        return visibleChildren.Select(BuildGridPageItem).ToArray();
    }

    public GridPageItem BuildGridPageItem(PageCacheItem page)
    {
        var imageMetaData = _imageMetaDataReading.GetBy(page.Id, ImageType.Page);
        var imageFrontendData =
            new ImageFrontendData(imageMetaData, _httpContextAccessor, _questionReadingRepo);

        return new GridPageItem
        {
            Id = page.Id,
            Name = page.Name,
            QuestionCount = page.GetAggregatedQuestions(_sessionUser.UserId, permissionCheck: _permissionCheck).Count,
            ChildrenCount = page.VisibleChildrenCount(_permissionCheck, _sessionUser.UserId),
            ImageUrl = imageFrontendData.GetImageUrl(128, true, false, ImageType.Page).Url,
            Visibility = page.Visibility,
            Parents = GetParents(page),
            KnowledgebarData = GetKnowledgebarData(page),
            IsChildOfPersonalWiki = page.IsChildOfPersonalWiki(_sessionUser, _permissionCheck),
            CreatorId = page.CreatorId,
            CanDelete = _sessionUser.IsLoggedIn &&
                        (page.CreatorId == _sessionUser.User.Id ||
                         _sessionUser.IsInstallationAdmin),
            IsWiki = page.IsWiki
        };
    }

    private KnowledgebarData GetKnowledgebarData(PageCacheItem page)
    {
        var knowledgeBarSummary =
            new PageKnowledgeBarModel(page, _sessionUser.UserId, _knowledgeSummaryLoader)
                .PageKnowledgeSummary;

        return new KnowledgebarData
        {
            Total = knowledgeBarSummary.Total,
            NeedsLearning = knowledgeBarSummary.NeedsLearning,
            NeedsLearningPercentage = knowledgeBarSummary.NeedsLearningPercentage,
            NeedsConsolidation = knowledgeBarSummary.NeedsConsolidation,
            NeedsConsolidationPercentage = knowledgeBarSummary.NeedsConsolidationPercentage,
            Solid = knowledgeBarSummary.Solid,
            SolidPercentage = knowledgeBarSummary.SolidPercentage,
            NotLearned = knowledgeBarSummary.NotLearned,
            NotLearnedPercentage = knowledgeBarSummary.NotLearnedPercentage,
            NotInWishknowledge = knowledgeBarSummary.NotInWishknowledge,
            NotInWishknowledgePercentage = knowledgeBarSummary.NotInWishknowledgePercentage
        };
    }

    private TinyPageModel[] GetParents(PageCacheItem page)
    {
        return page
            .Parents()
            .Where(_permissionCheck.CanView)
            .Select(p => new TinyPageModel
            {
                Id = p.Id,
                Name = p.Name,
                ImgUrl = new PageImageSettings(p.Id, _httpContextAccessor).GetUrl(50, true).Url
            })
            .ToArray();
    }
}