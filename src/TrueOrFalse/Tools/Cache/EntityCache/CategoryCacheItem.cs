using Seedworks.Lib.Persistence;
using System.Diagnostics;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class CategoryCacheItem : IPersistable
{
    public int CreatorId;

    public CategoryCacheItem()
    {
    }

    public CategoryCacheItem(string name)
    {
        Name = name;
    }

    public virtual UserCacheItem Creator =>
        EntityCache.GetUserById(CreatorId);

    public virtual int[] AuthorIds { get; set; }
    public virtual string CategoriesToExcludeIdsString { get; set; }
    public virtual string CategoriesToIncludeIdsString { get; set; }
    public virtual IList<CategoryCacheRelation> ParentRelations { get; set; }
    public virtual IList<CategoryCacheRelation> ChildRelations { get; set; }

    public virtual string Content { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }
    public virtual int CountQuestions { get; set; }

    public int CountQuestionsAggregated { get; set; }
    public virtual string CustomSegments { get; set; }

    public virtual DateTime DateCreated { get; set; }
    public virtual string Description { get; set; }

    public virtual bool DisableLearningFunctions { get; set; }
    public virtual bool TextIsHidden { get; set; }

    public virtual int FormerSetId { get; set; }
    public virtual int Id { get; set; }
    public virtual bool IsHistoric { get; set; }
    public virtual string Name { get; set; }
    public virtual bool SkipMigration { get; set; }
    public virtual string TopicMarkdown { get; set; }

    public virtual int TotalRelevancePersonalEntries { get; set; }

    public virtual CategoryType Type { get; set; }

    public virtual string TypeJson { get; set; }

    public virtual string Url { get; set; }

    public virtual string UrlLinkText { get; set; }
    public virtual CategoryVisibility Visibility { get; set; }

    public virtual string WikipediaURL { get; set; }

    /// <summary>
    /// Get Aggregated Topics
    /// </summary>
    /// <param name="permissionCheck"></param>
    /// <param name="includingSelf"></param>
    /// <returns>Dictionary&lt;int, CategoryCacheItem&gt;</returns>
    public Dictionary<int, CategoryCacheItem> AggregatedCategories(
        PermissionCheck permissionCheck,
        bool includingSelf = true)
    {
        var visibleVisited = VisibleChildCategories(this, permissionCheck);

        if (includingSelf && !visibleVisited.ContainsKey(Id))
        {
            visibleVisited.Add(Id, this);
        }
        else
        {
            if (visibleVisited.ContainsKey(Id))
            {
                visibleVisited.Remove(Id);
            }
        }

        return visibleVisited;
    }

    public virtual IList<QuestionCacheItem> GetAggregatedQuestionsFromMemoryCache(
        int userId,
        bool onlyVisible = true,
        bool fullList = true,
        int categoryId = 0)
    {
        IList<QuestionCacheItem> questions;

        if (fullList)
        {
            questions = AggregatedCategories(
                    new PermissionCheck(userId))
                .SelectMany(c => EntityCache.GetQuestionsForCategory(c.Key))
                .Distinct().ToList();
        }
        else
        {
            questions = EntityCache.GetQuestionsForCategory(categoryId)
                .Distinct().ToList();
        }

        if (onlyVisible)
        {
            var user = EntityCache.GetUserById(userId);
            var permissionCheck = new PermissionCheck(user);
            questions = questions.Where(permissionCheck.CanView).ToList();
        }

        if (questions.Any(q => q.Id == 0))
        {
            var questionsToDelete = questions.Where(qc => qc.Id == 0);
            questions.Remove(questionsToDelete.FirstOrDefault());
        }

        return questions.ToList();
    }

    public virtual int GetCountQuestionsAggregated(
        int userId,
        bool inCategoryOnly = false,
        int categoryId = 0)
    {
        if (inCategoryOnly)
        {
            return GetAggregatedQuestionsFromMemoryCache(
                userId,
                true,
                false,
                categoryId
            ).Count;
        }

        return GetAggregatedQuestionsFromMemoryCache(userId)
            .Count;
    }

    public virtual bool HasPublicParent()
    {
        return Parents().Any(c => c.Visibility == CategoryVisibility.All);
    }

    public bool IsStartPage()
    {
        if (Id == RootCategory.RootCategoryId)
            return true;

        if (Parents().Count == 0)
            return true;

        return Id == Creator.StartTopicId;
    }

    public virtual List<CategoryCacheItem> Parents()
    {
        return ParentRelations.Any()
            ? ParentRelations
                .Select(x => EntityCache.GetCategory(x.ParentId))
                .Where(x => x != null)
                .ToList()!
            : new List<CategoryCacheItem>();
    }

    public static IEnumerable<CategoryCacheItem> ToCacheCategories(List<Category> categories)
    {
        return categories.Select(c => ToCacheCategory(c));
    }

    public static IEnumerable<CategoryCacheItem> ToCacheCategories(IEnumerable<Category> categories)
    {
        return categories.Select(c => ToCacheCategory(c));
    }

    public static CategoryCacheItem ToCacheCategory(Category category)
    {
        var userEntityCacheCategoryRelations = new CategoryCacheRelation();

        var creatorId = category.Creator == null ? -1 : category.Creator.Id;
        var parentRelations = EntityCache.GetParentRelationsByChildId(category.Id);
        var childRelations = TopicOrderer.Sort(category.Id);
        var categoryCacheItem = new CategoryCacheItem
        {
            Id = category.Id,
            ChildRelations = childRelations,
            ParentRelations = parentRelations,
            CategoriesToExcludeIdsString = category.CategoriesToExcludeIdsString,
            CategoriesToIncludeIdsString = category.CategoriesToIncludeIdsString,
            Content = category.Content,
            CorrectnessProbability = category.CorrectnessProbability,
            CorrectnessProbabilityAnswerCount = category.CorrectnessProbabilityAnswerCount,
            CountQuestionsAggregated = category.CountQuestionsAggregated,
            CreatorId = creatorId,
            CustomSegments = category.CustomSegments,
            Description = category.Description,
            DisableLearningFunctions = category.DisableLearningFunctions,
            FormerSetId = category.FormerSetId,
            IsHistoric = category.IsHistoric,
            Name = category.Name,
            SkipMigration = category.SkipMigration,
            Visibility = category.Visibility,
            TopicMarkdown = category.TopicMarkdown,
            TotalRelevancePersonalEntries = category.TotalRelevancePersonalEntries,
            Type = category.Type,
            TypeJson = category.TypeJson,
            Url = category.Url,
            UrlLinkText = category.UrlLinkText,
            WikipediaURL = category.WikipediaURL,
            DateCreated = category.DateCreated,
            AuthorIds = category.AuthorIdsInts ?? new[] { creatorId },
            TextIsHidden = category.TextIsHidden,
        };
        return categoryCacheItem;
    }

    public void UpdateCountQuestionsAggregated(int userId)
    {
        CountQuestionsAggregated = GetCountQuestionsAggregated(userId);
    }

    private Dictionary<int, CategoryCacheItem> VisibleChildCategories(
        CategoryCacheItem parentCacheItem,
        PermissionCheck permissionCheck,
        Dictionary<int, CategoryCacheItem> _previousVisibleVisited = null)
    {
        var visibleVisited = new Dictionary<int, CategoryCacheItem>();

        if (_previousVisibleVisited != null)
        {
            visibleVisited = _previousVisibleVisited;
        }

        if (parentCacheItem.ChildRelations != null)
        {
            foreach (var r in parentCacheItem.ChildRelations)
            {
                if (!visibleVisited.ContainsKey(r.ChildId))
                {
                    var child = EntityCache.GetCategory(r.ChildId);
                    if (permissionCheck.CanView(child))
                    {
                        visibleVisited.Add(r.ChildId, child);
                        VisibleChildCategories(child, permissionCheck, visibleVisited);
                    }
                }
            }
        }

        return visibleVisited;
    }
}