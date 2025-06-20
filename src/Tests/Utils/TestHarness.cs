using Autofac;
using Autofac.Extensions.DependencyInjection;
using DotNet.Testcontainers.Builders;
using FakeItEasy;
using Meilisearch;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using Testcontainers.MySql;
using ContainerBuilder = Autofac.ContainerBuilder;
using IContainer = DotNet.Testcontainers.Containers.IContainer;

public sealed class TestHarness : IAsyncDisposable, IDisposable
{
    private const string TestDbName = "memoWikisTest";

    private readonly MySqlContainer _db;

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

    public RawDbDataLoader DbData = null!;
    public RawMeilisearchDataLoader SearchData = null!;

    public string ConnectionString => _db.GetConnectionString();
    public string MeilisearchUrl => $"http://localhost:{_meiliSearch.GetMappedPublicPort(7700)}";
    public static string MeilisearchMasterKey => "meilisearch-test-key";

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

    public static async Task<TestHarness> CreateAsync(bool enablePerfLogging = false, string? prebuiltDbImage = null)
    {
        if (!string.IsNullOrEmpty(prebuiltDbImage))
        {
            await DockerUtilities.LoadDockerImageAsync(prebuiltDbImage);
        }

        var harness = new TestHarness(enablePerfLogging, prebuiltDbImage);
        await harness.InitAsync();
        return harness;
    }

    // Modified constructor (now private and synchronous)
    private TestHarness(bool enablePerfLogging, string? prebuiltDbImage)
    {
        _enablePerfLogging = enablePerfLogging;
        _stopwatch = Stopwatch.StartNew();

        _db = new MySqlBuilder()
            .WithImage(prebuiltDbImage ?? "mysql:8.3.0")
            .WithName(ScenarioContainer.Name)
            .WithLabel(ScenarioContainer.Label, ScenarioContainer.Label)
            .WithUsername("test")
            .WithPassword("P@ssw0rd_#123")
            .WithDatabase(TestDbName)
            .WithCommand(
                "mysqld",
                "--lower_case_table_names=1"
            )
            .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilPortIsAvailable(3306))
            .WithReuse(true)
            .Build();

        PerfLog($"Container Startup");

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
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(userIdBytes);
        }

        A.CallTo(() => fakeSession.TryGetValue("userId", out userIdBytes)).Returns(true);

        PerfLog($"ctor end");
    }

    public T Resolve<T>() where T : notnull => _scope!.Resolve<T>();
    public T R<T>() where T : notnull => Resolve<T>();

    public async Task InitAsync(bool keepData = false)
    {
        _stopwatch = Stopwatch.StartNew();

        if (!keepData)
        {
            Settings.Initialize(new ConfigurationManager());

            var dbTask = Task.Run(async () =>
            {
                await _db.StartAsync();
                PerfLog("MySql container started");
                await new SchemaBuilder(PerfLog).Init(ConnectionString);
            });

            var searchTask = Task.Run(async () =>
            {
                await _meiliSearch.StartAsync();
                PerfLog("MeiliSearch container started");
            });

            await Task.WhenAll(dbTask, searchTask);
            await ClearMeilisearchIndices();
        }

        _factory = new ProgramWebApplicationFactory(_webHostEnv, _httpCtxAcc, ConnectionString);
        _client = _factory.CreateClient();

        Settings.MeilisearchUrl = MeilisearchUrl;
        Settings.MeilisearchMasterKey = MeilisearchMasterKey;

        var rootScope = _factory.Services.GetAutofacRoot();
        _scope = rootScope.BeginLifetimeScope();
        PerfLog("WebApplicationFactory + Autofac root");

        await InitializersMoreAsync();
        PerfLog("Legacy initializers");

        DbData = new RawDbDataLoader(ConnectionString);
        SearchData = new RawMeilisearchDataLoader(MeilisearchUrl, MeilisearchMasterKey);
    }

    private async Task ClearMeilisearchIndices()
    {
        var client = new MeilisearchClient(MeilisearchUrl, MeilisearchMasterKey);

        // Delete all indices used in the application
        var deletePageTaskId = (await client.DeleteIndexAsync(MeilisearchIndices.Pages)).TaskUid;
        var deleteQuestionTaskId = (await client.DeleteIndexAsync(MeilisearchIndices.Questions)).TaskUid;
        var deleteUserTaskId = (await client.DeleteIndexAsync(MeilisearchIndices.Users)).TaskUid;

        // Wait for all deletion tasks to complete
        await client.WaitForTaskAsync(deletePageTaskId);
        await client.WaitForTaskAsync(deleteQuestionTaskId);
        await client.WaitForTaskAsync(deleteUserTaskId);

        // Recreate the indices
        await client.CreateIndexAsync(MeilisearchIndices.Pages);
        await client.CreateIndexAsync(MeilisearchIndices.Questions);
        await client.CreateIndexAsync(MeilisearchIndices.Users);

        // Initialize filterable attributes for indices
        var pagesIndex = client.Index(MeilisearchIndices.Pages);
        var questionsIndex = client.Index(MeilisearchIndices.Questions);
        var usersIndex = client.Index(MeilisearchIndices.Users);

        await pagesIndex.UpdateFilterableAttributesAsync(["Language"]);
        await questionsIndex.UpdateFilterableAttributesAsync(["Language"]);
        await usersIndex.UpdateFilterableAttributesAsync(["ContentLanguages"]);
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

    private async Task InitializersMoreAsync()
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

    private async Task<string> FormatHttpResponse(HttpResponseMessage httpResponse)
    {
        httpResponse.EnsureSuccessStatusCode();
        var jsonContent = await httpResponse.Content.ReadAsStringAsync();
        var parsedJson = Newtonsoft.Json.Linq.JToken.Parse(jsonContent);
        var formattedJson = parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);
        return formattedJson;
    }

    public async Task<string> ApiCall([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        var httpResponse = await this.Client.GetAsync(uri);
        return await FormatHttpResponse(httpResponse);
    }

    public async Task<string> ApiPost([StringSyntax(StringSyntaxAttribute.Uri)] string uri, object body)
    {
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        var httpResponse = await this.Client.PostAsync(uri, jsonContent);
        return await FormatHttpResponse(httpResponse);
    }

    public async Task<T> ApiPost<T>([StringSyntax(StringSyntaxAttribute.Uri)] string uri, object body)
    {
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        var httpResponse = await this.Client.PostAsync(uri, jsonContent);
        var responseContent = await FormatHttpResponse(httpResponse);
        var result = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return result ?? throw new InvalidOperationException($"Failed to deserialize response to {typeof(T).Name}");
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

    public record struct DefaultPageVerificationData(
        List<Dictionary<string, object?>> DbPages,
        IList<PageCacheItem> EntityCachePages,
        List<SearchPageItem> SearchPages,
        List<Dictionary<string, object?>>? DbRelations = null,
        IList<PageRelationCache>? EntityCacheRelations = null);

    public async Task<DefaultPageVerificationData> GetDefaultPageVerificationDataAsync(bool includeRelations = true, int delayForSearch = 100)
    {
        var dbPages = await DbData.AllPagesAsync();

        // delay for search to ensure data is indexed, since Meilisearch indexing is asynchronous
        await Task.Delay(delayForSearch);
        // needs to be ordered by Id for consistent results
        var searchPages = (await SearchData.GetAllPages()).OrderBy(page => page.Id).ToList();

        if (includeRelations)
        {
            var dbRelations = await DbData.AllPageRelationsAsync();
            return new DefaultPageVerificationData
            {
                DbPages = dbPages,
                EntityCachePages = EntityCache.GetAllPagesList(),
                SearchPages = searchPages,
                DbRelations = dbRelations,
                EntityCacheRelations = EntityCache.GetAllRelations()
            };
        }

        return new DefaultPageVerificationData
        {
            DbPages = dbPages,
            EntityCachePages = EntityCache.GetAllPagesList(),
            SearchPages = searchPages
        };
    }
}
