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
using Testcontainers.MySql;
using ContainerBuilder = Autofac.ContainerBuilder;
using IContainer = DotNet.Testcontainers.Containers.IContainer;

/// <summary>
/// Test harness for integration tests with support for scenario images.
/// 
/// Scenario images allow you to save and reuse database states for faster test execution.
/// Use the various CreateWithScenarioImageAsync methods to work with scenario images:
/// </summary>
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
    private readonly IHttpContextAccessor _httpCtxAcc; private readonly bool _enablePerfLogging;
    private readonly bool _preserveContainerForScenarioImage;
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

        var harness = new TestHarness(enablePerfLogging, prebuiltDbImage, preserveContainerForScenarioImage: false);
        await harness.InitAsync();
        return harness;
    }

    public static async Task<TestHarness> CreateForScenarioBuilding(bool enablePerfLogging = false)
    {
        var harness = new TestHarness(enablePerfLogging, prebuiltDbImage: null, preserveContainerForScenarioImage: true);
        await harness.InitAsync();
        return harness;
    }

    public static async Task<TestHarness> CreateWithTinyScenario(bool enablePerfLogging = false)
        => await CreateWithScenarioImageAsync(ScenarioImageConstants.TagMicro, enablePerfLogging);


    /// <summary>Creates a new TestHarness using a specific scenario image tag.</summary>
    private static async Task<TestHarness> CreateWithScenarioImageAsync(string scenarioTag, bool enablePerfLogging)
    {
        var scenarioImageName = $"{ScenarioImageConstants.BaseName}:{scenarioTag}";
        return await CreateAsync(enablePerfLogging, scenarioImageName);
    }
    private TestHarness(bool enablePerfLogging, string? prebuiltDbImage, bool preserveContainerForScenarioImage = false)
    {
        _enablePerfLogging = enablePerfLogging;
        _preserveContainerForScenarioImage = preserveContainerForScenarioImage;
        _stopwatch = Stopwatch.StartNew();

        // Determine container name based on whether a prebuilt image is used
        string containerName;
        bool useReuse;
        if (!string.IsNullOrEmpty(prebuiltDbImage))
        {
            // For prebuilt images, use a specific name and clean up previous containers
            containerName = "memowikis-mysql-prebuilt";
            CleanupExistingContainer(containerName);
            useReuse = false; // Don't reuse when using prebuilt images
        }
        else if (preserveContainerForScenarioImage)
        {
            // For scenario building, use a specific name but keep the container
            containerName = "memowikis-mysql-scenario";
            CleanupExistingContainer(containerName);
            useReuse = false; // Don't reuse, but don't auto-dispose either
        }
        else
        {
            // For regular tests, use a fixed name since containers are auto-cleaned up
            containerName = "memowikis-mysql-test";
            useReuse = false; // Don't reuse, allow automatic cleanup
        }

        _db = new MySqlBuilder()
            .WithImage(prebuiltDbImage ?? "mysql:8.3.0")
            .WithName(containerName)
            .WithUsername("test")
            .WithPassword("P@ssw0rd_#123")
            .WithDatabase(TestDbName)
            .WithCommand(
                "mysqld",
                "--lower_case_table_names=1"
            ).WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole()).WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilCommandIsCompleted("mysqladmin", "ping", "-h", "localhost", "-u", "test", "-pP@ssw0rd_#123")
                    .UntilPortIsAvailable(3306))
            .WithReuse(useReuse)
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

        // Only dispose the database container if we're not preserving it for scenario image building
        if (!_preserveContainerForScenarioImage)
        {
            await _db.DisposeAsync();
        }

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

    public async Task<string> ApiCall([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        var httpResponse = await this.Client.GetAsync(uri);
        var jsonContent = await httpResponse.Content.ReadAsStringAsync();

        var parsedJson = Newtonsoft.Json.Linq.JToken.Parse(jsonContent);
        var formattedJson = parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);
        return formattedJson;
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

    private static void CleanupExistingContainer(string containerName)
    {
        try
        {
            // Use docker command to stop and remove existing container
            var processInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"rm -f {containerName}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            if (process != null)
            {
                process.WaitForExit();
                // Ignore exit code - container might not exist
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cleaning up existing container '{containerName}': {ex.Message}");
        }
    }
}
