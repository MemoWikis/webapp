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

    public virtual UserCacheItem Creator => EntityCache.GetUserById(CreatorId);

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

    public virtual int TotalViews { get; set; }
    public virtual List<DailyViews> ViewsOfPast90Days { get; set; }

    public virtual List<DailyViews> GetViewsOfPast90Days()
    {
        var startDate = DateTime.Now.Date.AddDays(-90);
        var endDate = DateTime.Now.Date;

        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(d => startDate.AddDays(d));

        if (ViewsOfPast90Days == null)
            GenerateEmptyViewsOfPast90DaysList();

        ViewsOfPast90Days = dateRange
            .GroupJoin(
                ViewsOfPast90Days,
                date => date,
                view => view.Date,
                (date, views) => views.DefaultIfEmpty(new DailyViews { Date = date, Count = 0 })
            )
            .SelectMany(group => group)
            .OrderBy(view => view.Date)
            .ToList();

        return ViewsOfPast90Days;
    }

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

    public static IEnumerable<CategoryCacheItem> ToCacheCategories(IEnumerable<Category> categories, IList<CategoryViewRepo.TopicViewSummaryWithId> views, IList<CategoryChange> categoryChanges)
    {
        var categoryViews = views
            .GroupBy(cv => cv.Category_Id)
            .ToDictionary(g => g.Key, g => g.ToList());

        var categoryChangesDictionary = categoryChanges
            .GroupBy(cc => cc.Category?.Id ?? -1)
            .ToDictionary(g => g.Key, g => g.ToList());

        return categories.Select(c =>
        {
            categoryViews.TryGetValue(c.Id, out var categoryViewsWithId);
            categoryChangesDictionary.TryGetValue(c.Id, out var categoryChanges);
            return ToCacheCategory(c, categoryViewsWithId, categoryChanges);
        });
    }

    public static CategoryCacheItem ToCacheCategory(Category category, List<CategoryViewRepo.TopicViewSummaryWithId>? views = null, List<CategoryChange>? categoryChanges = null)
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

        if (EntityCache.IsFirstStart)
        {

            if (views != null)
            {
                categoryCacheItem.TotalViews = views.Count();

                var startDate = DateTime.Now.Date.AddDays(-90);
                var endDate = DateTime.Now.Date;

                var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
                    .Select(d => startDate.AddDays(d));

                categoryCacheItem.ViewsOfPast90Days = views.Where(qv => dateRange.Contains(qv.DateOnly))
                    .Select(qv => new DailyViews
                    {
                        Date = qv.DateOnly,
                        Count = qv.Count
                    })
                    .OrderBy(v => v.Date)
                    .ToList();
            }

            if (categoryChanges != null)
            {
                CategoryEditData? previousData = null;
                CategoryEditData currentData;
                int? previousId = null;
                var categoryChangeCacheItems = new List<CategoryChangeCacheItem>();
                var currentGroupedCacheItem = new List<CategoryChangeCacheItem>();

                foreach (var curr in categoryChanges)
                {
                    if (curr.DataVersion == 1)
                    {
                        currentData = CategoryEditData_V1.CreateFromJson(curr.Data);
                    }
                    else if (curr.DataVersion == 2)
                    {
                        currentData = CategoryEditData_V2.CreateFromJson(curr.Data);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException($"Invalid data version number {curr.DataVersion} for category change id {curr.Id}");
                    }

                    if (currentData == null)
                        continue;

                    var currentCacheItem = CategoryChangeCacheItem.ToCategoryChangeCacheItem(curr, currentData, previousData, previousId);

                    if (categoryChangeCacheItems.Count > 0)
                    {
                        if (CategoryChangeCacheItem.CanBeGrouped(categoryChangeCacheItems.Last(), currentCacheItem))
                        {
                            if (currentGroupedCacheItem.Count == 0)
                            {
                                var previousCacheItem = categoryChangeCacheItems.Last();
                                previousCacheItem.IsPartOfGroup = true;
                                currentGroupedCacheItem.Add(previousCacheItem);
                            }
                            currentCacheItem.IsPartOfGroup = true;
                            currentGroupedCacheItem.Add(currentCacheItem);
                        }
                        else if (currentGroupedCacheItem.Count > 1)
                        {
                            var groupedCategoryChangeCacheItem = CategoryChangeCacheItem.ToGroupedCategoryChangeCacheItem(currentGroupedCacheItem);
                            categoryChangeCacheItems.Add(groupedCategoryChangeCacheItem);
                            currentGroupedCacheItem = new List<CategoryChangeCacheItem>();
                        }
                    }

                    categoryChangeCacheItems.Add(currentCacheItem);
                    previousData = currentData;
                    previousId = curr.Id;
                }

                // handle last group
                if (currentGroupedCacheItem.Count > 1)
                {
                    var groupedCategoryChangeCacheItem = CategoryChangeCacheItem.ToGroupedCategoryChangeCacheItem(currentGroupedCacheItem);
                    categoryChangeCacheItems.Add(groupedCategoryChangeCacheItem);
                }

                categoryCacheItem.CategoryChangeCacheItems = categoryChangeCacheItems
                    .Distinct()
                    .OrderByDescending(change => change.DateCreated)
                    .ToList();
            }
        }
        return categoryCacheItem;
    }

    public void AddTopicView(DateTime date)
    {
        if (ViewsOfPast90Days == null)
            GenerateEmptyViewsOfPast90DaysList();
        var existingView = ViewsOfPast90Days.FirstOrDefault(v => v.Date.Date == date);

        if (existingView != null)
        {
            existingView.Count++;
        }
        else
        {
            ViewsOfPast90Days.Add(new DailyViews
            {
                Date = date,
                Count = 1
            });
        }
        TotalViews++;
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

    private void GenerateEmptyViewsOfPast90DaysList()
    {
        ViewsOfPast90Days = Enumerable.Range(0, 90)
            .Select(i => new DailyViews
            {
                Date = DateTime.Now.Date.AddDays(-i),
                Count = 0
            })
            .Reverse()
            .ToList();
    }

    public virtual List<CategoryChangeCacheItem> CategoryChangeCacheItems { get; set; }

    public enum FeedType
    {
        Topic,
        Question
    }

    public record struct FeedItem(DateTime DateCreated, CategoryChangeCacheItem? CategoryChangeCacheItem, QuestionChangeCacheItem? QuestionChangeCacheItem);
    public (IEnumerable<FeedItem>, int maxCount) GetVisibleFeedItemsByPage(PermissionCheck permissionCheck, int userId, int page, int pageSize = 100, bool getDescendants = true, bool getQuestions = false, bool getItemsInGroup = false)
    {
        IEnumerable<FeedItem> changes;

        if (getDescendants)
        {
            var allVisibleDescendants = GraphService.VisibleDescendants(Id, permissionCheck, userId);

            var allTopics = new List<CategoryCacheItem> { this }.Concat(allVisibleDescendants).ToList();
            var unsortedTopicChanges = allTopics
                .Where(c => c != null && c.CategoryChangeCacheItems != null)
                .SelectMany(c => c.CategoryChangeCacheItems)
                .Select(tc => ToFeedItem(tc, null));

            if (getQuestions)
            {
                var allQuestions = GetAggregatedQuestionsFromMemoryCache(userId, onlyVisible: true, fullList: true, categoryId: Id);
                var unsortedQuestionChanges = allQuestions
                    .Where(q => q != null && q.QuestionChangeCacheItems != null)
                    .SelectMany(q => q.QuestionChangeCacheItems)
                    .Select(qc => ToFeedItem(null, qc));

                changes = unsortedTopicChanges
                    .Concat(unsortedQuestionChanges);
            }
            else
            {
                changes = unsortedTopicChanges;
            }
        }
        else
        {
            var topicChanges = CategoryChangeCacheItems.Select(tc => ToFeedItem(tc, null));

            if (getQuestions)
            {
                var allQuestions = GetAggregatedQuestionsFromMemoryCache(userId, onlyVisible: true, fullList: false, categoryId: Id);
                var unsortedQuestionChanges = allQuestions
                    .Where(q => q != null && q.QuestionChangeCacheItems != null)
                    .SelectMany(q => q.QuestionChangeCacheItems)
                    .Select(qc => ToFeedItem(null, qc));
                changes = topicChanges
                    .Concat(unsortedQuestionChanges);
            }
            else
            {
                changes = topicChanges;
            }
        }

        var topicChangeIds = new HashSet<int>();
        var questionChangeIds = new HashSet<int>();
        var visibleChanges = new List<FeedItem>();
        CategoryChangeCacheItem? previousChange = null;

        changes = changes
            .OrderByDescending(c => c.DateCreated)
            .ToList();

        foreach (var c in changes)
        {

            if (!IsVisibleForUser(userId, c))
            {
                continue;
            }

            if (c.CategoryChangeCacheItem == null)
            {
                if (c.QuestionChangeCacheItem != null)
                    visibleChanges.Add(c);

                continue;
            }

            if (!IsGroupItem(c.CategoryChangeCacheItem!, getItemsInGroup))
            {
                continue;
            }

            if (!IsDuplicateOfDelete(c.CategoryChangeCacheItem!, topicChangeIds, questionChangeIds))
            {
                continue;
            }

            if (!getItemsInGroup && IsPartOfTopicCreate(previousChange, c.CategoryChangeCacheItem) && previousChange?.CategoryId == visibleChanges.LastOrDefault().CategoryChangeCacheItem?.CategoryId)
            {
                visibleChanges.RemoveAt(visibleChanges.Count - 1);
            }

            previousChange = c.CategoryChangeCacheItem;
            visibleChanges.Add(c);
        }

        var pagedChanges = visibleChanges.Distinct()
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return (pagedChanges, visibleChanges.Count());
    }
    private FeedItem ToFeedItem(CategoryChangeCacheItem? categoryChangeCacheItem = null, QuestionChangeCacheItem? questionChangeCacheItem = null)
    {
        if (categoryChangeCacheItem != null)
            return new FeedItem
            {
                DateCreated = categoryChangeCacheItem.DateCreated,
                CategoryChangeCacheItem = categoryChangeCacheItem
            };
        if (questionChangeCacheItem != null)
        {
            return new FeedItem
            {
                DateCreated = questionChangeCacheItem.DateCreated,
                QuestionChangeCacheItem = questionChangeCacheItem
            };
        }

        throw new Exception("no valid changeItem");
    }

    private bool IsVisibleForUser(int userId, FeedItem feedItem)
    {
        if (feedItem.CategoryChangeCacheItem != null)
            return feedItem.CategoryChangeCacheItem.Visibility != CategoryVisibility.Owner || (feedItem.CategoryChangeCacheItem.AuthorId == userId || feedItem.CategoryChangeCacheItem.Category.CreatorId == userId);

        if (feedItem.QuestionChangeCacheItem != null)
            return feedItem.QuestionChangeCacheItem.Visibility != QuestionVisibility.Owner || (feedItem.QuestionChangeCacheItem.AuthorId == userId || feedItem.QuestionChangeCacheItem.Question.CreatorId == userId);

        return false;
    }

    private bool IsGroupItem(CategoryChangeCacheItem change, bool getItemsInGroup)
    {
        if (getItemsInGroup)
        {
            if (change.IsGroup)
                return false;
        }
        else
        {
            if (change.IsPartOfGroup)
                return false;
        }

        return true;
    }

    private bool IsDuplicateOfDelete(CategoryChangeCacheItem change, HashSet<int> topicChangeIds, HashSet<int> questionChangeIds)
    {

        var deleteChangeId = change.CategoryChangeData.DeleteData?.DeleteChangeId;
        if (deleteChangeId == null)
            return true;

        int changeId = deleteChangeId.Value;

        switch (change.Type)
        {
            case CategoryChangeType.ChildTopicDeleted:
                return topicChangeIds.Add(changeId);

            case CategoryChangeType.QuestionDeleted:
                return questionChangeIds.Add(changeId);

            default:
                return true;
        }
    }

    private bool IsPartOfTopicCreate(CategoryChangeCacheItem? previousChange, CategoryChangeCacheItem currentChange)
    {
        if (previousChange == null || previousChange.Type != CategoryChangeType.Relations)
            return false;

        if (currentChange.Type == CategoryChangeType.Create && currentChange.CategoryId ==
            previousChange.CategoryChangeData.RelationChange.AddedChildIds.LastOrDefault())
            return true;

        return false;
    }

    public void AddCategoryChangeToCategoryChangeCacheItems(CategoryChange categoryChange)
    {
        CategoryChangeCacheItems ??= new List<CategoryChangeCacheItem>();

        var currentData = categoryChange.GetCategoryChangeData();
        var previousChange = CategoryChangeCacheItems.FirstOrDefault();
        CategoryEditData? previousData = CategoryChangeCacheItems.Count > 0 ? previousChange.GetCategoryChangeData() : null;
        var previousId = CategoryChangeCacheItems.Count > 0 ? previousChange.Id : (int?)null;

        var newCacheItem = CategoryChangeCacheItem.ToCategoryChangeCacheItem(categoryChange, currentData, previousData, previousId);
        HandleNewGroup(newCacheItem);
    }

    private void HandleNewGroup(CategoryChangeCacheItem newCacheItem)
    {
        if (CategoryChangeCacheItems.Count > 0)
        {
            var previousCacheItem = CategoryChangeCacheItems.First();

            if (CategoryChangeCacheItem.CanBeGrouped(previousCacheItem, newCacheItem))
            {
                if (previousCacheItem.IsGroup)
                {
                    var newGroupedCacheItems = new List<CategoryChangeCacheItem>(previousCacheItem.GroupedCategoryChangeCacheItems);
                    newCacheItem.IsPartOfGroup = true;
                    newGroupedCacheItems.Add(newCacheItem);

                    CategoryChangeCacheItems[0] = newCacheItem;
                    var newGroup = CategoryChangeCacheItem.ToGroupedCategoryChangeCacheItem(newGroupedCacheItems);
                    CategoryChangeCacheItems.Insert(0, newGroup);
                }
                else
                {
                    previousCacheItem.IsPartOfGroup = true;
                    newCacheItem.IsPartOfGroup = true;

                    var groupedCacheItems = new List<CategoryChangeCacheItem> { previousCacheItem, newCacheItem };
                    CategoryChangeCacheItems.Insert(0, newCacheItem);
                    var newGroup = CategoryChangeCacheItem.ToGroupedCategoryChangeCacheItem(groupedCacheItems);
                    CategoryChangeCacheItems.Insert(0, newGroup);
                }

                EntityCache.AddOrUpdate(this);
                return;
            }
        }

        CategoryChangeCacheItems.Insert(0, newCacheItem);
        EntityCache.AddOrUpdate(this);
    }
}

