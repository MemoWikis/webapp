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
    public bool IsPublic => Visibility == CategoryVisibility.All;
    public virtual string WikipediaURL { get; set; }
    public List<DailyViews> ViewsLast30DaysAggregatedTopic { get; set; }
    public List<DailyViews> ViewsLast30DaysTopic { get; set; }
    public List<DailyViews> ViewsLast30DaysAggregatedQuestions { get; set; }
    public List<DailyViews> ViewsLast30DaysQuestions { get; set; }


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
    /// <summary>
    /// Get Aggregated Topics
    /// </summary>
    /// <param name="permissionCheck"></param>
    /// <param name="includingSelf"></param>
    /// <returns>Dictionary&lt;int, CategoryCacheItem&gt;</returns>
    public Dictionary<int, CategoryCacheItem> GetAllAggregatedCategories(bool includingSelf = true)
    {
        var allChildCategories = AllChildCategories(this);

        if (includingSelf && !allChildCategories.ContainsKey(Id))
        {
            allChildCategories.Add(Id, this);
        }
        else
        {
            if (allChildCategories.ContainsKey(Id))
            {
                allChildCategories.Remove(Id);
            }
        }

        return allChildCategories;
    }

    public void AddTopicViews(List<DailyViews> aggregated30DaysTopicViews, List<DailyViews> selfLast30DaysTopicViews)
    {
        ViewsLast30DaysAggregatedTopic = aggregated30DaysTopicViews;
        ViewsLast30DaysTopic = selfLast30DaysTopicViews;

    }

    public void AddQuestionViews(List<DailyViews> aggregated30DaysQuestionViews, List<DailyViews> selfLast30DaysQuestionViews)
    {
        ViewsLast30DaysAggregatedQuestions = aggregated30DaysQuestionViews;
        ViewsLast30DaysQuestions = selfLast30DaysQuestionViews;

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

    public static IEnumerable<CategoryCacheItem> ToCacheCategories(IEnumerable<Category> categories)
    {
        return categories.Select(c => ToCacheCategory(c));
    }

    public static CategoryCacheItem ToCacheCategory(Category category)
    {
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
            AuthorIds = category.AuthorIdsInts ?? [creatorId],
            TextIsHidden = category.TextIsHidden,
        };
        return categoryCacheItem;
    }

    public void IncrementTodayViewCounters(bool isTodayQuestionView = true)
    {
        var today = DateTime.Now.Date;

        if (isTodayQuestionView)
        {
            var todayAggregatedQuestionViews = ViewsLast30DaysAggregatedQuestions.Single(c => c.Date == today);
            todayAggregatedQuestionViews.Views++;


            var todayQuestionViews = ViewsLast30DaysQuestions.Single(c => c.Date == today);
            todayQuestionViews.Views++;
        }
        else
        {
            var todayAggregatedQuestionViews = ViewsLast30DaysAggregatedTopic.Single(c => c.Date == today);
            todayAggregatedQuestionViews.Views++;


            var todayQuestionViews = ViewsLast30DaysTopic.Single(c => c.Date == today);
            todayQuestionViews.Views++;
        }

        EntityCache.AddOrUpdate(this);
    }

    private Dictionary<int, CategoryCacheItem> AllChildCategories(
        CategoryCacheItem parentCacheItem,
        Dictionary<int, CategoryCacheItem> _previousVisited = null)
    {
        var visibleVisited = new Dictionary<int, CategoryCacheItem>();

        if (_previousVisited != null)
        {
            visibleVisited = _previousVisited;
        }

        if (parentCacheItem.ChildRelations != null)
        {
            foreach (var r in parentCacheItem.ChildRelations)
            {
                if (!visibleVisited.ContainsKey(r.ChildId))
                {
                    var child = EntityCache.GetCategory(r.ChildId);

                    visibleVisited.Add(r.ChildId, child);
                    AllChildCategories(child, visibleVisited);

                }
            }
        }

        return visibleVisited;
    }
}
public class DailyViews()
{
    public DateTime Date { get; set; }
    public long Views { get; set; }
}


