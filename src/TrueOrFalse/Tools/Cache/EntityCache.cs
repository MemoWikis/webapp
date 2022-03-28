using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

public class EntityCache : BaseCache
{
    private const string _cacheKeyQuestions = "allQuestions _EntityCache";
    private const string _cacheKeyCategories = "allCategories_EntityCache";
    private const string _cacheKeyCategoryQuestionsList = "categoryQuestionsList_EntityCache";

    public static bool IsFirstStart = true;
    private static ConcurrentDictionary<int, QuestionCacheItem> Questions => (ConcurrentDictionary<int, QuestionCacheItem>)HttpRuntime.Cache[_cacheKeyQuestions];
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
        var allCategories = Sl.CategoryRepo.GetAllEager();
        Logg.r().Information("EntityCache CategoriesLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var categories = CategoryCacheItem.ToCacheCategories(allCategories).ToList();
        Logg.r().Information("EntityCache CategoriesCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(_cacheKeyCategories, GraphService.AddChildrenIdsToCategoryCacheData(categories.ToConcurrentDictionary()));

        var allQuestions = Sl.QuestionRepo.GetAllEager();
        Logg.r().Information("EntityCache QuestionsLoadedFromRepo " + customMessage + "{Elapsed}", stopWatch.Elapsed);
        var questions = QuestionCacheItem.ToCacheQuestions(allQuestions).ToList();
        Logg.r().Information("EntityCache QuestionsCached " + customMessage + "{Elapsed}", stopWatch.Elapsed);


        Logg.r().Information("EntityCache LoadAllEntities" + customMessage + "{Elapsed}", stopWatch.Elapsed);

        IntoForeverCache(_cacheKeyQuestions, questions.ToConcurrentDictionary());
        IntoForeverCache(_cacheKeyCategoryQuestionsList, GetCategoryQuestionsList(questions));

        foreach (var question in allQuestions.Where(q => q.References.Any()))
        {
            Questions.FirstOrDefault(q => q.Key == question.Id).Value.References =
                ReferenceCacheItem.ToReferenceCacheItems(question.References).ToList();
        }
        Logg.r().Information("EntityCache PutIntoCache" + customMessage + "{Elapsed}", stopWatch.Elapsed);
        IsFirstStart = false;
    }

    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> GetCategoryQuestionsList(IList<QuestionCacheItem> questions)
    {
        var categoryQuestionList = new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>();
        foreach (var question in questions)
        {
            UpdateCategoryQuestionList(categoryQuestionList, question);
        }

        return categoryQuestionList;
    }


    public static IList<QuestionCacheItem> GetQuestionsForCategory(int categoryId)
    {
        return GetQuestionsByIds(GetQuestionsIdsForCategory(categoryId));
    }

    public static IList<int> GetQuestionsIdsForCategory(int categoryId)
    {
        if (CategoryQuestionsList == null)
        {
            Init();
        }

        var questionIds = CategoryQuestionsList.ContainsKey(categoryId) ? CategoryQuestionsList[categoryId].Keys.ToList() : new List<int>();
        return questionIds;
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

    public static IList<QuestionCacheItem> GetAllQuestions() => Questions.Values.ToList();

    public static QuestionCacheItem GetQuestionById(int questionId)
    {
        if (Questions.TryGetValue(questionId, out var question))
            return question;

        Logg.r().Warning("QuestionId is not available");
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

    private static void AddQuestionToCategories(
        QuestionCacheItem question,
        ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionsList,
        IList<CategoryCacheItem> categories = null)
    {
        if (categories == null)
        {
            categories = GetCategories(question.Categories.GetIds()).ToList();
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

    private static void RemoveQuestionFrom(ConcurrentDictionary<int, ConcurrentDictionary<int, int>> categoryQuestionList, QuestionCacheItem question)
    {
        foreach (var category in question.Categories)
        {
            var questionsInCategory = categoryQuestionList[category.Id];
            questionsInCategory.TryRemove(question.Id, out var outVar);
        }
    }

    public static void AddOrUpdate(QuestionCacheItem question, List<int> categoriesIdsToRemove = null)
    {
        AddOrUpdate(Questions, question);
        UpdateCategoryQuestionList(CategoryQuestionsList, question, categoriesIdsToRemove);
    }

    public static void Remove(QuestionCacheItem question)
    {
        Remove(Questions, question);
        RemoveQuestionFrom(CategoryQuestionsList, question);
    }

    public static void AddOrUpdate(CategoryCacheItem categoryCacheItem)
    {
        AddOrUpdate(Categories, categoryCacheItem);
        if (!Categories.ContainsKey(categoryCacheItem.Id)) return;
        var parentsToAdd = categoryCacheItem.ParentCategories();
        foreach (var parent in parentsToAdd)
        {
            parent.CachedData.AddChildId(categoryCacheItem.Id);
        }

        var parentsToRemove = Categories.Where(d => d.Value.CachedData.ChildrenIds.Contains(categoryCacheItem.Id)).ToList().Select(d => d.Value).ToList();
        foreach (var parent in parentsToRemove)
        {
            if (!categoryCacheItem.CategoryRelations.Any(c => c.RelatedCategoryId == parent.Id && c.CategoryRelationType == CategoryRelationType.IsChildOf) && !parentsToAdd.Contains(parent))
                parent.CachedData.RemoveChildId(categoryCacheItem.Id);
        }
    }

    public static void UpdateCategoryReferencesInQuestions(CategoryCacheItem categoryCacheItem)
    {
        var affectedQuestionsIds = GetQuestionsIdsForCategory(categoryCacheItem.Id);

        foreach (var questionId in affectedQuestionsIds)
        {
            if (Questions.TryGetValue(questionId, out var question))
            {
                var categoryToReplace = question.Categories.FirstOrDefault(c => c.Id == categoryCacheItem.Id);

                if (categoryToReplace == null) return;

                var index = question.Categories.IndexOf(categoryToReplace);
                question.Categories[index] = categoryCacheItem;
            }
        }
    }

    public static void Remove(int id) => Remove(GetCategory(id));
    public static void Remove(CategoryCacheItem category)
    {
        Remove(Categories, category);
        var connectedQuestions = category.GetAggregatedQuestionsFromMemoryCache();
        foreach (var connectedQuestion in connectedQuestions)
        {
            var categoryInQuestion = connectedQuestion.Categories.FirstOrDefault(c => c.Id == category.Id);
            connectedQuestion.Categories.Remove(categoryInQuestion);
        }

        var parentCategories = GetAllParents(category.Id);
        foreach (var parent in parentCategories)
        {
            parent.CachedData.RemoveChildId(category.Id);
        }
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

    private static void AddOrUpdate(ConcurrentDictionary<int, QuestionCacheItem> objectToCache, QuestionCacheItem obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void Remove(ConcurrentDictionary<int, CategoryCacheItem> objectToCache, CategoryCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out var outObj);
    }

    private static void Remove(ConcurrentDictionary<int, QuestionCacheItem> objectToCache, QuestionCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out var outObj);
    }

    public static IEnumerable<CategoryCacheItem> GetCategories(IEnumerable<int> getIds) =>
        getIds.Select(categoryId => GetCategory(categoryId));
    public static IEnumerable<CategoryCacheItem> GetCategories(IList<int> getIds, bool getDataFromEntityCache = true) =>
        getIds.Select(categoryId => GetCategory(categoryId, getDataFromEntityCache: getDataFromEntityCache));

    public static CategoryCacheItem GetCategory(Category category) => GetCategory(category.Id);

    //There is an infinite loop when the user is logged in to complaints and when the server is restarted
    //https://docs.google.com/document/d/1XgfHVvUY_Fh1ID93UZEWFriAqTwC1crhCwJ9yqAPtTY
    public static CategoryCacheItem GetCategory(int categoryId, bool isFromUserEntityCache = false, bool getDataFromEntityCache = false)
    {
        if (!IsFirstStart && !isFromUserEntityCache && !getDataFromEntityCache && UserCache.GetItem(SessionUser.UserId).IsFiltered)
        {
            var user = SessionUser.User;
            return UserEntityCache.GetCategoryWhenNotAvalaibleThenGetNextParent(categoryId, user.Id);
        }

        if (Categories == null) return null;
        Categories.TryGetValue(categoryId, out var category);
        return category;

    }

    public static List<CategoryCacheItem> CategoryCacheItemsForSearch(IEnumerable<int> categoryIds)
    {
        var categories = new List<CategoryCacheItem>();
        foreach (var categoryId in categoryIds)
        {
            Categories.TryGetValue(categoryId, out var category);
            if (category != null)
                categories.Add(category);
        }

        return categories.Where(PermissionCheck.CanView).ToList();
    }

    public static IList<CategoryCacheItem> GetAllCategories() => Categories.Values.ToList();

    public static List<CategoryCacheItem> GetChildren(int categoryId, bool isFromEntityCache = false, bool getFromEntityCache = false)
    {
        var allCategories = GetAllCategories();

        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf && cr.RelatedCategoryId == categoryId)
                .Select(cr => GetCategory(cr.CategoryId, isFromEntityCache, getFromEntityCache))).ToList();
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

    public static List<CategoryCacheItem> ParentCategories(int categoryId, bool isFromEntityCache = false, bool getFromEntityCache = false)
    {
        var allCategories = GetAllCategories();

        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf && cr.CategoryId == categoryId)
                .Select(cr => GetCategory(cr.CategoryId, isFromEntityCache, getFromEntityCache))).ToList();
    }

    public static IList<CategoryCacheItem>
        GetAllParents(int childId, bool getFromEntityCache = false)
    {
        var currentGeneration = ParentCategories(childId);
        var nextGeneration = new List<CategoryCacheItem>();
        var ascendants = new List<CategoryCacheItem>();

        while (currentGeneration.Count > 0)
        {
            ascendants.AddRange(currentGeneration);

            foreach (var parent in currentGeneration)
            {
                var parents = parent.ParentCategories();
                if (parents.Count > 0)
                {
                    nextGeneration.AddRange(parents);
                }
            }

            currentGeneration = nextGeneration.Except(ascendants).Where(c => c.Id != childId).Distinct().ToList();
            nextGeneration = new List<CategoryCacheItem>();
        }

        ascendants = ascendants.Distinct().ToList();
        var self = ascendants.Find(cci => cci.Id == childId);
        if (self != null)
            ascendants.Remove(self);

        return ascendants;
    }

    public static List<CategoryCacheItem> GetCategoryByName(string name, CategoryType type = CategoryType.Standard)
    {
        var allCategories = GetAllCategories();
        return allCategories.Where(c => c.Name == name).ToList();
    }

    public static QuestionCacheItem GetQuestion(Question question, bool isFromUserEntityCache = false,
        bool getDataFromEntityCache = false) => GetQuestion(question.Id, isFromUserEntityCache, getDataFromEntityCache);

    public static QuestionCacheItem GetQuestion(int questionId, bool isFromUserEntityCache = false, bool getDataFromEntityCache = false)
    {

        Questions.TryGetValue(questionId, out var question);
        return question;
    }

    public static void Clear()
    {
        Questions.Clear();
        Categories.Clear();
        CategoryQuestionsList.Clear();
    }
}