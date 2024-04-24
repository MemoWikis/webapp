class EntityCache_tests : BaseTest
{
    [Test]
    public void Should_get_direct_children()
    {
        var context = ContextCategory.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: CategoryVisibility.Owner)
            .Persist()
            .All;

        context.AddChild(root, children.ByName("Sub1"));
        context.AddChild(children.ByName("Sub1"), children.ByName("SubSub1"));
        context.AddChild(root, children.ByName("Sub2"));

        RecycleContainerAndEntityCache();

        var directChildren = GraphService.Children(root.Id);
        Assert.That(directChildren.Count, Is.EqualTo(2));
        Assert.That(directChildren.First().Name, Is.EqualTo("Sub1"));
        Assert.That(directChildren.Last().Name, Is.EqualTo("Sub2"));
    }

    [Test]
    public void Should_get_direct_visible_children()
    {
        var context = ContextCategory.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: CategoryVisibility.Owner)
            .Persist()
            .All;

        context.AddChild(root, children.ByName("Sub1"));
        context.AddChild(children.ByName("Sub1"), children.ByName("SubSub1"));

        context.AddChild(root, children.ByName("Sub2"));

        RecycleContainerAndEntityCache();

        var defaultUserId = -1;
        var permissionCheck = new PermissionCheck(defaultUserId);

        var directChildren = GraphService.VisibleChildren(root.Id, permissionCheck, defaultUserId)
            .First();
        Assert.That(directChildren.Name, Is.EqualTo("Sub1"));
    }

    [Test]
    public void Should_get_all_children()
    {
        var context = ContextCategory.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: CategoryVisibility.Owner)
            .Persist()
            .All;

        context.AddChild(root, children.ByName("Sub1"));
        context.AddChild(children.ByName("Sub1"), children.ByName("SubSub1"));
        context.AddChild(root, children.ByName("Sub2"));

        RecycleContainerAndEntityCache();

        var directChildren = GraphService.Descendants(root.Id);

        Assert.That(directChildren.Count, Is.EqualTo(3));
        Assert.That(directChildren.Select(i => i.Name), Does.Contain("Sub1"));
        Assert.That(directChildren.Select(i => i.Name), Does.Contain("Sub2"));
        Assert.That(directChildren.Select(i => i.Name), Does.Contain("SubSub1"));
    }

    [Test]
    public void Should_get_all_visible_children()
    {
        var context = ContextCategory.New();

        context.Add("RootElement").Add("RootElement2").Persist();

        var root = context.All.ByName("RootElement");
        var root2 = context.All.ByName("RootElement2");

        context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: CategoryVisibility.Owner)
            .Add("SubSub2", visibility: CategoryVisibility.Owner)
            .Add("SubSub1and2")
            .Add("Sub3", visibility: CategoryVisibility.Owner)
            .Add("SubSub3")
            .Persist();

        context.AddChild(root, context.All.ByName("Sub1"));
        context.AddChild(context.All.ByName("Sub1"), context.All.ByName("SubSub1"));

        context.AddChild(root, context.All.ByName("Sub2"));
        context.AddChild(context.All.ByName("Sub2"), context.All.ByName("SubSub2"));

        context.AddChild(context.All.ByName("Sub1"), context.All.ByName("SubSub1and2"));
        context.AddChild(context.All.ByName("Sub2"), context.All.ByName("SubSub1and2"));

        context.AddChild(root, context.All.ByName("Sub3"));
        context.AddChild(context.All.ByName("Sub3"), context.All.ByName("SubSub3"));
        context.AddChild(root2, context.All.ByName("SubSub3"));

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var defaultUserId = -1;
        var permissionCheck = new PermissionCheck(defaultUserId);

        var allChildren = GraphService.VisibleDescendants(root.Id, permissionCheck, defaultUserId);
        Assert.That(allChildren.Count, Is.EqualTo(3));
        Assert.That(allChildren.Last().Name, Is.EqualTo("SubSub1and2"));
    }

    [Test]
    public void Should_be_added_to_EntityCache()
    {
        var context = ContextCategory.New(false);
        var category = new Category
        {
            Id = 15,
            Name = "Test",
            Creator = new User
            {
                Id = 2,
                Name = "Daniel"
            }
        };
        context.AddToEntityCache(category);

        var cacheCategory = EntityCache.GetCategory(category);

        Assert.NotNull(cacheCategory);
        Assert.AreEqual(cacheCategory.Id, category.Id);
        Assert.AreEqual(cacheCategory.Name, category.Name);
        Assert.AreEqual(cacheCategory.Creator.Name, category.Creator.Name);
        Assert.AreEqual(cacheCategory.Creator.Id, category.Creator.Id);
        Assert.AreNotEqual(cacheCategory.Creator.Id, 0);
        Assert.AreNotEqual(cacheCategory.Id, 0);
        Assert.AreNotEqual(cacheCategory.Creator.Name, "");
        Assert.AreNotEqual(cacheCategory.Name, "");
    }
}