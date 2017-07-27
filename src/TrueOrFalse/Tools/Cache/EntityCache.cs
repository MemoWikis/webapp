using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Caching;
using NHibernate.Util;
using Seedworks.Lib.Persistence;

public class EntityCache
{
    private const string _cacheKeyQuestions = "allQuestions _EntityCache";
    private const string _cacheKeyCategories = "allCategories_EntityCache";
    private const string _cacheKeySets = "allSets_EntityCache";
    private const string _cacheKeyCategoryQuestionsList = "categoryQuestionsList_EntityCache";
    private const string _cacheKeyCategorySetsList = "categorySetsList_EntityCache";
    private const string _cacheKeyCategoryQuestionInSetList = "categoryQuestionInSetList_EntityCache";

    public static ConcurrentDictionary<int, Question> Questions => (ConcurrentDictionary<int, Question>)HttpRuntime.Cache[_cacheKeyQuestions];
    public static ConcurrentDictionary<int, Category> Categories => (ConcurrentDictionary<int, Category>)HttpRuntime.Cache[_cacheKeyCategories];
    public static ConcurrentDictionary<int, Set> Sets => (ConcurrentDictionary<int, Set>)HttpRuntime.Cache[_cacheKeySets];

    public static ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> CategoryQuestionsList => 
        (ConcurrentDictionary<int, ConcurrentDictionary<int, Question>>)HttpRuntime.Cache[_cacheKeyCategoryQuestionsList];
    public static ConcurrentDictionary<int, ConcurrentDictionary<int, Set>> CategorySetsList =>
        (ConcurrentDictionary<int, ConcurrentDictionary<int, Set>>)HttpRuntime.Cache[_cacheKeyCategorySetsList];

    /// <summary>
    /// Holds a list of questions that are subordinate by a set for each category. 
    /// Each question holds a list of the setsIds that bind them to the category.
    /// Last level ConcurrentDictionary is used for easy access to keys (set ids) only (the value is always 0).
    /// </summary>
    public static ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> CategoryQuestionInSetList =>
        (ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>)HttpRuntime.Cache[_cacheKeyCategoryQuestionInSetList];

    public static void Init()
    {
        var stopWatch = Stopwatch.StartNew();

        Logg.r().Information("EntityCache Start {Elapsed}", stopWatch.Elapsed);

        var questions = Sl.QuestionRepo.GetAll();
        var categories = Sl.CategoryRepo.GetAll().ToConcurrentDictionary();
        var sets = Sl.SetRepo.GetAllEager();
        var questionInSets = Sl.QuestionInSetRepo.GetAll();

        Logg.r().Information("EntityCache LoadAllEntities {Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(_cacheKeyQuestions, questions.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategories, categories);
        IntoForeverCache(_cacheKeySets, sets.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategoryQuestionsList, GetCategoryQuestionsList(questions.ToConcurrentDictionary()));
        IntoForeverCache(_cacheKeyCategorySetsList, GetCategorySetsList(sets.ToConcurrentDictionary()));
        IntoForeverCache(_cacheKeyCategoryQuestionInSetList, GetCategoryQuestionInSetList(questionInSets.ToConcurrentDictionary()));


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

    public static IList<int> GetQuestionsIdsForCategory(int categoryId)
    {
        return EntityCache.CategoryQuestionsList.ContainsKey(categoryId) 
            ? EntityCache.CategoryQuestionsList[categoryId].Keys.ToList() 
            : new List<int>();
    }

    public static IList<Question> GetQuestionsInSetsForCategory(int categoryId)
    {
        return GetQuestionsByIdsFromMemoryCache(GetQuestionsInSetsIdsForCategory(categoryId));
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

    public static IList<Question> GetQuestionsByIdsFromMemoryCache(IList<int> questionIds)
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

    

    private static ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> GetCategoryQuestionsList(ConcurrentDictionary<int, Question> questions)
    {
        var categoryQuestionList = new ConcurrentDictionary<int, ConcurrentDictionary<int, Question>>();
        foreach (var question in questions)
        {
            UpdateCategoryQuestionList(categoryQuestionList, question.Value);
        }

        return categoryQuestionList;
    }

    private static ConcurrentDictionary<int, ConcurrentDictionary<int, Set>> GetCategorySetsList(ConcurrentDictionary<int, Set> sets)
    {
        var categorySetsList = new ConcurrentDictionary<int, ConcurrentDictionary<int, Set>>();
        foreach (var set in sets)
        {
            UpdateCategorySetList(categorySetsList, set.Value);
        }

        return categorySetsList;
    }

    private static ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> 
        GetCategoryQuestionInSetList(ConcurrentDictionary<int, QuestionInSet> questionInSetItems)
    {
        var categoryQuestionInSetList =
            new ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>();

        foreach (var questionInSet in questionInSetItems)
        {
            AddQuestionInSetTo(categoryQuestionInSetList, questionInSet.Value);
        }

        return categoryQuestionInSetList;
    }

    private static void AddQuestionInSetTo(
        ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> categoryQuestionInSetList,
        QuestionInSet questionInSet)
    {
        foreach (var category in questionInSet.Set.Categories)
        {
            var set = questionInSet.Set;
            var question = questionInSet.Question;

            categoryQuestionInSetList.AddOrUpdate(category.Id, new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>(), (k, existingList) => existingList);

            categoryQuestionInSetList[category.Id]?.AddOrUpdate(question.Id, new ConcurrentDictionary<int, int>(), (k, existingList) => existingList);

            categoryQuestionInSetList[category.Id]?[question.Id]?.AddOrUpdate(set.Id, 0, (k, v) => 0);
        }
    }

    private static void RemoveQuestionInSetFrom(
        ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> categoryQuestionInSetList,
        QuestionInSet questionInSet)
    {
        foreach (var category in questionInSet.Set.Categories)
        {
            categoryQuestionInSetList[category.Id]?[questionInSet.Question.Id]?.TryRemove(questionInSet.Set.Id, out var removedQuestionInSet);
        }
    }

    private static void UpdateCategoryQuestionList(
        ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> categoryQuestionsList, 
        Question question, 
        List<int> affectedCategoryIds = null)
    {
        DeleteQuestionFromRemovedCategories(question, categoryQuestionsList, affectedCategoryIds);

        AddQuestionToCategories(question, categoryQuestionsList);
    }

    private static void UpdateCategorySetList(
        ConcurrentDictionary<int, ConcurrentDictionary<int, Set>> categorySetsList,
        Set set,
        List<int> affectedCategoryIds = null)
    {
        DeleteSetFromRemovedCategories(set, categorySetsList, affectedCategoryIds);

        AddSetToCategories(set, categorySetsList);
    }

    private static void DeleteQuestionFromRemovedCategories(
        Question question, 
        ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> categoryQuestionsList,
        List<int> affectedCategoryIds = null)
    {
        if (affectedCategoryIds != null)
        {
            foreach (var categoryId in affectedCategoryIds.Except(question.Categories.GetIds()))
            {
                if (categoryQuestionsList.ContainsKey(categoryId))
                    categoryQuestionsList[categoryId]?.TryRemove(question.Id, out var questionOut);
            }
        }
    }

    private static void DeleteSetFromRemovedCategories(
        Set set,
        ConcurrentDictionary<int, ConcurrentDictionary<int, Set>> categorySetsList,
        List<int> affectedCategoryIds = null)
    {
        if (affectedCategoryIds != null)
        {
            foreach (var categoryId in affectedCategoryIds.Except(set.Categories.GetIds()))
            {
                if (categorySetsList.ContainsKey(categoryId))
                    categorySetsList[categoryId]?.TryRemove(set.Id, out var questionOut);
            }
        }
    }

    private static void UpdateCategoryQuestionInSetList(
        ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> categoryQuestionsInSetList,
        Set set,
        List<int> affectedCategoryIds = null)
    {
        foreach (var questionInSet in set.QuestionsInSet)
        {
            DeleteQuestionInSetFromRemovedCategories(questionInSet, categoryQuestionsInSetList, affectedCategoryIds);

            AddQuestionInSetTo(categoryQuestionsInSetList, questionInSet);
        }
    }

    private static void DeleteQuestionInSetFromRemovedCategories(QuestionInSet questionInSet, ConcurrentDictionary<int, ConcurrentDictionary<int, ConcurrentDictionary<int, int>>> categoryQuestionsInSetList, List<int> affectedCategoryIds)
    {
        if (affectedCategoryIds != null)
        {
            foreach (var categoryId in affectedCategoryIds.Except(questionInSet.Set.Categories.GetIds()))
            {
                if (categoryQuestionsInSetList.ContainsKey(categoryId))
                    categoryQuestionsInSetList[categoryId]?[questionInSet.Question.Id]?.TryRemove(questionInSet.Set.Id, out var outValue);
            }
        }
    }

    private static void AddQuestionToCategories(
        Question question,
        ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> categoryQuestionsList,
        IList<Category> categories = null)
    {
        if (categories == null)
        {
            categories = question.Categories;
        }

        foreach (var category in categories)
        {
            categoryQuestionsList.AddOrUpdate(category.Id, new ConcurrentDictionary<int, Question>(), (k, existingList) => existingList);

            categoryQuestionsList[category.Id]?.AddOrUpdate(question.Id, question, (k, v) => question);
        }
    }

    private static void RemoveQuestionFrom(ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> categoryQuestionList, Question question)
    {
        foreach (var category in question.Categories)
        {
            var questionsInCategory = categoryQuestionList[category.Id];
            questionsInCategory.TryRemove(question.Id, out var questionOut);
        }
    }

    private static void AddSetToCategories(
        Set set,
        ConcurrentDictionary<int, ConcurrentDictionary<int, Set>> categorySetsList)
    {
        foreach (var category in set.Categories)
        {
            categorySetsList.AddOrUpdate(category.Id, new ConcurrentDictionary<int, Set>(), (k, existingList) => existingList);

            categorySetsList[category.Id]?.AddOrUpdate(set.Id, set, (k, v) => set);
        }
    }

    private static void RemoveSetFrom(ConcurrentDictionary<int, ConcurrentDictionary<int, Set>> categorySetList, Set set)
    {
        foreach (var category in set.Categories)
        {
            var questionsInCategory = categorySetList[category.Id];
            questionsInCategory.TryRemove(set.Id, out var setOut);
        }
    }

    public static void AddOrUpdate(Question question, List<int> affectedCategoriesIds = null)
    {
        AddOrUpdate(Questions, question);
        UpdateCategoryQuestionList(CategoryQuestionsList, question, affectedCategoriesIds);
    }

    public static void Remove(Question question)
    {
        Remove(Questions, question);
        RemoveQuestionFrom(CategoryQuestionsList, question);
    }

    public static void AddOrUpdate(Category category)
    {
        AddOrUpdate(Categories, category);
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
        UpdateCategorySetList(CategorySetsList, set, affectedCategoriesIds);
        UpdateCategoryQuestionInSetList(CategoryQuestionInSetList, set, affectedCategoriesIds);
    }

    public static void Remove(Set set)
    {
        Remove(Sets, set);
        RemoveSetFrom(CategorySetsList, set);
    }

    public static void AddOrUpdate(QuestionInSet questionInSet)
    {
        AddQuestionInSetTo(CategoryQuestionInSetList, questionInSet);
    }

    public static void Remove(QuestionInSet questionInSet)
    {
        RemoveQuestionInSetFrom(CategoryQuestionInSetList, questionInSet);
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
}