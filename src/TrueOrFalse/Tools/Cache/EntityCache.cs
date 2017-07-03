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
    private const string _cacheKeyCategoryRelations = "allCategoryRelations_EntityCache";
    private const string _cacheKeySets = "allSets_EntityCache";
    private const string _cacheKeyCategoryQuestionsList = "categoryQuestionsList_EntityCache";

    public static ConcurrentDictionary<int, Question> Questions => (ConcurrentDictionary<int, Question>)HttpRuntime.Cache[_cacheKeyQuestions];
    public static ConcurrentDictionary<int, Category> Categories => (ConcurrentDictionary<int, Category>)HttpRuntime.Cache[_cacheKeyCategories];
    public static ConcurrentDictionary<int, CategoryRelation> CategoryRelations => (ConcurrentDictionary<int, CategoryRelation>)HttpRuntime.Cache[_cacheKeyCategoryRelations];
    public static ConcurrentDictionary<int, Set> Sets => (ConcurrentDictionary<int, Set>)HttpRuntime.Cache[_cacheKeySets];

    public static ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> CategoryQuestionsList => 
        (ConcurrentDictionary<int, ConcurrentDictionary<int, Question>>)HttpRuntime.Cache[_cacheKeyCategoryQuestionsList];
     
    public static void Init()
    {
        var stopWatch = Stopwatch.StartNew();

        Logg.r().Information("EntityCache Start {Elapsed}", stopWatch.Elapsed);
        
        var questions = Sl.QuestionRepo.GetAll();
        var categories = Sl.CategoryRepo.GetAll();
        var aggregatedCategoryRelations = Sl.CategoryRelationRepo.GetAll();
        var sets = Sl.SetRepo.GetAllEager();

        Logg.r().Information("EntityCache LoadAllEntities {Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(_cacheKeyQuestions, questions.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategories, categories.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategoryRelations, aggregatedCategoryRelations.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeySets, sets.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategoryQuestionsList, GetCategoryQuestionsList(questions.ToConcurrentDictionary()));

        Logg.r().Information("EntityCache PutIntoCache {Elapsed}", stopWatch.Elapsed);
    }

    private static ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> GetCategoryQuestionsList(ConcurrentDictionary<int, Question> questions)
    {
        var categoryQuestionList = new ConcurrentDictionary<int, ConcurrentDictionary<int, Question>>();
        foreach (var question in questions)
        {
            AddQuestionTo(categoryQuestionList, question.Value);
        }

        return categoryQuestionList;
    }

    private static void AddQuestionTo(ConcurrentDictionary<int, ConcurrentDictionary<int, Question>> categoryQuestionList, Question question)
    {
        foreach (var category in question.Categories)
        {
            if (!categoryQuestionList.ContainsKey(category.Id))
                categoryQuestionList.TryAdd(category.Id, new ConcurrentDictionary<int, Question>());

            categoryQuestionList[category.Id].TryAdd(question.Id, question);
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

    public static void AddOrUpdate(Question question)
    {
        lock ("ebbe6d4a-70f0-46f8-9aaf-60b1a8ebb1bf")
        {
            AddOrUpdate(Questions, question);
            AddQuestionTo(CategoryQuestionsList, question);
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

    public static void AddOrUpdate(CategoryRelation categoryRelation)
    {
        lock ("c2c68be5-97fa-4d0a-b9dc-c6b77f18f974")
        {
            AddOrUpdate(CategoryRelations, categoryRelation);
        }
    }

    public static void Remove(CategoryRelation categoryRelation)
    {
        Remove(CategoryRelations, categoryRelation);
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