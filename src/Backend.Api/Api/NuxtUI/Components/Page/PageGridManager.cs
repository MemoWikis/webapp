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

    public readonly record struct KnowledgeStatusCounts(
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
        int Total
    );

    public readonly record struct KnowledgebarData(
        int Total,
        double KnowledgeStatusPoints,
        double KnowledgeStatusPointsTotal,
        int NotInWishknowledgePercentage,
        KnowledgeStatusCounts InWishknowledge,
        KnowledgeStatusCounts NotInWishknowledge
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
            Total = knowledgeBarSummary.TotalCount,
            KnowledgeStatusPoints = knowledgeBarSummary.KnowledgeStatusPoints,
            KnowledgeStatusPointsTotal = knowledgeBarSummary.KnowledgeStatusPointsTotal,
            NotInWishknowledgePercentage = knowledgeBarSummary.NotInWishknowledgePercentage,
            InWishknowledge = new KnowledgeStatusCounts(
                NotLearned: knowledgeBarSummary.InWishknowledge.NotLearned,
                NotLearnedPercentage: knowledgeBarSummary.InWishknowledge.NotLearnedPercentage,
                NeedsLearning: knowledgeBarSummary.InWishknowledge.NeedsLearning,
                NeedsLearningPercentage: knowledgeBarSummary.InWishknowledge.NeedsLearningPercentage,
                NeedsConsolidation: knowledgeBarSummary.InWishknowledge.NeedsConsolidation,
                NeedsConsolidationPercentage: knowledgeBarSummary.InWishknowledge.NeedsConsolidationPercentage,
                Solid: knowledgeBarSummary.InWishknowledge.Solid,
                SolidPercentage: knowledgeBarSummary.InWishknowledge.SolidPercentage,
                NotLearnedPercentageOfTotal: knowledgeBarSummary.InWishknowledge.NotLearnedPercentageOfTotal,
                NeedsLearningPercentageOfTotal: knowledgeBarSummary.InWishknowledge.NeedsLearningPercentageOfTotal,
                NeedsConsolidationPercentageOfTotal: knowledgeBarSummary.InWishknowledge.NeedsConsolidationPercentageOfTotal,
                SolidPercentageOfTotal: knowledgeBarSummary.InWishknowledge.SolidPercentageOfTotal,
                Total: knowledgeBarSummary.InWishknowledge.Total),
            NotInWishknowledge = new KnowledgeStatusCounts(
                NotLearned: knowledgeBarSummary.NotInWishknowledge.NotLearned,
                NotLearnedPercentage: knowledgeBarSummary.NotInWishknowledge.NotLearnedPercentage,
                NeedsLearning: knowledgeBarSummary.NotInWishknowledge.NeedsLearning,
                NeedsLearningPercentage: knowledgeBarSummary.NotInWishknowledge.NeedsLearningPercentage,
                NeedsConsolidation: knowledgeBarSummary.NotInWishknowledge.NeedsConsolidation,
                NeedsConsolidationPercentage: knowledgeBarSummary.NotInWishknowledge.NeedsConsolidationPercentage,
                Solid: knowledgeBarSummary.NotInWishknowledge.Solid,
                SolidPercentage: knowledgeBarSummary.NotInWishknowledge.SolidPercentage,
                NotLearnedPercentageOfTotal: knowledgeBarSummary.NotInWishknowledge.NotLearnedPercentageOfTotal,
                NeedsLearningPercentageOfTotal: knowledgeBarSummary.NotInWishknowledge.NeedsLearningPercentageOfTotal,
                NeedsConsolidationPercentageOfTotal: knowledgeBarSummary.NotInWishknowledge.NeedsConsolidationPercentageOfTotal,
                SolidPercentageOfTotal: knowledgeBarSummary.NotInWishknowledge.SolidPercentageOfTotal,
                Total: knowledgeBarSummary.NotInWishknowledge.Total)
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