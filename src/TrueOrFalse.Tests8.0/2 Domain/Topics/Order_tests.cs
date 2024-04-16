

class Order_tests : BaseTest
{
    [Test]
    public void SortTopics_ShouldCorrectlySortTopics()
    {
        var unsortedRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation { Id = 1, ChildId = 3, ParentId = 10, PreviousId = 2, NextId = null },
            new CategoryCacheRelation { Id = 2, ChildId = 1, ParentId = 10, PreviousId = null, NextId = 2 },
            new CategoryCacheRelation { Id = 3, ChildId = 2, ParentId = 10, PreviousId = 1, NextId = 3 }
        };

        var sortedRelations = TopicOrderer.Sort(unsortedRelations, 10);

        Assert.IsNotNull(sortedRelations);
        Assert.AreEqual(3, sortedRelations.Count);
        Assert.AreEqual(1, sortedRelations[0].ChildId);
        Assert.AreEqual(2, sortedRelations[1].ChildId);
        Assert.AreEqual(3, sortedRelations[2].ChildId);
    }

    [Test]
    public void SortTopics_ShouldCorrectlySortTopics_WithBrokenRelations()
    {
        var unsortedRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation { Id = 1, ChildId = 3, ParentId = 10, PreviousId = 2, NextId = null },
            new CategoryCacheRelation { Id = 2, ChildId = 1, ParentId = 10, PreviousId = null, NextId = 2 },
            new CategoryCacheRelation { Id = 3, ChildId = 2, ParentId = 10, PreviousId = 1, NextId = 3 },

            new CategoryCacheRelation { Id = 4, ChildId = 4, ParentId = 10, PreviousId = 2, NextId = null },
            new CategoryCacheRelation { Id = 5, ChildId = 5, ParentId = 10, PreviousId = null, NextId = null },
            new CategoryCacheRelation { Id = 6, ChildId = 6, ParentId = 10, PreviousId = null, NextId = 3 },
        };

        var sortedRelations = TopicOrderer.Sort(unsortedRelations, 10);

        Assert.IsNotNull(sortedRelations);
        Assert.AreEqual(6, sortedRelations.Count);
        Assert.AreEqual(1, sortedRelations[0].ChildId);
        Assert.AreEqual(2, sortedRelations[1].ChildId);
        Assert.AreEqual(3, sortedRelations[2].ChildId);

        Assert.AreEqual(6, sortedRelations[5].ChildId);
    }

    [Test]
    public void Should_Add_Creation_ToDb_and_EntityCache()
    {
        RecycleContainerAndEntityCache();

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
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetCategory(root);
        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(2));
        Assert.That(cachedRoot.ChildRelations[0].PreviousId, Is.EqualTo(null));

        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub1.Id));

        Assert.That(cachedRoot.ChildRelations[1].NextId, Is.EqualTo(null));
    }

    //Move sub1 after sub3
    [Test]
    public void Should_MoveRelation_Correctly_AfterSub3()
    {
        RecycleContainerAndEntityCache();

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
        var modifyRelationsForCategory = new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);
        TopicOrderer.MoveAfter(relationToMove, sub3.Id, cachedRoot.Id, 1, modifyRelationsForCategory);

        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(3));

        Assert.That(cachedRoot.ChildRelations[0].PreviousId, Is.EqualTo(null));
        Assert.That(cachedRoot.ChildRelations[0].ChildId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub3.Id));

        Assert.That(cachedRoot.ChildRelations[1].ChildId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[1].NextId, Is.EqualTo(sub1.Id));

        Assert.That(cachedRoot.ChildRelations[2].ChildId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[2].PreviousId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[2].NextId, Is.EqualTo(null));

        Task.Delay(200).Wait();

        var allRelationsInDb = categoryRelationRepo.GetAll();

        Assert.That(allRelationsInDb.Count, Is.EqualTo(3));

        var firstCachedId = cachedRoot.ChildRelations[0].Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.Child.Id, Is.EqualTo(cachedRoot.ChildRelations[0].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.PreviousId, Is.EqualTo(cachedRoot.ChildRelations[0].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.NextId, Is.EqualTo(cachedRoot.ChildRelations[0].NextId));

        Assert.That(allRelationsInDb[2].Child.Id, Is.EqualTo(cachedRoot.ChildRelations[2].ChildId));
        Assert.That(allRelationsInDb[2].PreviousId, Is.EqualTo(cachedRoot.ChildRelations[2].PreviousId));
        Assert.That(allRelationsInDb[2].NextId, Is.EqualTo(cachedRoot.ChildRelations[2].NextId));
    }

    //Move sub3 before sub1
    [Test]
    public void Should_MoveRelation_Correctly_BeforeSub1()
    {
        RecycleContainerAndEntityCache();

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
        var relationToMove = cachedRoot.ChildRelations[2];
        var categoryRelationRepo = R<CategoryRelationRepo>();
        var modifyRelationsForCategory = new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);
        TopicOrderer.MoveBefore(relationToMove, sub1.Id, cachedRoot.Id, 1, modifyRelationsForCategory);

        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(3));

        Assert.That(cachedRoot.ChildRelations[0].PreviousId, Is.EqualTo(null));
        Assert.That(cachedRoot.ChildRelations[0].ChildId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub1.Id));

        Assert.That(cachedRoot.ChildRelations[1].ChildId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[1].NextId, Is.EqualTo(sub2.Id));

        Assert.That(cachedRoot.ChildRelations[2].ChildId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[2].PreviousId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[2].NextId, Is.EqualTo(null));

        Task.Delay(200).Wait();

        var allRelationsInDb = categoryRelationRepo.GetAll();

        Assert.That(allRelationsInDb.Count, Is.EqualTo(3));

        var firstCachedId = cachedRoot.ChildRelations[0].Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.Child.Id, Is.EqualTo(cachedRoot.ChildRelations[0].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.PreviousId, Is.EqualTo(cachedRoot.ChildRelations[0].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.NextId, Is.EqualTo(cachedRoot.ChildRelations[0].NextId));

        var lastCachedId = cachedRoot.ChildRelations.LastOrDefault()?.Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.Child.Id, Is.EqualTo(cachedRoot.ChildRelations[2].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.PreviousId, Is.EqualTo(cachedRoot.ChildRelations[2].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.NextId, Is.EqualTo(cachedRoot.ChildRelations[2].NextId));
    }

    //Move sub1 after sub3 and before sub4
    [Test]
    public void Should_MoveRelation_Correctly_AfterSub3_BeforeSub4()
    {
        RecycleContainerAndEntityCache();

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
        var modifyRelationsForCategory = new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);
        TopicOrderer.MoveAfter(relationToMove, sub3.Id, cachedRoot.Id, 1, modifyRelationsForCategory);

        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(4));

        Assert.That(cachedRoot.ChildRelations[0].PreviousId, Is.EqualTo(null));
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
        Assert.That(cachedRoot.ChildRelations[3].NextId, Is.EqualTo(null));

        Task.Delay(200).Wait();

        var allRelationsInDb = categoryRelationRepo.GetAll();

        Assert.That(allRelationsInDb.Count, Is.EqualTo(4));

        var firstCachedId = cachedRoot.ChildRelations[0].Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.Child.Id, Is.EqualTo(cachedRoot.ChildRelations[0].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.PreviousId, Is.EqualTo(cachedRoot.ChildRelations[0].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.NextId, Is.EqualTo(cachedRoot.ChildRelations[0].NextId));

        var lastCachedId = cachedRoot.ChildRelations.LastOrDefault()?.Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.Child.Id, Is.EqualTo(cachedRoot.ChildRelations[3].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.PreviousId, Is.EqualTo(cachedRoot.ChildRelations[3].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.NextId, Is.EqualTo(cachedRoot.ChildRelations[3].NextId));
    }

    [Test]
    public void Should_Fail_Move_CircularReference()
    {
        RecycleContainerAndEntityCache();

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
        var modifyRelationsForCategory = new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);

        var ex = Assert.Throws<Exception>(() => TopicOrderer.MoveAfter(relationToMove, sub1sub1sub1.Id, sub1sub1.Id, 1, modifyRelationsForCategory));
        Assert.That(ex.Message, Is.EqualTo("circular reference"));

        var ex2 = Assert.Throws<Exception>(() => TopicOrderer.MoveAfter(relationToMove, sub1sub1.Id, sub1.Id, 1, modifyRelationsForCategory));
        Assert.That(ex2.Message, Is.EqualTo("circular reference"));
    }

    [Test]
    public void Should_correctly_Remove_and_Add_Parent_on_MoveIn()
    {
        RecycleContainerAndEntityCache();

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
        var modifyRelationsForCategory = new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);

        TopicOrderer.MoveIn(relationToMove, sub2.Id, 1, modifyRelationsForCategory, R<PermissionCheck>());

        Assert.That(cachedSub1.ChildRelations.Count, Is.EqualTo(0));

        var cachedSub2 = EntityCache.GetCategory(sub2);
        Assert.That(cachedSub2.ChildRelations.Count, Is.EqualTo(1));

        var movedSub = EntityCache.GetCategory(sub1sub1);

        Assert.That(cachedSub2.ChildRelations[0].ChildId, Is.EqualTo(movedSub.Id));
        Assert.That(movedSub.ParentRelations.Count, Is.EqualTo(1));
    }
}
