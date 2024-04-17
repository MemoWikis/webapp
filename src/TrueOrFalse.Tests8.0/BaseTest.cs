using Autofac;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Seedworks.Web.State;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;
using ISession = NHibernate.ISession;

[TestFixture]
public class BaseTest
{
    private static IContainer _container;
    protected ILifetimeScope LifetimeScope;
    private static User _sessionUser => new User
    {
        Name = "SessionUser",
        Id = 1
    };
    static BaseTest()
    {
        #if DEBUG
        //            NHibernateProfiler.Initialize();
        #endif
    }

    [SetUp]
    public void SetUp()
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
        var initializer = Resolve<EntityCacheInitializer>();
        initializer.Init(" (started in unit test) ");
        DateTimeX.ResetOffset();
        SetSessionUserInDatabase(_sessionUser);
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
    }

    public void RecycleContainerAndEntityCache()
    {
        EntityCache.Clear();
        Resolve<SessionData>().Clear();

        RecycleContainer();

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
        _container = AutofacWebInitializer.GetTestContainer(SetWebHostEnvironment(), SetHttpContextAccessor());
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
            Array.Reverse(userIdBytes);
        }

        A.CallTo(() => session.TryGetValue("userId", out userIdBytes)).Returns(true);
    }

    private static void SetSessionUserInDatabase(User user)
    {
        ContextUser.New(R<UserWritingRepo>())
            .Add(_sessionUser)
            .Persist();
    }

    public static T Resolve<T>() where T : notnull => _container.Resolve<T>();
    public static T R<T>() where T : notnull => _container.Resolve<T>();
}
