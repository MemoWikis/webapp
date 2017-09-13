using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Seedworks.Lib.Persistence;

public class EntityCache
{
    private const string _cacheKeyQuestions = "allQuestions _EntityCache";
    private const string _cacheKeyCategories = "allCategories_EntityCache";
    private const string _cacheKeySets = "allSets_EntityCache";
    private const string _cacheKeyCategoryQuestionsList = "categoryQuestionsList_EntityCache";
    private const string _cacheKeyCategorySetsList = "categorySetsList_EntityCache";
    private const string _cacheKeyCategoryQuestionInSetList = "categoryQuestionInSetList_EntityCache";

    private static ConcurrentDictionary<int, Question> Questions => (ConcurrentDictionary<int, Question>)HttpRuntime.Cache[_cacheKeyQuestions];
    private static ConcurrentDictionary<int, Category> Categories => (ConcurrentDictionary<int, Category>)HttpRuntime.Cache[_cacheKeyCategories];
    private static ConcurrentDictionary<int, Set> Sets => (ConcurrentDictionary<int, Set>)HttpRuntime.Cache[_cacheKeySets];

    //In category lists last level ConcurrentDictionary is used for easy access to keys (item ids) only (value is always 0)
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> CategoryQuestionsList => 
        (ConcurrentDictionary<int, ConcurrentDictionary<int, int>>)HttpRuntime.Cache[_cacheKeyCategoryQuestionsList];
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> CategorySetsList =>
        (ConcurrentDictionary<int, ConcurrentDictionary<int, int>>)HttpRuntime.Cache[_cacheKeyCategorySetsList];

    /// <summary>
    /// CategoryIds > QuestionIds (for each category) > SetsIds (of sets that bind question to category)
    /// </summary>
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> CategoryQuestionInSetList =>
        (ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>)HttpRuntime.Cache[_cacheKeyCategoryQuestionInSetList];

    public static void Init()
    {
        var stopWatch = Stopwatch.StartNew();

        Logg.r().Information("EntityCache Start {Elapsed}", stopWatch.Elapsed);

        var questions = Sl.QuestionRepo.GetAll();
        var categories = Sl.CategoryRepo.GetAllEager();
        var sets = Sl.SetRepo.GetAllEager();
        var questionInSets = Sl.QuestionInSetRepo.GetAll();

        Logg.r().Information("EntityCache LoadAllEntities {Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(_cacheKeyQuestions, questions.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategories, categories.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeySets, sets.ToConcurrentDictionary());

        IntoForeverCache(_cacheKeyCategoryQuestionsList, new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>());
        IntoForeverCache(_cacheKeyCategorySetsList, new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>());
        IntoForeverCache(_cacheKeyCategoryQuestionInSetList, new ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>());

        FillCategoryQuestionsList(questions);
        FillCategorySetsList(sets);
        FillCategoryQuestionInSetList(questionInSets);

        Logg.r().Information("EntityCache PutIntoCache {Elapsed}", stopWatch.Elapsed);
    }

    private static void IntoForeverCache<T>(string key, ConcurrentDictionary<int, T> objectToCache)
    {
        HttpRuntime.Cache.Insert(
            key,
            objectToCache,
            null,
            Cache.NoAbsoluteExpiration,
            Cache.NoSlidingExpiration,
            CacheItemPriority.NotRemovable,
            null);
    }

    private static void FillCategoryQuestionsList(IList<Question> questions)
    {
        foreach (var question in questions)
        {
            UpdateCategoryQuestionList(question);
        }
    }

    private static void FillCategorySetsList(IList<Set> sets)
    {
        foreach (var set in sets)
        {
            UpdateCategorySetList(set);
        }
    }

    private static void
        FillCategoryQuestionInSetList(IList<QuestionInSet> questionInSetItems)
    {
        foreach (var questionInSet in questionInSetItems)
        {
            AddToCategoryQuestionInSetList(questionInSet);
        }
    }

    public static IList<Question> GetQuestionsForCategory(int categoryId)
    {
        return GetQuestionsByIds(GetQuestionsIdsForCategory(categoryId));
    }

    public static IList<int> GetQuestionsIdsForCategory(int categoryId)
    {
        return CategoryQuestionsList.ContainsKey(categoryId) 
            ? CategoryQuestionsList[categoryId].Keys.ToList() 
            : new List<int>();
    }

    public static IList<Question> GetQuestionsInSetsForCategory(int categoryId)
    {
        return GetQuestionsByIds(GetQuestionsInSetsIdsForCategory(categoryId));
    }

    public static IList<Set> GetSetsForCategory(int categoryId)
    {
        return GetSetsByIds(GetSetIdsForCategory(categoryId));
    }

    public static IList<Set> GetSetsForCategories(IList<Category> categories)
    {
        return categories.SelectMany(c => GetSetsForCategory(c.Id)).Distinct().ToList();
    }

    public static IList<int> GetSetIdsForCategory(int categoryId)
    {
        return CategorySetsList.ContainsKey(categoryId)
            ? CategorySetsList[categoryId].Keys.ToList()
            : new List<int>();
    }

    public static IList<int> GetQuestionsInSetsIdsForCategory(int categoryId)
    {
        var questionIds = new List<int>();

        if (!CategoryQuestionInSetList.TryGetValue(categoryId, out var questionSetIdLists)) return questionIds;

        foreach (var questionId in questionSetIdLists.Keys)
        {
            if (questionSetIdLists.TryGetValue(questionId, out var setIdList)
                && !setIdList.IsEmpty)
            {
                questionIds.Add(questionId);
            }
        }

        return questionIds;
    }

    public static IList<Question> GetQuestionsByIds(IList<int> questionIds)
    {
        var questions = new List<Question>();

        foreach (var questionId in questionIds)
        {
            if (Questions.TryGetValue(questionId, out var questionToAdd))
            {
                questions.Add(questionToAdd);
            }
        }

        return questions;
    }

    public static IList<Set> GetSetsByIds(IList<int> setIds)
    {
        var sets = new List<Set>();

        foreach (var id in setIds)
        {
            if (Sets.TryGetValue(id, out var setToAdd))
            {
                sets.Add(setToAdd);
            }
        }

        return sets;
    }

    private static void AddToCategoryQuestionInSetList(QuestionInSet questionInSet)
    {
        foreach (var category in questionInSet.Set.Categories)
        {
            var set = questionInSet.Set;
            var question = questionInSet.Question;

            CategoryQuestionInSetList.AddOrUpdate(category.Id, new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>(), (k, existingList) => existingList);

            CategoryQuestionInSetList[category.Id]?.AddOrUpdate(question.Id, new ConcurrentDictionary<int, int>(), (k, existingList) => existingList);

            CategoryQuestionInSetList[category.Id]?[question.Id]?.AddOrUpdate(set.Id, 0, (k, v) => 0);
        }
    }

    private static void RemoveFromCategoryQuestionInSetList(QuestionInSet questionInSet)
    {
        foreach (var category in questionInSet.Set.Categories)
        {
            CategoryQuestionInSetList[category.Id]?[questionInSet.Question.Id]?.TryRemove(questionInSet.Set.Id, out var removedQuestionInSet);
        }
    }

    private static void UpdateCategoryQuestionList(
        Question question, 
        List<int> affectedCategoryIds = null)
    {
        DeleteQuestionFromRemovedCategories(question, affectedCategoryIds);

        AddQuestionToCategories(question);
    }

    private static void UpdateCategorySetList(
        Set set,
        List<int> affectedCategoryIds = null)
    {
        DeleteSetFromRemovedCategories(set, affectedCategoryIds);

        AddSetToCategories(set);
    }

    private static void DeleteQuestionFromRemovedCategories(
        Question question, 
        List<int> affectedCategoryIds = null)
    {
        if (affectedCategoryIds != null)
        {
            foreach (var categoryId in affectedCategoryIds.Except(question.Categories.GetIds()))
            {
                if (CategoryQuestionsList.ContainsKey(categoryId))
                    CategoryQuestionsList[categoryId]?.TryRemove(question.Id, out var outVar);
            }
        }
    }

    private static void DeleteSetFromRemovedCategories(
        Set set,
        List<int> affectedCategoryIds = null)
    {
        if (affectedCategoryIds != null)
        {
            foreach (var categoryId in affectedCategoryIds.Except(set.Categories.GetIds()))
            {
                if (CategorySetsList.ContainsKey(categoryId))
                    CategorySetsList[categoryId]?.TryRemove(set.Id, out var questionOut);
            }
        }
    }

    private static void UpdateCategoryQuestionInSetList(
        Set set,
        List<int> affectedCategoryIds = null)
    {
        foreach (var questionInSet in set.QuestionsInSet)
        {
            DeleteQuestionInSetFromRemovedCategories(questionInSet, affectedCategoryIds);

            AddToCategoryQuestionInSetList(questionInSet);
        }
    }

    private static void DeleteQuestionInSetFromRemovedCategories(QuestionInSet questionInSet, List<int> affectedCategoryIds)
    {
        if (affectedCategoryIds != null)
        {
            foreach (var categoryId in affectedCategoryIds.Except(questionInSet.Set.Categories.GetIds()))
            {
                if (CategoryQuestionInSetList.ContainsKey(categoryId))
                    CategoryQuestionInSetList[categoryId]?[questionInSet.Question.Id]?.TryRemove(questionInSet.Set.Id, out var outValue);
            }
        }
    }

    private static void AddQuestionToCategories(
        Question question,
        IList<Category> categories = null)
    {
        if (categories == null)
        {
            categories = question.Categories;
        }

        foreach (var category in categories)
        {
            CategoryQuestionsList.AddOrUpdate(category.Id, new ConcurrentDictionary<int, int>(), (k, existingList) => existingList);

            CategoryQuestionsList[category.Id]?.AddOrUpdate(question.Id, 0, (k, v) => 0);
        }
    }

    private static void RemoveQuestionFrom(Question question)
    {
        foreach (var category in question.Categories)
        {
            var questionsInCategory = CategoryQuestionsList[category.Id];
            questionsInCategory.TryRemove(question.Id, out var outVar);
        }
    }

    private static void AddSetToCategories(Set set)
    {
        foreach (var category in set.Categories)
        {
            CategorySetsList.AddOrUpdate(category.Id, new ConcurrentDictionary<int, int>(), (k, existingList) => existingList);

            CategorySetsList[category.Id]?.AddOrUpdate(set.Id, 0, (k, v) => 0);
        }
    }

    private static void RemoveFromCategorySetList(Set set)
    {
        foreach (var category in set.Categories)
        {
            var questionsInCategory = CategorySetsList[category.Id];
            questionsInCategory.TryRemove(set.Id, out var setOut);
        }
    }

    public static void AddOrUpdate(Question question, List<int> affectedCategoriesIds = null)
    {
        AddOrUpdate(Questions, question);
        UpdateCategoryQuestionList(question, affectedCategoriesIds);
    }

    public static void Remove(Question question)
    {
        Remove(Questions, question);
        RemoveQuestionFrom(question);
    }

    public static void AddOrUpdate(Category category)
    {
        AddOrUpdate(Categories, category);

        UpdateCategoryForQuestions(category);
        UpdateCategoryForSets(category);
    }

    private static void UpdateCategoryForSets(Category category)
    {
        var affectedSetIds = GetSetIdsForCategory(category.Id);

        foreach (var setId in affectedSetIds)
        {
            if (Sets.TryGetValue(setId, out var set))
            {
                set.Categories = set.Categories.Where(c => c.Id != category.Id).ToList();
                set.Categories.Add(category);
            }
        }
    }

    private static void UpdateCategoryForQuestions(Category category)
    {
        var affectedQuestionsIds = GetQuestionsIdsForCategory(category.Id);

        foreach (var questionId in affectedQuestionsIds)
        {
            if (Questions.TryGetValue(questionId, out var question))
            {
                question.Categories = question.Categories.Where(c => c.Id != category.Id).ToList();
                question.Categories.Add(category);
            }
        }
    }

    public static void Remove(Category category)
    {
        Remove(Categories, category);
        CategoryQuestionsList.TryRemove(category.Id, out var catOut);
        CategorySetsList.TryRemove(category.Id, out var catOut2);
        CategoryQuestionInSetList.TryRemove(category.Id, out var listOut);
    }

    public static void AddOrUpdate(Set set, List<int> affectedCategoriesIds = null)
    {
        AddOrUpdate(Sets, set);
        UpdateCategorySetList(set, affectedCategoriesIds);
        UpdateCategoryQuestionInSetList(set, affectedCategoriesIds);
    }

    public static void Remove(Set set)
    {
        Remove(Sets, set);
        RemoveFromCategorySetList(set);

        foreach (var questionInSet in set.QuestionsInSet)
        {
            RemoveFromCategoryQuestionInSetList(questionInSet);
        }
    }

    public static void AddOrUpdate(QuestionInSet questionInSet)
    {
        AddOrUpdateQuestionsInSet(questionInSet);
        AddToCategoryQuestionInSetList(questionInSet);
    }

    private static void AddOrUpdateQuestionsInSet(QuestionInSet questionInSet)
    {
        if (Sets.TryGetValue(questionInSet.Set.Id, out var outSet))
        {
            outSet.QuestionsInSet = new HashSet<QuestionInSet>(outSet.QuestionsInSet.Where(q => q.Id != questionInSet.Id));
            outSet.QuestionsInSet.Add(questionInSet);

            foreach (var questionInSetItem in outSet.QuestionsInSet)
            {
                questionInSetItem.Set = outSet;
            }
        }
    }

    public static void Remove(QuestionInSet questionInSet)
    {
        RemoveFromCategoryQuestionInSetList(questionInSet);
        RemoveQuestionFromSet(questionInSet);
    }

    public static void RemoveQuestionFromSet(QuestionInSet questionInSet)
    {
        if (Sets.TryGetValue(questionInSet.Set.Id, out var outSet))
        {
            outSet.QuestionsInSet = new HashSet<QuestionInSet>(outSet.QuestionsInSet.Where(q => q.Id != questionInSet.Id));
        }
    }

    /// <summary>
    /// Not ThreadSafe! 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectToCache"></param>
    /// <param name="obj"></param>
    private static void AddOrUpdate<T>(ConcurrentDictionary<int, T> objectToCache, T obj) where T : DomainEntity
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void Remove<T>(ConcurrentDictionary<int, T> objectToCache, T obj) where T : DomainEntity
    {
        objectToCache.TryRemove(obj.Id, out var outObj);
    }


    public static Category GetCategory(int categoryId) => Categories[categoryId];

    public static IEnumerable<Category> GetCategories(IEnumerable<int> getIds) => 
        getIds.Select(categoryId => Categories[categoryId]);

    /// <summary>
    /// Helps do debug, e.g. filter CategoryQuestionInSetList for certain questions in certain categories and the belonging set ids
    /// </summary>
    public static ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>
        FilterThreeLevelConcurrentDictionary(
            ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> dictionary, IList<int> firstLevelFilterIds, IList<int> secondLevelFilterIds)
    {
        var filteredDict = new ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>();

        foreach (var secondLevelFilterId in secondLevelFilterIds)
        {
            var listOfDict = firstLevelFilterIds.Select(id => new {Id = id, Dictionary = dictionary[id]}).ToList();

            listOfDict.ForEach(d =>
            {
                if (d.Dictionary.TryGetValue(secondLevelFilterId, out var outDict))
                {
                    filteredDict.TryAdd(d.Id, new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>());
                    filteredDict[d.Id].TryAdd(secondLevelFilterId, outDict);
                }
            });

        }

        return filteredDict;
    }
}