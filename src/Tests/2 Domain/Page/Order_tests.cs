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

    [Test]
    public async Task Should_maintain_integrity_when_moving_pages_multiple_times()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();

        var sessionUser = R<SessionUser>();
        var authorId = sessionUser.UserId;
        var creator = new User { Id = authorId };

        context.Add("root", creator: creator, isWiki: true).Persist();

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

        var cachedRoot = EntityCache.GetPage(root);
        if (cachedRoot == null)
            throw new InvalidOperationException("Cached root page is null");

        var initialTree = TreeRenderer.ToAsciiDiagram(cachedRoot);
        var snapshots = new Dictionary<string, string>();
        snapshots.Add("initial", initialTree); //Act - Perform multiple moves in sequence

        // Login once for all API calls in this test
        await _testHarness.LoginAsSessionUser();

        // Move 1: sub1 after sub3
        var move1Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = sub1.Id,
                TargetId = sub3.Id,
                Position = 1, // After
                NewParentId = root.Id,
                OldParentId = root.Id
            });

        await ReloadCaches();
        var rootAfterMove1 = EntityCache.GetPage(root);
        if (rootAfterMove1 == null)
            throw new InvalidOperationException("Root page is null after move 1");
        snapshots.Add("after_move1", TreeRenderer.ToAsciiDiagram(rootAfterMove1)); // Move 2: sub4 before sub2
        var move2Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = sub4.Id,
                TargetId = sub2.Id,
                Position = 0, // Before
                NewParentId = root.Id,
                OldParentId = root.Id
            });

        await ReloadCaches();
        var rootAfterMove2 = EntityCache.GetPage(root);
        if (rootAfterMove2 == null)
            throw new InvalidOperationException("Root page is null after move 2");
        snapshots.Add("after_move2", TreeRenderer.ToAsciiDiagram(rootAfterMove2)); // Move 3: sub2 after sub3
        var move3Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = sub2.Id,
                TargetId = sub3.Id,
                Position = 1, // After
                NewParentId = root.Id,
                OldParentId = root.Id
            });

        await ReloadCaches();
        var rootAfterMove3 = EntityCache.GetPage(root);
        if (rootAfterMove3 == null)
            throw new InvalidOperationException("Root page is null after move 3");
        snapshots.Add("after_move3",
            TreeRenderer.ToAsciiDiagram(rootAfterMove3)); // Move 4: sub3 to beginning (before sub4)
        var move4Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = sub3.Id,
                TargetId = sub4.Id,
                Position = 0, // Before
                NewParentId = root.Id,
                OldParentId = root.Id
            });

        await ReloadCaches();
        var rootAfterMove4 = EntityCache.GetPage(root);
        if (rootAfterMove4 == null)
            throw new InvalidOperationException("Root page is null after move 4");
        snapshots.Add("after_move4", TreeRenderer.ToAsciiDiagram(rootAfterMove4));

        // Final state verification
        var finalRootPage = EntityCache.GetPage(root);
        if (finalRootPage == null)
            throw new InvalidOperationException("Final root page is null");
        var finalTree = TreeRenderer.ToAsciiDiagram(finalRootPage);
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
                    relationErrors.Add(
                        $"First relation (childId: {relation.ChildId}) has non-null previousId: {relation.PreviousId}");
                }

                // Last relation should have null nextId
                if (i == childRelations.Count - 1 && relation.NextId != null)
                {
                    areRelationsValid = false;
                    relationErrors.Add(
                        $"Last relation (childId: {relation.ChildId}) has non-null nextId: {relation.NextId}");
                }

                // Middle relations should link correctly
                if (i > 0 && relation.PreviousId != childRelations[i - 1].ChildId)
                {
                    areRelationsValid = false;
                    relationErrors.Add(
                        $"Relation at index {i} (childId: {relation.ChildId}) has incorrect previousId: {relation.PreviousId}, expected: {childRelations[i - 1].ChildId}");
                }

                if (i < childRelations.Count - 1 && relation.NextId != childRelations[i + 1].ChildId)
                {
                    areRelationsValid = false;
                    relationErrors.Add(
                        $"Relation at index {i} (childId: {relation.ChildId}) has incorrect nextId: {relation.NextId}, expected: {childRelations[i + 1].ChildId}");
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
                    move1 = new { success = move1Result.Success, error = move1Result.Error },
                    move2 = new { success = move2Result.Success, error = move2Result.Error },
                    move3 = new { success = move3Result.Success, error = move3Result.Error },
                    move4 = new { success = move4Result.Success, error = move4Result.Error }
                }
            }
        });
    }

    [Test]
    public async Task Should_handle_rapid_sequential_page_moves_correctly()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var sessionUser = R<SessionUser>();
        var authorId = sessionUser.UserId;
        var creator = new User { Id = authorId };

        context.Add("root", creator: creator, isWiki: true).Persist();

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

        var initialRootPage = EntityCache.GetPage(root);
        if (initialRootPage == null)
            throw new InvalidOperationException("Initial root page is null");
        var initialTree = TreeRenderer.ToAsciiDiagram(initialRootPage);
        var intermediateSnapshots = new Dictionary<string, string>();

        // Act - Perform multiple moves in very rapid succession

        // Login once for all API calls in this test
        await _testHarness.LoginAsSessionUser();

        // Execute moves in rapid succession using API calls
        var moveResults = new List<EditPageRelationStoreController.MovePageResult>();

        // Request 1: sub1 before sub4
        var move1Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = sub1.Id,
                TargetId = sub4.Id,
                Position = 0, // Before
                NewParentId = root.Id,
                OldParentId = root.Id
            });
        moveResults.Add(move1Result);

        await ReloadCaches();
        var rootPageAfterMove1 = EntityCache.GetPage(root);
        if (rootPageAfterMove1 == null)
            throw new InvalidOperationException("Root page is null after move 1");
        intermediateSnapshots["after_move_1"] = TreeRenderer.ToAsciiDiagram(rootPageAfterMove1);

        // Request 2: sub3 after sub2
        var move2Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = sub3.Id,
                TargetId = sub2.Id,
                Position = 1, // After
                NewParentId = root.Id,
                OldParentId = root.Id
            });
        moveResults.Add(move2Result);

        await ReloadCaches();
        var rootPageAfterMove2 = EntityCache.GetPage(root);
        if (rootPageAfterMove2 == null)
            throw new InvalidOperationException("Root page is null after move 2");
        intermediateSnapshots["after_move_2"] = TreeRenderer.ToAsciiDiagram(rootPageAfterMove2);

        // Request 3: sub2 before sub1
        var move3Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = sub2.Id,
                TargetId = sub1.Id,
                Position = 0, // Before
                NewParentId = root.Id,
                OldParentId = root.Id
            });
        moveResults.Add(move3Result);

        await ReloadCaches();
        var rootPageAfterMove3 = EntityCache.GetPage(root);
        if (rootPageAfterMove3 == null)
            throw new InvalidOperationException("Root page is null after move 3");
        intermediateSnapshots["after_move_3"] =
            TreeRenderer.ToAsciiDiagram(rootPageAfterMove3); // Final reload to ensure we have the latest state
        await ReloadCaches();

        // Assert
        var finalRootPage = EntityCache.GetPage(root);
        if (finalRootPage == null)
            throw new InvalidOperationException("Final root page is null");
        var finalTree = TreeRenderer.ToAsciiDiagram(finalRootPage);
        var childRelations = EntityCache.GetPage(root)?.ChildRelations;

        // Validate relation integrity
        var areRelationsValid = true;
        var relationErrors = new List<string>();

        if (childRelations != null)
        {
            // First relation should have null previousId
            if (childRelations.Count > 0 && childRelations[0].PreviousId != null)
            {
                areRelationsValid = false;
                relationErrors.Add(
                    $"First relation (childId: {childRelations[0].ChildId}) has non-null previousId: {childRelations[0].PreviousId}");
            }

            // Last relation should have null nextId
            if (childRelations.Count > 0 && childRelations[childRelations.Count - 1].NextId != null)
            {
                areRelationsValid = false;
                relationErrors.Add(
                    $"Last relation (childId: {childRelations[childRelations.Count - 1].ChildId}) has non-null nextId: {childRelations[childRelations.Count - 1].NextId}");
            }

            // Middle relations should link correctly
            for (int i = 0; i < childRelations.Count - 1; i++)
            {
                if (childRelations[i].NextId != childRelations[i + 1].ChildId)
                {
                    areRelationsValid = false;
                    relationErrors.Add(
                        $"Relation at index {i} (childId: {childRelations[i].ChildId}) has incorrect nextId: {childRelations[i].NextId}, expected: {childRelations[i + 1].ChildId}");
                }

                if (i > 0 && childRelations[i].PreviousId != childRelations[i - 1].ChildId)
                {
                    areRelationsValid = false;
                    relationErrors.Add(
                        $"Relation at index {i} (childId: {childRelations[i].ChildId}) has incorrect previousId: {childRelations[i].PreviousId}, expected: {childRelations[i - 1].ChildId}");
                }
            }
        }

        await Verify(new
        {
            initialTree,
            intermediateSnapshots,
            finalTree,
            moveResults = moveResults.Select(r => new { success = r.Success, error = r.Error }).ToList(),
            childRelationOrder = childRelations?.Select(r => r.ChildId).ToList(),
            areRelationsValid,
            relationErrors
        });
    }
}