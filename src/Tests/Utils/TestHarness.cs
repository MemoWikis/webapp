using Autofac;
using Autofac.Extensions.DependencyInjection;
using DotNet.Testcontainers.Builders;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.MySql;
using ContainerBuilder = Autofac.ContainerBuilder;

public sealed class TestHarness : IAsyncDisposable, IDisposable
{
    private readonly MySqlContainer _db = new MySqlBuilder()
        .WithImage("mysql:8.3.0")
        .WithUsername("test")
        .WithPassword("P@ssw0rd_#123")
        .WithDatabase("memoWikisTest")
        .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
        .WithWaitStrategy(
            Wait.ForUnixContainer()
                .UntilPortIsAvailable(3306))
        .WithReuse(true)
        .Build();


    private ProgramWebApplicationFactory? _factory;
    private HttpClient? _client;

    // Autofac scope coming from the running host (for Resolve<T> convenience)
    private ILifetimeScope? _scope;

    public HttpClient Client => _client ?? throw new InvalidOperationException("Call InitAsync() first");
    public string ConnectionString => _db.GetConnectionString();

    private readonly IWebHostEnvironment _webHostEnv;
    private readonly IHttpContextAccessor _httpCtxAcc;

    public TestHarness()
    {
        // Prepare environment fake
        _webHostEnv = A.Fake<IWebHostEnvironment>();
        A.CallTo(() => _webHostEnv.EnvironmentName).Returns("TestEnvironment");

        // Prepare HttpContext/session fake with legacy userId=1
        _httpCtxAcc = A.Fake<IHttpContextAccessor>();
        var fakeHttpContext = A.Fake<HttpContext>();
        var fakeSession = A.Fake<ISession>();
        A.CallTo(() => _httpCtxAcc.HttpContext).Returns(fakeHttpContext);
        A.CallTo(() => fakeHttpContext.Session).Returns(fakeSession);
        byte[]? userIdBytes = BitConverter.GetBytes(1);
        if (BitConverter.IsLittleEndian) Array.Reverse(userIdBytes);
        A.CallTo(() => fakeSession.TryGetValue("userId", out userIdBytes)).Returns(true);
    }


    public T Resolve<T>() where T : notnull => _scope!.Resolve<T>();
    public T R<T>() where T : notnull => Resolve<T>();

    public async Task InitAsync()
    {
        await _db.StartAsync();

        Settings.Initialize(new ConfigurationManager());
        Settings.ConnectionString = ConnectionString;
        var configuration = SessionFactory.BuildTestConfiguration(ConnectionString);
        SessionFactory.BuildSchema();

        using var session = configuration.BuildSessionFactory().OpenSession();
        var repositoryDb = new RepositoryDb<DbSettings>(session);
        repositoryDb.Create(new DbSettings
        {
            Id = 1, 
            AppVersion = Int32.MaxValue,
        });
        

        _factory = new ProgramWebApplicationFactory(_webHostEnv, _httpCtxAcc, ConnectionString);
        _client = _factory.CreateClient();
        _scope = ((ILifetimeScope)_factory.Services).BeginLifetimeScope();

        await RunLegacyInitializersAsync();
    }

    public async ValueTask DisposeAsync()
    {
        JobScheduler.Clear();
        EntityCache.Clear();

        _scope?.Dispose();
        _factory?.Dispose();
        await _db.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    public void Dispose() => DisposeAsync().AsTask().GetAwaiter().GetResult();


    private async Task RunLegacyInitializersAsync()
    {
        ImageDirectoryCreator.CreateImageDirectories(_webHostEnv.ContentRootPath);

        Resolve<EntityCacheInitializer>().Init(" (started in unit test) ");

        DateTimeX.ResetOffset();
        SetSessionUserInDatabase();

        await JobScheduler.InitializeAsync();
    }

    private void SetSessionUserInDatabase()
    {
        ContextUser
            .New(R<UserWritingRepo>())
            .Add(new User { Id = 1, Name = "SessionUser" })
            .Persist();
    }

    private sealed class ProgramWebApplicationFactory(
        IWebHostEnvironment _fakeEnv,
        IHttpContextAccessor _fakeHttpCtx,
        string _connectionString)
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("TestEnvironment");
            builder.ConfigureAppConfiguration((_, cfg) =>
            {
                cfg.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["General:ConnectionString"] = _connectionString
                });
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.ConfigureContainer<ContainerBuilder>(b =>
            {
                b.RegisterInstance(_fakeEnv).As<IWebHostEnvironment>().SingleInstance();
                b.RegisterInstance(_fakeHttpCtx).As<IHttpContextAccessor>().SingleInstance();
            });
            return base.CreateHost(builder);
        }
    }
}
