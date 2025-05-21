using Antlr.Runtime.Tree;
using NHibernate.Driver;

class Order_tests : BaseTestHarness
{
    [Test]
    public void Should_Sort_Pages()
    {
        //Arrange
        var unsortedRelations = new List<PageRelationCache>
        {
            new() { Id = 1, ChildId = 3, ParentId = 10, PreviousId = 2, NextId = null },
            new() { Id = 2, ChildId = 1, ParentId = 10, PreviousId = null, NextId = 2 },
            new() { Id = 3, ChildId = 2, ParentId = 10, PreviousId = 1, NextId = 3 }
        };

        //Act
        var sortedRelations = PageOrderer.Sort(unsortedRelations, 10);

        //Assert
        Assert.That(sortedRelations, Is.Not.Null);
        Assert.That(sortedRelations.Count, Is.EqualTo(3));
        Assert.That(sortedRelations[0].ChildId, Is.EqualTo(1));
        Assert.That(sortedRelations[1].ChildId, Is.EqualTo(2));
        Assert.That(sortedRelations[2].ChildId, Is.EqualTo(3));
    }

    [Test]
    public void Should_Sort_Pages_with_faulty_relations()
    {
        //Arrange
        var unsortedRelations = new List<PageRelationCache>
        {
            new() { Id = 1, ChildId = 3, ParentId = 10, PreviousId = 2, NextId = null },
            new() { Id = 2, ChildId = 1, ParentId = 10, PreviousId = null, NextId = 2 },
            new() { Id = 3, ChildId = 2, ParentId = 10, PreviousId = 1, NextId = 3 },

            new() { Id = 4, ChildId = 4, ParentId = 10, PreviousId = 2, NextId = null },
            new() { Id = 5, ChildId = 5, ParentId = 10, PreviousId = null, NextId = null },
            new() { Id = 6, ChildId = 6, ParentId = 10, PreviousId = null, NextId = 3 },
        };

        //Act
        var sortedRelations = PageOrderer.Sort(unsortedRelations, 10);

        Assert.That(sortedRelations, Is.Not.Null);
        Assert.That(sortedRelations.Count, Is.EqualTo(6));
        Assert.That(sortedRelations[0].ChildId, Is.EqualTo(1));
        Assert.That(sortedRelations[1].ChildId, Is.EqualTo(2));
        Assert.That(sortedRelations[2].ChildId, Is.EqualTo(3));

        Assert.That(sortedRelations[5].ChildId, Is.EqualTo(6));
    }

    [Test]
    public async Task Should_init_children_in_EntityCache()
    {
        await ReloadCaches();
        
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
        await Verify(new
            {
                originalTree, 
                newTree,
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
        var relationToMove = cachedRoot.ChildRelations[2];
        var pageRelationRepo = R<PageRelationRepo>();
        var modifyRelationsForPage =
            new ModifyRelationsForPage(R<PageRepository>(), pageRelationRepo);

        //Act
        PageOrderer.MoveBefore(relationToMove, sub1.Id, cachedRoot.Id, 1,
            modifyRelationsForPage);

        //Assert
        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(3));

        Assert.That(cachedRoot.ChildRelations[0].PreviousId, Is.Null);
        Assert.That(cachedRoot.ChildRelations[0].ChildId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub1.Id));

        Assert.That(cachedRoot.ChildRelations[1].ChildId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[1].NextId, Is.EqualTo(sub2.Id));

        Assert.That(cachedRoot.ChildRelations[2].ChildId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[2].PreviousId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[2].NextId, Is.Null);

        var allRelationsInDb = pageRelationRepo.GetAll();

        Assert.That(allRelationsInDb.Count, Is.EqualTo(3));

        var firstCachedId = cachedRoot.ChildRelations[0].Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.Child.Id,
            Is.EqualTo(cachedRoot.ChildRelations[0].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.PreviousId,
            Is.EqualTo(cachedRoot.ChildRelations[0].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.NextId,
            Is.EqualTo(cachedRoot.ChildRelations[0].NextId));

        var lastCachedId = cachedRoot.ChildRelations.LastOrDefault()?.Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.Child.Id,
            Is.EqualTo(cachedRoot.ChildRelations[2].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.PreviousId,
            Is.EqualTo(cachedRoot.ChildRelations[2].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.NextId,
            Is.EqualTo(cachedRoot.ChildRelations[2].NextId));
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
        var relationToMove = cachedRoot.ChildRelations[0];
        var pageRelationRepo = R<PageRelationRepo>();
        var modifyRelationsForPage =
            new ModifyRelationsForPage(R<PageRepository>(), pageRelationRepo);

        //Act
        PageOrderer.MoveAfter(relationToMove, sub3.Id, cachedRoot.Id, 1,
            modifyRelationsForPage);

        //Assert
        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(4));

        Assert.That(cachedRoot.ChildRelations[0].PreviousId, Is.Null);
        Assert.That(cachedRoot.ChildRelations[0].ChildId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub3.Id));

        Assert.That(cachedRoot.ChildRelations[1].ChildId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[1].NextId, Is.EqualTo(sub1.Id));

        Assert.That(cachedRoot.ChildRelations[2].ChildId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[2].PreviousId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[2].NextId, Is.EqualTo(sub4.Id));

        Assert.That(cachedRoot.ChildRelations[3].ChildId, Is.EqualTo(sub4.Id));
        Assert.That(cachedRoot.ChildRelations[3].PreviousId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[3].NextId, Is.Null);

        var allRelationsInDb = pageRelationRepo.GetAll();

        Assert.That(allRelationsInDb.Count, Is.EqualTo(4));

        var firstCachedId = cachedRoot.ChildRelations[0].Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.Child.Id,
            Is.EqualTo(cachedRoot.ChildRelations[0].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.PreviousId,
            Is.EqualTo(cachedRoot.ChildRelations[0].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.NextId,
            Is.EqualTo(cachedRoot.ChildRelations[0].NextId));

        var lastCachedId = cachedRoot.ChildRelations.LastOrDefault()?.Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.Child.Id,
            Is.EqualTo(cachedRoot.ChildRelations[3].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.PreviousId,
            Is.EqualTo(cachedRoot.ChildRelations[3].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.NextId,
            Is.EqualTo(cachedRoot.ChildRelations[3].NextId));
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
        var relationToMove = cachedRoot.ChildRelations[0];
        var pageRelationRepo = R<PageRelationRepo>();
        var modifyRelationsForPage =
            new ModifyRelationsForPage(R<PageRepository>(), pageRelationRepo);

        //Act & Assert
        var ex = Assert.Throws<Exception>(() => PageOrderer.MoveAfter(relationToMove,
            sub1sub1sub1.Id, sub1sub1.Id, 1, modifyRelationsForPage));

        Assert.That(ex.Message, Is.EqualTo(FrontendMessageKeys.Error.Page.CircularReference));

        var ex2 = Assert.Throws<Exception>(() =>
            PageOrderer.MoveAfter(relationToMove, sub1sub1.Id, sub1.Id, 1,
                modifyRelationsForPage));
        Assert.That(ex2.Message, Is.EqualTo(FrontendMessageKeys.Error.Page.CircularReference));
    }

    [Test]
    public async Task Should_remove_old_parent_and_add_new_parent_on_MoveIn()
    {
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
        var relationToMove = cachedSub1.ChildRelations[0];
        var pageRelationRepo = R<PageRelationRepo>();
        var modifyRelationsForPage = new ModifyRelationsForPage(R<PageRepository>(), pageRelationRepo);
        var permissionCheck = new PermissionCheck(new SessionlessUser(authorId));

        //Act
        PageOrderer.MoveIn(relationToMove, sub2.Id, authorId, modifyRelationsForPage, permissionCheck);

        //Assert
        Assert.That(cachedSub1.ChildRelations.Count, Is.EqualTo(0));

        var cachedSub2 = EntityCache.GetPage(sub2);
        Assert.That(cachedSub2.ChildRelations.Count, Is.EqualTo(1));

        var movedSub = EntityCache.GetPage(sub1sub1);

        Assert.That(cachedSub2.ChildRelations[0].ChildId, Is.EqualTo(movedSub.Id));
        Assert.That(movedSub.ParentRelations.Count, Is.EqualTo(1));
    }
}