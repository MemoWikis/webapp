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

        if (addContextUser)
            _contextUser.Add("User").Persist();

        Add("Root Page", isWiki: true);
    }

    public ContextPage Add(int amount, bool isWiki = false)
    {
        var name = isWiki ? "Wiki name" : "Page name";

        for (var i = 0; i < amount; i++)
            Add($"{name} {i + 1}", isWiki: isWiki);

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

    public Page AddAndGet(
        string pageName,
        User? creator = null,
        PageVisibility visibility = PageVisibility.Public,
        bool isWiki = false) => Add(pageName, creator, visibility, isWiki).All.Last();

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

    public Page GetPageByName(string name)
    {
        return All.Single(c => c.Name == name);
    }

    public ContextUser ContextUser => _contextUser;
}