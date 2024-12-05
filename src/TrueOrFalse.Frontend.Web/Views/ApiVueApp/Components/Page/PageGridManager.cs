using Microsoft.AspNetCore.Http;
using System.Linq;

public class PageGridManager(
    PermissionCheck permissionCheck,
    SessionUser sessionUser,
    ImageMetaDataReadingRepo imageMetaDataReading,
    IHttpContextAccessor httpContextAccessor,
    KnowledgeSummaryLoader knowledgeSummaryLoader,
    QuestionReadingRepo questionReadingRepo)
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
        double NotLearnedPercentage
    );

    public GridPageItem[] GetChildren(int id)
    {
        var visibleChildren =
            GraphService.VisibleChildren(id, permissionCheck, sessionUser.UserId);
        return visibleChildren.Select(BuildGridPageItem).ToArray();
    }

    public GridPageItem BuildGridPageItem(PageCacheItem page)
    {
        var imageMetaData = imageMetaDataReading.GetBy(page.Id, ImageType.Page);
        var imageFrontendData =
            new ImageFrontendData(imageMetaData, httpContextAccessor, questionReadingRepo);

        return new GridPageItem
        {
            Id = page.Id,
            Name = page.Name,
            QuestionCount = page.GetAggregatedQuestionsFromMemoryCache(sessionUser.UserId).Count,
            ChildrenCount = GraphService
                .VisibleDescendants(page.Id, permissionCheck, sessionUser.UserId)
                .Count,
            ImageUrl = imageFrontendData.GetImageUrl(128, true, false, ImageType.Page).Url,
            Visibility = page.Visibility,
            Parents = GetParents(page),
            KnowledgebarData = GetKnowledgebarData(page),
            IsChildOfPersonalWiki = sessionUser.IsLoggedIn && GraphService
                .VisibleDescendants(sessionUser.User.StartPageId, permissionCheck,
                    sessionUser.UserId).Any(c => c.Id == page.Id),
            CreatorId = page.CreatorId,
            CanDelete = sessionUser.IsLoggedIn &&
                        (page.CreatorId == sessionUser.User.Id ||
                         sessionUser.IsInstallationAdmin),
            IsWiki = page.IsWiki
        };
    }

    private KnowledgebarData GetKnowledgebarData(PageCacheItem page)
    {
        var knowledgeBarSummary =
            new PageKnowledgeBarModel(page, sessionUser.UserId, knowledgeSummaryLoader)
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
            NotLearnedPercentage = knowledgeBarSummary.NotLearnedPercentage
        };
    }

    private TinyPageModel[] GetParents(PageCacheItem page)
    {
        return page.Parents().Where(permissionCheck.CanView).Select(p => new TinyPageModel
        {
            Id = p.Id,
            Name = p.Name,
            ImgUrl =
                    new PageImageSettings(p.Id, httpContextAccessor)
                        .GetUrl(50, true).Url
        })
            .ToArray();
    }
}