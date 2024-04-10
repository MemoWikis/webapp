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

    public TopicGridManager(PermissionCheck permissionCheck,
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
    public class GridTopicItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuestionCount { get; set; }
        public int ChildrenCount { get; set; }
        public string ImageUrl { get; set; }
        public CategoryVisibility Visibility { get; set; }
        public TinyTopicModel[] Parents { get; set; }
        public KnowledgebarData KnowledgebarData { get; set; }
        public bool IsChildOfPersonalWiki { get; set; }
        public int CreatorId { get; set; }
        public bool CanDelete { get; set; }

    }

    public class TinyTopicModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
    }

    public class KnowledgebarData
    {
        public int Total { get; set; }
        public int NeedsLearning { get; set; }
        public double NeedsLearningPercentage { get; set; }
        public int NeedsConsolidation { get; set; }
        public double NeedsConsolidationPercentage { get; set; }
        public int Solid { get; set; }
        public double SolidPercentage { get; set; }
        public int NotLearned { get; set; }
        public double NotLearnedPercentage { get; set; }
    }

    public GridTopicItem[] GetChildren(int id)
    {
        var visibleChildren = GraphService.VisibleChildren(id, _permissionCheck, _sessionUser.UserId);
        return visibleChildren.Select(BuildGridTopicItem).ToArray();
    }

    public GridTopicItem BuildGridTopicItem(CategoryCacheItem topic)
    {
        var imageMetaData = _imageMetaDataReading.GetBy(topic.Id, ImageType.Category);
        var imageFrontendData = new ImageFrontendData(imageMetaData, _httpContextAccessor, _questionReadingRepo);

        return new GridTopicItem
        {
            Id = topic.Id,
            Name = topic.Name,
            QuestionCount = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count,
            ChildrenCount = GraphService.VisibleDescendants(topic.Id, _permissionCheck, _sessionUser.UserId).Count,
            ImageUrl = imageFrontendData.GetImageUrl(128, true, false, ImageType.Category).Url,
            Visibility = topic.Visibility,
            Parents = GetParents(topic),
            KnowledgebarData = GetKnowledgebarData(topic),
            IsChildOfPersonalWiki = _sessionUser.IsLoggedIn && GraphService.VisibleDescendants(_sessionUser.User.StartTopicId, _permissionCheck, _sessionUser.UserId).Any(c => c.Id == topic.Id),
            CreatorId = topic.CreatorId,
            CanDelete = _sessionUser.IsLoggedIn && (topic.CreatorId == _sessionUser.User.Id || _sessionUser.IsInstallationAdmin)
        };
    }

    private KnowledgebarData GetKnowledgebarData(CategoryCacheItem topic)
    {
        var knowledgeBarSummary = new CategoryKnowledgeBarModel(topic, _sessionUser.UserId, _knowledgeSummaryLoader).CategoryKnowledgeSummary;

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

    private TinyTopicModel[] GetParents(CategoryCacheItem topic)
    {
        return topic.Parents().Where(_permissionCheck.CanView).Select(p => new TinyTopicModel
            { Id = p.Id, Name = p.Name, ImgUrl = new CategoryImageSettings(p.Id, _httpContextAccessor)
                .GetUrl(50, true).Url })
            .ToArray();
    }
}






