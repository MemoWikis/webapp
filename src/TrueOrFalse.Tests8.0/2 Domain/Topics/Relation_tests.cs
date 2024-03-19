

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
        Assert.AreEqual(cachedRoot.ChildRelations[0].PreviousId, null);

        Assert.AreEqual(cachedRoot.ChildRelations[0].NextId, sub2.Id);
        Assert.AreEqual(cachedRoot.ChildRelations[1].PreviousId, sub1.Id);

        Assert.AreEqual(cachedRoot.ChildRelations[1].NextId, null);

    }

}
