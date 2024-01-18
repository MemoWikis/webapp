
using NHibernate;

class GraphService_tests : BaseTest
{
    [Test]
    public void Should_get_all_parents()
    {
        var context = ContextCategory.New();

        context.Add("RootElement").Add("RootElement2").Persist();

        var root = context.All.ByName("RootElement");
        var root2 = context.All.ByName("RootElement2");

        context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: CategoryVisibility.Owner)
            .Add("SubSub2", visibility: CategoryVisibility.Owner)
            .Add("SubSub1and2")
            .Add("Sub3", visibility: CategoryVisibility.Owner)
            .Add("SubSub3")
            .Persist();

        context.AddChild(root, context.All.ByName("Sub1"));
        context.AddChild(context.All.ByName("Sub1"), context.All.ByName("SubSub1"));

        context.AddChild(root, context.All.ByName("Sub2"));
        context.AddChild(context.All.ByName("Sub2"), context.All.ByName("SubSub2")); 
        
        context.AddChild(context.All.ByName("Sub1"), context.All.ByName("SubSub1and2"));
        context.AddChild(context.All.ByName("Sub2"), context.All.ByName("SubSub1and2"));

        context.AddChild(root, context.All.ByName("Sub3"));
        context.AddChild(context.All.ByName("Sub3"), context.All.ByName("SubSub3"));
        context.AddChild(root2, context.All.ByName("SubSub3"));

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var allParents = GraphService.Ascendants(context.All.ByName("SubSub3").Id);
        Assert.That(allParents.Count, Is.EqualTo(3));
        Assert.That(allParents.Select(i => i.Name), Does.Contain("Sub3"));
        Assert.That(allParents.Select(i => i.Name), Does.Contain("RootElement"));
        Assert.That(allParents.Select(i => i.Name), Does.Contain("RootElement2"));
    }
}
