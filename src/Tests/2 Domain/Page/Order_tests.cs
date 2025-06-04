using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

class Order_tests : BaseTestHarness
{
    [Test]
    public async Task Should_Sort_Pages()
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
        await Verify(sortedRelations);
    }

    [Test]
    public async Task Should_Sort_Pages_with_faulty_relations()
    {
        await ClearData();

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
        await Verify(new
        {
            originalTree,
            newTree,
            allRelationsInDb,
            childRelations = cachedRoot.ChildRelations
        });
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
        await Verify(new
        {
            originalTree,
            newTree,
            allRelationsInDb,
            childRelations = cachedRoot.ChildRelations
        });
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

        await Verify(new
        {
            originalTree,
            errorMessage1 = ex1.Message,
            errorMessage2 = ex2.Message
        });
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

    [Test]
    public async Task Should_maintain_integrity_when_moving_pages_multiple_times()
    {
        await ClearData();

        //Arrange
        var context = NewPageContext();
        var sessionUser = R<SessionUser>();
        var authorId = sessionUser.UserId;
        var creator = new User { Id = authorId };

        context.Add("root", creator: creator).Persist();

        context
            .Add("sub1", creator: creator)
            .Add("sub2", creator: creator)
            .Add("sub3", creator: creator)
            .Add("sub4", creator: creator)
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

        var pageRelationRepo = R<PageRelationRepo>();
        var pageRepo = R<PageRepository>();
        var permissionCheck = new PermissionCheck(new SessionlessUser(authorId));
        var httpContextAccessor = R<IHttpContextAccessor>();
        var webHostEnvironment = R<IWebHostEnvironment>();
        var userWritingRepo = R<UserWritingRepo>();
        var searchResultBuilder = R<SearchResultBuilder>();

        var controller = new EditPageRelationStoreController(
            sessionUser,
            httpContextAccessor,
            permissionCheck,
            pageRepo,
            pageRelationRepo,
            userWritingRepo,
            webHostEnvironment,
            searchResultBuilder);

        var cachedRoot = EntityCache.GetPage(root);
        var initialTree = TreeRenderer.ToAsciiDiagram(cachedRoot);
        var snapshots = new Dictionary<string, string>();
        snapshots.Add("initial", initialTree);

        //Act - Perform multiple moves in sequence

        // Move 1: sub1 after sub3
        var move1Result = controller.MovePage(new EditPageRelationStoreController.MovePageRequest(
            sub1.Id, sub3.Id, EditPageRelationStoreController.TargetPosition.After, root.Id, root.Id));

        await ReloadCaches();
        snapshots.Add("after_move1", TreeRenderer.ToAsciiDiagram(EntityCache.GetPage(root)));

        // Move 2: sub4 before sub2
        var move2Result = controller.MovePage(new EditPageRelationStoreController.MovePageRequest(
            sub4.Id, sub2.Id, EditPageRelationStoreController.TargetPosition.Before, root.Id, root.Id));

        await ReloadCaches();
        snapshots.Add("after_move2", TreeRenderer.ToAsciiDiagram(EntityCache.GetPage(root)));

        // Move 3: sub2 after sub3
        var move3Result = controller.MovePage(new EditPageRelationStoreController.MovePageRequest(
            sub2.Id, sub3.Id, EditPageRelationStoreController.TargetPosition.After, root.Id, root.Id));

        await ReloadCaches();
        snapshots.Add("after_move3", TreeRenderer.ToAsciiDiagram(EntityCache.GetPage(root)));

        // Move 4: sub3 to beginning (before sub4)
        var move4Result = controller.MovePage(new EditPageRelationStoreController.MovePageRequest(
            sub3.Id, sub4.Id, EditPageRelationStoreController.TargetPosition.Before, root.Id, root.Id));

        await ReloadCaches();
        snapshots.Add("after_move4", TreeRenderer.ToAsciiDiagram(EntityCache.GetPage(root)));

        // Final state verification
        var finalTree = TreeRenderer.ToAsciiDiagram(EntityCache.GetPage(root));
        var allRelationsInDb = pageRelationRepo.GetAll();
        var childRelations = EntityCache.GetPage(root)?.ChildRelations;

        // Verify proper linking (each relation should have correct previous and next references)
        var areRelationsValid = true;
        var relationErrors = new List<string>();

        if (childRelations != null)
        {
            for (int i = 0; i < childRelations.Count; i++)
            {
                var relation = childRelations[i];

                // First relation should have null previousId
                if (i == 0 && relation.PreviousId != null)
                {
                    areRelationsValid = false;
                    relationErrors.Add($"First relation (childId: {relation.ChildId}) has non-null previousId: {relation.PreviousId}");
                }

                // Last relation should have null nextId
                if (i == childRelations.Count - 1 && relation.NextId != null)
                {
                    areRelationsValid = false;
                    relationErrors.Add($"Last relation (childId: {relation.ChildId}) has non-null nextId: {relation.NextId}");
                }

                // Middle relations should link correctly
                if (i > 0 && relation.PreviousId != childRelations[i - 1].ChildId)
                {
                    areRelationsValid = false;
                    relationErrors.Add($"Relation at index {i} (childId: {relation.ChildId}) has incorrect previousId: {relation.PreviousId}, expected: {childRelations[i - 1].ChildId}");
                }

                if (i < childRelations.Count - 1 && relation.NextId != childRelations[i + 1].ChildId)
                {
                    areRelationsValid = false;
                    relationErrors.Add($"Relation at index {i} (childId: {relation.ChildId}) has incorrect nextId: {relation.NextId}, expected: {childRelations[i + 1].ChildId}");
                }
            }
        }

        //Assert
        await Verify(new
        {
            treeSnapshots = snapshots,
            finalState = new
            {
                allRelationsInDb,
                childRelations,
                areRelationsValid,
                relationErrors,
                moveResults = new
                {
                    move1 = move1Result.Success,
                    move2 = move2Result.Success,
                    move3 = move3Result.Success,
                    move4 = move4Result.Success
                }
            }
        });
    }
}