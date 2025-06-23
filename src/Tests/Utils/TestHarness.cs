using Autofac;
using Autofac.Extensions.DependencyInjection;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
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
using ContainerBuilder = DotNet.Testcontainers.Builders.ContainerBuilder;
using IContainer = DotNet.Testcontainers.Containers.IContainer;

/// <summary>
/// Test harness for integration tests with scenario-image support.
/// A scenario image is a saved database state that lets tests start faster.
/// </summary>
public sealed class TestHarness : IAsyncDisposable, IDisposable
{
    // --------------------------------------------------------------------
    // Private fields
    // --------------------------------------------------------------------
    private readonly MySqlContainer _databaseContainer;
    private readonly IContainer _meilisearchContainer;

    private ProgramWebApplicationFactory? _webApplicationFactory;
    private HttpClient? _httpClient;
    private ILifetimeScope? _lifetimeScope;

    // Fake hosting dependencies required for testing environment
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Configuration options
    private readonly bool _enablePerformanceLogging;
    private readonly string? _dumpTagToLoad;
    private Stopwatch? _stopwatch;

    private bool _isDisposed;

    // --------------------------------------------------------------------
    // Public surface (unchanged!)
    // --------------------------------------------------------------------
    /// <summary>
    /// HTTP client for making API requests to the test application
    /// </summary>
    public HttpClient Client =>
        _httpClient ?? throw new InvalidOperationException("Call InitAsync() first.");

    /// <summary>
    /// Direct database access for test setup and verification
    /// </summary>
    public RawDbDataLoader DbData = null!;

    /// <summary>
    /// Direct search index access for test setup and verification
    /// </summary>
    public RawMeilisearchDataLoader SearchData = null!;

    // API wrapper helpers for common operations
    public LearningSessionStoreApiWrapper ApiLearningSessionStore = null!;
    public AnswerBodyApiWrapper ApiAnswerBody = null!;
    public LearningSessionResultApiWrapper ApiLearningSessionResult = null!;

    /// <summary>
    /// Connection string for the test MySQL database
    /// </summary>
    public string ConnectionString => _databaseContainer.GetConnectionString();

    /// <summary>
    /// URL for the test Meilisearch instance
    /// </summary>
    public string MeilisearchUrl => $"http://localhost:{_meilisearchContainer.GetMappedPublicPort(7700)}";

    /// <summary>
    /// Master key for test Meilisearch authentication
    /// </summary>
    public static string MeilisearchMasterKey => "meilisearch-test-key";

    /// <summary>
    /// Resolve service from the DI container
    /// </summary>
    public T Resolve<T>() where T : notnull => _lifetimeScope!.Resolve<T>();

    /// <summary>
    /// Short alias for Resolve method
    /// </summary>
    public T R<T>() where T : notnull => Resolve<T>();

    // --------------------------------------------------------------------
    // Factory helpers (names preserved)
    // --------------------------------------------------------------------
    /// <summary>
    /// Creates a new test harness instance with optional performance logging and database dumps
    /// </summary>
    public static async Task<TestHarness> CreateAsync(
        bool enablePerfLogging = false,
        string? prebuiltDbImage = null,
        string? dumpTagToLoad = null)
    {
        // Load prebuilt database image if specified
        if (!string.IsNullOrWhiteSpace(prebuiltDbImage))
        {
            await DockerUtilities.LoadDockerImageAsync(prebuiltDbImage);
        }

        var harness = new TestHarness(enablePerfLogging, prebuiltDbImage, dumpTagToLoad);
        await harness.InitAsync();
        return harness;
    }

    /// <summary>
    /// Creates a test harness with the tiny scenario dataset pre-loaded
    /// </summary>
    public static Task<TestHarness> CreateWithTinyScenario(bool enablePerfLogging = false) =>
        CreateAsync(enablePerfLogging, dumpTagToLoad: ScenarioDumpConstants.TagTiny);

    // --------------------------------------------------------------------
    // Constructor
    // --------------------------------------------------------------------
    private TestHarness(bool enablePerfLogging, string? prebuiltDbImage, string? dumpTagToLoad)
    {
        _enablePerformanceLogging = enablePerfLogging;
        _dumpTagToLoad = dumpTagToLoad;
        _stopwatch = Stopwatch.StartNew();

        // ------------------------------------------------------------
        // MySQL container configuration
        // ------------------------------------------------------------
        var containerName = string.IsNullOrWhiteSpace(prebuiltDbImage)
            ? "memowikis-mysql-test"
            : "memowikis-mysql-prebuilt";

        // Remove any existing container with the same name
        CleanupExistingContainer(containerName);

        _databaseContainer = new MySqlBuilder()
            .WithImage(prebuiltDbImage ?? "mysql:8.3.0")
            .WithName(containerName)
            .WithUsername(TestConstants.MySqlUsername)
            .WithPassword(TestConstants.MySqlPassword)
            .WithDatabase(TestConstants.TestDbName)
            // Use case-insensitive table names for Windows compatibility
            .WithCommand("mysqld", "--lower_case_table_names=1")
            .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilCommandIsCompleted("mysqladmin", "ping", "-h", "localhost", "-u",
                        TestConstants.MySqlUsername, $"-p{TestConstants.MySqlPassword}")
                    .UntilPortIsAvailable(3306))
            // Enable container reuse for better performance unless using prebuilt image
            .WithReuse(string.IsNullOrWhiteSpace(prebuiltDbImage))
            .Build();

        // ------------------------------------------------------------
        // Meilisearch container configuration
        // ------------------------------------------------------------
        _meilisearchContainer = new ContainerBuilder()
            .WithImage("getmeili/meilisearch:v1.5")
            .WithPortBinding(7778, 7700)
            .WithEnvironment("MEILI_MASTER_KEY", MeilisearchMasterKey)
            .WithEnvironment("MEILI_NO_ANALYTICS", "true")
            .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(r => r.ForPath("/health").ForPort(7700)))
            .WithReuse(true)
            .Build();

        LogPerf("Container configuration finished");

        // ------------------------------------------------------------
        // Setup fake hosting environment for testing
        // ------------------------------------------------------------
        _webHostEnvironment = A.Fake<IWebHostEnvironment>();
        A.CallTo(() => _webHostEnvironment.EnvironmentName).Returns("Test");

        // Setup fake HTTP context with session containing test user ID
        _httpContextAccessor = A.Fake<IHttpContextAccessor>();
        var fakeHttpContext = A.Fake<HttpContext>();
        var fakeSession = A.Fake<ISession>();

        A.CallTo(() => _httpContextAccessor.HttpContext).Returns(fakeHttpContext);
        A.CallTo(() => fakeHttpContext.Session).Returns(fakeSession);

        // Configure session to return user ID 1 (test user)
        var userIdBytes = BitConverter.GetBytes(1);
        if (BitConverter.IsLittleEndian) Array.Reverse(userIdBytes);
        A.CallTo(() => fakeSession.TryGetValue("userId", out userIdBytes)).Returns(true);

        LogPerf("Constructor completed");
    }

    // --------------------------------------------------------------------
    // Initialization
    // --------------------------------------------------------------------
    /// <summary>
    /// Initialize the test harness by starting containers and setting up the application
    /// </summary>
    public async Task InitAsync(bool keepData = false)
    {
        _stopwatch = Stopwatch.StartNew();

        if (!keepData)
        {
            Settings.Initialize(new ConfigurationManager());

            // Start database and search containers in parallel for better performance
            var dbTask = Task.Run(async () =>
            {
                await _databaseContainer.StartAsync();
                LogPerf("MySQL container started");

                // Load scenario dump if specified, otherwise build fresh schema
                if (!string.IsNullOrWhiteSpace(_dumpTagToLoad))
                {
                    await ScenarioDumpManager.LoadDumpAsync(_dumpTagToLoad);
                    LogPerf($"Database dump '{_dumpTagToLoad}' loaded");
                }
                else
                {
                    await new SchemaBuilder(LogPerf).Init(ConnectionString);
                }
            });

            var searchTask = Task.Run(async () =>
            {
                await _meilisearchContainer.StartAsync();
                LogPerf("Meilisearch container started");
            });

            await Task.WhenAll(dbTask, searchTask);
            await ClearMeilisearchIndices();
        }

        // ------------------------------------------------------------
        // Setup web application factory for API testing
        // ------------------------------------------------------------
        _webApplicationFactory =
            new ProgramWebApplicationFactory(_webHostEnvironment, _httpContextAccessor, ConnectionString);

        _httpClient = _webApplicationFactory.CreateClient();

        // Configure global settings for Meilisearch
        Settings.MeilisearchUrl = MeilisearchUrl;
        Settings.MeilisearchMasterKey = MeilisearchMasterKey;

        // Initialize dependency injection scope
        var rootScope = _webApplicationFactory.Services.GetAutofacRoot();
        _lifetimeScope = rootScope.BeginLifetimeScope();

        LogPerf("WebApplicationFactory + Autofac root created");

        // Run initialization code required by legacy parts of the system
        await RunLegacyInitializersAsync();

        // ------------------------------------------------------------
        // Initialize data loaders and API wrappers
        // ------------------------------------------------------------
        DbData = new RawDbDataLoader(ConnectionString);
        SearchData = new RawMeilisearchDataLoader(MeilisearchUrl, MeilisearchMasterKey);

        ApiLearningSessionStore = new LearningSessionStoreApiWrapper(this);
        ApiAnswerBody = new AnswerBodyApiWrapper(this);
        ApiLearningSessionResult = new LearningSessionResultApiWrapper(this);
    }

    /// <summary>
    /// Reset Meilisearch indices to clean state for testing
    /// </summary>
    private async Task ClearMeilisearchIndices()
    {
        var client = new MeilisearchClient(MeilisearchUrl, MeilisearchMasterKey);

        // Delete existing indices
        var deleteTaskIds = new[]
        {
            (await client.DeleteIndexAsync(MeilisearchIndices.Pages)).TaskUid,
            (await client.DeleteIndexAsync(MeilisearchIndices.Questions)).TaskUid,
            (await client.DeleteIndexAsync(MeilisearchIndices.Users)).TaskUid
        };

        // Wait for all deletions to complete
        foreach (var taskId in deleteTaskIds)
        {
            await client.WaitForTaskAsync(taskId);
        }

        // Recreate indices with fresh configuration
        await client.CreateIndexAsync(MeilisearchIndices.Pages);
        await client.CreateIndexAsync(MeilisearchIndices.Questions);
        await client.CreateIndexAsync(MeilisearchIndices.Users);

        // Configure filterable attributes for search functionality
        await client.Index(MeilisearchIndices.Pages).UpdateFilterableAttributesAsync(["Language"]);
        await client.Index(MeilisearchIndices.Questions).UpdateFilterableAttributesAsync(["Language"]);
        await client.Index(MeilisearchIndices.Users).UpdateFilterableAttributesAsync(["ContentLanguages"]);
    }

    /// <summary>
    /// Initialize legacy components that require special setup
    /// </summary>
    private async Task RunLegacyInitializersAsync()
    {
        _stopwatch = Stopwatch.StartNew();

        // Create necessary directory structure for image uploads
        ImageDirectoryCreator.CreateImageDirectories(_webHostEnvironment.ContentRootPath);
        LogPerf("Image directories created");

        // Initialize entity caching system
        _lifetimeScope!.Resolve<EntityCacheInitializer>().Init(" (started in unit test) ");
        LogPerf("EntityCache initialized");

        // Reset date/time handling and prepare test user session
        DateTimeX.ResetOffset();
        SetSessionUserInDatabase();
        LogPerf("DateTime and session user prepared");

        // Initialize background job scheduler
        await JobScheduler.InitializeAsync();
        LogPerf("JobScheduler initialized");
    }

    /// <summary>
    /// Create a test user in the database for session handling
    /// </summary>
    private void SetSessionUserInDatabase()
    {
        ContextUser
            .New(_lifetimeScope!.Resolve<UserWritingRepo>())
            .Add(new User { Id = 1, Name = "SessionUser" })
            .Persist();
    }

    // --------------------------------------------------------------------
    // REST helpers (names preserved)
    // --------------------------------------------------------------------
    /// <summary>
    /// Perform GET request and return formatted response
    /// </summary>
    public async Task<string> ApiGet([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        var response = await Client.GetAsync(uri);
        return await FormatHttpResponse(response);
    }

    /// <summary>
    /// Perform GET request and deserialize response to specified type
    /// </summary>
    public async Task<TResult> ApiGet<TResult>([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        var response = await Client.GetAsync(uri);
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TResult>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    /// <summary>
    /// Perform POST request with JSON body and return deserialized response
    /// </summary>
    public async Task<T> ApiPost<T>([StringSyntax(StringSyntaxAttribute.Uri)] string uri, object body)
    {
        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var response = await Client.PostAsync(uri, content);
        var formatted = await FormatHttpResponse(response);

        return JsonSerializer.Deserialize<T>(formatted,
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
               ?? throw new InvalidOperationException($"Cannot deserialize to {typeof(T).Name}");
    }

    /// <summary>
    /// Format HTTP response for better readability in test output
    /// </summary>
    private static async Task<string> FormatHttpResponse(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}\n{json}");
        }

        var parsed = Newtonsoft.Json.Linq.JToken.Parse(json);
        return parsed.ToString(Newtonsoft.Json.Formatting.Indented);
    }

    /// <summary>
    /// Enhanced POST helper providing detailed error information.
    /// </summary>
    public async Task<TResult> ApiPostJson<TRequest, TResult>(string endpoint, TRequest requestBody)
    {
        var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var response = await Client.PostAsync(endpoint,
            new StringContent(json, Encoding.UTF8, "application/json"));

        var responseJson = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TResult>(responseJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    /// <summary>POST helper that returns the raw response for negative-case tests.</summary>
    public async Task<HttpResponseMessage> ApiCall<TRequest>(string endpoint, TRequest requestBody)
    {
        var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return await Client.PostAsync(endpoint,
            new StringContent(json, Encoding.UTF8, "application/json"));
    }

    // --------------------------------------------------------------------
    // Verification helpers (names preserved)
    // --------------------------------------------------------------------
    /// <summary>
    /// Standard data structure for verifying page-related test outcomes
    /// </summary>
    public record struct DefaultPageVerificationData(
        List<Dictionary<string, object?>> DbPages,
        IList<PageCacheItem> EntityCachePages,
        List<SearchPageItem> SearchPages,
        List<Dictionary<string, object?>>? DbRelations = null,
        IList<PageRelationCache>? EntityCacheRelations = null);

    /// <summary>
    /// Collect comprehensive page data from all sources for test verification
    /// </summary>
    public async Task<DefaultPageVerificationData> GetDefaultPageVerificationDataAsync(
        bool includeRelations = true, int delayForSearch = 100)
    {
        var dbPages = await DbData.AllPagesAsync();

        // Allow time for asynchronous search indexing to complete
        await Task.Delay(delayForSearch);

        var searchPages = (await SearchData.GetAllPages())
            .OrderBy(p => p.Id).ToList();

        if (includeRelations)
        {
            var dbRelations = await DbData.AllPageRelationsAsync();
            return new DefaultPageVerificationData(
                dbPages,
                EntityCache.GetAllPagesList(),
                searchPages,
                dbRelations,
                EntityCache.GetAllRelations());
        }

        return new DefaultPageVerificationData(
            dbPages,
            EntityCache.GetAllPagesList(),
            searchPages);
    }

    // --------------------------------------------------------------------
    // Disposal pattern (interface unchanged)
    // --------------------------------------------------------------------
    /// <summary>
    /// Cleanup all resources used by the test harness
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_isDisposed) return;

        // Clear global singletons and caches
        JobScheduler.Clear();
        EntityCache.Clear();

        // Dispose dependency injection scope
        _lifetimeScope?.Dispose();
        _webApplicationFactory?.Dispose();
        _httpClient?.Dispose();

        // Stop and dispose containers
        await _meilisearchContainer.DisposeAsync();
        await _databaseContainer.DisposeAsync();

        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Synchronous disposal wrapper
    /// </summary>
    public void Dispose() => DisposeAsync().AsTask().GetAwaiter().GetResult();

    // --------------------------------------------------------------------
    // Helper utilities
    // --------------------------------------------------------------------
    /// <summary>
    /// Log performance metrics if enabled
    /// </summary>
    [MemberNotNull(nameof(_stopwatch))]
    private void LogPerf(string message)
    {
        if (!_enablePerformanceLogging) return;

        Console.WriteLine(
            $"[PERF {DateTime.Now:HH:mm:ss.fff}] {message} ({_stopwatch!.ElapsedMilliseconds:N0} ms)");
        _stopwatch.Restart();
    }

    /// <summary>
    /// Remove any existing Docker container with the specified name
    /// </summary>
    private static void CleanupExistingContainer(string containerName)
    {
        try
        {
            var info = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"rm -f {containerName}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(info);
            process?.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cleaning up '{containerName}': {ex.Message}");
        }
    }

    // --------------------------------------------------------------------
    // Nested WebApplicationFactory
    // --------------------------------------------------------------------
    /// <summary>
    /// Custom web application factory for configuring the test environment
    /// </summary>
    private sealed class ProgramWebApplicationFactory(
        IWebHostEnvironment fakeEnv,
        IHttpContextAccessor fakeHttpCtx,
        string connectionString)
        : WebApplicationFactory<Program>
    {
        /// <summary>
        /// Configure the web host for testing with custom connection string
        /// </summary>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureAppConfiguration((_, cfg) =>
            {
                cfg.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["General:ConnectionString"] = connectionString
                });
            });
        }

        /// <summary>
        /// Configure dependency injection with fake hosting dependencies
        /// </summary>
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.ConfigureContainer<Autofac.ContainerBuilder>(b =>
            {
                b.RegisterInstance(fakeEnv).As<IWebHostEnvironment>().SingleInstance();
                b.RegisterInstance(fakeHttpCtx).As<IHttpContextAccessor>().SingleInstance();
            });
            return base.CreateHost(builder);
        }
    }
}
