﻿
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using static CategoryRepository;

public class EntityCache : BaseEntityCache
{
    public const string CacheKeyUsers = "allUsers_EntityCache";
    public const string CacheKeyQuestions = "allQuestions_EntityCache";
    public const string CacheKeyCategories = "allCategories_EntityCache";
    public const string CacheKeyCategoryQuestionsList = "categoryQuestionsList_EntityCache";

    public static bool IsFirstStart = true;
    private static ConcurrentDictionary<int, UserCacheItem> Users => _cache.Get<ConcurrentDictionary<int, UserCacheItem>>(CacheKeyUsers);

    private static ConcurrentDictionary<int, CategoryCacheItem> Categories => _cache.Get<ConcurrentDictionary<int, CategoryCacheItem>>(CacheKeyCategories);

    public static ConcurrentDictionary<int, QuestionCacheItem> Questions => _cache.Get<ConcurrentDictionary<int, QuestionCacheItem>>(CacheKeyQuestions);

    /// <summary>
    /// Dictionary(key:categoryId, value:questions)
    /// </summary>
    private static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> CategoryQuestionsList =>
        _cache.Get<ConcurrentDictionary<int, ConcurrentDictionary<int, int>>>(CacheKeyCategoryQuestionsList);

    public static List<UserCacheItem> GetUsersByIds(IEnumerable<int> ids) => 
        ids.Select(id => GetUserById(id))
            .ToList(); 
    public static UserCacheItem GetUserById(int userId)
    {
        if (Users.TryGetValue(userId, out var user))
            return user;

        return new UserCacheItem();
    }

    public static ConcurrentDictionary<int, ConcurrentDictionary<int, int>> GetCategoryQuestionsList(IList<QuestionCacheItem> questions)
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

    public static void UpdateCachedData(CategoryCacheItem categoryCacheItem, CreateDeleteUpdate createDeleteUpdate)
    {
        //Create
        if (createDeleteUpdate == CreateDeleteUpdate.Create)
        {
            //Update EntityCache
            var parents = GetCategories(GraphService.GetDirectParentIds(categoryCacheItem));
            foreach (var parent in parents)
            {
                parent.CachedData.AddChildId(categoryCacheItem.Id);
            }
        }

        //Update
        if (createDeleteUpdate == CreateDeleteUpdate.Update)
        {
            var oldCategoryCacheItem1 = GetCategory(categoryCacheItem.Id);

            var parentIdsCacheItem1 = categoryCacheItem.CategoryRelations
                .Select(cr => cr.RelatedCategoryId).ToList();

            var parentIdsOldCategoryCacheItem1 = oldCategoryCacheItem1.CategoryRelations
                .Select(cr => cr.RelatedCategoryId).ToList();

            var exceptIdsToDelete1 = parentIdsOldCategoryCacheItem1.Except(parentIdsCacheItem1).ToList();
            var exceptIdsToAdd1 = parentIdsCacheItem1.Except(parentIdsOldCategoryCacheItem1).ToList();

            if (exceptIdsToAdd1.Any() || exceptIdsToDelete1.Any())
            {
                foreach (var id in exceptIdsToAdd1)
                {
                    GetCategory(id).CachedData
                        .AddChildId(categoryCacheItem.Id);
                }

                foreach (var id in exceptIdsToDelete1)
                {
                    GetCategory(id).CachedData.RemoveChildId(categoryCacheItem.Id);
                }
            }
        }

        //Delete
        if (createDeleteUpdate == CreateDeleteUpdate.Delete)
        {
            //EntityCache
            foreach (var parent in categoryCacheItem.ParentCategories(true))
            {
                parent.CachedData.RemoveChildId(categoryCacheItem.Id);
            }
        }
    }

    public static IList<QuestionCacheItem> GetAllQuestions() => Questions.Values.ToList();

    public static QuestionCacheItem GetQuestionById(int questionId,
        IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment webHostEnvironment)
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
            categoryQuestionsList.AddOrUpdate(category.Id, new ConcurrentDictionary<int, int>(),
                    (k, existingList) => existingList);

                categoryQuestionsList[category.Id]?.AddOrUpdate(question.Id, 0, (k, v) => 0);
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
    public static void AddOrUpdate(UserCacheItem user)
    {
        AddOrUpdate(Users, user);
    }

    public static void RemoveUser(int id)
    {
        Remove(GetUserById(id));
    }

    public static void Remove(UserCacheItem user)
    {
        Remove(Users, user);
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
            if (categoryCacheItem.CategoryRelations.All(c => c.RelatedCategoryId != parent.Id) && !parentsToAdd.Contains(parent))
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

    public static void Remove(int id,PermissionCheck permissionCheck,int userId) => Remove(GetCategory(id),permissionCheck,userId);
    public static void Remove(CategoryCacheItem category,PermissionCheck permissionCheck, int userId)
    {
        Remove(Categories, category);
        var connectedQuestions = category.GetAggregatedQuestionsFromMemoryCache(userId);

        foreach (var connectedQuestion in connectedQuestions)
        {
            var categoryInQuestion = connectedQuestion.Categories.FirstOrDefault(c => c.Id == category.Id);
            connectedQuestion.Categories.Remove(categoryInQuestion);
        }

        var parentCategories = GetAllParents(category.Id,permissionCheck);
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
    private static void AddOrUpdate(ConcurrentDictionary<int, UserCacheItem> objectToCache, UserCacheItem obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }
    private static void AddOrUpdate(ConcurrentDictionary<int, CategoryCacheItem> objectToCache, CategoryCacheItem obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void AddOrUpdate(ConcurrentDictionary<int, QuestionCacheItem> objectToCache, QuestionCacheItem obj)
    {
        objectToCache.AddOrUpdate(obj.Id, obj, (k, v) => obj);
    }

    private static void Remove(ConcurrentDictionary<int, UserCacheItem> objectToCache, UserCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out _);
    }

    private static void Remove(ConcurrentDictionary<int, CategoryCacheItem> objectToCache, CategoryCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out _);
    }

    private static void Remove(ConcurrentDictionary<int, QuestionCacheItem> objectToCache, QuestionCacheItem obj)
    {
        objectToCache.TryRemove(obj.Id, out _);
    }

    public static IEnumerable<CategoryCacheItem> GetCategories(IEnumerable<int> getIds)
    {
       var c =  getIds.Select(categoryId => GetCategory(categoryId)).ToList();
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

    public static IList<CategoryCacheItem> GetAllCategories() => Categories.Values.ToList();

    public static List<CategoryCacheItem> GetChildren(int categoryId)
    {
        var allCategories = GetAllCategories();

        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => cr.RelatedCategoryId == categoryId)
                .Select(cr => GetCategory(cr.CategoryId))).ToList();
    }

    public static List<CategoryCacheItem> GetChildren(CategoryCacheItem category, bool isFromEntityCache = false) => GetChildren(category.Id);

    public static IList<CategoryCacheItem> GetAllChildren(int parentId, bool getFromEntityCache = false)
    {
        var currentGeneration = GetChildren(parentId).ToList();
        var nextGeneration = new List<CategoryCacheItem>();
        var descendants = new List<CategoryCacheItem>();

        while (currentGeneration.Count > 0)
        {
            descendants.AddRange(currentGeneration);

            foreach (var category in currentGeneration)
            {
                var children = GetChildren(category.Id).ToList();
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
    public static IEnumerable<int> GetPrivateCategoryIdsFromUser(int userId) => GetAllCategories()
        .Where(c => c.Creator.Id == userId && c.Visibility == CategoryVisibility.Owner)
        .Select(c => c.Id);

    public static List<CategoryCacheItem> ParentCategories(int categoryId,PermissionCheck permissionCheck, bool visibleOnly = false)
    {
        var allCategories = GetAllCategories();
        if (visibleOnly)
        {
           return allCategories.SelectMany(c =>
                c.CategoryRelations.Where(cr => cr.CategoryId == categoryId &&
                                                permissionCheck.CanViewCategory(cr.RelatedCategoryId))
                    .Select(cr => GetCategory(cr.RelatedCategoryId))).ToList();
        }
        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => cr.CategoryId == categoryId)
                .Select(cr => GetCategory(cr.RelatedCategoryId))).ToList();
    }

    public static IList<CategoryCacheItem> GetAllParents(int childId,PermissionCheck permissionCheck, bool getFromEntityCache = false,bool visibleOnly = false)
    {
        var currentGeneration = ParentCategories(childId, permissionCheck, visibleOnly);
        var nextGeneration = new List<CategoryCacheItem>();
        var ascendants = new List<CategoryCacheItem>();

        while (currentGeneration.Count > 0)
        {
            ascendants.AddRange(currentGeneration);

            foreach (var parent in currentGeneration)
            {
                var parents = ParentCategories(parent.Id, permissionCheck, visibleOnly);
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
        return allCategories.Where(c => c.Name.ToLower() == name.ToLower()).ToList();
    }

    public static IEnumerable<int> GetPrivateQuestionIdsFromUser(int userId, 
        IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment webHostEnvironment) => GetAllQuestions()
        .Where(q => q.Creator.Id == userId && q.IsPrivate())
        .Select(q => q.Id);

    public static QuestionCacheItem GetQuestion(int questionId)
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