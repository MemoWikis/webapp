using System.Diagnostics;
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
    private const string TestDbName = "memoWikisTest";

    private readonly MySqlContainer _db = new MySqlBuilder()
        .WithImage("mysql:8.3.0")
        .WithUsername("test")
        .WithPassword("P@ssw0rd_#123")
        .WithDatabase(TestDbName)
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

    private readonly bool _enablePerfLogging;

    private void PerfLog(string msg)
    {
        if (_enablePerfLogging)
        {
            Console.WriteLine($"[PERF {DateTime.Now:HH:mm:ss.fff}] {msg}");
        }
    }

    public TestHarness(bool enablePerfLogging = false)
    {
        _enablePerfLogging = enablePerfLogging;
        var sw = Stopwatch.StartNew();

        // Prepare environment fake
        _webHostEnv = A.Fake<IWebHostEnvironment>();
        A.CallTo(() => _webHostEnv.EnvironmentName).Returns("TestEnvironment");

        // Prepare HttpContext/session fake with legacy userId=1
        _httpCtxAcc = A.Fake<IHttpContextAccessor>();
        var fakeHttpContext = A.Fake<HttpContext>();
        var fakeSession = A.Fake<ISession>();
        A.CallTo(() => _httpCtxAcc.HttpContext).Returns(fakeHttpContext);
        A.CallTo(() => fakeHttpContext.Session).Returns(fakeSession);
        var userIdBytes = BitConverter.GetBytes(1);
        if (BitConverter.IsLittleEndian) Array.Reverse(userIdBytes);
        A.CallTo(() => fakeSession.TryGetValue("userId", out userIdBytes)).Returns(true);

        PerfLog($"ctor finished in {sw.ElapsedMilliseconds:N0} ms");
    }

    public T Resolve<T>() where T : notnull => _scope!.Resolve<T>();
    public T R<T>() where T : notnull => Resolve<T>();


    public async Task InitAsync()
    {
        var sw = Stopwatch.StartNew();

        await _db.StartAsync();
        PerfLog($"MySql container started ({sw.ElapsedMilliseconds:N0} ms)");
        sw.Restart();

        Settings.Initialize(new ConfigurationManager());
        Settings.ConnectionString = ConnectionString;
        var configuration = SessionFactory.BuildTestConfiguration(ConnectionString);
        SessionFactory.BuildSchema();
        PerfLog($"NHibernate bootstrap ({sw.ElapsedMilliseconds:N0} ms)");
        sw.Restart();

        using var session = configuration.BuildSessionFactory().OpenSession();
        var repositoryDb = new RepositoryDb<DbSettings>(session);
        repositoryDb.Create(new DbSettings
        {
            Id = 1,
            AppVersion = int.MaxValue,
        });

        PerfLog($"Seed initial DB data ({sw.ElapsedMilliseconds:N0} ms)");
        sw.Restart();

        _factory = new ProgramWebApplicationFactory(_webHostEnv, _httpCtxAcc, ConnectionString);
        _client = _factory.CreateClient();
        var rootScope = _factory.Services.GetAutofacRoot();
        _scope = rootScope.BeginLifetimeScope();
        PerfLog($"WebApplicationFactory + Autofac root ({sw.ElapsedMilliseconds:N0} ms)");
        sw.Restart();

        await RunLegacyInitializersAsync();
        PerfLog($"Legacy initializers ({sw.ElapsedMilliseconds:N0} ms)");
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
        var sw = Stopwatch.StartNew();

        ImageDirectoryCreator.CreateImageDirectories(_webHostEnv.ContentRootPath);
        PerfLog($"Created image directories ({sw.ElapsedMilliseconds:N0} ms)");
        sw.Restart();

        Resolve<EntityCacheInitializer>().Init(" (started in unit test) ");
        PerfLog($"EntityCache init ({sw.ElapsedMilliseconds:N0} ms)");
        sw.Restart();

        DateTimeX.ResetOffset();
        SetSessionUserInDatabase();
        PerfLog($"DateTime+SessionUser ({sw.ElapsedMilliseconds:N0} ms)");
        sw.Restart();

        await JobScheduler.InitializeAsync();
        PerfLog($"JobScheduler init ({sw.ElapsedMilliseconds:N0} ms)");
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
