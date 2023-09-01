
using System.Linq;
using System.Web.Mvc;

namespace VueApp;
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class GridItemLogic : IRegisterAsInstancePerLifetime
{
    private readonly PermissionCheck _permissionCheck;
    private readonly SessionUser _sessionUser;

    public GridItemLogic(PermissionCheck permissionCheck, SessionUser sessionUser)
    {
        _permissionCheck = permissionCheck;
        _sessionUser = sessionUser;
    }

    public class GridTopicItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public int questionCount { get; set; }
        public int childrenCount { get; set; }
        public string imageUrl { get; set; }
        public CategoryVisibility visibility { get; set; }
        public TinyTopicModel[] parents { get; set; }
        public KnowledgebarData knowledgebarData { get; set; }
        public bool isChildOfPersonalWiki { get; set; }
        public int creatorId { get; set; }
        public bool canDelete { get; set; }

    }

    public class TinyTopicModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imgUrl { get; set; }
    }

    public class KnowledgebarData
    {
        public int total { get; set; }
        public int needsLearning { get; set; }
        public double needsLearningPercentage { get; set; }
        public int needsConsolidation { get; set; }
        public double needsConsolidationPercentage { get; set; }
        public int solid { get; set; }
        public double solidPercentage { get; set; }
        public int notLearned { get; set; }
        public double notLearnedPercentage { get; set; }
    }

    public GridTopicItem[] GetChildren(int id)
    {
        return EntityCache.GetChildren(id)
            .Where(c => _permissionCheck.CanView(c))
            .Select(BuildGridTopicItem)
            .ToArray();
    }

    public GridTopicItem BuildGridTopicItem(CategoryCacheItem topic)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(topic.Id, ImageType.Category);
        var imageFrontendData = new ImageFrontendData(imageMetaData);

        return new GridTopicItem
        {
            id = topic.Id,
            name = topic.Name,
            questionCount = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count,
            childrenCount =
                EntityCache.GetChildren(topic.Id).Count(c => _permissionCheck.CanView((CategoryCacheItem)c)),
            imageUrl = imageFrontendData.GetImageUrl(128, true, false, ImageType.Category).Url,
            visibility = topic.Visibility,
            parents = GetParents(topic),
            knowledgebarData = GetKnowledgebarData(topic),
            isChildOfPersonalWiki = _sessionUser.IsLoggedIn && EntityCache.GetChildren(_sessionUser.User.StartTopicId).Any(c => c.Id == topic.Id),
            creatorId = topic.CreatorId,
            canDelete = _sessionUser.IsLoggedIn && (topic.CreatorId == _sessionUser.User.Id || _sessionUser.IsInstallationAdmin)
        };
    }

    private KnowledgebarData GetKnowledgebarData(CategoryCacheItem topic)
    {
        var knowledgeBarSummary = new CategoryKnowledgeBarModel(topic, _sessionUser.UserId).CategoryKnowledgeSummary;

        return new KnowledgebarData
        {
            total = knowledgeBarSummary.Total,
            needsLearning = knowledgeBarSummary.NeedsLearning,
            needsLearningPercentage = knowledgeBarSummary.NeedsLearningPercentage,
            needsConsolidation = knowledgeBarSummary.NeedsConsolidation,
            needsConsolidationPercentage = knowledgeBarSummary.NeedsConsolidationPercentage,
            solid = knowledgeBarSummary.Solid,
            solidPercentage = knowledgeBarSummary.SolidPercentage,
            notLearned = knowledgeBarSummary.NotLearned,
            notLearnedPercentage = knowledgeBarSummary.NotLearnedPercentage
        };
    }

    private TinyTopicModel[] GetParents(CategoryCacheItem topic)
    {
        return topic.ParentCategories().Where(_permissionCheck.CanView).Select(p => new TinyTopicModel
            { id = p.Id, name = p.Name, imgUrl = new CategoryImageSettings(p.Id).GetUrl(50, true).Url }).ToArray();
    }
}






