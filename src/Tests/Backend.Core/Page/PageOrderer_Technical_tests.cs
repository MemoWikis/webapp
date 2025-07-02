internal class PageOrderer_Technical_tests : BaseTestHarness
{
    private UserLoginApiWrapper _userLoginApi => _testHarness.ApiUserLogin;

    [Test]
    public async Task Should_maintain_integrity_when_moving_pages_multiple_times()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();

        var creator = _testHarness.GetDefaultSessionUserFromDb();

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

        await _userLoginApi.LoginAsSessionUser();

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
        var creator = _testHarness.GetDefaultSessionUserFromDb();

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
        await _userLoginApi.LoginAsSessionUser();

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

        var allRelationsDb = R<PageRelationRepo>().GetAll();

        await Verify(new
        {
            initialTree,
            intermediateSnapshots,
            finalTree,
            moveResults = moveResults.Select(r => new { success = r.Success, error = r.Error }).ToList(),
            childRelationOrder = childRelations?.Select(r => r.ChildId).ToList(),
            areRelationsValid,
            relationErrors,
            allRelationsDb
        });
    }

    [Test]
    public async Task Should_move_pages_between_different_parent_levels()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        // Create root pages
        context.Add("rootA", creator: creator, isWiki: true).Persist();
        context.Add("rootB", creator: creator, isWiki: true).Persist();

        // Create child pages
        context
            .Add("childPageA1", creator: creator)
            .Add("childPageA2", creator: creator)
            .Add("childPageB1", creator: creator)
            .Persist();

        var rootA = context.All.ByName("rootA");
        var rootB = context.All.ByName("rootB");
        var childPageA1 = context.All.ByName("childPageA1");
        var childPageA2 = context.All.ByName("childPageA2");
        var childPageB1 = context.All.ByName("childPageB1");

        // Set initial hierarchy: rootA > childPageA1, childPageA2 and rootB > childPageB1
        context.AddChild(rootA, childPageA1);
        context.AddChild(rootA, childPageA2);
        context.AddChild(rootB, childPageB1);

        await ReloadCaches();
        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var initialRootA = EntityCache.GetPage(rootA);
        var initialRootB = EntityCache.GetPage(rootB);
        if (initialRootA == null || initialRootB == null)
            throw new InvalidOperationException("Initial root pages are null");

        var snapshots = new Dictionary<string, string>();
        snapshots.Add("initial_rootA", TreeRenderer.ToAsciiDiagram(initialRootA));
        snapshots.Add("initial_rootB", TreeRenderer.ToAsciiDiagram(initialRootB));

        await _userLoginApi.LoginAsSessionUser();

        // Act 1: Move childPageA2 from rootA to rootB (after childPageB1)
        var moveChildToNewParentResult = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = childPageA2.Id,
                TargetId = childPageB1.Id,
                Position = 1, // After
                NewParentId = rootB.Id,
                OldParentId = rootA.Id
            });

        await ReloadCaches();
        var rootAAfterMove1 = EntityCache.GetPage(rootA);
        var rootBAfterMove1 = EntityCache.GetPage(rootB);
        if (rootAAfterMove1 == null || rootBAfterMove1 == null)
            throw new InvalidOperationException("Root pages are null after move 1");

        snapshots.Add("after_move1_rootA", TreeRenderer.ToAsciiDiagram(rootAAfterMove1));
        snapshots.Add("after_move1_rootB", TreeRenderer.ToAsciiDiagram(rootBAfterMove1));

        // Act 2: Move childPageB1 from rootB to rootA (before childPageA1)
        var moveChildBackResult = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = childPageB1.Id,
                TargetId = childPageA1.Id,
                Position = 0, // Before
                NewParentId = rootA.Id,
                OldParentId = rootB.Id
            });

        await ReloadCaches();
        var rootAAfterMove2 = EntityCache.GetPage(rootA);
        var rootBAfterMove2 = EntityCache.GetPage(rootB);
        if (rootAAfterMove2 == null || rootBAfterMove2 == null)
            throw new InvalidOperationException("Root pages are null after move 2");

        snapshots.Add("final_rootA", TreeRenderer.ToAsciiDiagram(rootAAfterMove2));
        snapshots.Add("final_rootB", TreeRenderer.ToAsciiDiagram(rootBAfterMove2)); // Assert
        var finalChildRelationsA = EntityCache.GetPage(rootA)?.ChildRelations;
        var finalChildRelationsB = EntityCache.GetPage(rootB)?.ChildRelations;
        var pageRelationRepo = R<PageRelationRepo>();
        var allRelationsInDb = pageRelationRepo.GetAll();

        // Get entity cache snapshots
        var entityCacheSnapshots = new Dictionary<string, object>();
        var finalRootACache = EntityCache.GetPage(rootA);
        var finalRootBCache = EntityCache.GetPage(rootB);

        entityCacheSnapshots.Add("final_rootA_cache",
            new
            {
                Id = finalRootACache?.Id,
                Name = finalRootACache?.Name,
                ChildRelations = finalRootACache?.ChildRelations?.Select(r => new
                {
                    r.ChildId,
                    r.ParentId,
                    r.PreviousId,
                    r.NextId,
                    r.Id
                }).ToList()
            });

        entityCacheSnapshots.Add("final_rootB_cache",
            new
            {
                Id = finalRootBCache?.Id,
                Name = finalRootBCache?.Name,
                ChildRelations = finalRootBCache?.ChildRelations?.Select(r => new
                {
                    r.ChildId,
                    r.ParentId,
                    r.PreviousId,
                    r.NextId,
                    r.Id
                }).ToList()
            });

        // Get database snapshots
        var dbSnapshots = new Dictionary<string, object>();
        var rootARelationsInDb = allRelationsInDb.Where(r => r.Parent.Id == rootA.Id).ToList();
        var rootBRelationsInDb = allRelationsInDb.Where(r => r.Parent.Id == rootB.Id).ToList();

        dbSnapshots.Add("final_rootA_db_relations",
            rootARelationsInDb.Select(r => new
            {
                ChildId = r.Child.Id,
                ParentId = r.Parent.Id,
                r.PreviousId,
                r.NextId,
                r.Id
            }).ToList());

        dbSnapshots.Add("final_rootB_db_relations",
            rootBRelationsInDb.Select(r => new
            {
                ChildId = r.Child.Id,
                ParentId = r.Parent.Id,
                r.PreviousId,
                r.NextId,
                r.Id
            }).ToList());

        await Verify(new
        {
            treeSnapshots = snapshots,
            entityCacheSnapshots,
            dbSnapshots,
            moveResults =
                new
                {
                    moveChildToNewParent =
                        new
                        {
                            success = moveChildToNewParentResult.Success,
                            error = moveChildToNewParentResult.Error
                        },
                    moveChildBack =
                        new { success = moveChildBackResult.Success, error = moveChildBackResult.Error }
                },
            finalState = new
            {
                rootAChildrenIds = finalChildRelationsA?.Select(r => r.ChildId).ToList(),
                rootBChildrenIds = finalChildRelationsB?.Select(r => r.ChildId).ToList(),
                allRelationsInDb = allRelationsInDb.Select(r => new
                {
                    ChildId = r.Child.Id,
                    ParentId = r.Parent.Id,
                    r.PreviousId,
                    r.NextId,
                    r.Id
                }).ToList()
            }
        });
    }

    [Test]
    public async Task Should_handle_complex_nested_page_movements()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        // Create root and nested structure
        context.Add("rootParent", creator: creator, isWiki: true).Persist();
        context.Add("rootSibling", creator: creator, isWiki: true).Persist();

        context
            .Add("childPageToMove", creator: creator)
            .Add("childPageStaysA", creator: creator)
            .Add("childPageStaysB", creator: creator)
            .Add("nestedChildPage", creator: creator)
            .Persist();

        var rootParent = context.All.ByName("rootParent");
        var rootSibling = context.All.ByName("rootSibling");
        var childPageToMove = context.All.ByName("childPageToMove");
        var childPageStaysA = context.All.ByName("childPageStaysA");
        var childPageStaysB = context.All.ByName("childPageStaysB");
        var nestedChildPage = context.All.ByName("nestedChildPage");

        // Create initial nested structure:
        // rootParent > childPageStaysA, childPageToMove > nestedChildPage, childPageStaysB
        // rootSibling (empty)
        context.AddChild(rootParent, childPageStaysA);
        context.AddChild(rootParent, childPageToMove);
        context.AddChild(childPageToMove, nestedChildPage);
        context.AddChild(rootParent, childPageStaysB);

        await ReloadCaches();
        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var initialRootParent = EntityCache.GetPage(rootParent);
        var initialRootSibling = EntityCache.GetPage(rootSibling);
        if (initialRootParent == null || initialRootSibling == null)
            throw new InvalidOperationException("Initial root pages are null");

        var snapshots = new Dictionary<string, string>();
        snapshots.Add("initial_rootParent", TreeRenderer.ToAsciiDiagram(initialRootParent));
        snapshots.Add("initial_rootSibling", TreeRenderer.ToAsciiDiagram(initialRootSibling));

        await _userLoginApi.LoginAsSessionUser();

        // Act 1: Move childPageToMove (with its nested child) to rootSibling as first child
        var moveNestedStructureResult = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = childPageToMove.Id,
                TargetId = rootSibling.Id,
                Position = 2, // Inner (as child)
                NewParentId = rootSibling.Id,
                OldParentId = rootParent.Id
            });

        await ReloadCaches();
        var rootParentAfterMove = EntityCache.GetPage(rootParent);
        var rootSiblingAfterMove = EntityCache.GetPage(rootSibling);
        if (rootParentAfterMove == null || rootSiblingAfterMove == null)
            throw new InvalidOperationException("Root pages are null after move");

        snapshots.Add("after_move_rootParent", TreeRenderer.ToAsciiDiagram(rootParentAfterMove));
        snapshots.Add("after_move_rootSibling", TreeRenderer.ToAsciiDiagram(rootSiblingAfterMove));

        // Verify nested structure is preserved
        var movedChildPage = EntityCache.GetPage(childPageToMove);
        var nestedChildStillExists = movedChildPage?.ChildRelations?.Any(r => r.ChildId == nestedChildPage.Id) == true;

        // Assert
        var finalChildRelationsParent = EntityCache.GetPage(rootParent)?.ChildRelations;
        var finalChildRelationsSibling = EntityCache.GetPage(rootSibling)?.ChildRelations;

        await Verify(new
        {
            snapshots,
            moveResult =
                new { success = moveNestedStructureResult.Success, error = moveNestedStructureResult.Error },
            finalState = new
            {
                rootParentChildrenIds = finalChildRelationsParent?.Select(r => r.ChildId).ToList(),
                rootSiblingChildrenIds = finalChildRelationsSibling?.Select(r => r.ChildId).ToList(),
                nestedChildStillExists,
                movedChildPageChildrenIds = movedChildPage?.ChildRelations?.Select(r => r.ChildId).ToList()
            }
        });
    }

    [Test]
    public async Task Should_reject_circular_reference_moves()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        context.Add("rootPage", creator: creator, isWiki: true).Persist();
        context
            .Add("parentPage", creator: creator)
            .Add("childPage", creator: creator)
            .Add("grandChildPage", creator: creator)
            .Persist();

        var rootPage = context.All.ByName("rootPage");
        var parentPage = context.All.ByName("parentPage");
        var childPage = context.All.ByName("childPage");
        var grandChildPage = context.All.ByName("grandChildPage");

        // Create hierarchy: rootPage > parentPage > childPage > grandChildPage
        context.AddChild(rootPage, parentPage);
        context.AddChild(parentPage, childPage);
        context.AddChild(childPage, grandChildPage);

        await ReloadCaches();
        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var initialRoot = EntityCache.GetPage(rootPage);
        if (initialRoot == null)
            throw new InvalidOperationException("Initial root page is null");

        var initialTree = TreeRenderer.ToAsciiDiagram(initialRoot);

        await _userLoginApi.LoginAsSessionUser();

        // Act & Assert - Try to create circular reference: move parentPage under grandChildPage
        var circularMoveResult = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = parentPage.Id,
                TargetId = grandChildPage.Id,
                Position = 2, // Inner
                NewParentId = grandChildPage.Id,
                OldParentId = rootPage.Id
            });

        await ReloadCaches();
        var finalRoot = EntityCache.GetPage(rootPage);
        if (finalRoot == null)
            throw new InvalidOperationException("Final root page is null");

        var finalTree = TreeRenderer.ToAsciiDiagram(finalRoot);

        await Verify(new
        {
            initialTree,
            finalTree,
            circularMoveResult = new { success = circularMoveResult.Success, error = circularMoveResult.Error },
            structureUnchanged = initialTree == finalTree
        });
    }

    [Test]
    public async Task Should_handle_extensive_multi_level_page_movements_with_position_changes()
    {
        await ClearData();

        // Arrange
        var context = NewPageContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        // Create multiple root pages
        context.Add("rootWikiA", creator: creator, isWiki: true).Persist();
        context.Add("rootWikiB", creator: creator, isWiki: true).Persist();
        context.Add("rootWikiC", creator: creator, isWiki: true).Persist();

        // Create many child pages
        context
            .Add("pageA1", creator: creator)
            .Add("pageA2", creator: creator)
            .Add("pageA3", creator: creator)
            .Add("pageA4", creator: creator)
            .Add("pageB1", creator: creator)
            .Add("pageB2", creator: creator)
            .Add("pageC1", creator: creator)
            .Add("nestedPageA1", creator: creator)
            .Add("nestedPageA2", creator: creator)
            .Add("deepNestedPage", creator: creator)
            .Persist();

        var rootWikiA = context.All.ByName("rootWikiA");
        var rootWikiB = context.All.ByName("rootWikiB");
        var rootWikiC = context.All.ByName("rootWikiC");
        var pageA1 = context.All.ByName("pageA1");
        var pageA2 = context.All.ByName("pageA2");
        var pageA3 = context.All.ByName("pageA3");
        var pageA4 = context.All.ByName("pageA4");
        var pageB1 = context.All.ByName("pageB1");
        var pageB2 = context.All.ByName("pageB2");
        var pageC1 = context.All.ByName("pageC1");
        var nestedPageA1 = context.All.ByName("nestedPageA1");
        var nestedPageA2 = context.All.ByName("nestedPageA2");
        var deepNestedPage = context.All.ByName("deepNestedPage");

        // Set initial complex hierarchy:
        // rootWikiA > pageA1, pageA2 > nestedPageA1 > deepNestedPage, pageA3, pageA4 > nestedPageA2
        // rootWikiB > pageB1, pageB2
        // rootWikiC > pageC1
        context.AddChild(rootWikiA, pageA1);
        context.AddChild(rootWikiA, pageA2);
        context.AddChild(pageA2, nestedPageA1);
        context.AddChild(nestedPageA1, deepNestedPage);
        context.AddChild(rootWikiA, pageA3);
        context.AddChild(rootWikiA, pageA4);
        context.AddChild(pageA4, nestedPageA2);

        context.AddChild(rootWikiB, pageB1);
        context.AddChild(rootWikiB, pageB2);

        context.AddChild(rootWikiC, pageC1);

        await ReloadCaches();
        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var initialRootA = EntityCache.GetPage(rootWikiA);
        var initialRootB = EntityCache.GetPage(rootWikiB);
        var initialRootC = EntityCache.GetPage(rootWikiC);
        if (initialRootA == null || initialRootB == null || initialRootC == null)
            throw new InvalidOperationException("Initial root pages are null");

        var snapshots = new Dictionary<string, string>();
        snapshots.Add("00_initial_rootA", TreeRenderer.ToAsciiDiagram(initialRootA));
        snapshots.Add("00_initial_rootB", TreeRenderer.ToAsciiDiagram(initialRootB));
        snapshots.Add("00_initial_rootC", TreeRenderer.ToAsciiDiagram(initialRootC));

        await _userLoginApi.LoginAsSessionUser();

        var moveResults = new List<EditPageRelationStoreController.MovePageResult>();

        // Act 1: Move pageA3 from rootWikiA to rootWikiB (after pageB2)
        var move1Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = pageA3.Id,
                TargetId = pageB2.Id,
                Position = 1, // After
                NewParentId = rootWikiB.Id,
                OldParentId = rootWikiA.Id
            });
        moveResults.Add(move1Result);
        await ReloadCaches();
        var rootBAfterMove1 = EntityCache.GetPage(rootWikiB);
        if (rootBAfterMove1 != null)
            snapshots.Add("01_after_move_pageA3_to_rootB", TreeRenderer.ToAsciiDiagram(rootBAfterMove1));

        // Act 2: Move pageB1 from rootWikiB to rootWikiC (before pageC1)
        var move2Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = pageB1.Id,
                TargetId = pageC1.Id,
                Position = 0, // Before
                NewParentId = rootWikiC.Id,
                OldParentId = rootWikiB.Id
            });
        moveResults.Add(move2Result);
        await ReloadCaches();
        var rootCAfterMove2 = EntityCache.GetPage(rootWikiC);
        if (rootCAfterMove2 != null)
            snapshots.Add("02_after_move_pageB1_to_rootC", TreeRenderer.ToAsciiDiagram(rootCAfterMove2));

        // Act 3: Move pageA2 (with its nested structure) from rootWikiA to rootWikiC (after pageC1)
        var move3Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = pageA2.Id,
                TargetId = pageC1.Id,
                Position = 1, // After
                NewParentId = rootWikiC.Id,
                OldParentId = rootWikiA.Id
            });
        moveResults.Add(move3Result);
        await ReloadCaches();
        var rootCAfterMove3 = EntityCache.GetPage(rootWikiC);
        if (rootCAfterMove3 != null)
            snapshots.Add("03_after_move_pageA2_nested_to_rootC", TreeRenderer.ToAsciiDiagram(rootCAfterMove3));

        // Act 4: Move pageA4 (with nestedPageA2) from rootWikiA to rootWikiB (as first child)
        var move4Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = pageA4.Id,
                TargetId = pageB2.Id,
                Position = 0, // Before
                NewParentId = rootWikiB.Id,
                OldParentId = rootWikiA.Id
            });
        moveResults.Add(move4Result);
        await ReloadCaches();
        var rootBAfterMove4 = EntityCache.GetPage(rootWikiB);
        if (rootBAfterMove4 != null)
            snapshots.Add("04_after_move_pageA4_to_rootB", TreeRenderer.ToAsciiDiagram(rootBAfterMove4));

        // Act 5: Reorder within rootWikiC - move pageA2 before pageB1
        var move5Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = pageA2.Id,
                TargetId = pageB1.Id,
                Position = 0, // Before
                NewParentId = rootWikiC.Id,
                OldParentId = rootWikiC.Id
            });
        moveResults.Add(move5Result);
        await ReloadCaches();
        var rootCAfterMove5 = EntityCache.GetPage(rootWikiC);
        if (rootCAfterMove5 != null)
            snapshots.Add("05_after_reorder_pageA2_before_pageB1", TreeRenderer.ToAsciiDiagram(rootCAfterMove5));

        // Act 6: Move pageC1 from rootWikiC to rootWikiA (as only child now)
        var move6Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = pageC1.Id,
                TargetId = rootWikiA.Id,
                Position = 2, // Inner (as child)
                NewParentId = rootWikiA.Id,
                OldParentId = rootWikiC.Id
            });
        moveResults.Add(move6Result);
        await ReloadCaches();
        var rootAAfterMove6 = EntityCache.GetPage(rootWikiA);
        if (rootAAfterMove6 != null)
            snapshots.Add("06_after_move_pageC1_to_rootA", TreeRenderer.ToAsciiDiagram(rootAAfterMove6));

        // Act 7: Complex reordering in rootWikiB - move pageA3 before pageA4
        var move7Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = pageA3.Id,
                TargetId = pageA4.Id,
                Position = 0, // Before
                NewParentId = rootWikiB.Id,
                OldParentId = rootWikiB.Id
            });
        moveResults.Add(move7Result);
        await ReloadCaches();
        var rootBAfterMove7 = EntityCache.GetPage(rootWikiB);
        if (rootBAfterMove7 != null)
            snapshots.Add("07_after_reorder_pageA3_before_pageA4", TreeRenderer.ToAsciiDiagram(rootBAfterMove7));

        // Act 8: Move nestedPageA1 (with deepNestedPage) from pageA2 to rootWikiA (after pageC1)
        var move8Result = await _testHarness.ApiPost<EditPageRelationStoreController.MovePageResult>(
            "apiVue/EditPageRelationStore/MovePage", new
            {
                MovingPageId = nestedPageA1.Id,
                TargetId = pageC1.Id,
                Position = 1, // After
                NewParentId = rootWikiA.Id,
                OldParentId = pageA2.Id
            });
        moveResults.Add(move8Result);
        await ReloadCaches();
        var rootAAfterMove8 = EntityCache.GetPage(rootWikiA);
        if (rootAAfterMove8 != null)
            snapshots.Add("08_after_move_nestedPageA1_to_rootA", TreeRenderer.ToAsciiDiagram(rootAAfterMove8));

        // Final snapshots of all root pages
        var finalRootA = EntityCache.GetPage(rootWikiA);
        var finalRootB = EntityCache.GetPage(rootWikiB);
        var finalRootC = EntityCache.GetPage(rootWikiC);
        if (finalRootA == null || finalRootB == null || finalRootC == null)
            throw new InvalidOperationException("Final root pages are null");

        snapshots.Add("09_final_rootA", TreeRenderer.ToAsciiDiagram(finalRootA));
        snapshots.Add("09_final_rootB", TreeRenderer.ToAsciiDiagram(finalRootB));
        snapshots.Add("09_final_rootC", TreeRenderer.ToAsciiDiagram(finalRootC));

        // Comprehensive verification
        var pageRelationRepo = R<PageRelationRepo>();
        var allRelationsInDb = pageRelationRepo.GetAll();

        // Entity cache snapshots for all roots
        var entityCacheSnapshots = new Dictionary<string, object>();
        entityCacheSnapshots.Add("final_rootA_cache",
            new
            {
                Id = finalRootA.Id,
                Name = finalRootA.Name,
                ChildRelations = finalRootA.ChildRelations?.Select(r => new
                {
                    r.ChildId,
                    r.ParentId,
                    r.PreviousId,
                    r.NextId,
                    r.Id
                }).ToList()
            });

        entityCacheSnapshots.Add("final_rootB_cache",
            new
            {
                Id = finalRootB.Id,
                Name = finalRootB.Name,
                ChildRelations = finalRootB.ChildRelations?.Select(r => new
                {
                    r.ChildId,
                    r.ParentId,
                    r.PreviousId,
                    r.NextId,
                    r.Id
                }).ToList()
            });

        entityCacheSnapshots.Add("final_rootC_cache",
            new
            {
                Id = finalRootC.Id,
                Name = finalRootC.Name,
                ChildRelations = finalRootC.ChildRelations?.Select(r => new
                {
                    r.ChildId,
                    r.ParentId,
                    r.PreviousId,
                    r.NextId,
                    r.Id
                }).ToList()
            });

        // Database snapshots for all roots
        var dbSnapshots = new Dictionary<string, object>();
        var rootARelationsInDb = allRelationsInDb.Where(r => r.Parent.Id == rootWikiA.Id).ToList();
        var rootBRelationsInDb = allRelationsInDb.Where(r => r.Parent.Id == rootWikiB.Id).ToList();
        var rootCRelationsInDb = allRelationsInDb.Where(r => r.Parent.Id == rootWikiC.Id).ToList();

        dbSnapshots.Add("final_rootA_db_relations",
            rootARelationsInDb.Select(r => new
            {
                ChildId = r.Child.Id,
                ParentId = r.Parent.Id,
                r.PreviousId,
                r.NextId,
                r.Id
            }).ToList());

        dbSnapshots.Add("final_rootB_db_relations",
            rootBRelationsInDb.Select(r => new
            {
                ChildId = r.Child.Id,
                ParentId = r.Parent.Id,
                r.PreviousId,
                r.NextId,
                r.Id
            }).ToList());

        dbSnapshots.Add("final_rootC_db_relations",
            rootCRelationsInDb.Select(r => new
            {
                ChildId = r.Child.Id,
                ParentId = r.Parent.Id,
                r.PreviousId,
                r.NextId,
                r.Id
            }).ToList());

        // Verify nested structures are preserved
        var pageA2AfterMoves = EntityCache.GetPage(pageA2);
        var nestedPageA1AfterMoves = EntityCache.GetPage(nestedPageA1);
        var pageA4AfterMoves = EntityCache.GetPage(pageA4);

        var structureIntegrity = new
        {
            deepNestedPageStillUnderNestedA1 =
                nestedPageA1AfterMoves?.ChildRelations?.Any(r => r.ChildId == deepNestedPage.Id) == true,
            nestedPageA2StillUnderPageA4 =
                pageA4AfterMoves?.ChildRelations?.Any(r => r.ChildId == nestedPageA2.Id) == true,
            pageA2HasNoNestedPageA1 =
                pageA2AfterMoves?.ChildRelations?.Any(r => r.ChildId == nestedPageA1.Id) == false
        };

        await Verify(new
        {
            movementSequence = snapshots,
            entityCacheSnapshots,
            dbSnapshots,
            moveResults =
                moveResults.Select((r, i) => new { moveNumber = i + 1, success = r.Success, error = r.Error })
                    .ToList(),
            structureIntegrity,
            finalState = new
            {
                rootAChildrenIds = finalRootA.ChildRelations?.Select(r => r.ChildId).ToList(),
                rootBChildrenIds = finalRootB.ChildRelations?.Select(r => r.ChildId).ToList(),
                rootCChildrenIds = finalRootC.ChildRelations?.Select(r => r.ChildId).ToList(),
                totalRelationsInDb = allRelationsInDb.Count,
                allRelationsInDb = allRelationsInDb.Select(r => new
                {
                    ChildId = r.Child.Id,
                    ParentId = r.Parent.Id,
                    r.PreviousId,
                    r.NextId,
                    r.Id
                }).ToList()
            }
        });
    }
}