using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Seedworks.Lib.Persistence;

public class EntityCache : BaseCache
{
    private const string _cacheKeyQuestions = "allQuestions _EntityCache";
    private const string _cacheKeyCategories = "allCategories_EntityCache";
    private const string _cacheKeyCategoryQuestionsList = "categoryQuestionsList_EntityCache";

    private static ConcurrentDictionary<int, Question> Questions => (ConcurrentDictionary<int, Question>)HttpRuntime.Cache[_cacheKeyQuestions];
    private static ConcurrentDictionary<int, Category> Categories => (ConcurrentDictionary<int, Category>)HttpRuntime.Cache[_cacheKeyCategories];

    //In category lists last level ConcurrentDictionary is used for easy access to keys (item ids) only (value is always 0)
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> CategoryQuestionsList => 
        (ConcurrentDictionary<int, ConcurrentDictionary<int, int>>)HttpRuntime.Cache[_cacheKeyCategoryQuestionsList];


    public static void Init(string customMessage = "")
    {
        var stopWatch = Stopwatch.StartNew();

        Logg.r().Information("EntityCache Start" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        var questions = Sl.QuestionRepo.GetAll();
        var categories = Sl.CategoryRepo.GetAllEager();

        Logg.r().Information("EntityCache LoadAllEntities" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(_cacheKeyQuestions, questions.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategories, categories.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategoryQuestionsList, GetCategoryQuestionsList(questions));

        Logg.r().Information("EntityCache PutIntoCache" + customMessage + "{Elapsed}", stopWatch.Elapsed);
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

        throw new Exception("Question not in Cache");
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
        IList<Category> categories = null)
    {
        if (categories == null)
        {
            categories = question.Categories;
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

    public static void AddOrUpdate(Category category)
    {
        AddOrUpdate(Categories, category);

        UpdateCategoryForQuestions(category);
    }

    private static void UpdateCategoryForQuestions(Category category)
    {
        var affectedQuestionsIds = GetQuestionsIdsForCategory(category.Id);

        foreach (var questionId in affectedQuestionsIds)
        {
            if (Questions.TryGetValue(questionId, out var question))
            {
                var categoryToReplace = question.Categories.FirstOrDefault(c => c.Id == category.Id);

                if(categoryToReplace == null) return;

                var index = question.Categories.IndexOf(categoryToReplace);
                question.Categories[index] = category;
            }
        }
    }

    public static void Remove(Category category)
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
    private static void AddOrUpdate<T>(ConcurrentDictionary<int, T> objectToCache, T obj) where T : DomainEntity
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void Remove<T>(ConcurrentDictionary<int, T> objectToCache, T obj) where T : DomainEntity
    {
        objectToCache.TryRemove(obj.Id, out var outObj);
    }

    //There is an infinite loop when the user is logged in to complaints and when the server is restarted
    //https://docs.google.com/document/d/1XgfHVvUY_Fh1ID93UZEWFriAqTwC1crhCwJ9yqAPtTY
    public static Category GetCategory(int categoryId, bool isFromUserEntityCache = false)
    {
        if (UserCache.IsFiltered && !isFromUserEntityCache)
            return UserEntityCache.GetCategory(categoryId, Sl.SessionUser.UserId);

        return Categories[categoryId];
    }

    public static IEnumerable<Category> GetCategories(IEnumerable<int> getIds) => 
        getIds.Select(categoryId => Categories[categoryId]);

    public static IEnumerable<Category> GetAllCategories() => Categories.Values.ToList();


    public static List<Category> GetChildren(int categoryId, bool isFromEntityCache = false)
    {
        var category = GetCategory(categoryId, isFromEntityCache);

        var allCategories = GetAllCategories();

        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildCategoryOf && cr.RelatedCategory.Id == category.Id)
                .Select(cr => cr.Category)).ToList();
    }

    public static List<Category> GetChildren(Category category, bool isFromEntityCache = false) => GetChildren(category.Id, isFromEntityCache);  

    public static IList<Category> GetDescendants(int parentId, bool isFromUserEntityCache = false)
    {
        var currentGeneration = GetChildren(parentId, isFromUserEntityCache).ToList();
        var nextGeneration = new List<Category>();
        var descendants = new List<Category>();

        while (currentGeneration.Count > 0)
        {
            descendants.AddRange(currentGeneration);

            foreach (var category in currentGeneration)
            {
                var children = GetChildren(category.Id, isFromUserEntityCache).ToList();
                if (children.Count > 0)
                {
                    nextGeneration.AddRange(children);
                }
            }

            currentGeneration = nextGeneration.Except(descendants).Where(c => c.Id != parentId).Distinct().ToList();
            nextGeneration = new List<Category>();
        }

        return descendants;
    }

    public static IList<Category> GetDescendants(Category category, bool isFromEntityCache = false) =>
        GetDescendants(category.Id, isFromEntityCache);

    public static List<Category> GetByName(string name, CategoryType type = CategoryType.Standard)
    {
        var allCategories = GetAllCategories();
        return allCategories.Where(c => c.Name == name).ToList();
    }

}