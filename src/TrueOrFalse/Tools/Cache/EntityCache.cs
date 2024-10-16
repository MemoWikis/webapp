﻿using System.Collections.Concurrent;
using System.Diagnostics;

public class EntityCache
{
    public const string CacheKeyUsers = "allUsers_EntityCache";
    public const string CacheKeyQuestions = "allQuestions_EntityCache";
    public const string CacheKeyCategories = "allCategories_EntityCache";
    public const string CacheKeyCategoryQuestionsList = "categoryQuestionsList_EntityCache";
    public const string CacheKeyRelations = "allRelations_EntityCache";

    public static bool IsFirstStart = true;

    private static ConcurrentDictionary<int, UserCacheItem> Users =>
        Cache.Mgr.Get<ConcurrentDictionary<int, UserCacheItem>>(CacheKeyUsers);

    public static ConcurrentDictionary<int, CategoryCacheItem> Categories =>
        Cache.Mgr.Get<ConcurrentDictionary<int, CategoryCacheItem>>(CacheKeyCategories);

    public static ConcurrentDictionary<int, QuestionCacheItem> Questions =>
        Cache.Mgr.Get<ConcurrentDictionary<int, QuestionCacheItem>>(CacheKeyQuestions);

    private static ConcurrentDictionary<int, CategoryCacheRelation> Relations =>
        Cache.Mgr.Get<ConcurrentDictionary<int, CategoryCacheRelation>>(CacheKeyRelations);

    /// <summary>
    /// Dictionary(key:categoryId, value:questions)
    /// </summary>
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>>
        CategoryQuestionsList =>
        Cache.Mgr.Get<ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>(
            CacheKeyCategoryQuestionsList);

    public static List<UserCacheItem> GetUsersByIds(IEnumerable<int> ids) =>
        ids.Select(id => GetUserById(id))
            .ToList();

    public static void AddViewsLast30DaysToTopics(CategoryViewRepo categoryViewRepo, List<CategoryCacheItem> categoryCacheItems)
    {
        var categoriesViewsLast30Days = categoryViewRepo.GetViewsForLastNDaysGroupByCategoryId(30);
        foreach (var categoryCacheItem in categoryCacheItems)
        {
            var aggregatedCategories = categoryCacheItem.GetAllAggregatedCategories()
                .Select(t => t.Key);

            var aggregatedTopicViews30Days = categoriesViewsLast30Days
                .Where(view => aggregatedCategories.Contains(view.Category_Id))
                .GroupBy(view => view.DateOnly)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalCount = g.Sum(v => v.Count)
                })
                .OrderBy(result => result.Date)
                .Select(v => new DailyViews() { Date = v.Date, Count = v.TotalCount })
                .ToList();

            var selfCategoryViews30Days = categoriesViewsLast30Days
                .Where(view => (view.Category_Id == categoryCacheItem.Id))
                .GroupBy(view => view.DateOnly)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalCount = g.Sum(v => v.Count)
                })
                .OrderBy(result => result.Date)
                .Select(v => new DailyViews { Date = v.Date, Count = v.TotalCount })
                .ToList();

            DateTimeUtils.EnsureLastDaysIncluded(aggregatedTopicViews30Days, 30);
            DateTimeUtils.EnsureLastDaysIncluded(selfCategoryViews30Days, 30);
            //categoryCacheItem.AddTopicViews(aggregatedTopicViews30Days, selfCategoryViews30Days);
        }
    }

    public static void AddViewsLast30DaysToQuestion(QuestionViewRepository questionViewRepo, List<CategoryCacheItem> categoryCacheItems)
    {
        var watch = Stopwatch.StartNew();
        var questionViewsLast90Days = questionViewRepo.GetViewsForLastNDaysGroupByQuestionId(90);
        foreach (var categoryCacheItem in categoryCacheItems)
        {
            var aggregatedQuestionsFromAllAggregatedTopics = categoryCacheItem.GetAggregatedQuestionsFromMemoryCache(2, false, true, categoryCacheItem.Id)
                .Select(t => t.Id);

            var aggregatedQuestionsViews90Days = questionViewsLast90Days
                .Where(view => aggregatedQuestionsFromAllAggregatedTopics.Contains(view.QuestionId))
                .GroupBy(view => view.DateOnly)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalCount = g.Sum(v => v.Count)
                })
                .OrderBy(result => result.Date)
                .Select(v => new DailyViews() { Date = v.Date, Count = v.TotalCount })
                .ToList();

            var selfQuestionsFromTopic = categoryCacheItem.GetAggregatedQuestionsFromMemoryCache(2, false, false, categoryCacheItem.Id)
                .Select(t => t.Id);

            var topicQuestions90Days = questionViewsLast90Days
                .Where(view => selfQuestionsFromTopic.Contains(view.QuestionId))
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
            DateTimeUtils.EnsureLastDaysIncluded(topicQuestions90Days, 90);
            //categoryCacheItem.AddQuestionViews(aggregatedQuestionsViews90Days, topicQuestions90Days);
        }

        var ellapsedTime = watch.ElapsedMilliseconds;
        Logg.r.Information(nameof(AddViewsLast30DaysToQuestion) + ellapsedTime);
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
        GetCategoryQuestionsListForCacheInitilizer(IList<QuestionCacheItem> questions)
    {
        var categoryQuestionList = new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>();
        foreach (var question in questions)
        {
            UpdateCategoryQuestionList(categoryQuestionList, question);
        }

        return categoryQuestionList;
    }

    public static bool TopicHasQuestion(int topicId)
    {
        return EntityCache.GetQuestionsIdsForCategory(topicId)?
            .Any() ?? false;
    }

    public static IList<QuestionCacheItem> GetQuestionsForCategory(int categoryId)
    {
        return GetQuestionsByIds(GetQuestionsIdsForCategory(categoryId));
    }

    public static List<int> GetQuestionsIdsForCategory(int categoryId)
    {
        CategoryQuestionsList.TryGetValue(categoryId, out var questionIds);

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

    private static void UpdateCategoryQuestionList(
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionsList,
        QuestionCacheItem question,
        List<int> affectedCategoryIds = null)
    {
        DeleteQuestionFromRemovedCategories(question, categoryQuestionsList, affectedCategoryIds);

        AddQuestionToCategories(question, categoryQuestionsList);
    }

    private static void DeleteQuestionFromRemovedCategories(
        QuestionCacheItem question,
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionsList,
        List<int> affectedCategoryIds = null)
    {
        if (affectedCategoryIds != null)
        {
            foreach (var categoryId in affectedCategoryIds.Except(question.Categories.GetIds()))
            {
                if (categoryQuestionsList.ContainsKey(categoryId))
                    categoryQuestionsList[categoryId]?.TryRemove(question.Id, out var outVar);
            }
        }
    }

    public static void AddQuestionsToCategory(int categoryId, List<int> questionIds)
    {
        foreach (int questionId in questionIds)
        {
            CategoryQuestionsList.AddOrUpdate(categoryId, new ConcurrentDictionary<int, int>(),
                (k, existingList) => existingList);

            CategoryQuestionsList[categoryId]?.AddOrUpdate(questionId, 0, (k, v) => 0);
        }
    }

    private static void AddQuestionToCategories(
        QuestionCacheItem question,
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestions,
        IList<CategoryCacheItem> categories = null)
    {
        if (categories == null)
        {
            categories = GetCategories(question.Categories.GetIds()).ToList();
        }

        foreach (var category in categories)
        {
            categoryQuestions.AddOrUpdate(category.Id, new ConcurrentDictionary<int, int>(),
                (k, existingList) => existingList);

            categoryQuestions[category.Id]?.AddOrUpdate(question.Id, 0, (k, v) => 0);
        }
    }

    private static void RemoveQuestionFrom(
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionList,
        QuestionCacheItem question)
    {
        foreach (var category in question.Categories)
        {
            var questionsInCategory = categoryQuestionList[category.Id];
            questionsInCategory.TryRemove(question.Id, out var outVar);
        }
    }

    public static IList<CategoryCacheRelation> GetChildRelationsByParentId(int id)
    {
        return Relations.Values
            .Where(relation => relation.ParentId == id)
            .ToList();
    }

    public static IList<CategoryCacheRelation> GetParentRelationsByChildId(int id)
    {
        return Relations.Values
            .Where(relation => relation.ChildId == id)
            .ToList();
    }

    public static void AddOrUpdate(UserCacheItem user)
    {
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

    public static void Remove(CategoryCacheRelation relation)
    {
        Remove(Relations, relation);
    }

    public static void AddOrUpdate(
        QuestionCacheItem question,
        List<int> categoriesIdsToRemove = null)
    {
        AddOrUpdate(Questions, question);
        UpdateCategoryQuestionList(CategoryQuestionsList, question, categoriesIdsToRemove);
    }

    public static void Remove(QuestionCacheItem question)
    {
        Remove(Questions, question);
        RemoveQuestionFrom(CategoryQuestionsList, question);
    }

    public static void AddOrUpdate(CategoryCacheRelation categoryCacheRelation)
    {
        AddOrUpdate(Relations, categoryCacheRelation);
    }

    public static void AddOrUpdate(CategoryCacheItem categoryCacheItem)
    {
        AddOrUpdate(Categories, categoryCacheItem);
    }
    public static void UpdateCategoryReferencesInQuestions(CategoryCacheItem categoryCacheItem)
    {
        var affectedQuestionsIds = GetQuestionsIdsForCategory(categoryCacheItem.Id);

        foreach (var questionId in affectedQuestionsIds)
        {
            if (Questions.TryGetValue(questionId, out var question))
            {
                var categoryToReplace =
                    question.Categories.FirstOrDefault(c => c.Id == categoryCacheItem.Id);

                if (categoryToReplace == null) return;

                var index = question.Categories.IndexOf(categoryToReplace);
                question.Categories[index] = categoryCacheItem;
            }
        }
    }

    public static void Remove(int id, int userId) => Remove(GetCategory(id), userId);

    public static void Remove(CategoryCacheItem category, int userId)
    {
        Remove(Categories, category);
        var connectedQuestions = category.GetAggregatedQuestionsFromMemoryCache(userId);

        foreach (var connectedQuestion in connectedQuestions)
        {
            var categoryInQuestion =
                connectedQuestion.Categories.FirstOrDefault(c => c.Id == category.Id);
            connectedQuestion.Categories.Remove(categoryInQuestion);
        }

        CategoryQuestionsList.TryRemove(category.Id, out _);
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
        ConcurrentDictionary<int, CategoryCacheItem> objectToCache,
        CategoryCacheItem obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void AddOrUpdate(
        ConcurrentDictionary<int, CategoryCacheRelation> objectToCache,
        CategoryCacheRelation obj)
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
        ConcurrentDictionary<int, CategoryCacheItem> objectToCache,
        CategoryCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out _);
    }

    private static void Remove(
        ConcurrentDictionary<int, CategoryCacheRelation> objectToCache,
        CategoryCacheRelation obj)
    {
        objectToCache.TryRemove(obj.Id, out _);
    }

    private static void Remove(
        ConcurrentDictionary<int, QuestionCacheItem> objectToCache,
        QuestionCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out _);
    }

    public static IEnumerable<CategoryCacheItem> GetCategories(IEnumerable<int> getIds)
    {
        var c = getIds.Select(categoryId => GetCategory(categoryId)).ToList();
        return c;
    }

    public static CategoryCacheItem GetCategory(Category category) => GetCategory(category.Id);

    //There is an infinite loop when the user is logged in to complaints and when the server is restarted
    //https://docs.google.com/document/d/1XgfHVvUY_Fh1ID93UZEWFriAqTwC1crhCwJ9yqAPtTY
    public static CategoryCacheItem? GetCategory(int categoryId)
    {
        if (Categories == null) return null;
        Categories.TryGetValue(categoryId, out var category);
        return category;
    }

    public static IList<CategoryCacheItem> GetAllCategoriesList() => Categories.Values.ToList();

    public static IEnumerable<int> GetPrivateCategoryIdsFromUser(int userId) =>
        GetAllCategoriesList()
            .Where(c => c.Creator.Id == userId && c.Visibility == CategoryVisibility.Owner)
            .Select(c => c.Id);

    public static List<CategoryCacheItem> GetCategoryByName(
        string name,
        CategoryType type = CategoryType.Standard)
    {
        var allCategories = GetAllCategoriesList();
        return allCategories.Where(c => c.Name.ToLower() == name.ToLower()).ToList();
    }

    public static IEnumerable<int> GetPrivateQuestionIdsFromUser(int userId) => GetAllQuestions()
        .Where(q => q.Creator.Id == userId && q.IsPrivate())
        .Select(q => q.Id);

    public static QuestionCacheItem? GetQuestion(int questionId)
    {
        Questions.TryGetValue(questionId, out var question);
        return question;
    }

    public static CategoryCacheRelation? GetRelation(int relationId)
    {
        if (Relations == null) return null;
        Relations.TryGetValue(relationId, out var relation);
        return relation;
    }

    public static IList<CategoryCacheRelation> GetAllRelations() => Relations.Values.ToList();

    public static List<CategoryCacheRelation> GetCacheRelationsByChildId(int childId) =>
        GetAllRelations().Where(r => r.ChildId == childId).ToList();

    public static List<CategoryCacheRelation> GetCacheRelationsByParentId(int parentId) =>
        GetAllRelations().Where(r => r.ParentId == parentId).ToList();

    public static IEnumerable<CategoryCacheRelation> GetCacheRelationsByTopicId(int topicId) =>
        GetAllRelations().Where(r => r.ParentId == topicId || r.ChildId == topicId);

    public static void Clear()
    {
        Cache.Mgr.Clear();
    }
}