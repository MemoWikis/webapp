using System;
using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tests;

public class ContextCategory: BaseTest
{
    private readonly CategoryRepository _categoryRepository;
    private readonly ContextUser _contextUser = ContextUser.New(R<UserWritingRepo>());
    private int NamesCounter = 0;

    public List<Category> All = new();

    public static ContextCategory New(bool addContextUser = true)
    {
        return new ContextCategory(addContextUser);
    }

    private ContextCategory(bool addContextUser = true)
    {
        _categoryRepository = R<CategoryRepository>();

        if (addContextUser)
            _contextUser.Add("User" + NamesCounter).Persist();
    }

    public ContextCategory Add(int amount)
    {
        for (var i = 0; i < amount; i++)
            Add($"category name {i + 1}", id: i + 1);

        return this;
    }

    public ContextCategory Add(Category category)
    {
        All.Add(category);
        return this;
    }

    public ContextCategory Add(
        string categoryName,
        CategoryType categoryType = CategoryType.Standard,
        User creator = null,
        Category parent = null,
        List<Category> parents = null,
        int id = 0)
    {
        Category category;
        if (_categoryRepository.Exists(categoryName))
        {
            category = _categoryRepository.GetByName(categoryName).First();
        }
        else
        {
            category = new Category
            {
                Name = categoryName,
                Creator = creator ?? _contextUser.All.First(),
                Type = categoryType,
            };

            if (id > 0)
                category.Id = id;
        }

        var categoryRelations = category.CategoryRelations.Count != 0
            ? category.CategoryRelations
            : new List<CategoryRelation>();

        if (parent != null) // set parent
        {
            categoryRelations.Add(new CategoryRelation
            {
                Child = category,
                Parent = parent,
            });

            category.CategoryRelations = categoryRelations;
        }

        if (parents != null) // set parent
        {
            foreach (var p in parents)
            {
                categoryRelations.Add(new CategoryRelation
                {
                    Child = category,
                    Parent = p,
                });
            }

            category.CategoryRelations = categoryRelations;
        }

        if (!_categoryRepository.Exists(categoryName))
        {
            All.Add(category);
        }

        return this;
    }

    public ContextCategory AddToEntityCache(string categoryName,
        CategoryType categoryType = CategoryType.Standard,
        User creator = null,
        bool withId = false,
        int categoryId = 0
    )
    {
        var category = new Category();

        if (withId && categoryId == 0)
            category.Id = 0;
        else
            category.Id = categoryId;

        category.Name = categoryName;
        category.Creator = creator == null ? _contextUser.All.FirstOrDefault() : creator;
        category.Type = categoryType;

        var categoryCacheItem = CategoryCacheItem.ToCacheCategory(category);
        EntityCache.AddOrUpdate(categoryCacheItem);
        EntityCache.UpdateCategoryReferencesInQuestions(categoryCacheItem);

        All.Add(category);
        return this;
    }


    public ContextCategory QuestionCount(int questionCount)
    {
        All.Last().CountQuestions = questionCount;
        return this;
    }

    public ContextCategory Persist()
    {
        foreach (var cat in All)
            if (cat.Id <= 0) //if not already created
                _categoryRepository.Create(cat);

        return this;
    }

    public ContextCategory Update(Category category)
    {
        _categoryRepository.Update(category);

        _categoryRepository.Session.Flush();
        return this;
    }

    public ContextCategory Update()
    {
        foreach (var cat in All)
            _categoryRepository.Update(cat);

        _categoryRepository.Session.Flush();

        return this;
    }

    public User AddCaseThreeToCache(bool withWishKnowledge = true, ContextUser contextUser = null)
    {
        //Add this Case: https://drive.google.com/file/d/1CEMMm1iIhfNKvuKng5oM6erR0bVDWHr6/view?usp=sharing
        var rootElement = Add("A").Persist().All.First();

        if (contextUser == null)
            contextUser = ContextUser.New(R<UserWritingRepo>());
        var user = contextUser.Add("User" + new Random().Next(0, 32000)).Persist(true, this).All[0];

        var firstChildren =
            Add("X", parent: rootElement)
                .Add("X1", parent: rootElement)
                .Add("X2", parent: rootElement)
                .Add("X3", parent: rootElement)
                .Persist().All;

        Add("X1", parent: firstChildren.ByName("X3"));

        var secondChildren = Add("B", parent: rootElement)
            .Add("C", parent: firstChildren.ByName("X"))
            .Persist().All;

        Add("C", parent: firstChildren.ByName("X1")).Persist();
        Add("C", parent: firstChildren.ByName("X2")).Persist();

        Add("H", parent: firstChildren.ByName("C"))
            .Add("G", parent: secondChildren.ByName("C"))
            .Add("F", parent: secondChildren.ByName("C"))
            .Add("E", parent: secondChildren.ByName("C"))
            .Add("D", parent: secondChildren.ByName("B"))
            .Persist();

        Add("I", parent: secondChildren.ByName("C")).Persist();
        Add("I", parent: secondChildren.ByName("E")).Persist();
        Add("I", parent: secondChildren.ByName("G")).Persist();

        foreach (var category in firstChildren)
        {
            category.Visibility = CategoryVisibility.All;
        }
        foreach (var category in secondChildren)
        {
            category.Visibility = CategoryVisibility.All;
        }
        Resolve<EntityCacheInitializer>().Init();

        Resolve<SessionUser>().Login(user);
        Resolve<SessionUser>().Logout();
        return user;
    }

    public static bool HasCorrectChild(CategoryCacheItem categoryCachedItem, int childId)
    {
        var permissionCheck = R<PermissionCheck>();

       var aggregatedCategorys =  categoryCachedItem.AggregatedCategories(permissionCheck);

       if(aggregatedCategorys.Any() == false )
           return false;

       return aggregatedCategorys.TryGetValue(childId, out _);
    }

    public static bool isIdAvailableInRelations(CategoryCacheItem categoryCacheItem, int deletedId)
    {
        return categoryCacheItem.CategoryRelations.Any(cr =>
            cr.RelatedCategoryId == deletedId || cr.ChildCategoryId == deletedId);
    }
}