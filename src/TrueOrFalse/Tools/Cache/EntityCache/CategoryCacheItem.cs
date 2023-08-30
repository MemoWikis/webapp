using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class CategoryCacheItem
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

    public virtual int GetCountQuestionsAggregated(int userId,
        bool inCategoryOnly = false,
        int categoryId = 0)
    {
        if (inCategoryOnly)
        {
            return GetAggregatedQuestionsFromMemoryCache(userId,
                true,
                false,
                categoryId)
                .Count;
        }

        return GetAggregatedQuestionsFromMemoryCache(userId)
            .Count;
    }

    public virtual bool HasPublicParent()
    {
        return ParentCategories().Any(c => c.Visibility == CategoryVisibility.All);
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

    public virtual IList<CategoryCacheItem> ParentCategories(bool getFromEntityCache = false)
    {
        return CategoryRelations != null && CategoryRelations.Any()
            ? CategoryRelations
                .Select(x => EntityCache.GetCategory(x.RelatedCategoryId))
                .ToList()
            : new List<CategoryCacheItem>();
    }

    public static IEnumerable<CategoryCacheItem> ToCacheCategories(List<Category> categories, ILogger logg)
    {
        return categories.Select(c => ToCacheCategory(c, logg));
    }

    public static IEnumerable<CategoryCacheItem> ToCacheCategories(IEnumerable<Category> categories, ILogger logg)
    {
        return categories.Select(c =>
            ToCacheCategory(c, logg));
    }

    public static CategoryCacheItem ToCacheCategory(Category category, ILogger logg)
    {
        var userEntityCacheCategoryRelations = new CategoryCacheRelation();

        var creatorId = category.Creator == null ? -1 : category.Creator.Id;
        var categoryCacheItem = new CategoryCacheItem
        {
            Id = category.Id,
            CachedData = new CategoryCachedData(),
            CategoryRelations = userEntityCacheCategoryRelations
                .ToListCategoryRelations(category.CategoryRelations, logg),
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
                        VisibleChildCategories(child, permissionCheck, visibleVisited);
                    }
                }
            }
        }

        return visibleVisited;
    }
}