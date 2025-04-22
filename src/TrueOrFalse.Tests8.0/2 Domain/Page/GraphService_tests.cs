class GraphService_tests : BaseTest
{
    [Test]
    public void Should_get_all_ascendants()
    {
        //Arrange
        var context = ContextPage.New();

        context.Add("RootElement").Add("RootElement2").Persist();

        var root = context.All.ByName("RootElement");
        var root2 = context.All.ByName("RootElement2");

        context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: PageVisibility.Private)
            .Add("SubSub2", visibility: PageVisibility.Private)
            .Add("SubSub1and2")
            .Add("Sub3", visibility: PageVisibility.Private)
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

        //Act
        var allAscendants = GraphService.Ascendants(context.All.ByName("SubSub3").Id);

        //Assert
        Assert.That(allAscendants.Count, Is.EqualTo(3));
        Assert.That(allAscendants.Select(i => i.Name), Does.Contain("Sub3"));
        Assert.That(allAscendants.Select(i => i.Name), Does.Contain("RootElement"));
        Assert.That(allAscendants.Select(i => i.Name), Does.Contain("RootElement2"));
    }

    [Test]
    public void Should_get_all_descendants()
    {
        //Arrange
        var context = ContextPage.New();

        context.Add("RootElement").Add("RootElement2").Persist();

        var root = context.All.ByName("RootElement");
        var root2 = context.All.ByName("RootElement2");

        context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: PageVisibility.Private)
            .Add("SubSub2", visibility: PageVisibility.Private)
            .Add("SubSub1and2")
            .Add("Sub3", visibility: PageVisibility.Private)
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

        //Act
        var allDescendants = GraphService.Descendants(context.All.ByName("RootElement").Id);

        //Assert
        Assert.That(allDescendants.Count, Is.EqualTo(7));
    }

    [Test]
    public void Should_get_direct_children()
    {
        var context = ContextPage.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: PageVisibility.Private)
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
        var context = ContextPage.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: PageVisibility.Private)
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
        var context = ContextPage.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: PageVisibility.Private)
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
        var context = ContextPage.New();

        context.Add("RootElement").Add("RootElement2").Persist();

        var root = context.All.ByName("RootElement");
        var root2 = context.All.ByName("RootElement2");

        context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: PageVisibility.Private)
            .Add("SubSub2", visibility: PageVisibility.Private)
            .Add("SubSub1and2")
            .Add("Sub3", visibility: PageVisibility.Private)
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
}