class Order_tests : BaseTest
{
    [Test]
    public void Should_Sort_Topics()
    {
        //Arrange
        var unsortedRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
                { Id = 1, ChildId = 3, ParentId = 10, PreviousId = 2, NextId = null },
            new CategoryCacheRelation
                { Id = 2, ChildId = 1, ParentId = 10, PreviousId = null, NextId = 2 },
            new CategoryCacheRelation
                { Id = 3, ChildId = 2, ParentId = 10, PreviousId = 1, NextId = 3 }
        };

        //Act
        var sortedRelations = TopicOrderer.Sort(unsortedRelations, 10);

        //Assert
        Assert.IsNotNull(sortedRelations);
        Assert.That(sortedRelations.Count, Is.EqualTo(3));
        Assert.That(sortedRelations[0].ChildId, Is.EqualTo(1));
        Assert.That(sortedRelations[1].ChildId, Is.EqualTo(2));
        Assert.That(sortedRelations[2].ChildId, Is.EqualTo(3));
    }

    [Test]
    public void Should_Sort_Topics_with_faulty_relations()
    {
        //Arrange
        var unsortedRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation
                { Id = 1, ChildId = 3, ParentId = 10, PreviousId = 2, NextId = null },
            new CategoryCacheRelation
                { Id = 2, ChildId = 1, ParentId = 10, PreviousId = null, NextId = 2 },
            new CategoryCacheRelation
                { Id = 3, ChildId = 2, ParentId = 10, PreviousId = 1, NextId = 3 },

            new CategoryCacheRelation
                { Id = 4, ChildId = 4, ParentId = 10, PreviousId = 2, NextId = null },
            new CategoryCacheRelation
                { Id = 5, ChildId = 5, ParentId = 10, PreviousId = null, NextId = null },
            new CategoryCacheRelation
                { Id = 6, ChildId = 6, ParentId = 10, PreviousId = null, NextId = 3 },
        };

        //Act
        var sortedRelations = TopicOrderer.Sort(unsortedRelations, 10);

        Assert.IsNotNull(sortedRelations);
        Assert.That(sortedRelations.Count, Is.EqualTo(6));
        Assert.That(sortedRelations[0].ChildId, Is.EqualTo(1));
        Assert.That(sortedRelations[1].ChildId, Is.EqualTo(2));
        Assert.That(sortedRelations[2].ChildId, Is.EqualTo(3));

        Assert.That(sortedRelations[5].ChildId, Is.EqualTo(6));
    }

    [Test]
    public void Should_init_children_in_EntityCache()
    {
        //Arrange
        var context = ContextCategory.New();

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

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();

        //Act
        entityCacheInitializer.Init();

        //Assert
        var cachedRoot = EntityCache.GetCategory(root);
        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(2));
        Assert.IsNull(cachedRoot.ChildRelations[0].PreviousId);

        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub1.Id));

        Assert.IsNull(cachedRoot.ChildRelations[1].NextId);
    }

    //Move sub1 after sub3
    [Test]
    public async Task Should_move_relation_after_sub3()
    {
        //Arrange
        var context = ContextCategory.New();

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

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetCategory(root);
        var relationToMove = cachedRoot.ChildRelations[0];
        var categoryRelationRepo = R<CategoryRelationRepo>();
        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);

        //Act
        await TopicOrderer.MoveAfterAsync(relationToMove, sub3.Id, cachedRoot.Id, 1,
            modifyRelationsForCategory).ConfigureAwait(false);

        //Assert
        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(3));

        Assert.IsNull(cachedRoot.ChildRelations[0].PreviousId);
        Assert.That(cachedRoot.ChildRelations[0].ChildId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub3.Id));

        Assert.That(cachedRoot.ChildRelations[1].ChildId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[1].NextId, Is.EqualTo(sub1.Id));

        Assert.That(cachedRoot.ChildRelations[2].ChildId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[2].PreviousId, Is.EqualTo(sub3.Id));
        Assert.IsNull(cachedRoot.ChildRelations[2].NextId);

        var allRelationsInDb = categoryRelationRepo.GetAll();

        Assert.That(allRelationsInDb.Count, Is.EqualTo(3));

        var firstCachedId = cachedRoot.ChildRelations[0].Id;
        var firstRelation = allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId);
        categoryRelationRepo.Refresh(firstRelation);

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.Child.Id,
            Is.EqualTo(cachedRoot.ChildRelations[0].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.PreviousId,
            Is.EqualTo(cachedRoot.ChildRelations[0].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.NextId,
            Is.EqualTo(cachedRoot.ChildRelations[0].NextId));

        Assert.That(allRelationsInDb[2].Child.Id, Is.EqualTo(cachedRoot.ChildRelations[2].ChildId));
        Assert.That(allRelationsInDb[2].PreviousId,
            Is.EqualTo(cachedRoot.ChildRelations[2].PreviousId));
        Assert.That(allRelationsInDb[2].NextId, Is.EqualTo(cachedRoot.ChildRelations[2].NextId));
    }

    //Move sub3 before sub1
    [Test]
    public async Task Should_move_relation_before_sub1()
    {
        //Arrange
        var context = ContextCategory.New();

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

        RecycleContainerAndEntityCache();

        var cachedRoot = EntityCache.GetCategory(root);
        var relationToMove = cachedRoot.ChildRelations[2];
        var categoryRelationRepo = R<CategoryRelationRepo>();
        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);

        //Act
        await TopicOrderer.MoveBeforeAsync(relationToMove, sub1.Id, cachedRoot.Id, 1,
            modifyRelationsForCategory).ConfigureAwait(false);

        //Assert
        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(3));

        Assert.IsNull(cachedRoot.ChildRelations[0].PreviousId);
        Assert.That(cachedRoot.ChildRelations[0].ChildId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub1.Id));

        Assert.That(cachedRoot.ChildRelations[1].ChildId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[1].NextId, Is.EqualTo(sub2.Id));

        Assert.That(cachedRoot.ChildRelations[2].ChildId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[2].PreviousId, Is.EqualTo(sub1.Id));
        Assert.IsNull(cachedRoot.ChildRelations[2].NextId);

        var allRelationsInDb = categoryRelationRepo.GetAll();

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
        //Arrange
        var context = ContextCategory.New();

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

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetCategory(root);
        var relationToMove = cachedRoot.ChildRelations[0];
        var categoryRelationRepo = R<CategoryRelationRepo>();
        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);

        //Act
        await TopicOrderer.MoveAfterAsync(relationToMove, sub3.Id, cachedRoot.Id, 1,
            modifyRelationsForCategory).ConfigureAwait(false);

        //Assert
        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(4));

        Assert.IsNull(cachedRoot.ChildRelations[0].PreviousId);
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
        Assert.IsNull(cachedRoot.ChildRelations[3].NextId);

        var allRelationsInDb = categoryRelationRepo.GetAll();

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
        var context = ContextCategory.New();

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

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetCategory(root);
        var relationToMove = cachedRoot.ChildRelations[0];
        var categoryRelationRepo = R<CategoryRelationRepo>();
        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);

        //Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await TopicOrderer.MoveAfterAsync(
            relationToMove,
            sub1sub1sub1.Id, sub1sub1.Id, 1, modifyRelationsForCategory));

        Assert.That(ex.Message, Is.EqualTo(FrontendMessageKeys.Error.Category.CircularReference));

        var ex2 = Assert.ThrowsAsync<Exception>(async () =>
            await TopicOrderer.MoveAfterAsync(relationToMove, sub1sub1.Id, sub1.Id, 1,
                modifyRelationsForCategory));
        Assert.That(ex2.Message, Is.EqualTo(FrontendMessageKeys.Error.Category.CircularReference));
    }

    [Test]
    public async Task Should_remove_old_parent_and_add_new_parent_on_MoveIn()
    {
        //Arrange
        var context = ContextCategory.New();

        context.Add("root").Persist();

        context
            .Add("sub1")
            .Add("sub2")
            .Add("sub1sub1")
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub2 = context.All.ByName("sub2");
        var sub1sub1 = context.All.ByName("sub1sub1");

        context.AddChild(root, sub1);
        context.AddChild(sub1, sub1sub1);
        context.AddChild(root, sub2);

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedSub1 = EntityCache.GetCategory(sub1);
        var relationToMove = cachedSub1.ChildRelations[0];
        var categoryRelationRepo = R<CategoryRelationRepo>();
        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);

        //Act
        await TopicOrderer.MoveInAsync(relationToMove, sub2.Id, 1, modifyRelationsForCategory,
            R<PermissionCheck>()).ConfigureAwait(false);

        //Assert
        Assert.That(cachedSub1.ChildRelations.Count, Is.EqualTo(0));

        var cachedSub2 = EntityCache.GetCategory(sub2);
        Assert.That(cachedSub2.ChildRelations.Count, Is.EqualTo(1));

        var movedSub = EntityCache.GetCategory(sub1sub1);

        Assert.That(cachedSub2.ChildRelations[0].ChildId, Is.EqualTo(movedSub.Id));
        Assert.That(movedSub.ParentRelations.Count, Is.EqualTo(1));
    }
}