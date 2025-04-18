﻿using System.Collections.Concurrent;
using System.Diagnostics;

public class EntityCache
{
    public const string CacheKeyUsers = "allUsers_EntityCache";
    public const string CacheKeyQuestions = "allQuestions_EntityCache";
    public const string CacheKeyPages = "allPages_EntityCache";
    public const string CacheKeyPageQuestionsList = "pageQuestionsList_EntityCache";
    public const string CacheKeyRelations = "allRelations_EntityCache";

    public static bool IsFirstStart = true;

    private static ConcurrentDictionary<int, UserCacheItem> Users =>
        Cache.Mgr.Get<ConcurrentDictionary<int, UserCacheItem>>(CacheKeyUsers);

    public static ConcurrentDictionary<int, PageCacheItem> Pages =>
        Cache.Mgr.Get<ConcurrentDictionary<int, PageCacheItem>>(CacheKeyPages);

    public static ConcurrentDictionary<int, QuestionCacheItem> Questions =>
        Cache.Mgr.Get<ConcurrentDictionary<int, QuestionCacheItem>>(CacheKeyQuestions);

    private static ConcurrentDictionary<int, PageRelationCache> Relations =>
        Cache.Mgr.Get<ConcurrentDictionary<int, PageRelationCache>>(CacheKeyRelations);

    /// <summary>
    /// Dictionary(key:pageId, value:questions)
    /// </summary>
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>>
        PageQuestionsList =>
        Cache.Mgr.Get<ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>(
            CacheKeyPageQuestionsList);

    public static List<UserCacheItem> GetUsersByIds(IEnumerable<int> ids) =>
        ids.Select(id => GetUserById(id))
            .ToList();

    public static void AddViewsLast30DaysToPages(PageViewRepo pageViewRepo, List<PageCacheItem> pageCacheItems)
    {
        var pagesViewsLast30Days = pageViewRepo.GetViewsForLastNDaysGroupByPageId(30);
        foreach (var pageCacheItem in pageCacheItems)
        {
            var aggregatedPages = pageCacheItem.GetAllAggregatedPages()
                .Select(t => t.Key);

            var aggregatedPageViews30Days = pagesViewsLast30Days
                .Where(view => aggregatedPages.Contains(view.PageId))
                .GroupBy(view => view.DateOnly)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalCount = g.Sum(v => v.Count)
                })
                .OrderBy(result => result.Date)
                .Select(v => new DailyViews() { Date = v.Date, Count = v.TotalCount })
                .ToList();

            var pageViews30Days = pagesViewsLast30Days
                .Where(view => (view.PageId == pageCacheItem.Id))
                .GroupBy(view => view.DateOnly)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalCount = g.Sum(v => v.Count)
                })
                .OrderBy(result => result.Date)
                .Select(v => new DailyViews { Date = v.Date, Count = v.TotalCount })
                .ToList();

            DateTimeUtils.EnsureLastDaysIncluded(aggregatedPageViews30Days, 30);
            DateTimeUtils.EnsureLastDaysIncluded(pageViews30Days, 30);
        }
    }

    public static void AddViewsLast30DaysToQuestion(QuestionViewRepository questionViewRepo, List<PageCacheItem> pageCacheItems)
    {
        var watch = Stopwatch.StartNew();
        var questionViewsLast90Days = questionViewRepo.GetViewsForLastNDaysGroupByQuestionId(90);
        foreach (var pageCacheItem in pageCacheItems)
        {
            var aggregatedQuestionsFromAllAggregatedPages = pageCacheItem.GetAggregatedQuestionsFromMemoryCache(2, false, true, pageCacheItem.Id)
                .Select(t => t.Id);

            var aggregatedQuestionsViews90Days = questionViewsLast90Days
                .Where(view => aggregatedQuestionsFromAllAggregatedPages.Contains(view.QuestionId))
                .GroupBy(view => view.DateOnly)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalCount = g.Sum(v => v.Count)
                })
                .OrderBy(result => result.Date)
                .Select(v => new DailyViews() { Date = v.Date, Count = v.TotalCount })
                .ToList();

            var selfQuestionsFromPage = pageCacheItem.GetAggregatedQuestionsFromMemoryCache(2, false, false, pageCacheItem.Id)
                .Select(t => t.Id);

            var pageQuestions90Days = questionViewsLast90Days
                .Where(view => selfQuestionsFromPage.Contains(view.QuestionId))
                .GroupBy(view => view.DateOnly)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalCount = g.Sum(v => v.Count)
                })
                .OrderBy(result => result.Date)
                .Select(v => new DailyViews() { Date = v.Date, Count = v.TotalCount })
                .ToList();

            DateTimeUtils.EnsureLastDaysIncluded(aggregatedQuestionsViews90Days, 90);
            DateTimeUtils.EnsureLastDaysIncluded(pageQuestions90Days, 90);
        }

        var elapsedTime = watch.ElapsedMilliseconds;
        Logg.r.Information(nameof(AddViewsLast30DaysToQuestion) + elapsedTime);
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

    public static bool PageHasQuestion(int pageId)
    {
        return EntityCache.GetQuestionIdsForPage(pageId)?
            .Any() ?? false;
    }

    public static IList<QuestionCacheItem> GetQuestionsForPage(int pageId)
    {
        return GetQuestionsByIds(GetQuestionIdsForPage(pageId));
    }

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

        Logg.r.Warning("QuestionId is not available");
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
        user.CleanupWikiIdsAndFavoriteIds();
        user.PreserveContentLanguages();
        AddOrUpdate(Users, user);
    }

    public static ICollection<UserCacheItem> GetAllUsers()
    {
        return Users.Values;
    }

    public static void RemoveUser(int id)
    {
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

    public static void Remove(int id, int userId) => Remove(GetPage(id), userId);

    public static void Remove(PageCacheItem page, int userId)
    {
        Remove(Pages, page);
        var connectedQuestions = page.GetAggregatedQuestionsFromMemoryCache(userId);

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

    public static List<PageCacheItem?> GetPages(IEnumerable<int> getIds) => getIds.Select(GetPage).ToList();

    public static PageCacheItem GetPage(Page page) => GetPage(page.Id);

    //There is an infinite loop when the user is logged in to complaints and when the server is restarted
    //https://docs.google.com/document/d/1XgfHVvUY_Fh1ID93UZEWFriAqTwC1crhCwJ9yqAPtTY
    public static PageCacheItem? GetPage(int pageId)
    {
        if (Pages == null) return null;
        Pages.TryGetValue(pageId, out var page);
        return page;
    }

    public static IList<PageCacheItem> GetAllPagesList() => Pages.Values.ToList();

    public static IEnumerable<int> GetPrivatePageIdsFromUser(int userId) =>
        GetAllPagesList()
            .Where(c => c.Creator.Id == userId && c.Visibility == PageVisibility.Owner)
            .Select(c => c.Id);

    public static List<PageCacheItem> GetByPageName(
        string name,
        PageType type = PageType.Standard)
    {
        var allPages = GetAllPagesList();
        return allPages.Where(c => c.Name.ToLower() == name.ToLower()).ToList();
    }

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

    public static void Clear()
    {
        Cache.Mgr.Clear();
    }
}