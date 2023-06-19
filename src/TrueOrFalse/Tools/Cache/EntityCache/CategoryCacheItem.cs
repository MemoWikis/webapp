using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class CategoryCacheItem
{
    public int CreatorId;

    private IEnumerable<int> _categoriesToExcludeIds;

    private IEnumerable<int> _categoriesToIncludeIds;

    public CategoryCacheItem()
    {
    }

    public CategoryCacheItem(string name)
    {
        Name = name;
    }

    public virtual UserCacheItem Creator => EntityCache.GetUserById(CreatorId);
    public virtual int[] AuthorIds { get; set; }

    public virtual CategoryCachedData CachedData { get; set; } = new();

    public virtual string CategoriesToExcludeIdsString { get; set; }
    public virtual string CategoriesToIncludeIdsString { get; set; }
    public virtual IList<CategoryCacheRelation> CategoryRelations { get; set; }
    public virtual string Content { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }
    public virtual int CountQuestions { get; set; }

    public int CountQuestionsAggregated { get; set; }
    public virtual string CustomSegments { get; set; }

    public virtual DateTime DateCreated { get; set; }
    public virtual string Description { get; set; }

    public virtual IList<int> DirectChildrenIds { get; set; }

    public virtual bool DisableLearningFunctions { get; set; }

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

    public Dictionary<int, CategoryCacheItem> AggregatedCategories(PermissionCheck permissionCheck, bool includingSelf = true)
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

    public virtual IList<CategoryCacheItem> CategoriesToExclude()
    {
        return !string.IsNullOrEmpty(CategoriesToExcludeIdsString)
            ? ToCacheCategories(Sl.R<CategoryRepository>().GetByIdsFromString(CategoriesToExcludeIdsString)).ToList()
            : new List<CategoryCacheItem>();
    }

    public virtual IEnumerable<int> CategoriesToExcludeIds()
    {
        return _categoriesToExcludeIds ?? (_categoriesToExcludeIds = CategoriesToExcludeIdsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x)));
    }

    public virtual IList<CategoryCacheItem> CategoriesToInclude()
    {
        return !string.IsNullOrEmpty(CategoriesToIncludeIdsString)
            ? ToCacheCategories(Sl.R<CategoryRepository>().GetByIdsFromString(CategoriesToIncludeIdsString)).ToList()
            : new List<CategoryCacheItem>();
    }

    public virtual IEnumerable<int> CategoriesToIncludeIds()
    {
        return _categoriesToIncludeIds ?? (_categoriesToIncludeIds = CategoriesToIncludeIdsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x)));
    }

    public bool Contains(CategoryCacheRelation categoryRelation)
    {
        return CategoryRelations.Any(
            cr => cr.RelatedCategoryId == categoryRelation.RelatedCategoryId
        );
    }

    public virtual IList<int> GetAggregatedQuestionIdsFromMemoryCache(PermissionCheck permissionCheck)
    {
        return AggregatedCategories(permissionCheck)
            .SelectMany(c => EntityCache.GetQuestionsIdsForCategory(c.Key))
            .Distinct()
            .ToList();
    }


    public virtual IList<QuestionCacheItem> GetAggregatedQuestionsFromMemoryCache(
        bool onlyVisible = true,
        bool fullList = true,
        int categoryId = 0)
    {
        IList<QuestionCacheItem> questions;

        if (fullList)
        {
            questions = AggregatedCategories()
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
            questions = questions.Where(permissionCheck.CanView).ToList();
        }

        if (questions.Any(q => q.Id == 0))
        {
            var questionsToDelete = questions.Where(qc => qc.Id == 0);
            questions.Remove(questionsToDelete.FirstOrDefault());
        }

        return questions.ToList();
    }

    public virtual int GetCountQuestionsAggregated(bool inCategoryOnly = false, int categoryId = 0)
    {
        if (inCategoryOnly)
        {
            return GetAggregatedQuestionsFromMemoryCache(true, false, categoryId).Count;
        }

        return GetAggregatedQuestionsFromMemoryCache().Count;
    }

    public virtual bool HasPublicParent()
    {
        return ParentCategories().Any(c => c.Visibility == CategoryVisibility.All);
    }

    public virtual bool HasRelation(CategoryCacheRelation newRelation)
    {
        foreach (var categoryRelation in CategoryRelations)
        {
            if (CategoryCacheRelation.IsCategorRelationEqual(categoryRelation, newRelation))
            {
                return true;
            }
        }

        return false;
    }

    public virtual bool IsInWishknowledge()
    {
        return SessionUserCache.IsInWishknowledge(Sl.CurrentUserId, Id);
    }

    public virtual bool IsSpoiler(QuestionCacheItem question)
    {
        return IsSpoilerCategory.Yes(Name, question);
    }

    public bool IsStartPage()
    {
        if (Id == RootCategory.RootCategoryId)
        {
            return true;
        }

        if (Creator != null)
        {
            return Id == Creator.StartTopicId;
        }

        return false;
    }

    public virtual bool IsStartTopicModified()
    {
        if (CachedData.ChildrenIds.Count == 0)
        {
            return false;
        }

        return EntityCache.GetCategories(CachedData.ChildrenIds)
            .Count(cci => cci.Visibility == CategoryVisibility.All) > 0;
    }

    public virtual IList<CategoryCacheItem> ParentCategories(bool getFromEntityCache = false)
    {
        return CategoryRelations != null && CategoryRelations.Any()
            ? CategoryRelations
                .Select(x => EntityCache.GetCategory(x.RelatedCategoryId))
                .ToList()
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
        var categoryCacheItem = new CategoryCacheItem
        {
            Id = category.Id,
            CachedData = new CategoryCachedData(),
            CategoryRelations = userEntityCacheCategoryRelations.ToListCategoryRelations(category.CategoryRelations),
            CategoriesToExcludeIdsString = category.CategoriesToExcludeIdsString,
            CategoriesToIncludeIdsString = category.CategoriesToIncludeIdsString,
            Content = category.Content,
            CorrectnessProbability = category.CorrectnessProbability,
            CorrectnessProbabilityAnswerCount = category.CorrectnessProbabilityAnswerCount,
            CountQuestions = category.CountQuestions,
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
            AuthorIds = category.AuthorIdsInts ?? new[] { creatorId }
        };
        return categoryCacheItem;
    }

    public ConcurrentDictionary<int, CategoryCacheItem> ToConcurrentDictionary(
        ConcurrentDictionary<int, Category> concurrentDictionary)
    {
        var concDic = new ConcurrentDictionary<int, CategoryCacheItem>();

        foreach (var keyValuePair in concurrentDictionary)
        {
            concDic.TryAdd(keyValuePair.Key, ToCacheCategory(keyValuePair.Value));
        }

        return concDic;
    }

    public IEnumerable<CategoryCacheItem> ToIEnumerable(
        IEnumerable<Category> categoryList,
        bool withCachedData = false,
        bool withRealtions = false)
    {
        var categories = new List<CategoryCacheItem>();

        foreach (var category in categoryList)
        {
            categories.Add(ToCacheCategory(category));
        }

        return categories;
    }

    public void UpdateCountQuestionsAggregated()
    {
        CountQuestionsAggregated = GetCountQuestionsAggregated();
    }

    private Dictionary<int, CategoryCacheItem> VisibleChildCategories(
        CategoryCacheItem parentCacheItem,
        PermissionCheck permissionCheck,
        Dictionary<int, CategoryCacheItem> _previousVisibleVisited = null)
    {
        var visibleVisited = new Dictionary<int, CategoryCacheItem>();
        if (parentCacheItem.DirectChildrenIds == null)
        {
            parentCacheItem.DirectChildrenIds = EntityCache.GetChildren(parentCacheItem).Select(cci => cci.Id).ToList();
            EntityCache.AddOrUpdate(parentCacheItem);
        }

        if (_previousVisibleVisited != null)
        {
            visibleVisited = _previousVisibleVisited;
        }

        if (parentCacheItem.DirectChildrenIds != null)
        {
            foreach (var childId in parentCacheItem.DirectChildrenIds)
            {
                if (!visibleVisited.ContainsKey(childId))
                {
                    var child = EntityCache.GetCategory(childId);
                    if (permissionCheck.CanView(child))
                    {
                        visibleVisited.Add(childId, child);
                        VisibleChildCategories(child,permissionCheck, visibleVisited);
                    }
                }
            }
        }

        return visibleVisited;
    }
}