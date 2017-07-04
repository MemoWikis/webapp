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

    public static ConcurrentDictionary<int, Question> Questions => (ConcurrentDictionary<int, Question>)HttpRuntime.Cache[_cacheKeyQuestions];
    public static ConcurrentDictionary<int, Category> Categories => (ConcurrentDictionary<int, Category>)HttpRuntime.Cache[_cacheKeyCategories];
    public static ConcurrentDictionary<int, Set> Sets => (ConcurrentDictionary<int, Set>)HttpRuntime.Cache[_cacheKeySets];

    public static ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> CategoryQuestionsList => 
        (ConcurrentDictionary<int, ConcurrentDictionary<int, Question>>)HttpRuntime.Cache[_cacheKeyCategoryQuestionsList];
     
    public static void Init()
    {
        var stopWatch = Stopwatch.StartNew();

        Logg.r().Information("EntityCache Start {Elapsed}", stopWatch.Elapsed);
        
        var questions = Sl.QuestionRepo.GetAll();
        var categories = Sl.CategoryRepo.GetAll().ToConcurrentDictionary();
        var categoryRelations = Sl.CategoryRelationRepo.GetAll();
        var sets = Sl.SetRepo.GetAllEager();

        Logg.r().Information("EntityCache LoadAllEntities {Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(_cacheKeyQuestions, questions.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategories, categories);
        IntoForeverCache(_cacheKeySets, sets.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategoryQuestionsList, GetCategoryQuestionsList(questions.ToConcurrentDictionary()));

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

    private static ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> GetCategoryQuestionsList(ConcurrentDictionary<int, Question> questions)
    {
        var categoryQuestionList = new ConcurrentDictionary<int, ConcurrentDictionary<int, Question>>();
        foreach (var question in questions)
        {
            UpdateCategoryQuestionList(categoryQuestionList, question.Value);
        }

        return categoryQuestionList;
    }

    private static void UpdateCategoryQuestionList(
        ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> categoryQuestionsList, 
        Question question, 
        List<int> affectedCategoryIds = null)
    {
        DeleteQuestionFromRemovedCategories(question, categoryQuestionsList, affectedCategoryIds);

        AddQuestionToCategories(question, categoryQuestionsList);
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

    private static void AddQuestionToCategories(
        Question question,
        ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> categoryQuestionsList)
    {
        foreach (var category in question.Categories)
        {
            if (!categoryQuestionsList.ContainsKey(category.Id))
                categoryQuestionsList.TryAdd(category.Id, new ConcurrentDictionary<int, Question>());

            categoryQuestionsList[category.Id]?.AddOrUpdate(question.Id, question, (k, v) => v);
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

    public static void AddOrUpdate(Question question, List<int> affectedCategoriesIds = null)
    {
        lock ("ebbe6d4a-70f0-46f8-9aaf-60b1a8ebb1bf")
        {
            AddOrUpdate(Questions, question);
            UpdateCategoryQuestionList(CategoryQuestionsList, question, affectedCategoriesIds);
        }
    }

    public static void Remove(Question question)
    {
        Remove(Questions, question);
        RemoveQuestionFrom(CategoryQuestionsList, question);
    }

    public static void AddOrUpdate(Category category)
    {
        lock ("5345455b-ab89-4ba2-87ad-a34ae43cdd06")
        {
            AddOrUpdate(Categories, category);
        }
    }

    public static void Remove(Category category)
    {
        Remove(Categories, category);
    }

    public static void AddOrUpdate(Set set)
    {
        lock ("deb61dfa-9279-41d5-98d0-e8b9b221a685")
        {
            AddOrUpdate(Sets, set);
        }
    }

    public static void Remove(Set set)
    {
        Remove(Sets, set);
    }

    /// <summary>
    /// Not ThreadSafe!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectToCache"></param>
    /// <param name="obj"></param>
    private static void AddOrUpdate<T>(ConcurrentDictionary<int, T> objectToCache, T obj) where T : DomainEntity
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => v);
    }

    private static void Remove<T>(ConcurrentDictionary<int, T> objectToCache, T obj) where T : DomainEntity
    {
        objectToCache.TryRemove(obj.Id, out var outObj);
    }
}