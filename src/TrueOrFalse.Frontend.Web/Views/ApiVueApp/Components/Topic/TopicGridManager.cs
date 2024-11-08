using System.Linq;
using Microsoft.AspNetCore.Http;

public class TopicGridManager
    : IRegisterAsInstancePerLifetime
{
    private readonly PermissionCheck _permissionCheck;
    private readonly SessionUser _sessionUser;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReading;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public TopicGridManager(
        PermissionCheck permissionCheck,
        SessionUser sessionUser,
        ImageMetaDataReadingRepo imageMetaDataReading,
        IHttpContextAccessor httpContextAccessor,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        QuestionReadingRepo questionReadingRepo)
    {
        _permissionCheck = permissionCheck;
        _sessionUser = sessionUser;
        _imageMetaDataReading = imageMetaDataReading;
        _httpContextAccessor = httpContextAccessor;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _questionReadingRepo = questionReadingRepo;
    }

    public readonly record struct GridTopicItem(
        int Id,
        string Name,
        int QuestionCount,
        int ChildrenCount,
        string ImageUrl,
        PageVisibility Visibility,
        TinyTopicModel[] Parents,
        KnowledgebarData KnowledgebarData,
        bool IsChildOfPersonalWiki,
        int CreatorId,
        bool CanDelete
    );

    public readonly record struct TinyTopicModel(
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

    public GridTopicItem[] GetChildren(int id)
    {
        var visibleChildren =
            GraphService.VisibleChildren(id, _permissionCheck, _sessionUser.UserId);
        return visibleChildren.Select(BuildGridTopicItem).ToArray();
    }

    public GridTopicItem BuildGridTopicItem(PageCacheItem topic)
    {
        var imageMetaData = _imageMetaDataReading.GetBy(topic.Id, ImageType.Page);
        var imageFrontendData =
            new ImageFrontendData(imageMetaData, _httpContextAccessor, _questionReadingRepo);

        return new GridTopicItem
        {
            Id = topic.Id,
            Name = topic.Name,
            QuestionCount = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count,
            ChildrenCount = GraphService
                .VisibleDescendants(topic.Id, _permissionCheck, _sessionUser.UserId)
                .Count,
            ImageUrl = imageFrontendData.GetImageUrl(128, true, false, ImageType.Page).Url,
            Visibility = topic.Visibility,
            Parents = GetParents(topic),
            KnowledgebarData = GetKnowledgebarData(topic),
            IsChildOfPersonalWiki = _sessionUser.IsLoggedIn && GraphService
                .VisibleDescendants(_sessionUser.User.StartTopicId, _permissionCheck,
                    _sessionUser.UserId).Any(c => c.Id == topic.Id),
            CreatorId = topic.CreatorId,
            CanDelete = _sessionUser.IsLoggedIn &&
                        (topic.CreatorId == _sessionUser.User.Id ||
                         _sessionUser.IsInstallationAdmin)
        };
    }

    private KnowledgebarData GetKnowledgebarData(PageCacheItem topic)
    {
        var knowledgeBarSummary =
            new PageKnowledgeBarModel(topic, _sessionUser.UserId, _knowledgeSummaryLoader)
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

    private TinyTopicModel[] GetParents(PageCacheItem topic)
    {
        return topic.Parents().Where(_permissionCheck.CanView).Select(p => new TinyTopicModel
            {
                Id = p.Id, Name = p.Name, ImgUrl =
                    new PageImageSettings(p.Id, _httpContextAccessor)
                        .GetUrl(50, true).Url
            })
            .ToArray();
    }
}