using Autofac;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ISession = NHibernate.ISession;


[TestFixture]
public abstract class BaseTest
{
    private IContainer _container;
    protected ILifetimeScope LifetimeScope { get; private set; }

    /// <summary>
    /// The “session user” we seed into the fake HTTP session & DB.
    /// </summary>
    protected static User SessionUser { get; } = new User
    {
        Name = "SessionUser",
        Id = 1
    };

    protected static User _sessionUser = SessionUser;

    [SetUp]
    public async Task SetUp()
    {
        // 1) Build + wire up a fresh container for *this* test:
        BuildContainer();

        // 2) Reset NHibernate + DB, then open one scope:
        InitializeScope();

        // 3) Start any background jobs
        await JobScheduler.InitializeAsync();
    }

    [TearDown]
    public void TearDown()
    {
        // 1) Flush NHibernate work
        LifetimeScope.Resolve<ISession>().Flush();

        // 2) Dispose in reverse order:
        LifetimeScope.Dispose();
        _container.Dispose();

        // 3) Clear any static/singleton state
        JobScheduler.Clear();
        EntityCache.Clear();
        DateTimeX.ResetOffset();
    }

    /// <summary>
    /// Exactly like your old method—tear down & rebuild everything mid-test,
    /// so you can hit your DB + EntityCache initialization logic again.
    /// </summary>
    public async Task RecycleContainerAndEntityCache()
    {
        // flush & dispose
        LifetimeScope.Resolve<ISession>().Flush();
        LifetimeScope.Dispose();
        _container.Dispose();

        // rebuild
        BuildContainer();
        InitializeScope();
        await JobScheduler.InitializeAsync();

        // re-clear any static bits
        JobScheduler.Clear();
        EntityCache.Clear();
        DateTimeX.ResetOffset();
    }

    /// <summary>
    /// Helper alias so your existing tests can still call R<T>().
    /// </summary>
    protected T R<T>() where T : notnull
        => LifetimeScope.Resolve<T>();

    #region —–– Private Helpers –––—

    private void BuildContainer()
    {
        _container = AutofacWebInitializer.GetTestContainer(
            CreateWebHostEnvironment(),
            CreateHttpContextAccessor());
        ServiceLocator.Init(_container);
    }

    private void InitializeScope()
    {
        // rebuild NHibernate config & clear all tables
        SessionFactory.BuildTestConfiguration();
        SessionFactory.TruncateAllTables();

        // open a fresh scope
        LifetimeScope = _container.BeginLifetimeScope();

        // re-create any on-disk directories your tests expect
        ImageDirectoryCreator.CreateImageDirectories(App.Environment.ContentRootPath);

        // seed EntityCache, session-user, etc.
        var initializer = LifetimeScope.Resolve<EntityCacheInitializer>();
        initializer.Init(" (started in unit test)");

        // reset any date-offset overrides
        DateTimeX.ResetOffset();

        // persist our fake SessionUser into the DB
        ContextUser
            .New(LifetimeScope.Resolve<UserWritingRepo>())
            .Add(SessionUser)
            .Persist();
    }

    private IWebHostEnvironment CreateWebHostEnvironment()
    {
        var fakeEnv = A.Fake<IWebHostEnvironment>();
        App.Environment = fakeEnv;
        A.CallTo(() => fakeEnv.EnvironmentName)
         .Returns("TestEnvironment");
        return fakeEnv;
    }

    private IHttpContextAccessor CreateHttpContextAccessor()
    {
        // fake the accessor and HttpContext
        var accessor = A.Fake<IHttpContextAccessor>();
        var context = A.Fake<HttpContext>();
        // IMPORTANT: fully qualify the Core session, not NHibernate.ISession
        var session = A.Fake<Microsoft.AspNetCore.Http.ISession>();

        A.CallTo(() => accessor.HttpContext).Returns(context);
        A.CallTo(() => context.Session).Returns(session);

        // prepare the bytes we want TryGetValue to output
        byte[] userIdBytes = BitConverter.GetBytes(SessionUser.Id);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(userIdBytes);

        // set up the fake so TryGetValue returns true *and* writes out our bytes
        A.CallTo(() => session.TryGetValue("userId", out userIdBytes)).Returns(true);

        return accessor;
    }


    #endregion
}
