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
    public virtual IList<PageRelationCache> ParentRelations { get; set; }
    public virtual IList<PageRelationCache> ChildRelations { get; set; }

    public virtual string? Content { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }
    public virtual int CountQuestions { get; set; }

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

    public virtual string TypeJson { get; set; }

    public virtual string Url { get; set; }

    public virtual string UrlLinkText { get; set; }
    public virtual PageVisibility Visibility { get; set; }
    public bool IsPublic => Visibility == PageVisibility.Public;
    public virtual string WikipediaURL { get; set; }

    public virtual int TotalViews { get; set; }
    public virtual List<DailyViews> ViewsOfPast90Days { get; set; }
    public virtual bool IsWiki { get; set; }
    public virtual string Language { get; set; } = "en";
    
    /// <summary>
    /// Current images in the page content - used for delayed cleanup to allow undo/revert
    /// </summary>
    public virtual string[] CurrentImageUrls { get; set; } = Array.Empty<string>();

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
    /// <returns>Dictionary&lt;int, PageCacheItem&gt;</returns>
    public Dictionary<int, PageCacheItem> VisibleAggregatedPages(
        PermissionCheck permissionCheck,
        bool includingSelf = true)
    {
        var visibleVisited = VisibleChildPages(this, permissionCheck);

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
    /// <param name="includingSelf"></param>
    /// <returns>Dictionary&lt;int, PageCacheItem&gt;</returns>
    public Dictionary<int, PageCacheItem> GetAllAggregatedPages(bool includingSelf = true)
    {
        var allChildPages = AllChildPages(this);

        if (includingSelf && !allChildPages.ContainsKey(Id))
        {
            allChildPages.Add(Id, this);
        }
        else
        {
            if (allChildPages.ContainsKey(Id))
            {
                allChildPages.Remove(Id);
            }
        }

        return allChildPages;
    }

    private Dictionary<int, PageCacheItem> VisibleChildPages(
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
                        VisibleChildPages(child, permissionCheck, visibleVisited);
                    }
                }
            }
        }

        return visibleVisited;
    }

    public virtual IList<QuestionCacheItem> GetAggregatedPublicQuestions() =>
        GetAggregatedQuestions(
            -1,
            onlyVisible: true,
            getQuestionsFromChildPages: true,
            permissionCheck: new PermissionCheck(new SessionlessUser(-1))
        );

    public virtual IList<QuestionCacheItem> GetDirectQuestions(
        int userId = -1,
        bool onlyVisible = true,
        int pageId = 0,
        PermissionCheck? permissionCheck = null) => GetAggregatedQuestions(
        userId,
        onlyVisible,
        getQuestionsFromChildPages: false,
        pageId,
        permissionCheck);

    public virtual IList<QuestionCacheItem> GetAggregatedQuestions(
        int userId,
        bool onlyVisible = true,
        bool getQuestionsFromChildPages = true,
        int pageId = 0,
        PermissionCheck? permissionCheck = null)
    {
        IList<QuestionCacheItem> questions;
        var sessionlessUser = new SessionlessUser(userId);

        if (getQuestionsFromChildPages)
        {
            if (onlyVisible)
            {
                permissionCheck ??= new PermissionCheck(sessionlessUser);
                questions = VisibleAggregatedPages(permissionCheck)
                    .SelectMany(c => EntityCache.GetQuestionsForPage(c.Key))
                    .Distinct().ToList();
            }
            else
            {
                questions = GetAllAggregatedPages()
                    .SelectMany(c => EntityCache.GetQuestionsForPage(c.Key))
                    .Distinct().ToList();
            }
        }
        else
        {
            if (pageId == 0)
                pageId = Id; // use current page if no pageId is given

            questions = EntityCache
                .GetQuestionsForPage(pageId)
                .Distinct()
                .ToList();
        }

        if (onlyVisible)
        {
            permissionCheck ??= new PermissionCheck(sessionlessUser);
            questions = questions.Where(permissionCheck.CanView).ToList();
        }

        if (questions.Any(q => q.Id == 0))
        {
            var questionsToDelete = questions.Where(qc => qc.Id == 0);
            questions.Remove(questionsToDelete.FirstOrDefault());
        }

        return questions.ToList();
    }

    public virtual IList<QuestionCacheItem> GetAllAggregatedQuestions() => GetAggregatedQuestions(
        -1,
        onlyVisible: false,
        getQuestionsFromChildPages: true,
        permissionCheck: null
    );

    public virtual int GetCountQuestionsAggregated(
        int userId,
        bool inPageOnly = false,
        int pageId = 0,
        PermissionCheck? permissionCheck = null)
    {
        if (inPageOnly)
        {
            return GetAggregatedQuestions(
                userId,
                true,
                false,
                pageId,
                permissionCheck
            ).Count;
        }

        return GetAggregatedQuestions(userId, permissionCheck: permissionCheck)
            .Count;
    }

    public virtual int GetQuestionViewCount(int userId,
        bool onlyVisible = true,
        bool fullList = true,
        int pageId = 0,
        PermissionCheck? permissionCheck = null) =>
        GetAggregatedQuestions(userId, onlyVisible, fullList, pageId, permissionCheck)
            .Sum(question => question.TotalViews);

    public virtual bool HasPublicParent()
    {
        return Parents().Any(c => c.Visibility == PageVisibility.Public);
    }

    public bool IsWikiType()
    {
        if (IsWiki)
            return true;

        if (Id == FeaturedPage.RootPageId)
            return true;

        if (Parents().Count == 0)
            return true;

        return false;
    }

    public virtual List<PageCacheItem> VisibleParents(PermissionCheck permissionCheck) =>
        Parents().Where(permissionCheck.CanView).ToList();

    public virtual List<PageCacheItem> Parents()
    {
        return ParentRelations.Any()
            ? ParentRelations
                .Select(x => EntityCache.GetPage(x.ParentId))
                .Where(x => x != null)
                .ToList()!
            : new List<PageCacheItem>();
    }

    public static IEnumerable<PageCacheItem> ToCachePages(IEnumerable<Page> pages,
        IList<PageViewSummaryWithId> views, IList<PageChange> changes)
    {
        var pageViewsDict = views
            .GroupBy(cv => cv.PageId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var pageChangesDict = changes
            .GroupBy(cc => cc.Page?.Id ?? -1)
            .ToDictionary(g => g.Key, g => g.ToList());

        return pages.Select(c =>
        {
            pageViewsDict.TryGetValue(c.Id, out var pageViews);
            pageChangesDict.TryGetValue(c.Id, out var pageChanges);
            return ToCachePage(c, pageViews, pageChanges);
        });
    }

    public static PageCacheItem ToCachePage(Page page, List<PageViewSummaryWithId>? views = null,
        List<PageChange>? pageChanges = null)
    {
        var creatorId = page.Creator == null ? -1 : page.Creator.Id;
        var parentRelations = EntityCache.GetParentRelationsByChildId(page.Id);
        var childRelations = PageOrderer.Sort(page.Id);
        var pageCacheItem = new PageCacheItem
        {
            Id = page.Id,
            ChildRelations = childRelations,
            ParentRelations = parentRelations,
            Content = page.Content,
            CorrectnessProbability = page.CorrectnessProbability,
            CorrectnessProbabilityAnswerCount = page.CorrectnessProbabilityAnswerCount,
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
            Url = page.Url,
            UrlLinkText = page.UrlLinkText,
            WikipediaURL = page.WikipediaURL,
            DateCreated = page.DateCreated,
            AuthorIds = page.AuthorIdsInts ?? [creatorId],
            TextIsHidden = page.TextIsHidden,
            IsWiki = page.IsWiki,
            Language = page.Language,
            CurrentImageUrls = Array.Empty<string>() // Will be populated on first content save
        };

        if (EntityCache.IsFirstStart)
        {
            SetPageViews(pageCacheItem, views);

            if (pageChanges != null)
            {
                PageEditData? previousData = null;
                PageEditData currentData;
                int? previousId = null;
                var pageChangeCacheItem = new List<PageChangeCacheItem>();
                var currentGroupedCacheItem = new List<PageChangeCacheItem>();

                foreach (var curr in pageChanges)
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
                        throw new ArgumentOutOfRangeException(
                            $"Invalid data version number {curr.DataVersion} for page change id {curr.Id}");
                    }

                    if (currentData == null)
                        continue;

                    var currentCacheItem =
                        PageChangeCacheItem.ToPageChangeCacheItem(curr, currentData, previousData, previousId);

                    if (pageChangeCacheItem.Count > 0)
                    {
                        if (PageChangeCacheItem.CanBeGrouped(pageChangeCacheItem.Last(), currentCacheItem))
                        {
                            if (currentGroupedCacheItem.Count == 0)
                            {
                                var previousCacheItem = pageChangeCacheItem.Last();
                                previousCacheItem.IsPartOfGroup = true;
                                currentGroupedCacheItem.Add(previousCacheItem);
                            }

                            currentCacheItem.IsPartOfGroup = true;
                            currentGroupedCacheItem.Add(currentCacheItem);
                        }
                        else if (currentGroupedCacheItem.Count > 1)
                        {
                            var groupedPageChangeCacheItem =
                                PageChangeCacheItem.ToGroupedPageChangeCacheItem(currentGroupedCacheItem);
                            pageChangeCacheItem.Add(groupedPageChangeCacheItem);
                            currentGroupedCacheItem = new List<PageChangeCacheItem>();
                        }
                    }

                    pageChangeCacheItem.Add(currentCacheItem);
                    previousData = currentData;
                    previousId = curr.Id;
                }

                // handle last group
                if (currentGroupedCacheItem.Count > 1)
                {
                    var groupedPageChangeCacheItem =
                        PageChangeCacheItem.ToGroupedPageChangeCacheItem(currentGroupedCacheItem);
                    pageChangeCacheItem.Add(groupedPageChangeCacheItem);
                }

                pageCacheItem.PageChangeCacheItems = pageChangeCacheItem
                    .Distinct()
                    .OrderByDescending(change => change.DateCreated)
                    .ToList();
            }
        }

        LanguageExtensions.SetContentLanguageOnAuthors(pageCacheItem);

        return pageCacheItem;
    }

    public static void SetPageViews(PageCacheItem pageCacheItem, List<PageViewSummaryWithId>? views = null)
    {
        if (views == null || views.Count == 0)
            return;

        pageCacheItem.TotalViews = (int)views.Sum(view => view.Count);

        var startDate = DateTime.Now.Date.AddDays(-90);
        var endDate = DateTime.Now.Date;

        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(d => startDate.AddDays(d));

        pageCacheItem.ViewsOfPast90Days = views.Where(qv => dateRange.Contains(qv.DateOnly))
            .Select(qv => new DailyViews { Date = qv.DateOnly, Count = qv.Count })
            .OrderBy(v => v.Date)
            .ToList();
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
            ViewsOfPast90Days.Add(new DailyViews { Date = date, Count = 1 });
        }

        TotalViews++;
    }

    private Dictionary<int, PageCacheItem> AllChildPages(
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
                    AllChildPages(child, visibleVisited);
                }
            }
        }

        return visibleVisited;
    }

    private void GenerateEmptyViewsOfPast90DaysList()
    {
        ViewsOfPast90Days = Enumerable.Range(0, 90)
            .Select(i => new DailyViews { Date = DateTime.Now.Date.AddDays(-i), Count = 0 })
            .Reverse()
            .ToList();
    }

    public virtual List<PageChangeCacheItem> PageChangeCacheItems { get; set; }

    public enum FeedType
    {
        Page,
        Question
    }

    public record struct FeedItem(
        DateTime DateCreated,
        PageChangeCacheItem? PageChangeCacheItem,
        QuestionChangeCacheItem? QuestionChangeCacheItem);

    public (IEnumerable<FeedItem>, int maxCount) GetVisibleFeedItemsByPage(PermissionCheck permissionCheck, int userId,
        int page, int pageSize = 100, bool getDescendants = true, bool getQuestions = false,
        bool getItemsInGroup = false)
    {
        IEnumerable<FeedItem> changes;

        if (getDescendants)
        {
            var allVisibleDescendants = GraphService.VisibleDescendants(Id, permissionCheck, userId);

            var allPages = new List<PageCacheItem> { this }.Concat(allVisibleDescendants).ToList();
            var unsortedPageChanges = allPages
                .Where(c => c != null && c.PageChangeCacheItems != null)
                .SelectMany(c => c.PageChangeCacheItems)
                .Select(tc => ToFeedItem(tc, null));

            if (getQuestions)
            {
                var allQuestions = GetAggregatedQuestions(userId, onlyVisible: true, getQuestionsFromChildPages: true,
                    pageId: Id);
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
            var pageChanges = PageChangeCacheItems.Select(tc => ToFeedItem(tc, null));

            if (getQuestions)
            {
                var allQuestions = GetAggregatedQuestions(userId, onlyVisible: true, getQuestionsFromChildPages: false,
                    pageId: Id);
                var unsortedQuestionChanges = allQuestions
                    .Where(q => q != null && q.QuestionChangeCacheItems != null)
                    .SelectMany(q => q.QuestionChangeCacheItems)
                    .Select(qc => ToFeedItem(null, qc));
                changes = pageChanges
                    .Concat(unsortedQuestionChanges);
            }
            else
            {
                changes = pageChanges;
            }
        }

        var pageChangeIds = new HashSet<int>();
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

            if (!IsDuplicateOfDelete(c.PageChangeCacheItem!, pageChangeIds, questionChangeIds))
            {
                continue;
            }

            if (!getItemsInGroup && IsPartOfPageCreate(previousChange, c.PageChangeCacheItem) &&
                previousChange?.PageId == visibleChanges.LastOrDefault().PageChangeCacheItem?.PageId)
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

    private FeedItem ToFeedItem(PageChangeCacheItem? pageChangeCacheItem = null,
        QuestionChangeCacheItem? questionChangeCacheItem = null)
    {
        if (pageChangeCacheItem != null)
            return new FeedItem
            {
                DateCreated = pageChangeCacheItem.DateCreated,
                PageChangeCacheItem = pageChangeCacheItem
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
            return feedItem.PageChangeCacheItem.Visibility != PageVisibility.Private ||
                   (feedItem.PageChangeCacheItem.AuthorId == userId ||
                    feedItem.PageChangeCacheItem.Page.CreatorId == userId);

        if (feedItem.QuestionChangeCacheItem != null)
            return feedItem.QuestionChangeCacheItem.Visibility != QuestionVisibility.Private ||
                   (feedItem.QuestionChangeCacheItem.AuthorId == userId ||
                    feedItem.QuestionChangeCacheItem.Question.CreatorId == userId);

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

    private bool IsDuplicateOfDelete(PageChangeCacheItem change, HashSet<int> pageChangeIds,
        HashSet<int> questionChangeIds)
    {
        var deleteChangeId = change.PageChangeData.DeleteData?.DeleteChangeId;
        if (deleteChangeId == null)
            return true;

        int changeId = deleteChangeId.Value;

        switch (change.Type)
        {
            case PageChangeType.ChildPageDeleted:
                return pageChangeIds.Add(changeId);

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
        PageChangeCacheItems ??= new List<PageChangeCacheItem>();

        var currentData = pageChange.GetPageChangeData();
        var previousChange = PageChangeCacheItems.FirstOrDefault();
        PageEditData? previousData = PageChangeCacheItems.Count > 0 ? previousChange.GetPageChangeData() : null;
        var previousId = PageChangeCacheItems.Count > 0 ? previousChange.Id : (int?)null;

        var newCacheItem = PageChangeCacheItem.ToPageChangeCacheItem(pageChange, currentData, previousData, previousId);
        HandleNewGroup(newCacheItem);
    }

    private void HandleNewGroup(PageChangeCacheItem newCacheItem)
    {
        if (PageChangeCacheItems.Count > 0)
        {
            var previousCacheItem = PageChangeCacheItems.First();

            if (PageChangeCacheItem.CanBeGrouped(previousCacheItem, newCacheItem))
            {
                if (previousCacheItem.IsGroup)
                {
                    var newGroupedCacheItems =
                        new List<PageChangeCacheItem>(previousCacheItem.GroupedPageChangeCacheItems);
                    newCacheItem.IsPartOfGroup = true;
                    newGroupedCacheItems.Add(newCacheItem);

                    PageChangeCacheItems[0] = newCacheItem;
                    var newGroup = PageChangeCacheItem.ToGroupedPageChangeCacheItem(newGroupedCacheItems);
                    PageChangeCacheItems.Insert(0, newGroup);
                }
                else
                {
                    previousCacheItem.IsPartOfGroup = true;
                    newCacheItem.IsPartOfGroup = true;

                    var groupedCacheItems = new List<PageChangeCacheItem> { previousCacheItem, newCacheItem };
                    PageChangeCacheItems.Insert(0, newCacheItem);
                    var newGroup = PageChangeCacheItem.ToGroupedPageChangeCacheItem(groupedCacheItems);
                    PageChangeCacheItems.Insert(0, newGroup);
                }

                EntityCache.AddOrUpdate(this);
                return;
            }
        }

        PageChangeCacheItems.Insert(0, newCacheItem);
        EntityCache.AddOrUpdate(this);
    }

    public IList<ShareCacheItem> GetDirectShares()
    {
        return EntityCache.GetPageShares(Id);
    }

    public virtual int VisibleChildrenCount(PermissionCheck permissionCheck, int userId)
    {
        return GraphService
            .VisibleDescendants(Id, permissionCheck, userId)
            .Count;
    }

    public virtual bool IsChildOfPersonalWiki(SessionUser sessionUser, PermissionCheck permissionCheck)
    {
        if (!sessionUser.IsLoggedIn)
        {
            return false;
        }

        return GraphService
            .VisibleDescendants(sessionUser.User.FirstWikiId, permissionCheck, sessionUser.UserId)
            .Any(descendant => descendant.Id == Id);
    }
}