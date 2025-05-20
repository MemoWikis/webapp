public class ContextPage
{
    private readonly PageRepository _pageRepository;
    private readonly ContextUser _contextUser;

    public List<Page> All = new();
    private readonly TestHarness _testHarness;

    public ContextPage(TestHarness testHarness, bool addContextUser)
    {
        _testHarness = testHarness;
        _pageRepository = testHarness.R<PageRepository>();
        _contextUser = ContextUser.New(_testHarness.R<UserWritingRepo>());

        if (!addContextUser)
            return;

        _contextUser.Add("User").Persist();
    }

    public ContextPage Add(int amount)
    {
        for (var i = 0; i < amount; i++)
            Add($"page name {i + 1}");

        return this;
    }

    public ContextPage Add(Page page)
    {
        All.Add(page);
        return this;
    }

    public ContextPage Add(
        string pageName,
        User? creator = null,
        PageVisibility visibility = PageVisibility.Public,
        bool isWiki = false)
    {
        var page = new Page
        {
            Name = pageName,
            Creator = creator ?? _contextUser.All.First(),
            Visibility = visibility,
            IsWiki = isWiki,
        };

        All.Add(page);

        return this;
    }

    public ContextPage AddChild(Page parent, Page child)
    {
        var modifyRelationsForPage = new ModifyRelationsForPage(_pageRepository, _testHarness.R<PageRelationRepo>());
        modifyRelationsForPage.AddChild(parent.Id, child.Id, 1);

        return this;
    }

    public ContextPage AddToEntityCache(Page page)
    {
        var pageCacheItem = PageCacheItem.ToCachePage(page);

        var cacheUser = UserCacheItem.ToCacheUser(page.Creator);
        EntityCache.AddOrUpdate(cacheUser);
        EntityCache.AddOrUpdate(pageCacheItem);
        EntityCache.UpdatePageReferencesInQuestions(pageCacheItem);

        All.Add(page);
        return this;
    }

    public ContextPage Persist()
    {
        foreach (var page in All)
        {
            if (page.Id <= 0) //if not already created
                _pageRepository.Create(page);
            else
                _pageRepository.Update(
                    page,
                    authorId: page.AuthorIds.First(),
                    type: PageChangeType.Relations
                );
        }


        return this;
    }

    public ContextPage Update(Page page)
    {
        _pageRepository.Update(page);
        return this;
    }

    public ContextPage UpdateAll()
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

    //    foreach (var page in firstChildren)
    //    {
    //        page.Visibility = PageVisibility.All;
    //    }
    //    foreach (var page in secondChildren)
    //    {
    //        page.Visibility = PageVisibility.All;
    //    }
    //    Resolve<EntityCacheInitializer>().Init();

    //    Resolve<SessionUser>().Login(user);
    //    Resolve<SessionUser>().Logout();
    //    return user;
    //}

    public bool HasCorrectChild(PageCacheItem pageCachedItem, int childId)
    {
        var permissionCheck = _testHarness.R<PermissionCheck>();

        var aggregatedPages = pageCachedItem.AggregatedPages(permissionCheck);

        if (aggregatedPages.Any() == false)
            return false;

        return aggregatedPages.TryGetValue(childId, out _);
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