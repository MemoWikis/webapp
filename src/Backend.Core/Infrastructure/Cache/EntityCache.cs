using System.Collections.Concurrent;
using System.Diagnostics;

public class EntityCache
{
    public const string CacheKeyUsers = "allUsers_EntityCache";
    public const string CacheKeyQuestions = "allQuestions_EntityCache";
    public const string CacheKeyPages = "allPages_EntityCache";
    public const string CacheKeyPageQuestionsList = "pageQuestionsList_EntityCache";
    public const string CacheKeyRelations = "allRelations_EntityCache";
    public const string CacheKeyPageShares = "pageShares_EntityCache";

    public static bool IsFirstStart = true;

    private static ConcurrentDictionary<int, UserCacheItem> Users =>
        MemoCache.Get<ConcurrentDictionary<int, UserCacheItem>>(CacheKeyUsers);

    public static ConcurrentDictionary<int, PageCacheItem> Pages =>
        MemoCache.Get<ConcurrentDictionary<int, PageCacheItem>>(CacheKeyPages);

    public static ConcurrentDictionary<int, QuestionCacheItem> Questions =>
        MemoCache.Get<ConcurrentDictionary<int, QuestionCacheItem>>(CacheKeyQuestions);

    private static ConcurrentDictionary<int, PageRelationCache> Relations =>
        MemoCache.Get<ConcurrentDictionary<int, PageRelationCache>>(CacheKeyRelations);

    private static ConcurrentDictionary<int, List<ShareCacheItem>> PageShares =>
        MemoCache.Get<ConcurrentDictionary<int, List<ShareCacheItem>>>(CacheKeyPageShares);

    /// <summary>
    /// Dictionary(key:pageId, value:questions)
    /// </summary>
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>>
        PageQuestionsList =>
        MemoCache.Get<ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>(
            CacheKeyPageQuestionsList);

    public static List<UserCacheItem> GetUsersByIds(IEnumerable<int> ids) =>
        ids.Select(id => GetUserById(id))
            .ToList();

    public static void AddViewsLast30DaysToPages(List<PageViewSummaryWithId> allPageViews, List<PageCacheItem> pageCacheItems)
    {
        var thirtyDaysAgo = DateTime.Now.AddDays(-30).Date;
        var pagesViewsLast30Days = allPageViews.Where(view => view.DateOnly >= thirtyDaysAgo).ToList();
        foreach (var pageCacheItem in pageCacheItems)
        {
            var aggregatedPages = pageCacheItem.GetAllAggregatedPages()
                .Select(t => t.Key);

            var aggregatedPageViews30Days = pagesViewsLast30Days
                .Where(view => aggregatedPages.Contains(view.PageId))
                .GroupBy(view => view.DateOnly)
                .Select(g => new { Date = g.Key, TotalCount = g.Sum(v => v.Count) })
                .OrderBy(result => result.Date)
                .Select(v => new DailyViews() { Date = v.Date, Count = v.TotalCount })
                .ToList();

            var pageViews30Days = pagesViewsLast30Days
                .Where(view => view.PageId == pageCacheItem.Id)
                .GroupBy(view => view.DateOnly)
                .Select(g => new { Date = g.Key, TotalCount = g.Sum(v => v.Count) })
                .OrderBy(result => result.Date)
                .Select(v => new DailyViews { Date = v.Date, Count = v.TotalCount })
                .ToList();

            DateTimeUtils.EnsureLastDaysIncluded(aggregatedPageViews30Days, 30);
            DateTimeUtils.EnsureLastDaysIncluded(pageViews30Days, 30);
        }
    }

    public static void AddViewsLast30DaysToQuestion(QuestionViewRepository questionViewRepo,
        List<PageCacheItem> pageCacheItems)
    {
        var watch = Stopwatch.StartNew();
        var questionViewsLast90Days = questionViewRepo.GetViewsForLastNDaysGroupByQuestionId(90);
        foreach (var pageCacheItem in pageCacheItems)
        {
            var aggregatedQuestionsFromAllAggregatedPages = pageCacheItem
                .GetAggregatedQuestions(2, false, true, pageCacheItem.Id)
                .Select(t => t.Id);

            var aggregatedQuestionsViews90Days = questionViewsLast90Days
                .Where(view => aggregatedQuestionsFromAllAggregatedPages.Contains(view.QuestionId))
                .GroupBy(view => view.DateOnly)
                .Select(g => new { Date = g.Key, TotalCount = g.Sum(v => v.Count) })
                .OrderBy(result => result.Date)
                .Select(v => new DailyViews() { Date = v.Date, Count = v.TotalCount })
                .ToList();

            var selfQuestionsFromPage = pageCacheItem.GetAggregatedQuestions(2, false, false, pageCacheItem.Id)
                .Select(t => t.Id);

            var pageQuestions90Days = questionViewsLast90Days
                .Where(view => selfQuestionsFromPage.Contains(view.QuestionId))
                .GroupBy(view => view.DateOnly)
                .Select(g => new { Date = g.Key, TotalCount = g.Sum(v => v.Count) })
                .OrderBy(result => result.Date)
                .Select(v => new DailyViews() { Date = v.Date, Count = v.TotalCount })
                .ToList();

            DateTimeUtils.EnsureLastDaysIncluded(aggregatedQuestionsViews90Days, 90);
            DateTimeUtils.EnsureLastDaysIncluded(pageQuestions90Days, 90);
        }

        var elapsedTime = watch.ElapsedMilliseconds;
        Log.Information(nameof(AddViewsLast30DaysToQuestion) + elapsedTime);
    }

    public static UserCacheItem? GetUserByIdNullable(int userId)
    {
        Users.TryGetValue(userId, out var user);
        return user;
    }

    public static UserCacheItem GetUserById(int userId)
    {
        if (Users.TryGetValue(userId, out var user))
            return user;

        return new UserCacheItem();
    }

    public static ConcurrentDictionary<int, ConcurrentDictionary<int, int>>
        GetPageQuestionsListForCacheInitializer(IList<QuestionCacheItem> questions)
    {
        var pageQuestionList = new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>();
        foreach (var question in questions)
        {
            UpdatePageQuestionList(pageQuestionList, question);
        }

        return pageQuestionList;
    }

    public static bool PageHasQuestion(int pageId) =>
        GetQuestionIdsForPage(pageId)?.Any() ?? false;

    public static IList<QuestionCacheItem> GetQuestionsForPage(int pageId) =>
        GetQuestionsByIds(GetQuestionIdsForPage(pageId));

    public static List<int> GetQuestionIdsForPage(int pageId)
    {
        PageQuestionsList.TryGetValue(pageId, out var questionIds);

        return questionIds?.Keys.ToList() ?? new List<int>();
    }

    public static IList<QuestionCacheItem> GetQuestionsByIds(IList<int> questionIds)
    {
        var questions = new List<QuestionCacheItem>();

        var cachedQuestions = Questions;

        foreach (var questionId in questionIds)
        {
            if (cachedQuestions.TryGetValue(questionId, out var questionToAdd))
            {
                questions.Add(questionToAdd);
            }
        }

        return questions;
    }

    public static IList<QuestionCacheItem> GetQuestionsByIds(IEnumerable<int> questionIds)
    {
        var questions = new List<QuestionCacheItem>();

        var cachedQuestions = Questions;

        foreach (var questionId in questionIds)
        {
            if (cachedQuestions.TryGetValue(questionId, out var questionToAdd))
            {
                questions.Add(questionToAdd);
            }
        }

        return questions;
    }

    public static IList<QuestionCacheItem> GetAllQuestions() => Questions.Values.ToList();

    public static QuestionCacheItem GetQuestionById(int questionId)
    {
        if (Questions.TryGetValue(questionId, out var question))
            return question;

        Log.Warning("QuestionId is not available");
        return new QuestionCacheItem();
    }

    private static void UpdatePageQuestionList(
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> pageQuestionsList,
        QuestionCacheItem question,
        List<int>? affectedPageIds = null)
    {
        DeleteQuestionFromRemovedPages(question, pageQuestionsList, affectedPageIds);

        AddQuestionToPages(question, pageQuestionsList);
    }

    private static void DeleteQuestionFromRemovedPages(
        QuestionCacheItem question,
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> pageQuestionsList,
        List<int>? affectedPageIds = null)
    {
        if (affectedPageIds != null)
        {
            foreach (var pageId in affectedPageIds.Except(question.Pages.GetIds()))
            {
                if (pageQuestionsList.ContainsKey(pageId))
                    pageQuestionsList[pageId]?.TryRemove(question.Id, out var outVar);
            }
        }
    }

    public static void AddQuestionsToPage(int pageId, List<int> questionIds)
    {
        foreach (int questionId in questionIds)
        {
            PageQuestionsList.AddOrUpdate(pageId, new ConcurrentDictionary<int, int>(),
                (k, existingList) => existingList);

            PageQuestionsList[pageId]?.AddOrUpdate(questionId, 0, (k, v) => 0);
        }
    }

    private static void AddQuestionToPages(
        QuestionCacheItem question,
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> pageQuestions,
        IList<PageCacheItem>? pages = null)
    {
        if (pages == null)
        {
            pages = GetPages(question.Pages.GetIds()).ToList();
        }

        foreach (var page in pages)
        {
            pageQuestions.AddOrUpdate(page.Id, new ConcurrentDictionary<int, int>(),
                (k, existingList) => existingList);

            pageQuestions[page.Id]?.AddOrUpdate(question.Id, 0, (k, v) => 0);
        }
    }

    private static void RemoveQuestionFrom(
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> pageQuestionList,
        QuestionCacheItem question)
    {
        foreach (var page in question.Pages)
        {
            var questionsInPage = pageQuestionList[page.Id];
            questionsInPage.TryRemove(question.Id, out var outVar);
        }
    }

    public static IList<PageRelationCache> GetChildRelationsByParentId(int id)
    {
        return Relations.Values
            .Where(relation => relation.ParentId == id)
            .ToList();
    }

    public static IList<PageRelationCache> GetParentRelationsByChildId(int id)
    {
        return Relations.Values
            .Where(relation => relation.ChildId == id)
            .ToList();
    }

    public static void AddOrUpdate(UserCacheItem user)
    {
        user.PreserveContentLanguages();
        AddOrUpdate(Users, user);
    }

    public static ICollection<UserCacheItem> GetAllUsers()
    {
        return Users.Values;
    }

    public static void RemoveUser(int id)
    {
        RemoveAllSharesByUserId(id);
        Remove(GetUserById(id));
    }

    public static void Remove(UserCacheItem user)
    {
        Remove(Users, user);
    }

    public static void Remove(PageRelationCache relation)
    {
        Remove(Relations, relation);
    }

    public static void AddOrUpdate(
        QuestionCacheItem question,
        List<int>? affectedPageIds = null)
    {
        AddOrUpdate(Questions, question);
        UpdatePageQuestionList(PageQuestionsList, question, affectedPageIds);
    }

    public static void Remove(QuestionCacheItem question)
    {
        Remove(Questions, question);
        RemoveQuestionFrom(PageQuestionsList, question);
    }

    public static void AddOrUpdate(PageRelationCache pageRelationCache)
    {
        AddOrUpdate(Relations, pageRelationCache);
    }

    public static void AddOrUpdate(PageCacheItem pageCacheItem)
    {
        AddOrUpdate(Pages, pageCacheItem);
    }

    public static void AddOrUpdate(ShareCacheItem shareCacheItem)
    {
        var pageId = shareCacheItem.PageId;
        var shareCacheItems = GetPageShares(pageId);
        if (shareCacheItems.Any(c => c.Id == shareCacheItem.Id))
        {
            var index = shareCacheItems.IndexOf(shareCacheItems.First(c => c.Id == shareCacheItem.Id));
            shareCacheItems[index] = shareCacheItem;
            AddOrUpdate(pageId, shareCacheItems);
        }
        else
        {
            shareCacheItems.Add(shareCacheItem);
            AddOrUpdate(pageId, shareCacheItems);
        }

        if (shareCacheItem.SharedWith != null)
        {
            var userCacheItem = GetUserByIdNullable(shareCacheItem.SharedWith.Id);
            if (userCacheItem != null && !userCacheItem.SharedPageIds.Contains(pageId))
            {
                userCacheItem.SharedPageIds.Add(pageId);
                if (shareCacheItem.Permission != SharePermission.RestrictAccess)
                    userCacheItem.VisibleSharedPageIds.Add(pageId);

                AddOrUpdate(userCacheItem);
            }
        }
    }

    public static void UpdatePageReferencesInQuestions(PageCacheItem pageCacheItem)
    {
        var affectedQuestionsIds = GetQuestionIdsForPage(pageCacheItem.Id);

        foreach (var questionId in affectedQuestionsIds)
        {
            if (Questions.TryGetValue(questionId, out var question))
            {
                var pageToReplace = question.Pages.FirstOrDefault(c => c.Id == pageCacheItem.Id);

                if (pageToReplace == null) return;

                var index = question.Pages.IndexOf(pageToReplace);
                question.Pages[index] = pageCacheItem;
            }
        }
    }

    public static void AddOrUpdate(int pageId, List<ShareCacheItem> shareCacheItems)
    {
        PageShares.AddOrUpdate(pageId, shareCacheItems, (key, existingList) => shareCacheItems);
    }

    public static void Remove(int id, int userId) => Remove(GetPage(id), userId);

    public static void Remove(PageCacheItem page, int userId)
    {
        Remove(Pages, page);
        var connectedQuestions = page.GetAggregatedQuestions(userId);

        foreach (var connectedQuestion in connectedQuestions)
        {
            var pageInQuestion = connectedQuestion.Pages.FirstOrDefault(c => c.Id == page.Id);
            connectedQuestion.Pages.Remove(pageInQuestion);
        }

        PageQuestionsList.TryRemove(page.Id, out _);
    }

    /// <summary>
    /// Not ThreadSafe! 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectToCache"></param>
    /// <param name="obj"></param>
    private static void AddOrUpdate(
        ConcurrentDictionary<int, UserCacheItem> objectToCache,
        UserCacheItem obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void AddOrUpdate(
        ConcurrentDictionary<int, PageCacheItem> objectToCache,
        PageCacheItem obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void AddOrUpdate(
        ConcurrentDictionary<int, PageRelationCache> objectToCache,
        PageRelationCache obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void AddOrUpdate(
        ConcurrentDictionary<int, QuestionCacheItem> objectToCache,
        QuestionCacheItem obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void AddOrUpdate(
        ConcurrentDictionary<int, ShareCacheItem> objectToCache,
        ShareCacheItem obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void Remove(
        ConcurrentDictionary<int, UserCacheItem> objectToCache,
        UserCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out _);
    }

    private static void Remove(
        ConcurrentDictionary<int, PageCacheItem> objectToCache,
        PageCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out _);
    }

    private static void Remove(
        ConcurrentDictionary<int, PageRelationCache> objectToCache,
        PageRelationCache obj)
    {
        objectToCache.TryRemove(obj.Id, out _);
    }

    private static void Remove(
        ConcurrentDictionary<int, QuestionCacheItem> objectToCache,
        QuestionCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out _);
    }

    private static void Remove(
        ConcurrentDictionary<int, ShareCacheItem> objectToCache,
        ShareCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out _);
    }

    public static List<PageCacheItem> GetPages(IEnumerable<int> getIds) =>
        getIds
            .Select(GetPage)
            .Where(page => page != null)
            .ToList()!;

    public static PageCacheItem? GetPage(Page page) => GetPage(page.Id);

    public static PageCacheItem? GetPage(int pageId)
    {
        Pages.TryGetValue(pageId, out var page);
        return page;
    }

    public static IList<PageCacheItem> GetAllPagesList() => Pages.Values.ToList();

    public static IEnumerable<int> GetPrivatePageIdsFromUser(int userId) =>
        GetAllPagesList()
            .Where(c => c.Creator.Id == userId && c.Visibility == PageVisibility.Private)
            .Select(c => c.Id);

    public static List<PageCacheItem> GetByPageName(string name)
    {
        var allPages = GetAllPagesList();
        return allPages.Where(c => c.Name.ToLower() == name.ToLower()).ToList();
    }

    public static List<PageCacheItem> GetWikisByUserId(int userId)
        => Pages.Values.Where(page => page.IsWiki && page.CreatorId == userId).ToList();

    public static IEnumerable<int> GetPrivateQuestionIdsFromUser(int userId) => GetAllQuestions()
        .Where(q => q.Creator.Id == userId && q.IsPrivate())
        .Select(q => q.Id);

    public static QuestionCacheItem? GetQuestion(int questionId)
    {
        Questions.TryGetValue(questionId, out var question);
        return question;
    }

    public static PageRelationCache? GetRelation(int relationId)
    {
        if (Relations == null) return null;
        Relations.TryGetValue(relationId, out var relation);
        return relation;
    }

    public static IList<PageRelationCache> GetAllRelations() => Relations.Values.ToList();

    public static List<PageRelationCache> GetCacheRelationsByChildId(int childId) =>
        GetAllRelations().Where(r => r.ChildId == childId).ToList();

    public static List<PageRelationCache> GetCacheRelationsByParentId(int parentId) =>
        GetAllRelations().Where(r => r.ParentId == parentId).ToList();

    public static IEnumerable<PageRelationCache> GetCacheRelationsByPageId(int pageId) =>
        GetAllRelations().Where(r => r.ParentId == pageId || r.ChildId == pageId);

    public static void Clear() => MemoCache.Clear();

    // Helper methods for updating share info:
    public static void AddOrUpdatePageShares(int pageId, List<ShareCacheItem> shareCacheItems)
    {
        PageShares.AddOrUpdate(pageId, shareCacheItems, (key, existingList) => shareCacheItems);
    }

    public static List<ShareCacheItem> GetPageShares(int pageId)
    {
        PageShares.TryGetValue(pageId, out var list);
        return list ?? new List<ShareCacheItem>();
    }

    public static void RemovePageShares(int pageId)
    {
        PageShares.TryRemove(pageId, out _);
    }

    public static void RemoveShares(int pageId, IList<int> shareIdsToRemove)
    {
        if (!shareIdsToRemove.Any())
            return;

        var currentShares = GetPageShares(pageId);
        if (!currentShares.Any())
            return;

        var affectedUsers = currentShares
            .Where(share => shareIdsToRemove.Contains(share.Id) && share.SharedWith != null)
            .Select(share => share.SharedWith.Id)
            .Distinct()
            .ToList();

        var shareIdsSet = new HashSet<int>(shareIdsToRemove);
        var updatedShares = currentShares.Where(share => !shareIdsSet.Contains(share.Id)).ToList();

        if (updatedShares.Count != currentShares.Count)
        {
            AddOrUpdatePageShares(pageId, updatedShares);

            foreach (var userId in affectedUsers)
            {
                var userCacheItem = GetUserByIdNullable(userId);
                if (userCacheItem != null)
                {
                    if (!updatedShares.Any(s => s.SharedWith?.Id == userId))
                    {
                        userCacheItem.SharedPageIds.Remove(pageId);
                        userCacheItem.VisibleSharedPageIds.Remove(pageId);
                        AddOrUpdate(userCacheItem);
                    }
                }
            }
        }
    }

    public static List<ShareCacheItem> GetSharesByUserId(int userId)
    {
        return PageShares.Values
            .SelectMany(shares => shares)
            .Where(share => share.SharedWith?.Id == userId)
            .ToList();
    }

    public static void RemoveAllSharesByUserId(int userId)
    {
        var userShares = GetSharesByUserId(userId);

        if (!userShares.Any())
            return;

        var sharesByPage = userShares.GroupBy(share => share.PageId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var pageEntry in sharesByPage)
        {
            var pageId = pageEntry.Key;
            var allPageShares = GetPageShares(pageId);

            var updatedShares = allPageShares
                .Where(s => s.SharedWith?.Id != userId)
                .ToList();

            AddOrUpdatePageShares(pageId, updatedShares);
        }
    }

    // Extended User Cache Methods - delegated to SlidingCache

    public static ExtendedUserCacheItem? GetExtendedUserByIdNullable(int userId) =>
        SlidingCache.GetExtendedUserByIdNullable(userId);

    public static void AddOrUpdate(ExtendedUserCacheItem extendedUser) =>
        SlidingCache.AddOrUpdate(extendedUser);

    public static void Remove(ExtendedUserCacheItem extendedUser) =>
        SlidingCache.Remove(extendedUser);

    public static ICollection<ExtendedUserCacheItem> GetAllExtendedUsers()
    {
        return SlidingCache.GetAllActiveExtendedUsers();
    }
}