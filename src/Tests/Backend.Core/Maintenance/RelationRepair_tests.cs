internal class RelationRepair_tests : BaseTestHarness
{
    [Test]
    public async Task Should_heal_relations()
    {
        await ClearData();

        // Arrange - Create scenario with relation errors
        var context = NewPageContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        // Create pages - first 3 relations will be fine, next 4 will have issues, last one will be clean
        context.Add("rootWiki", creator: creator, isWiki: true).Persist();
        context
            .Add("parent1", creator: creator)  // Will have 3 clean relations
            .Add("parent2", creator: creator)  // Will have 4 problematic relations
            .Add("parent3", creator: creator)  // Will have 3 clean relations (no issues)
            .Add("child1", creator: creator)
            .Add("child2", creator: creator)
            .Add("child3", creator: creator)
            .Add("child4", creator: creator)
            .Add("child5", creator: creator)
            .Add("child6", creator: creator)
            .Add("child7", creator: creator)
            .Add("child8", creator: creator)
            .Add("child9", creator: creator)
            .Add("child10", creator: creator)
            .Persist();

        var rootWiki = context.All.ByName("rootWiki");
        var parent1 = context.All.ByName("parent1");
        var parent2 = context.All.ByName("parent2");
        var parent3 = context.All.ByName("parent3");

        // Set up clean relations for parent1 (first 3 relations - these should be fine)
        context.AddChild(parent1, context.All.ByName("child1"));
        context.AddChild(parent1, context.All.ByName("child2"));
        context.AddChild(parent1, context.All.ByName("child3"));

        // Set up clean relations for parent3 (last 3 relations - these should be fine)
        context.AddChild(parent3, context.All.ByName("child8"));
        context.AddChild(parent3, context.All.ByName("child9"));
        context.AddChild(parent3, context.All.ByName("child10"));

        await ReloadCaches();

        // Create problematic relations for parent2 (next 4 relations with issues)
        var pageRelationRepo = R<PageRelationRepo>();

        // Create duplicate relations for same child
        var duplicateRelation1 = new PageRelation
        {
            Parent = parent2,
            Child = context.All.ByName("child4"),
            PreviousId = null,
            NextId = null
        };
        var duplicateRelation2 = new PageRelation
        {
            Parent = parent2,
            Child = context.All.ByName("child4"), // Same child - duplicate
            PreviousId = null,
            NextId = null
        };
        pageRelationRepo.Create(duplicateRelation1);
        pageRelationRepo.Create(duplicateRelation2);

        // Create normal relation for child5
        var normalRelation = new PageRelation
        {
            Parent = parent2,
            Child = context.All.ByName("child5"),
            PreviousId = null,
            NextId = null
        };
        pageRelationRepo.Create(normalRelation);

        // Create a temporary child page that we'll delete to simulate a broken link
        var tempChild = new Page
        {
            Name = "TempChild",
            Creator = creator,
            IsWiki = false,
            Visibility = PageVisibility.Public
        };
        var pageRepo = R<PageRepository>();
        pageRepo.Create(tempChild);

        // Create relation to the temporary child
        var brokenRelation = new PageRelation
        {
            Parent = parent2,
            Child = tempChild,
            PreviousId = null,
            NextId = null
        };
        pageRelationRepo.Create(brokenRelation);

        // Now delete the child page to create a broken link
        // But keep the relation in the database to simulate corruption
        pageRepo.Delete(tempChild.Id);

        await ReloadCaches();
        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var relationErrors = new RelationErrors(pageRelationRepo, R<PageRepository>());

        // Act - Test healing for parent2 (the problematic one)
        var healResult = relationErrors.HealErrors(parent2.Id);

        // Reload cache after healing
        await ReloadCaches();
        entityCacheInitializer.Init();

        // Assert - Verify healing worked
        var finalErrorResponse = relationErrors.GetErrors();
        var allRelationCacheItems = EntityCache.GetAllRelations();
        var allDbItems = pageRelationRepo.GetAll();

        var verificationData = new
        {
            HealingResult = healResult,
            GetErrorsResult = finalErrorResponse,
            AllRelationCacheItems = allRelationCacheItems,
            CacheItemsCount = allRelationCacheItems.Count,
            AllDbItems = allDbItems,
            DbItemsCount = allDbItems.Count
        };

        await Verify(verificationData);
    }

    [Test]
    public async Task Should_return_no_errors_when_relations_are_healthy()
    {
        await ClearData();

        // Arrange - Create a page with 3 clean child relations
        var context = NewPageContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        context.Add("rootWiki", creator: creator, isWiki: true).Persist();
        context
            .Add("parent", creator: creator)
            .Add("child1", creator: creator)
            .Add("child2", creator: creator)
            .Add("child3", creator: creator)
            .Persist();

        var parent = context.All.ByName("parent");

        // Set up 3 clean relations
        context.AddChild(parent, context.All.ByName("child1"));
        context.AddChild(parent, context.All.ByName("child2"));
        context.AddChild(parent, context.All.ByName("child3"));

        await ReloadCaches();
        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var pageRelationRepo = R<PageRelationRepo>();
        var relationErrors = new RelationErrors(pageRelationRepo, R<PageRepository>());

        // Act
        var errorResponse = relationErrors.GetErrors();
        var allRelationCacheItems = EntityCache.GetAllRelations();
        var allDbItems = pageRelationRepo.GetAll();

        // Assert - Should have no errors
        var verificationData = new
        {
            GetErrorsResult = errorResponse,
            AllRelationCacheItems = allRelationCacheItems,
            CacheItemsCount = allRelationCacheItems.Count,
            AllDbItems = allDbItems,
            DbItemsCount = allDbItems.Count
        };

        await Verify(verificationData);
    }

    [Test]
    public async Task Should_detect_all_error_types_comprehensively()
    {
        await ClearData();

        // Arrange - Create comprehensive scenario covering all error types
        var context = NewPageContext();
        var creator = _testHarness.GetDefaultSessionUserFromDb();

        // Create pages for comprehensive error testing
        context.Add("rootWiki", creator: creator, isWiki: true).Persist();
        context
            .Add("parentDuplicates", creator: creator)         // Parent with duplicate children
            .Add("parentMultipleFirsts", creator: creator)     // Parent with multiple first items
            .Add("parentMultipleLasts", creator: creator)      // Parent with multiple last items
            .Add("parentDuplicatePrevious", creator: creator)  // Parent with duplicate PreviousId
            .Add("parentDuplicateNext", creator: creator)      // Parent with duplicate NextId
            .Add("parentInconsistent", creator: creator)       // Parent with inconsistent chain links
            .Add("parentCircular", creator: creator)           // Parent with circular references
            .Add("parentOrphaned", creator: creator)           // Parent with orphaned relations
            .Add("parentNoStart", creator: creator)            // Parent with no chain start
            .Add("child1", creator: creator)
            .Add("child2", creator: creator)
            .Add("child3", creator: creator)
            .Add("child4", creator: creator)
            .Add("child5", creator: creator)
            .Add("child6", creator: creator)
            .Add("child7", creator: creator)
            .Add("child8", creator: creator)
            .Add("child9", creator: creator)
            .Add("child10", creator: creator)
            .Persist();

        var rootWiki = context.All.ByName("rootWiki");
        var parentDuplicates = context.All.ByName("parentDuplicates");
        var parentMultipleFirsts = context.All.ByName("parentMultipleFirsts");
        var parentMultipleLasts = context.All.ByName("parentMultipleLasts");
        var parentDuplicatePrevious = context.All.ByName("parentDuplicatePrevious");
        var parentDuplicateNext = context.All.ByName("parentDuplicateNext");
        var parentInconsistent = context.All.ByName("parentInconsistent");
        var parentCircular = context.All.ByName("parentCircular");
        var parentOrphaned = context.All.ByName("parentOrphaned");
        var parentNoStart = context.All.ByName("parentNoStart");
        var child1 = context.All.ByName("child1");
        var child2 = context.All.ByName("child2");
        var child3 = context.All.ByName("child3");
        var child4 = context.All.ByName("child4");
        var child5 = context.All.ByName("child5");
        var child6 = context.All.ByName("child6");
        var child7 = context.All.ByName("child7");
        var child8 = context.All.ByName("child8");
        var child9 = context.All.ByName("child9");
        var child10 = context.All.ByName("child10");

        // Set up normal wiki structure first
        context.AddChild(rootWiki, parentDuplicates);
        context.AddChild(rootWiki, parentMultipleFirsts);
        context.AddChild(rootWiki, parentMultipleLasts);
        context.AddChild(rootWiki, parentDuplicatePrevious);
        context.AddChild(rootWiki, parentDuplicateNext);
        context.AddChild(rootWiki, parentInconsistent);
        context.AddChild(rootWiki, parentCircular);
        context.AddChild(rootWiki, parentOrphaned);
        context.AddChild(rootWiki, parentNoStart);

        // Create problematic relations directly in database to simulate corruption
        var pageRelationRepo = R<PageRelationRepo>();

        // 1. Duplicate relations
        var duplicate1A = new PageRelation { Parent = parentDuplicates, Child = child1, PreviousId = null, NextId = null };
        var duplicate1B = new PageRelation { Parent = parentDuplicates, Child = child1, PreviousId = null, NextId = null };
        pageRelationRepo.Create(duplicate1A);
        pageRelationRepo.Create(duplicate1B);

        // 2. Multiple relations with null PreviousId (multiple first items)
        var multiFirst1 = new PageRelation { Parent = parentMultipleFirsts, Child = child2, PreviousId = null, NextId = null };
        var multiFirst2 = new PageRelation { Parent = parentMultipleFirsts, Child = child3, PreviousId = null, NextId = null };
        pageRelationRepo.Create(multiFirst1);
        pageRelationRepo.Create(multiFirst2);

        // 3. Multiple relations with null NextId (multiple last items)
        var multiLast1 = new PageRelation { Parent = parentMultipleLasts, Child = child4, PreviousId = null, NextId = null };
        var multiLast2 = new PageRelation { Parent = parentMultipleLasts, Child = child5, PreviousId = child4.Id, NextId = null };
        pageRelationRepo.Create(multiLast1);
        pageRelationRepo.Create(multiLast2);

        // 4. Duplicate PreviousId
        var dupPrev1 = new PageRelation { Parent = parentDuplicatePrevious, Child = child6, PreviousId = null, NextId = child7.Id };
        var dupPrev2 = new PageRelation { Parent = parentDuplicatePrevious, Child = child7, PreviousId = child6.Id, NextId = child8.Id };
        var dupPrev3 = new PageRelation { Parent = parentDuplicatePrevious, Child = child8, PreviousId = child6.Id, NextId = null }; // Same PreviousId as child7
        pageRelationRepo.Create(dupPrev1);
        pageRelationRepo.Create(dupPrev2);
        pageRelationRepo.Create(dupPrev3);

        // 5. Duplicate NextId
        var dupNext1 = new PageRelation { Parent = parentDuplicateNext, Child = child9, PreviousId = null, NextId = child10.Id };
        var dupNext2 = new PageRelation { Parent = parentDuplicateNext, Child = child1, PreviousId = child9.Id, NextId = child10.Id }; // Same NextId as child9
        var dupNext3 = new PageRelation { Parent = parentDuplicateNext, Child = child10, PreviousId = child1.Id, NextId = null };
        pageRelationRepo.Create(dupNext1);
        pageRelationRepo.Create(dupNext2);
        pageRelationRepo.Create(dupNext3);

        // 6. Inconsistent chain links
        var inconsistent1 = new PageRelation { Parent = parentInconsistent, Child = child2, PreviousId = null, NextId = child3.Id };
        var inconsistent2 = new PageRelation { Parent = parentInconsistent, Child = child3, PreviousId = child4.Id, NextId = null }; // Wrong PreviousId
        pageRelationRepo.Create(inconsistent1);
        pageRelationRepo.Create(inconsistent2);

        // 7. Circular reference (but break the circle to avoid infinite loops)
        var circular1 = new PageRelation { Parent = parentCircular, Child = child5, PreviousId = null, NextId = child6.Id };
        var circular2 = new PageRelation { Parent = parentCircular, Child = child6, PreviousId = child5.Id, NextId = child7.Id };
        var circular3 = new PageRelation { Parent = parentCircular, Child = child7, PreviousId = child6.Id, NextId = null }; // Break the circle - don't point back to child5
        pageRelationRepo.Create(circular1);
        pageRelationRepo.Create(circular2);
        pageRelationRepo.Create(circular3);

        // 8. Orphaned relations (disconnected from main chain)
        var orphaned1 = new PageRelation { Parent = parentOrphaned, Child = child8, PreviousId = null, NextId = child9.Id };
        var orphaned2 = new PageRelation { Parent = parentOrphaned, Child = child9, PreviousId = child8.Id, NextId = null };
        var orphaned3 = new PageRelation { Parent = parentOrphaned, Child = child10, PreviousId = null, NextId = null }; // Isolated
        pageRelationRepo.Create(orphaned1);
        pageRelationRepo.Create(orphaned2);
        pageRelationRepo.Create(orphaned3);

        // 9. No chain start (all relations have non-null PreviousId) - now should be handled by robust Sort method
        var noStart1 = new PageRelation { Parent = parentNoStart, Child = child1, PreviousId = child2.Id, NextId = child3.Id };
        var noStart2 = new PageRelation { Parent = parentNoStart, Child = child2, PreviousId = child3.Id, NextId = child1.Id };
        var noStart3 = new PageRelation { Parent = parentNoStart, Child = child3, PreviousId = child1.Id, NextId = child2.Id };
        pageRelationRepo.Create(noStart1);
        pageRelationRepo.Create(noStart2);
        pageRelationRepo.Create(noStart3);

        // Reload cache to include problematic relations
        EntityCache.Clear();
        await ReloadCaches();
        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        // Get service instance
        var relationErrors = new RelationErrors(pageRelationRepo, R<PageRepository>());

        // Act
        var errorResponse = relationErrors.GetErrors();
        var allRelationCacheItems = EntityCache.GetAllRelations();
        var allDbItems = pageRelationRepo.GetAll();

        // Assert
        var verificationData = new
        {
            GetErrorsResult = errorResponse,
            CacheItemsCount = allRelationCacheItems.Count,
            DbItemsCount = allDbItems.Count,
            AllRelationCacheItems = allRelationCacheItems,
            AllDbItems = allDbItems
        };

        await Verify(verificationData);
    }
}