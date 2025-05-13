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
using MySql.Data.MySqlClient;
using NHibernate.Cfg;
using Testcontainers.MySql;
using ContainerBuilder = Autofac.ContainerBuilder;
using IContainer = DotNet.Testcontainers.Containers.IContainer;

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

    private readonly IContainer _meiliSearch = new DotNet.Testcontainers.Builders.ContainerBuilder()
        .WithImage("getmeili/meilisearch:v1.5")
        .WithPortBinding(7778, 7700)
        .WithEnvironment("MEILI_MASTER_KEY", "meilisearch-test-key")
        .WithEnvironment("MEILI_NO_ANALYTICS", "true")
        .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
        .WithWaitStrategy(
            Wait.ForUnixContainer()
                .UntilHttpRequestIsSucceeded(r => r.ForPath("/health").ForPort(7700)))
        .WithReuse(true)
        .Build();

    private ProgramWebApplicationFactory? _factory;
    private HttpClient? _client;

    // Autofac scope coming from the running host (for Resolve<T> convenience)
    private ILifetimeScope? _scope;

    public HttpClient Client => _client ?? throw new InvalidOperationException("Call InitAsync() first");

    public RawDbDataLoader DbData;
    public RawMeilisearchDataLoader SearchData;

    public string ConnectionString => _db.GetConnectionString();
    public string MeilisearchUrl => $"http://localhost:{_meiliSearch.GetMappedPublicPort(7700)}";
    public string MeilisearchMasterKey => "meilisearch-test-key";

    private readonly IWebHostEnvironment _webHostEnv;
    private readonly IHttpContextAccessor _httpCtxAcc;

    private readonly bool _enablePerfLogging;
    private Stopwatch? _stopwatch;

    private void PerfLog(string message)
    {
        if (_stopwatch == null)
            return;

        if (_enablePerfLogging)
        {
            Console.WriteLine($"[PERF {DateTime.Now:HH:mm:ss.fff}] {message} ({_stopwatch.ElapsedMilliseconds:N0} ms)");
        }

        _stopwatch.Restart();
    }

    public TestHarness(bool enablePerfLogging = false)
    {
        _enablePerfLogging = enablePerfLogging;
        _stopwatch = Stopwatch.StartNew();

        // Prepare environment fake
        _webHostEnv = A.Fake<IWebHostEnvironment>();
        A.CallTo(() => _webHostEnv.EnvironmentName).Returns("Test");

        // Prepare HttpContext/session fake with legacy userId=1
        _httpCtxAcc = A.Fake<IHttpContextAccessor>();
        var fakeHttpContext = A.Fake<HttpContext>();
        var fakeSession = A.Fake<ISession>();
        A.CallTo(() => _httpCtxAcc.HttpContext).Returns(fakeHttpContext);
        A.CallTo(() => fakeHttpContext.Session).Returns(fakeSession);
        var userIdBytes = BitConverter.GetBytes(1);
        if (BitConverter.IsLittleEndian) Array.Reverse(userIdBytes);
        A.CallTo(() => fakeSession.TryGetValue("userId", out userIdBytes)).Returns(true);

        PerfLog($"ctor finished in {_stopwatch.ElapsedMilliseconds:N0} ms");
    }

    public T Resolve<T>() where T : notnull => _scope!.Resolve<T>();
    public T R<T>() where T : notnull => Resolve<T>();

    public ContextPage NewPageContext(bool addContextUser = true) => new(this, addContextUser);

    public async Task InitAsync(bool keepData = false)
    {
        _stopwatch = Stopwatch.StartNew();

        if (!keepData)
        {
            Settings.Initialize(new ConfigurationManager());

            await _db.StartAsync();
            PerfLog("MySql container started");

            await InitDbSchema();

            await _meiliSearch.StartAsync();
            PerfLog("MeiliSearch container started");
        }
        
        _factory = new ProgramWebApplicationFactory(_webHostEnv, _httpCtxAcc, ConnectionString);
        _client = _factory.CreateClient();

        Settings.MeilisearchUrl = MeilisearchUrl;
        Settings.MeilisearchMasterKey = MeilisearchMasterKey;

        var rootScope = _factory.Services.GetAutofacRoot();
        _scope = rootScope.BeginLifetimeScope();
        PerfLog("WebApplicationFactory + Autofac root");

        await RunLegacyInitializersAsync();
        PerfLog("Legacy initializers");

        DbData = new RawDbDataLoader(ConnectionString);
        SearchData = new RawMeilisearchDataLoader(MeilisearchUrl, MeilisearchMasterKey);
    }

    private async Task InitDbSchema()
    {
        Settings.ConnectionString = ConnectionString;
        var configuration = SessionFactory.BuildTestConfiguration(ConnectionString);
        PerfLog("NHibernate bootstrap: Init");

        if (!await SchemaExistsAsync())
        {
            SessionFactory.BuildSchema();
            PerfLog("NHibernate bootstrap: Build schema");
        }
        else
        {
            SessionFactory.TruncateAllTables();
            PerfLog("NHibernate bootstrap: Truncate All Tables");
        }

        CreateAppVersionSetting(configuration);
    }

    private void CreateAppVersionSetting(Configuration configuration)
    {
        using var session = configuration.BuildSessionFactory().OpenSession();
        var repositoryDb = new RepositoryDb<DbSettings>(session);
        repositoryDb.Create(new DbSettings
        {
            Id = 1,
            AppVersion = int.MaxValue,
        });

        PerfLog("Seed initial DB data");
    }

    public async ValueTask DisposeAsync()
    {
        JobScheduler.Clear();
        EntityCache.Clear();

        _scope?.Dispose();
        _factory?.Dispose();
        await _db.DisposeAsync();
        await _meiliSearch.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    public void Dispose() => DisposeAsync().AsTask().GetAwaiter().GetResult();

    private async Task RunLegacyInitializersAsync()
    {
        _stopwatch = Stopwatch.StartNew();

        ImageDirectoryCreator.CreateImageDirectories(_webHostEnv.ContentRootPath);
        PerfLog("Created image directories");

        Resolve<EntityCacheInitializer>().Init(" (started in unit test) ");
        PerfLog("EntityCache init");

        DateTimeX.ResetOffset();
        SetSessionUserInDatabase();
        PerfLog("DateTime+SessionUser");

        await JobScheduler.InitializeAsync();
        PerfLog("JobScheduler init");
    }

    private void SetSessionUserInDatabase()
    {
        ContextUser
            .New(R<UserWritingRepo>())
            .Add(new User { Id = 1, Name = "SessionUser" })
            .Persist();
    }

    private static async Task<bool> SchemaExistsAsync()
    {
        await using var conn = new MySqlConnection(Settings.ConnectionString);
        await conn.OpenAsync();

        const string sql =
            """
               SELECT COUNT(*)
               FROM information_schema.tables
               WHERE table_schema = DATABASE()
                 AND table_name   = 'User'
            """;

        await using var cmd = new MySqlCommand(sql, conn);
        var count = (long)(await cmd.ExecuteScalarAsync())!;
        return count > 0;
    }

    private sealed class ProgramWebApplicationFactory(
        IWebHostEnvironment _fakeEnv,
        IHttpContextAccessor _fakeHttpCtx,
        string _connectionString)
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
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
