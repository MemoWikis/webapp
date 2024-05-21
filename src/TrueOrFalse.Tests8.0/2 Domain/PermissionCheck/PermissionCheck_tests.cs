class PermissionCheck_tests : BaseTest
{
    [Test]
    public void CanMoveTopic_MoveTopicCreator_And_ParentTopicCreator_IsUser()
    {
        RecycleContainerAndEntityCache();

        var context = ContextCategory.New();
        var user = new User { Id = 1 };

        context.Add("root", creator: user).Persist();

        context
            .Add("sub1", creator: user)
            .Add("subsub1", creator: user)
            .Add("sub2", creator: user)
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var subsub1 = context.All.ByName("subsub1");
        var sub2 = context.All.ByName("sub2");

        context.AddChild(root, sub1);
        context.AddChild(sub1, subsub1);
        context.AddChild(root, sub2);

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var permissionCheck = new PermissionCheck(user.Id);
        Assert.IsTrue(permissionCheck.CanMoveTopic(subsub1.Id, sub1.Id, 42));
    }

    [Test]
    public void CanMoveTopic_MoveTopicCreator_IsUser()
    {
        RecycleContainerAndEntityCache();

        var context = ContextCategory.New();
        var user = new User { Id = 1 };
        var user2 = new User { Id = 2 };
        context.Add("root", creator: user2).Persist();

        context
            .Add("sub1", creator: user2)
            .Add("subsub1", creator: user)
            .Add("sub2", creator: user2)
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var subsub1 = context.All.ByName("subsub1");
        var sub2 = context.All.ByName("sub2");

        context.AddChild(root, sub1);
        context.AddChild(sub1, subsub1);
        context.AddChild(root, sub2);

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var permissionCheck = new PermissionCheck(user.Id);
        Assert.That(true, Is.EqualTo(permissionCheck.CanMoveTopic(subsub1.Id, sub1.Id, 42)));
    }

    [Test]
    public void CanMoveTopic_ParentTopicCreator_IsUser()
    {
        RecycleContainerAndEntityCache();

        var context = ContextCategory.New();
        var user = new User { Id = 1 };
        var user2 = new User { Id = 2 };
        context.Add("root", creator: user2).Persist();

        context
            .Add("sub1", creator: user2)
            .Add("subsub1", creator: user)
            .Add("sub2", creator: user2)
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var subsub1 = context.All.ByName("subsub1");
        var sub2 = context.All.ByName("sub2");

        context.AddChild(root, sub1);
        context.AddChild(sub1, subsub1);
        context.AddChild(root, sub2);

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var permissionCheck = new PermissionCheck(user2.Id);
        Assert.That(true, Is.EqualTo(permissionCheck.CanMoveTopic(subsub1.Id, sub1.Id, 42)));
    }

    [Test]
    public void CanMoveTopic_Disallowed()
    {
        RecycleContainerAndEntityCache();

        var context = ContextCategory.New();
        var user = new User { Id = 1 };
        var user2 = new User { Id = 2 };
        var user3 = new User { Id = 3 };

        context.Add("root", creator: user2).Persist();

        context
            .Add("sub1", creator: user2)
            .Add("subsub1", creator: user)
            .Add("sub2", creator: user2)
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var subsub1 = context.All.ByName("subsub1");
        var sub2 = context.All.ByName("sub2");

        context.AddChild(root, sub1);
        context.AddChild(sub1, subsub1);
        context.AddChild(root, sub2);

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var permissionCheck = new PermissionCheck(user3.Id);
        Assert.That(false, Is.EqualTo(permissionCheck.CanMoveTopic(subsub1.Id, sub1.Id, 42)));
        Assert.That(false, Is.EqualTo(permissionCheck.CanMoveTopic(subsub1.Id, sub1.Id, root.Id)));
    }
}