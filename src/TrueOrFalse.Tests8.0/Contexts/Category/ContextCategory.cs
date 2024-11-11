public class ContextCategory : BaseTest
{
    private readonly PageRepository _pageRepository;
    private readonly ContextUser _contextUser = ContextUser.New(R<UserWritingRepo>());
    private int NamesCounter = 0;

    public List<Page> All = new();

    public static ContextCategory New(bool addContextUser = true)
    {
        return new ContextCategory(addContextUser);
    }

    private ContextCategory(bool addContextUser = true)
    {
        _pageRepository = R<PageRepository>();

        if (addContextUser)
            _contextUser.Add("User" + NamesCounter).Persist();
    }

    public ContextCategory Add(int amount)
    {
        for (var i = 0; i < amount; i++)
            Add($"category name {i + 1}");

        return this;
    }

    public ContextCategory Add(Page page)
    {
        All.Add(page);
        return this;
    }

    public ContextCategory Add(
        string categoryName,
        PageType pageType = PageType.Standard,
        User? creator = null,
        PageVisibility visibility = PageVisibility.All)
    {
        var category = new Page
        {
            Name = categoryName,
            Creator = creator ?? _contextUser.All.First(),
            Type = pageType,
            Visibility = visibility
        };

        All.Add(category);

        return this;
    }

    public ContextCategory AddChild(Page parent, Page child)
    {
        var modifyRelationsForCategory = new ModifyRelationsForCategory(_pageRepository, R<PageRelationRepo>());
        modifyRelationsForCategory.AddChild(parent.Id, child.Id, 1);

        return this;
    }

    public ContextCategory AddToEntityCache(Page page)
    {
        var categoryCacheItem = PageCacheItem.ToCacheCategory(page);

        var cacheUser = UserCacheItem.ToCacheUser(page.Creator);
        EntityCache.AddOrUpdate(cacheUser);
        EntityCache.AddOrUpdate(categoryCacheItem);
        EntityCache.UpdateCategoryReferencesInQuestions(categoryCacheItem);

        All.Add(page);
        return this;
    }

    public ContextCategory Persist()
    {
        foreach (var cat in All)
            if (cat.Id <= 0) //if not already created
                _pageRepository.Create(cat);
            else
            {
                _pageRepository.Update(cat, authorId: cat.AuthorIds.First(),
                    type: PageChangeType.Relations);
            }

        return this;
    }

    public ContextCategory Update(Page page)
    {
        _pageRepository.Update(page);
        return this;
    }

    public ContextCategory UpdateAll()
    {
        foreach (var cat in All)
            _pageRepository.Update(cat);

        return this;
    }

    //public User AddCaseThreeToCache(bool withWuwi = true, ContextUser contextUser = null)
    //{
    //    //Add this Case: https://drive.google.com/file/d/1CEMMm1iIhfNKvuKng5oM6erR0bVDWHr6/view?usp=sharing
    //    var rootElement = Add("A").Persist().All.First();

    //    if (contextUser == null)
    //        contextUser = ContextUser.New(R<UserWritingRepo>());
    //    var user = contextUser.Add("User" + new Random().Next(0, 32000)).Persist(true, this).All[0];

    //    var firstChildren =
    //        Add("X", parent: rootElement)
    //            .Add("X1", parent: rootElement)
    //            .Add("X2", parent: rootElement)
    //            .Add("X3", parent: rootElement)
    //            .Persist().All;

    //    Add("X1", parent: firstChildren.ByName("X3"));

    //    var secondChildren = Add("B", parent: rootElement)
    //        .Add("C", parent: firstChildren.ByName("X"))
    //        .Persist().All;

    //    Add("C", parent: firstChildren.ByName("X1")).Persist();
    //    Add("C", parent: firstChildren.ByName("X2")).Persist();

    //    Add("H", parent: firstChildren.ByName("C"))
    //        .Add("G", parent: secondChildren.ByName("C"))
    //        .Add("F", parent: secondChildren.ByName("C"))
    //        .Add("E", parent: secondChildren.ByName("C"))
    //        .Add("D", parent: secondChildren.ByName("B"))
    //        .Persist();

    //    Add("I", parent: secondChildren.ByName("C")).Persist();
    //    Add("I", parent: secondChildren.ByName("E")).Persist();
    //    Add("I", parent: secondChildren.ByName("G")).Persist();

    //    foreach (var category in firstChildren)
    //    {
    //        category.Visibility = CategoryVisibility.All;
    //    }
    //    foreach (var category in secondChildren)
    //    {
    //        category.Visibility = CategoryVisibility.All;
    //    }
    //    Resolve<EntityCacheInitializer>().Init();

    //    Resolve<SessionUser>().Login(user);
    //    Resolve<SessionUser>().Logout();
    //    return user;
    //}

    public static bool HasCorrectChild(PageCacheItem pageCachedItem, int childId)
    {
        var permissionCheck = R<PermissionCheck>();

        var aggregatedCategorys = pageCachedItem.AggregatedCategories(permissionCheck);

        if (aggregatedCategorys.Any() == false)
            return false;

        return aggregatedCategorys.TryGetValue(childId, out _);
    }

    public static bool isIdAvailableInRelations(PageCacheItem pageCacheItem, int deletedId)
    {
        return pageCacheItem.ParentRelations.Any(cr =>
            cr.ParentId == deletedId || cr.ChildId == deletedId);
    }

    public Page GetPageByName(string name)
    {
        return All.Single(c => c.Name == name);
    }
}