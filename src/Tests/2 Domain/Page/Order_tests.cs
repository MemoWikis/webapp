class Order_tests : BaseTestHarness
{
    [Test]
    public async Task Should_Sort_Pages()
    {
        //Arrange
        var unsortedRelations = new List<PageRelationCache>
        {
            new()
            {
                Id = 1,
                ChildId = 3,
                ParentId = 10,
                PreviousId = 2,
                NextId = null
            },
            new()
            {
                Id = 2,
                ChildId = 1,
                ParentId = 10,
                PreviousId = null,
                NextId = 2
            },
            new()
            {
                Id = 3,
                ChildId = 2,
                ParentId = 10,
                PreviousId = 1,
                NextId = 3
            }
        };

        //Act
        var sortedRelations = PageOrderer.Sort(unsortedRelations, 10);

        //Assert
        await Verify(sortedRelations);
    }

    [Test]
    public async Task Should_Sort_Pages_with_faulty_relations()
    {
        await ClearData();

        //Arrange
        var unsortedRelations = new List<PageRelationCache>
        {
            new()
            {
                Id = 1,
                ChildId = 3,
                ParentId = 10,
                PreviousId = 2,
                NextId = null
            },
            new()
            {
                Id = 2,
                ChildId = 1,
                ParentId = 10,
                PreviousId = null,
                NextId = 2
            },
            new()
            {
                Id = 3,
                ChildId = 2,
                ParentId = 10,
                PreviousId = 1,
                NextId = 3
            },
            new()
            {
                Id = 4,
                ChildId = 4,
                ParentId = 10,
                PreviousId = 2,
                NextId = null
            },
            new()
            {
                Id = 5,
                ChildId = 5,
                ParentId = 10,
                PreviousId = null,
                NextId = null
            },
            new()
            {
                Id = 6,
                ChildId = 6,
                ParentId = 10,
                PreviousId = null,
                NextId = 3
            },
        };

        //Act
        var sortedRelations = PageOrderer.Sort(unsortedRelations, 10);

        //Assert
        await Verify(sortedRelations);
    }

    [Test]
    public async Task Should_init_children_in_EntityCache()
    {
        await ClearData();

        //Arrange
        var context = NewPageContext();

        context.Add("root").Persist();

        context
            .Add("sub1")
            .Add("sub2")
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub2 = context.All.ByName("sub2");

        context.AddChild(root, sub1);
        context.AddChild(root, sub2);

        //Act
        await base.ReloadCaches();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        //Assert
        var cachedRoot = EntityCache.GetPage(root);

        await Verify(cachedRoot!.ChildRelations);
    }

    //Move sub1 after sub3
    [Test]
    public async Task Should_move_relation_after_sub3()
    {
        await ClearData();

        //Arrange
        var context = NewPageContext();

        context.Add("root").Persist();

        context
            .Add("sub1")
            .Add("sub2")
            .Add("sub3")
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub2 = context.All.ByName("sub2");
        var sub3 = context.All.ByName("sub3");

        context.AddChild(root, sub1);
        context.AddChild(root, sub2);
        context.AddChild(root, sub3);

        await ReloadCaches();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetPage(root);
        var originalTree = TreeRenderer.ToAsciiDiagram(cachedRoot);

        var relationToMove = cachedRoot.ChildRelations[0]; //sub1
        var modifyRelationsForPage = R<ModifyRelationsForPage>();

        //Act
        PageOrderer.MoveAfter(relationToMove, sub3.Id, cachedRoot.Id, 1, modifyRelationsForPage);

        var newTree = TreeRenderer.ToAsciiDiagram(cachedRoot);

        //Assert
        var allRelationsInDb = R<PageRelationRepo>().GetAll();
        var allRelationsCache = EntityCache.GetPage(root)?.ChildRelations;

        await Verify(
            new
            {
                originalTree,
                newTree,
                allRelationsCache,
                allRelationsInDb
            }
        );
    }

    //Move sub3 before sub1
    [Test]
    public async Task Should_move_relation_before_sub1()
    {
        await ReloadCaches();

        //Arrange
        var context = NewPageContext();

        context.Add("root").Persist();

        context
            .Add("sub1")
            .Add("sub2")
            .Add("sub3")
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub2 = context.All.ByName("sub2");
        var sub3 = context.All.ByName("sub3");

        context.AddChild(root, sub1);
        context.AddChild(root, sub2);
        context.AddChild(root, sub3);

        await ReloadCaches();

        var cachedRoot = EntityCache.GetPage(root);
        if (cachedRoot == null)
        {
            throw new InvalidOperationException("Cached root page is null");
        }

        var originalTree = TreeRenderer.ToAsciiDiagram(cachedRoot);
        var relationToMove = cachedRoot.ChildRelations[2];
        var pageRelationRepo = R<PageRelationRepo>();
        var modifyRelationsForPage =
            new ModifyRelationsForPage(R<PageRepository>(), pageRelationRepo);

        //Act
        PageOrderer.MoveBefore(relationToMove, sub1.Id, cachedRoot.Id, 1,
            modifyRelationsForPage);

        var newTree = TreeRenderer.ToAsciiDiagram(cachedRoot);
        var allRelationsInDb = pageRelationRepo.GetAll();

        //Assert
        await Verify(new { originalTree, newTree, allRelationsInDb, childRelations = cachedRoot.ChildRelations });
    }

    //Move sub1 after sub3 and before sub4
    [Test]
    public async Task Should_move_relation_after_sub3_and_before_sub4()
    {
        await ReloadCaches();

        //Arrange
        var context = NewPageContext();

        context.Add("root").Persist();

        context
            .Add("sub1")
            .Add("sub2")
            .Add("sub3")
            .Add("sub4")
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub2 = context.All.ByName("sub2");
        var sub3 = context.All.ByName("sub3");
        var sub4 = context.All.ByName("sub4");

        context.AddChild(root, sub1);
        context.AddChild(root, sub2);
        context.AddChild(root, sub3);
        context.AddChild(root, sub4);

        await ReloadCaches();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetPage(root);
        if (cachedRoot == null)
        {
            throw new InvalidOperationException("Cached root page is null");
        }

        var originalTree = TreeRenderer.ToAsciiDiagram(cachedRoot);
        var relationToMove = cachedRoot.ChildRelations[0];
        var pageRelationRepo = R<PageRelationRepo>();
        var modifyRelationsForPage =
            new ModifyRelationsForPage(R<PageRepository>(), pageRelationRepo);

        //Act
        PageOrderer.MoveAfter(relationToMove, sub3.Id, cachedRoot.Id, 1,
            modifyRelationsForPage);

        var newTree = TreeRenderer.ToAsciiDiagram(cachedRoot);
        var allRelationsInDb = pageRelationRepo.GetAll();

        //Assert
        await Verify(new { originalTree, newTree, allRelationsInDb, childRelations = cachedRoot.ChildRelations });
    }

    [Test]
    public async Task Should_fail_move_relation_caused_by_circularReference()
    {
        //Arrange
        var context = NewPageContext();

        context.Add("root").Persist();

        context
            .Add("sub1")
            .Add("sub1sub1")
            .Add("sub1sub1sub1")
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub1sub1 = context.All.ByName("sub1sub1");
        var sub1sub1sub1 = context.All.ByName("sub1sub1sub1");

        context.AddChild(root, sub1);
        context.AddChild(sub1, sub1sub1);
        context.AddChild(sub1sub1, sub1sub1sub1);

        await ReloadCaches();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetPage(root);
        if (cachedRoot == null)
        {
            throw new InvalidOperationException("Cached root page is null");
        }

        var originalTree = TreeRenderer.ToAsciiDiagram(cachedRoot);
        var relationToMove = cachedRoot.ChildRelations[0];
        var pageRelationRepo = R<PageRelationRepo>();
        var modifyRelationsForPage =
            new ModifyRelationsForPage(R<PageRepository>(), pageRelationRepo);

        //Act & Assert
        var ex1 = Assert.Throws<Exception>(() => PageOrderer.MoveAfter(relationToMove,
            sub1sub1sub1.Id, sub1sub1.Id, 1, modifyRelationsForPage));

        var ex2 = Assert.Throws<Exception>(() =>
            PageOrderer.MoveAfter(relationToMove, sub1sub1.Id, sub1.Id, 1,
                modifyRelationsForPage));

        await Verify(new { originalTree, errorMessage1 = ex1.Message, errorMessage2 = ex2.Message });
    }

    [Test]
    public async Task Should_remove_old_parent_and_add_new_parent_on_MoveIn()
    {
        await ClearData();

        //Arrange
        var context = NewPageContext();
        var authorId = 1;

        var creator = new User { Id = authorId };

        context.Add("root", creator: creator).Persist();

        context
            .Add("sub1", creator: creator)
            .Add("sub2", creator: creator)
            .Add("sub1sub1", creator: creator)
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub2 = context.All.ByName("sub2");
        var sub1sub1 = context.All.ByName("sub1sub1");

        context.AddChild(root, sub1);
        context.AddChild(sub1, sub1sub1);
        context.AddChild(root, sub2);

        await ReloadCaches();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedSub1 = EntityCache.GetPage(sub1);
        if (cachedSub1 == null)
        {
            throw new InvalidOperationException("Cached sub1 page is null");
        }

        var cachedSub1Tree = TreeRenderer.ToAsciiDiagram(cachedSub1);
        var cachedSub2 = EntityCache.GetPage(sub2);
        if (cachedSub2 == null)
        {
            throw new InvalidOperationException("Cached sub2 page is null");
        }

        var cachedSub2Tree = TreeRenderer.ToAsciiDiagram(cachedSub2);
        var relationToMove = cachedSub1.ChildRelations[0];
        var pageRelationRepo = R<PageRelationRepo>();
        var modifyRelationsForPage = new ModifyRelationsForPage(R<PageRepository>(), pageRelationRepo);
        var permissionCheck = new PermissionCheck(new SessionlessUser(authorId));

        //Act
        PageOrderer.MoveIn(relationToMove, sub2.Id, authorId, modifyRelationsForPage, permissionCheck);

        //Assert
        var newCachedSub1Tree = TreeRenderer.ToAsciiDiagram(cachedSub1);
        var newCachedSub2Tree = TreeRenderer.ToAsciiDiagram(cachedSub2);
        var movedSub = EntityCache.GetPage(sub1sub1);

        await Verify(new
        {
            originalSub1Tree = cachedSub1Tree,
            originalSub2Tree = cachedSub2Tree,
            newSub1Tree = newCachedSub1Tree,
            newSub2Tree = newCachedSub2Tree,
            sub1ChildRelationsCount = cachedSub1.ChildRelations.Count,
            sub2ChildRelationsCount = cachedSub2.ChildRelations.Count,
            movedSubParentRelationsCount = movedSub?.ParentRelations?.Count
        });
    }


}