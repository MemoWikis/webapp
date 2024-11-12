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
        bool CanDelete
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

    public GridPageItem BuildGridPageItem(PageCacheItem topic)
    {
        var imageMetaData = imageMetaDataReading.GetBy(topic.Id, ImageType.Page);
        var imageFrontendData =
            new ImageFrontendData(imageMetaData, httpContextAccessor, questionReadingRepo);

        return new GridPageItem
        {
            Id = topic.Id,
            Name = topic.Name,
            QuestionCount = topic.GetAggregatedQuestionsFromMemoryCache(sessionUser.UserId).Count,
            ChildrenCount = GraphService
                .VisibleDescendants(topic.Id, permissionCheck, sessionUser.UserId)
                .Count,
            ImageUrl = imageFrontendData.GetImageUrl(128, true, false, ImageType.Page).Url,
            Visibility = topic.Visibility,
            Parents = GetParents(topic),
            KnowledgebarData = GetKnowledgebarData(topic),
            IsChildOfPersonalWiki = sessionUser.IsLoggedIn && GraphService
                .VisibleDescendants(sessionUser.User.StartPageId, permissionCheck,
                    sessionUser.UserId).Any(c => c.Id == topic.Id),
            CreatorId = topic.CreatorId,
            CanDelete = sessionUser.IsLoggedIn &&
                        (topic.CreatorId == sessionUser.User.Id ||
                         sessionUser.IsInstallationAdmin)
        };
    }

    private KnowledgebarData GetKnowledgebarData(PageCacheItem topic)
    {
        var knowledgeBarSummary =
            new PageKnowledgeBarModel(topic, sessionUser.UserId, knowledgeSummaryLoader)
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

    private TinyPageModel[] GetParents(PageCacheItem topic)
    {
        return topic.Parents().Where(permissionCheck.CanView).Select(p => new TinyPageModel
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