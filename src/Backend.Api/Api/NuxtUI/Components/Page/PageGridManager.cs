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
        int TotalCount,
        double KnowledgeStatusPoints,
        double KnowledgeStatusPointsTotal,
        int NotInWishKnowledgePercentage,
        KnowledgeStatusCounts InWishKnowledge,
        KnowledgeStatusCounts NotInWishKnowledge,
        KnowledgeStatusCounts Total
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
            TotalCount = knowledgeBarSummary.TotalCount,
            KnowledgeStatusPoints = knowledgeBarSummary.KnowledgeStatusPoints,
            KnowledgeStatusPointsTotal = knowledgeBarSummary.KnowledgeStatusPointsTotal,
            NotInWishKnowledgePercentage = knowledgeBarSummary.NotInWishKnowledgePercentage,
            InWishKnowledge = new KnowledgeStatusCounts(
                NotLearned: knowledgeBarSummary.InWishKnowledge.NotLearned,
                NotLearnedPercentage: knowledgeBarSummary.InWishKnowledge.NotLearnedPercentage,
                NeedsLearning: knowledgeBarSummary.InWishKnowledge.NeedsLearning,
                NeedsLearningPercentage: knowledgeBarSummary.InWishKnowledge.NeedsLearningPercentage,
                NeedsConsolidation: knowledgeBarSummary.InWishKnowledge.NeedsConsolidation,
                NeedsConsolidationPercentage: knowledgeBarSummary.InWishKnowledge.NeedsConsolidationPercentage,
                Solid: knowledgeBarSummary.InWishKnowledge.Solid,
                SolidPercentage: knowledgeBarSummary.InWishKnowledge.SolidPercentage,
                NotLearnedPercentageOfTotal: knowledgeBarSummary.InWishKnowledge.NotLearnedPercentageOfTotal,
                NeedsLearningPercentageOfTotal: knowledgeBarSummary.InWishKnowledge.NeedsLearningPercentageOfTotal,
                NeedsConsolidationPercentageOfTotal: knowledgeBarSummary.InWishKnowledge.NeedsConsolidationPercentageOfTotal,
                SolidPercentageOfTotal: knowledgeBarSummary.InWishKnowledge.SolidPercentageOfTotal,
                Total: knowledgeBarSummary.InWishKnowledge.Total),
            NotInWishKnowledge = new KnowledgeStatusCounts(
                NotLearned: knowledgeBarSummary.NotInWishKnowledge.NotLearned,
                NotLearnedPercentage: knowledgeBarSummary.NotInWishKnowledge.NotLearnedPercentage,
                NeedsLearning: knowledgeBarSummary.NotInWishKnowledge.NeedsLearning,
                NeedsLearningPercentage: knowledgeBarSummary.NotInWishKnowledge.NeedsLearningPercentage,
                NeedsConsolidation: knowledgeBarSummary.NotInWishKnowledge.NeedsConsolidation,
                NeedsConsolidationPercentage: knowledgeBarSummary.NotInWishKnowledge.NeedsConsolidationPercentage,
                Solid: knowledgeBarSummary.NotInWishKnowledge.Solid,
                SolidPercentage: knowledgeBarSummary.NotInWishKnowledge.SolidPercentage,
                NotLearnedPercentageOfTotal: knowledgeBarSummary.NotInWishKnowledge.NotLearnedPercentageOfTotal,
                NeedsLearningPercentageOfTotal: knowledgeBarSummary.NotInWishKnowledge.NeedsLearningPercentageOfTotal,
                NeedsConsolidationPercentageOfTotal: knowledgeBarSummary.NotInWishKnowledge.NeedsConsolidationPercentageOfTotal,
                SolidPercentageOfTotal: knowledgeBarSummary.NotInWishKnowledge.SolidPercentageOfTotal,
                Total: knowledgeBarSummary.NotInWishKnowledge.Total),
            Total = new KnowledgeStatusCounts(
                NotLearned: knowledgeBarSummary.Total.NotLearned,
                NotLearnedPercentage: knowledgeBarSummary.Total.NotLearnedPercentage,
                NeedsLearning: knowledgeBarSummary.Total.NeedsLearning,
                NeedsLearningPercentage: knowledgeBarSummary.Total.NeedsLearningPercentage,
                NeedsConsolidation: knowledgeBarSummary.Total.NeedsConsolidation,
                NeedsConsolidationPercentage: knowledgeBarSummary.Total.NeedsConsolidationPercentage,
                Solid: knowledgeBarSummary.Total.Solid,
                SolidPercentage: knowledgeBarSummary.Total.SolidPercentage,
                NotLearnedPercentageOfTotal: knowledgeBarSummary.Total.NotLearnedPercentageOfTotal,
                NeedsLearningPercentageOfTotal: knowledgeBarSummary.Total.NeedsLearningPercentageOfTotal,
                NeedsConsolidationPercentageOfTotal: knowledgeBarSummary.Total.NeedsConsolidationPercentageOfTotal,
                SolidPercentageOfTotal: knowledgeBarSummary.Total.SolidPercentageOfTotal,
                Total: knowledgeBarSummary.Total.Total)
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