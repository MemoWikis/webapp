

using System.Diagnostics;

class Relation_tests : BaseTest
{
    [Test]
    public void Should_Add_Creation_ToDb_and_EntityCache()
    {
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
        var modifyRelationsForCategory = new ModifyRelationsForCategory(R<CategoryRepository>(), R<CategoryRelationRepo>());
        ModifyRelationsEntityCache.MoveAfter(relationToMove, sub3.Id, cachedRoot.Id, cachedRoot.Id, 1, modifyRelationsForCategory);

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
    }

}
