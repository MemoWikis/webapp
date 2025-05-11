using Autofac;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ISession = NHibernate.ISession;

[TestFixture]
public class BaseTestLegacy
{
    private static IContainer _container;
    protected ILifetimeScope LifetimeScope;

    protected static User _sessionUser => new User
    {
        Name = "SessionUser",
        Id = 1
    };

    static BaseTestLegacy()
    {
#if DEBUG
        //            NHibernateProfiler.Initialize();
#endif
    }

    [SetUp]
    public async Task SetUp()
    {
        SessionFactory.BuildTestConfiguration();
        // CleanEmailsFromPickupDirectory.Run();
        InitializeContainer();
        LifetimeScope = _container.BeginLifetimeScope();
        foreach (var registration in _container.ComponentRegistry.Registrations)
        {
            foreach (var service in registration.Services)
            {
                Console.WriteLine(service);
            }
        }

        ImageDirectoryCreator.CreateImageDirectories(App.Environment.ContentRootPath);

        var initializer = Resolve<EntityCacheInitializer>();
        initializer.Init(" (started in unit test) ");
        DateTimeX.ResetOffset();
        SetSessionUserInDatabase();

        await JobScheduler.InitializeAsync();
    }

    [TearDown]
    public void RecycleContainer()
    {
        App.Environment = null;
        R<ISession>().Flush();
        AutofacWebInitializer.Dispose();

        MySQL5FlexibleDialect.Engine = "MEMORY";
        BuildContainer();
        ServiceLocator.Init(_container);

        JobScheduler.Clear();
        EntityCache.Clear();

        _container.Dispose();
        LifetimeScope.Dispose();
    }

    public void RecycleContainerAndEntityCache()
    {
        EntityCache.Clear();
        Resolve<SessionData>().Clear();

        App.Environment = null;
        R<ISession>().Flush();
        AutofacWebInitializer.Dispose();

        MySQL5FlexibleDialect.Engine = "MEMORY";
        BuildContainer();
        ServiceLocator.Init(_container);

        var initializer = Resolve<EntityCacheInitializer>();
        initializer.Init(" (started in unit test) ");
    }

    public static void InitializeContainer()
    {
        MySQL5FlexibleDialect.Engine = "MEMORY";
        BuildContainer();
        ServiceLocator.Init(_container);
        SessionFactory.TruncateAllTables();
    }

    private static void BuildContainer()
    {
        _container =
            AutofacWebInitializer.GetTestContainer(SetWebHostEnvironment(),
                SetHttpContextAccessor());
        Console.WriteLine(_container.GetHashCode());
    }

    private static IWebHostEnvironment SetWebHostEnvironment()
    {
        var fakeWebHostEnvironment = A.Fake<IWebHostEnvironment>();
        App.Environment = fakeWebHostEnvironment;
        A.CallTo(() => fakeWebHostEnvironment.EnvironmentName).Returns("TestEnvironment");
        return fakeWebHostEnvironment;
    }

    private static IHttpContextAccessor SetHttpContextAccessor()
    {
        var httpContextAccessor = A.Fake<IHttpContextAccessor>();
        var httpContext = A.Fake<HttpContext>();
        var session = A.Fake<Microsoft.AspNetCore.Http.ISession>();
        A.CallTo(() => httpContextAccessor.HttpContext).Returns(httpContext);
        A.CallTo(() => httpContext.Session).Returns(session);

        SetSessionValues(session);

        return httpContextAccessor;
    }

    private static void SetSessionValues(Microsoft.AspNetCore.Http.ISession session)
    {
        byte[] userIdBytes = BitConverter.GetBytes(1);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(userIdBytes); //// hier
        }

        A.CallTo(() => session.TryGetValue("userId", out userIdBytes)).Returns(true);
    }

    private static void SetSessionUserInDatabase()
    {
        ContextUser.New(R<UserWritingRepo>())
            .Add(_sessionUser)
            .Persist();
    }

    public static T Resolve<T>() where T : notnull => _container.Resolve<T>();

    public static T R<T>() where T : notnull => _container.Resolve<T>();

    public void ClearNHibernateCaches()
    {
        var session = R<ISession>();
        session.Clear(); // Clear the first-level cache (session cache)

        var sessionFactory = session.SessionFactory;
        foreach (var collectionMetadata in sessionFactory.GetAllCollectionMetadata())
        {
            sessionFactory.EvictCollection(collectionMetadata.Key);
        }

        foreach (var classMetadata in sessionFactory.GetAllClassMetadata())
        {
            sessionFactory.EvictEntity(classMetadata.Key);
        }

        sessionFactory.EvictQueries();
    }
}