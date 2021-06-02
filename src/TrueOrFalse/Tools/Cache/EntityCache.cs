using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using Microsoft.Owin.Security.Provider;

public class EntityCache : BaseCache
{
    private const string _cacheKeyQuestions = "allQuestions _EntityCache";
    private const string _cacheKeyCategories = "allCategories_EntityCache";
    private const string _cacheKeyCategoryQuestionsList = "categoryQuestionsList_EntityCache";

    private static bool IsFirstStart = true; 

    private static ConcurrentDictionary<int, Question> Questions => (ConcurrentDictionary<int, Question>)HttpRuntime.Cache[_cacheKeyQuestions];
    private static ConcurrentDictionary<int, CategoryCacheItem> Categories => (ConcurrentDictionary<int, CategoryCacheItem>)HttpRuntime.Cache[_cacheKeyCategories];

    /// <summary>
    /// Dictionary(key:categoryId, value:questions)
    /// </summary>
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> CategoryQuestionsList => 
        (ConcurrentDictionary<int, ConcurrentDictionary<int, int>>)HttpRuntime.Cache[_cacheKeyCategoryQuestionsList];

    public static void Init(string customMessage = "")
    {
        var stopWatch = Stopwatch.StartNew();

        Logg.r().Information("EntityCache Start" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var categories = CategoryCacheItem.ToCacheCategories(Sl.CategoryRepo.GetAllEager()).ToList();
        var questions = Sl.QuestionRepo.GetAllEager();
        

        Logg.r().Information("EntityCache LoadAllEntities" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(_cacheKeyQuestions, questions.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategories, GraphService.AddChildrenToCategory(categories.ToConcurrentDictionary()));
        IntoForeverCache(_cacheKeyCategoryQuestionsList, GetCategoryQuestionsList(questions));

        Logg.r().Information("EntityCache PutIntoCache" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        Logg.r().Warning("ist gebaut");
        IsFirstStart = false; 
    }

    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> GetCategoryQuestionsList(IList<Question> questions)
    {
        var categoryQuestionList = new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>();
        foreach (var question in questions)
        {
            UpdateCategoryQuestionList(categoryQuestionList, question);
        }

        return categoryQuestionList;
    }

    public static IList<Question> GetQuestionsForCategory(int categoryId)
    {
        return GetQuestionsByIds(GetQuestionsIdsForCategory(categoryId));
    }

    public static IList<int> GetQuestionsIdsForCategory(int categoryId)
    {
        if (CategoryQuestionsList == null)
        {
            Init();
        }
        return CategoryQuestionsList.ContainsKey(categoryId) ?  CategoryQuestionsList[categoryId].Keys.ToList() : new List<int>();
    }

    public static IList<Question> GetQuestionsByIds(IList<int> questionIds)
    {
        var questions = new List<Question>();

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

    public static IList<Question> GetAllQuestions() => Questions.Values.ToList();
 
    public static Question GetQuestionById(int questionId)
    {
        if (Questions.TryGetValue(questionId, out var question))
            return question;

        Logg.r().Warning("QuestionId is not available");
        return new Question();
    }

    private static void UpdateCategoryQuestionList(
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionsList, 
        Question question, 
        List<int> affectedCategoryIds = null)
    {
        DeleteQuestionFromRemovedCategories(question, categoryQuestionsList, affectedCategoryIds);

        AddQuestionToCategories(question, categoryQuestionsList);
    }

    private static void DeleteQuestionFromRemovedCategories(
        Question question, 
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

    private static void AddQuestionToCategories(
        Question question,
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionsList,
        IList<CategoryCacheItem> categories = null)
    {
        if (categories == null)
        {
            categories = GetCategoryCacheItems(question.Categories.GetIds()).ToList();
        }

        foreach (var category in categories)
        {
            try
            {
                categoryQuestionsList.AddOrUpdate(category.Id, new ConcurrentDictionary<int, int>(),
                    (k, existingList) => existingList);

                categoryQuestionsList[category.Id]?.AddOrUpdate(question.Id, 0, (k, v) => 0);
            }
            catch
            {
                Logg.r().Error("Update failed in AddQuestionToCategorie");
            }
        }
    }

    private static void RemoveQuestionFrom(ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionList, Question question)
    {
        foreach (var category in question.Categories)
        {
            var questionsInCategory = categoryQuestionList[category.Id];
            questionsInCategory.TryRemove(question.Id, out var outVar);
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

    public static void AddOrUpdate(CategoryCacheItem categoryCacheItem)
    {
        AddOrUpdate(Categories, categoryCacheItem);
    }

    public static void UpdateCategoryReferencesInQuestions(CategoryCacheItem categoryCacheItem, Category category)
    {
        var affectedQuestionsIds = GetQuestionsIdsForCategory(categoryCacheItem.Id);

        foreach (var questionId in affectedQuestionsIds)
        {
            if (Questions.TryGetValue(questionId, out var question))
            {
                var categoryToReplace = question.Categories.FirstOrDefault(c => c.Id == categoryCacheItem.Id);

                if(categoryToReplace == null) return;

                var index = question.Categories.IndexOf(categoryToReplace);
                question.Categories[index] = category ;
            }
        }
    }

    public static void Remove(CategoryCacheItem category)
    {
        Remove(Categories, category);
        CategoryQuestionsList.TryRemove(category.Id, out var catOut);
    }

    /// <summary>
    /// Not ThreadSafe! 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectToCache"></param>
    /// <param name="obj"></param>
    private static void AddOrUpdate(ConcurrentDictionary<int, CategoryCacheItem> objectToCache, CategoryCacheItem obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void AddOrUpdate(ConcurrentDictionary<int, Question> objectToCache, Question obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void Remove(ConcurrentDictionary<int, CategoryCacheItem> objectToCache, CategoryCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out var outObj);
    }

    private static void Remove(ConcurrentDictionary<int, Question> objectToCache, Question obj)
    {
        objectToCache.TryRemove(obj.Id, out var outObj);
    }


    //There is an infinite loop when the user is logged in to complaints and when the server is restarted
    //https://docs.google.com/document/d/1XgfHVvUY_Fh1ID93UZEWFriAqTwC1crhCwJ9yqAPtTY
    public static CategoryCacheItem GetCategoryCacheItem(int categoryId, bool isFromUserEntityCache = false,  bool getDataFromEntityCache = false)
    {
        if ( !IsFirstStart && !isFromUserEntityCache && !getDataFromEntityCache && UserCache.GetItem(Sl.SessionUser.UserId).IsFiltered)
            return UserEntityCache.GetCategoryWhenNotAvalaibleThenGetNextParent(categoryId, Sl.SessionUser.UserId);

        return Categories[categoryId];
    }

    public static IEnumerable<CategoryCacheItem> GetCategoryCacheItems(IEnumerable<int> getIds) =>
        getIds.Select(categoryId => GetCategoryCacheItem(categoryId));
    public static IEnumerable<CategoryCacheItem> GetCategoryCacheItems(IList<int> getIds, bool getDataFromEntityCache = true) =>
        getIds.Select(categoryId => GetCategoryCacheItem(categoryId, getDataFromEntityCache: getDataFromEntityCache));

    public static List<CategoryCacheItem> CategoryCacheItemsForSearch(IEnumerable<int> categoryIds)
    {
        var categories = new List<CategoryCacheItem>();
        foreach (var categoryId in categoryIds)
        {
            Categories.TryGetValue(categoryId, out var category);
            if(category != null)
                categories.Add(category);
        }

        return categories.Where(c => c.IsVisibleToCurrentUser()).ToList();
    }

    public static IList<CategoryCacheItem> GetAllCategories() => Categories.Values.ToList();

    public static List<CategoryCacheItem> GetChildren(int categoryId, bool isFromEntityCache = false)
    {
        var allCategories = GetAllCategories();

        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf && cr.RelatedCategoryId == categoryId)
                .Select(cr => GetCategoryCacheItem(cr.CategoryId, isFromEntityCache))).ToList();
    }

    public static List<CategoryCacheItem> GetChildren(CategoryCacheItem category, bool isFromEntityCache = false) => GetChildren(category.Id, isFromEntityCache);  

    public static IList<CategoryCacheItem> GetAllChildren(int parentId, bool getFromEntityCache = false)
    {
        var currentGeneration = GetChildren(parentId, getFromEntityCache).ToList();
        var nextGeneration = new List<CategoryCacheItem>();
        var descendants = new List<CategoryCacheItem>();

        while (currentGeneration.Count > 0)
        {
            descendants.AddRange(currentGeneration);

            foreach (var category in currentGeneration)
            {
                var children = GetChildren(category.Id, getFromEntityCache).ToList();
                if (children.Count > 0)
                {
                    nextGeneration.AddRange(children);
                }
            }

            currentGeneration = nextGeneration.Except(descendants).Where(c => c.Id != parentId).Distinct().ToList();
            nextGeneration = new List<CategoryCacheItem>();
        }

        return descendants;
    }

    public static List<CategoryCacheItem> GetByName(string name, CategoryType type = CategoryType.Standard)
    {
        var allCategories = GetAllCategories();
        return allCategories.Where(c => c.Name == name).ToList();
    }

    public static void Clear()
    {
        Questions.Clear();
        Categories.Clear();
        CategoryQuestionsList.Clear();
    }
}