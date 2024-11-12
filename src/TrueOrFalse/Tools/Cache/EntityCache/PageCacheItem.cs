using Seedworks.Lib.Persistence;
using System.Diagnostics;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class PageCacheItem : IPersistable
{
    public int CreatorId;

    public PageCacheItem()
    {
    }

    public PageCacheItem(string name)
    {
        Name = name;
    }

    public virtual UserCacheItem Creator => EntityCache.GetUserById(CreatorId);

    public virtual int[] AuthorIds { get; set; }
    public virtual string CategoriesToExcludeIdsString { get; set; }
    public virtual string CategoriesToIncludeIdsString { get; set; }
    public virtual IList<PageRelationCache> ParentRelations { get; set; }
    public virtual IList<PageRelationCache> ChildRelations { get; set; }

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
    public virtual string PageMarkdown { get; set; }

    public virtual int TotalRelevancePersonalEntries { get; set; }

    public virtual PageType Type { get; set; }

    public virtual string TypeJson { get; set; }

    public virtual string Url { get; set; }

    public virtual string UrlLinkText { get; set; }
    public virtual PageVisibility Visibility { get; set; }
    public bool IsPublic => Visibility == PageVisibility.All;
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
    /// Get Aggregated Pages
    /// </summary>
    /// <param name="permissionCheck"></param>
    /// <param name="includingSelf"></param>
    /// <returns>Dictionary&lt;int, CategoryCacheItem&gt;</returns>
    public Dictionary<int, PageCacheItem> AggregatedCategories(
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
    /// Get Aggregated Pages
    /// </summary>
    /// <param name="permissionCheck"></param>
    /// <param name="includingSelf"></param>
    /// <returns>Dictionary&lt;int, CategoryCacheItem&gt;</returns>
    public Dictionary<int, PageCacheItem> GetAllAggregatedPages(bool includingSelf = true)
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

    private Dictionary<int, PageCacheItem> VisibleChildCategories(
        PageCacheItem parentCacheItem,
        PermissionCheck permissionCheck,
        Dictionary<int, PageCacheItem> _previousVisibleVisited = null)
    {
        var visibleVisited = new Dictionary<int, PageCacheItem>();

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
                    var child = EntityCache.GetPage(r.ChildId);
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
        int pageId = 0)
    {
        IList<QuestionCacheItem> questions;

        if (fullList)
        {
            questions = AggregatedCategories(
                    new PermissionCheck(userId))
                .SelectMany(c => EntityCache.GetQuestionsForPage(c.Key))
                .Distinct().ToList();
        }
        else
        {
            questions = EntityCache.GetQuestionsForPage(pageId)
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
        int pageId = 0)
    {
        if (inCategoryOnly)
        {
            return GetAggregatedQuestionsFromMemoryCache(
                userId,
                true,
                false,
                pageId
            ).Count;
        }

        return GetAggregatedQuestionsFromMemoryCache(userId)
            .Count;
    }

    public virtual bool HasPublicParent()
    {
        return Parents().Any(c => c.Visibility == PageVisibility.All);
    }

    public bool IsStartPage()
    {
        if (Id == RootPage.RootCategoryId)
            return true;

        if (Parents().Count == 0)
            return true;

        return Id == Creator.StartPageId;
    }

    public virtual List<PageCacheItem> Parents()
    {
        return ParentRelations.Any()
            ? ParentRelations
                .Select(x => EntityCache.GetPage(x.ParentId))
                .Where(x => x != null)
                .ToList()!
            : new List<PageCacheItem>();
    }

    public static IEnumerable<PageCacheItem> ToCacheCategories(IEnumerable<Page> categories, IList<PageViewRepo.PageViewSummaryWithId> views, IList<PageChange> categoryChanges)
    {
        var categoryViews = views
            .GroupBy(cv => cv.PageId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var categoryChangesDictionary = categoryChanges
            .GroupBy(cc => cc.Page?.Id ?? -1)
            .ToDictionary(g => g.Key, g => g.ToList());

        return categories.Select(c =>
        {
            categoryViews.TryGetValue(c.Id, out var categoryViewsWithId);
            categoryChangesDictionary.TryGetValue(c.Id, out var categoryChanges);
            return ToCacheCategory(c, categoryViewsWithId, categoryChanges);
        });
    }

    public static PageCacheItem ToCacheCategory(Page page, List<PageViewRepo.PageViewSummaryWithId>? views = null, List<PageChange>? categoryChanges = null)
    {
        var creatorId = page.Creator == null ? -1 : page.Creator.Id;
        var parentRelations = EntityCache.GetParentRelationsByChildId(page.Id);
        var childRelations = PageOrderer.Sort(page.Id);
        var categoryCacheItem = new PageCacheItem
        {
            Id = page.Id,
            ChildRelations = childRelations,
            ParentRelations = parentRelations,
            CategoriesToExcludeIdsString = page.CategoriesToExcludeIdsString,
            CategoriesToIncludeIdsString = page.CategoriesToIncludeIdsString,
            Content = page.Content,
            CorrectnessProbability = page.CorrectnessProbability,
            CorrectnessProbabilityAnswerCount = page.CorrectnessProbabilityAnswerCount,
            CountQuestionsAggregated = page.CountQuestionsAggregated,
            CreatorId = creatorId,
            CustomSegments = page.CustomSegments,
            Description = page.Description,
            DisableLearningFunctions = page.DisableLearningFunctions,
            IsHistoric = page.IsHistoric,
            Name = page.Name,
            SkipMigration = page.SkipMigration,
            Visibility = page.Visibility,
            PageMarkdown = page.Markdown,
            TotalRelevancePersonalEntries = page.TotalRelevancePersonalEntries,
            Type = page.Type,
            TypeJson = page.TypeJson,
            Url = page.Url,
            UrlLinkText = page.UrlLinkText,
            WikipediaURL = page.WikipediaURL,
            DateCreated = page.DateCreated,
            AuthorIds = page.AuthorIdsInts ?? [creatorId],
            TextIsHidden = page.TextIsHidden,
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
                PageEditData? previousData = null;
                PageEditData currentData;
                int? previousId = null;
                var categoryChangeCacheItems = new List<PageChangeCacheItem>();
                var currentGroupedCacheItem = new List<PageChangeCacheItem>();

                foreach (var curr in categoryChanges)
                {
                    if (curr.DataVersion == 1)
                    {
                        currentData = PageEditDataV1.CreateFromJson(curr.Data);
                    }
                    else if (curr.DataVersion == 2)
                    {
                        currentData = PageEditData_V2.CreateFromJson(curr.Data);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException($"Invalid data version number {curr.DataVersion} for category change id {curr.Id}");
                    }

                    if (currentData == null)
                        continue;

                    var currentCacheItem = PageChangeCacheItem.ToPageChangeCacheItem(curr, currentData, previousData, previousId);

                    if (categoryChangeCacheItems.Count > 0)
                    {
                        if (PageChangeCacheItem.CanBeGrouped(categoryChangeCacheItems.Last(), currentCacheItem))
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
                            var groupedCategoryChangeCacheItem = PageChangeCacheItem.ToGroupedCategoryChangeCacheItem(currentGroupedCacheItem);
                            categoryChangeCacheItems.Add(groupedCategoryChangeCacheItem);
                            currentGroupedCacheItem = new List<PageChangeCacheItem>();
                        }
                    }

                    categoryChangeCacheItems.Add(currentCacheItem);
                    previousData = currentData;
                    previousId = curr.Id;
                }

                // handle last group
                if (currentGroupedCacheItem.Count > 1)
                {
                    var groupedCategoryChangeCacheItem = PageChangeCacheItem.ToGroupedCategoryChangeCacheItem(currentGroupedCacheItem);
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

    public void AddPageView(DateTime date)
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

    private Dictionary<int, PageCacheItem> AllChildCategories(
        PageCacheItem parentCacheItem,
        Dictionary<int, PageCacheItem> _previousVisited = null)
    {
        var visibleVisited = new Dictionary<int, PageCacheItem>();

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
                    var child = EntityCache.GetPage(r.ChildId);

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

    public virtual List<PageChangeCacheItem> CategoryChangeCacheItems { get; set; }

    public enum FeedType
    {
        Page,
        Question
    }

    public record struct FeedItem(DateTime DateCreated, PageChangeCacheItem? PageChangeCacheItem, QuestionChangeCacheItem? QuestionChangeCacheItem);
    public (IEnumerable<FeedItem>, int maxCount) GetVisibleFeedItemsByPage(PermissionCheck permissionCheck, int userId, int page, int pageSize = 100, bool getDescendants = true, bool getQuestions = false, bool getItemsInGroup = false)
    {
        IEnumerable<FeedItem> changes;

        if (getDescendants)
        {
            var allVisibleDescendants = GraphService.VisibleDescendants(Id, permissionCheck, userId);

            var allPages = new List<PageCacheItem> { this }.Concat(allVisibleDescendants).ToList();
            var unsortedPageChanges = allPages
                .Where(c => c != null && c.CategoryChangeCacheItems != null)
                .SelectMany(c => c.CategoryChangeCacheItems)
                .Select(tc => ToFeedItem(tc, null));

            if (getQuestions)
            {
                var allQuestions = GetAggregatedQuestionsFromMemoryCache(userId, onlyVisible: true, fullList: true, pageId: Id);
                var unsortedQuestionChanges = allQuestions
                    .Where(q => q != null && q.QuestionChangeCacheItems != null)
                    .SelectMany(q => q.QuestionChangeCacheItems)
                    .Select(qc => ToFeedItem(null, qc));

                changes = unsortedPageChanges
                    .Concat(unsortedQuestionChanges);
            }
            else
            {
                changes = unsortedPageChanges;
            }
        }
        else
        {
            var topicChanges = CategoryChangeCacheItems.Select(tc => ToFeedItem(tc, null));

            if (getQuestions)
            {
                var allQuestions = GetAggregatedQuestionsFromMemoryCache(userId, onlyVisible: true, fullList: false, pageId: Id);
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
        PageChangeCacheItem? previousChange = null;

        changes = changes
            .OrderByDescending(c => c.DateCreated)
            .ToList();

        foreach (var c in changes)
        {

            if (!IsVisibleForUser(userId, c))
            {
                continue;
            }

            if (c.PageChangeCacheItem == null)
            {
                if (c.QuestionChangeCacheItem != null)
                    visibleChanges.Add(c);

                continue;
            }

            if (!IsGroupItem(c.PageChangeCacheItem!, getItemsInGroup))
            {
                continue;
            }

            if (!IsDuplicateOfDelete(c.PageChangeCacheItem!, topicChangeIds, questionChangeIds))
            {
                continue;
            }

            if (!getItemsInGroup && IsPartOfPageCreate(previousChange, c.PageChangeCacheItem) && previousChange?.PageId == visibleChanges.LastOrDefault().PageChangeCacheItem?.PageId)
            {
                visibleChanges.RemoveAt(visibleChanges.Count - 1);
            }

            previousChange = c.PageChangeCacheItem;
            visibleChanges.Add(c);
        }

        var pagedChanges = visibleChanges.Distinct()
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return (pagedChanges, visibleChanges.Count());
    }
    private FeedItem ToFeedItem(PageChangeCacheItem? categoryChangeCacheItem = null, QuestionChangeCacheItem? questionChangeCacheItem = null)
    {
        if (categoryChangeCacheItem != null)
            return new FeedItem
            {
                DateCreated = categoryChangeCacheItem.DateCreated,
                PageChangeCacheItem = categoryChangeCacheItem
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
        if (feedItem.PageChangeCacheItem != null)
            return feedItem.PageChangeCacheItem.Visibility != PageVisibility.Owner || (feedItem.PageChangeCacheItem.AuthorId == userId || feedItem.PageChangeCacheItem.Page.CreatorId == userId);

        if (feedItem.QuestionChangeCacheItem != null)
            return feedItem.QuestionChangeCacheItem.Visibility != QuestionVisibility.Owner || (feedItem.QuestionChangeCacheItem.AuthorId == userId || feedItem.QuestionChangeCacheItem.Question.CreatorId == userId);

        return false;
    }

    private bool IsGroupItem(PageChangeCacheItem change, bool getItemsInGroup)
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

    private bool IsDuplicateOfDelete(PageChangeCacheItem change, HashSet<int> topicChangeIds, HashSet<int> questionChangeIds)
    {

        var deleteChangeId = change.PageChangeData.DeleteData?.DeleteChangeId;
        if (deleteChangeId == null)
            return true;

        int changeId = deleteChangeId.Value;

        switch (change.Type)
        {
            case PageChangeType.ChildPageDeleted:
                return topicChangeIds.Add(changeId);

            case PageChangeType.QuestionDeleted:
                return questionChangeIds.Add(changeId);

            default:
                return true;
        }
    }

    private bool IsPartOfPageCreate(PageChangeCacheItem? previousChange, PageChangeCacheItem currentChange)
    {
        if (previousChange == null || previousChange.Type != PageChangeType.Relations)
            return false;

        if (currentChange.Type == PageChangeType.Create && currentChange.PageId ==
            previousChange.PageChangeData.RelationChange.AddedChildIds.LastOrDefault())
            return true;

        return false;
    }

    public void AddPageChangeToPageChangeCacheItems(PageChange pageChange)
    {
        CategoryChangeCacheItems ??= new List<PageChangeCacheItem>();

        var currentData = pageChange.GetPageChangeData();
        var previousChange = CategoryChangeCacheItems.FirstOrDefault();
        PageEditData? previousData = CategoryChangeCacheItems.Count > 0 ? previousChange.GetPageChangeData() : null;
        var previousId = CategoryChangeCacheItems.Count > 0 ? previousChange.Id : (int?)null;

        var newCacheItem = PageChangeCacheItem.ToPageChangeCacheItem(pageChange, currentData, previousData, previousId);
        HandleNewGroup(newCacheItem);
    }

    private void HandleNewGroup(PageChangeCacheItem newCacheItem)
    {
        if (CategoryChangeCacheItems.Count > 0)
        {
            var previousCacheItem = CategoryChangeCacheItems.First();

            if (PageChangeCacheItem.CanBeGrouped(previousCacheItem, newCacheItem))
            {
                if (previousCacheItem.IsGroup)
                {
                    var newGroupedCacheItems = new List<PageChangeCacheItem>(previousCacheItem.GroupedCategoryChangeCacheItems);
                    newCacheItem.IsPartOfGroup = true;
                    newGroupedCacheItems.Add(newCacheItem);

                    CategoryChangeCacheItems[0] = newCacheItem;
                    var newGroup = PageChangeCacheItem.ToGroupedCategoryChangeCacheItem(newGroupedCacheItems);
                    CategoryChangeCacheItems.Insert(0, newGroup);
                }
                else
                {
                    previousCacheItem.IsPartOfGroup = true;
                    newCacheItem.IsPartOfGroup = true;

                    var groupedCacheItems = new List<PageChangeCacheItem> { previousCacheItem, newCacheItem };
                    CategoryChangeCacheItems.Insert(0, newCacheItem);
                    var newGroup = PageChangeCacheItem.ToGroupedCategoryChangeCacheItem(groupedCacheItems);
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

