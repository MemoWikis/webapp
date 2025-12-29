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

    // Cookie storage for API requests
    public Dictionary<string, string> Cookies { get; } = new Dictionary<string, string>();

    // Fake hosting dependencies required for testing environment
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Configuration options
    private readonly bool _enablePerformanceLogging;
    private readonly string? _dumpTagToLoad;
    private readonly bool _skipDefaultUsers;
    private Stopwatch? _stopwatch;

    private bool _isDisposed;

    // Default test user IDs
    public int DefaultSessionUserId = 1;
    public string DefaultSessionUserPassword = "test123";
    public int DefaultTestUserId = 2;

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
    public UserLoginApiWrapper ApiUserLogin = null!;

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
    /// Waits for all pending Meilisearch indexing operations to complete.
    /// This provides a more reliable alternative to using fixed delays in tests.
    /// </summary>
    /// <param name="timeoutMs">Maximum time to wait in milliseconds</param>
    /// <returns>True if all tasks completed, false if timeout occurred</returns>
    public async Task<bool> WaitForMeilisearchIndexing(int timeoutMs = 10000)
    {
        var waiter = new MeilisearchTaskWaiter(MeilisearchUrl, MeilisearchMasterKey);
        return await waiter.WaitForAllTasksToComplete(timeoutMs);
    }

    /// <summary>
    /// Gets the current status of Meilisearch tasks for debugging purposes.
    /// </summary>
    public async Task<TaskStatusSummary> GetMeilisearchTaskStatus()
    {
        var waiter = new MeilisearchTaskWaiter(MeilisearchUrl, MeilisearchMasterKey);
        return await waiter.GetTaskStatusSummary();
    }

    /// <summary>
    /// Resolve service from the DI container
    /// </summary>
    public T Resolve<T>() where T : notnull => _lifetimeScope!.Resolve<T>();

    /// <summary>
    /// Short alias for Resolve method
    /// </summary>
    public T R<T>() where T : notnull => Resolve<T>();

    /// <summary>
    /// Create a new page context with proper sequence initialization
    /// </summary>
    public ContextPage NewPageContext(bool addContextUser = true) => new(this, addContextUser);

    // --------------------------------------------------------------------
    // Factory helpers (names preserved)
    // --------------------------------------------------------------------
    /// <summary>
    /// Creates a new test harness instance with optional performance logging and database dumps
    /// </summary>
    public static async Task<TestHarness> CreateAsync(
        bool enablePerfLogging = false,
        string? prebuiltDbImage = null,
        string? dumpTagToLoad = null,
        bool skipDefaultUsers = false)
    {
        // Load prebuilt database image if specified
        if (!string.IsNullOrWhiteSpace(prebuiltDbImage))
        {
            await DockerUtilities.LoadDockerImageAsync(prebuiltDbImage);
        }

        var harness = new TestHarness(enablePerfLogging, prebuiltDbImage, dumpTagToLoad, skipDefaultUsers);
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
    private TestHarness(bool enablePerfLogging, string? prebuiltDbImage, string? dumpTagToLoad, bool skipDefaultUsers)
    {
        _enablePerformanceLogging = enablePerfLogging;
        _dumpTagToLoad = dumpTagToLoad;
        _skipDefaultUsers = skipDefaultUsers;
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
            .WithPortBinding(TestConstants.MySqlTestPort, 3306) // Bind to a fixed port to avoid random ports
            .WithCommand("mysqld",
                "--lower_case_table_names=1") // Use case-insensitive table names for Windows compatibility
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
            .WithPortBinding(TestConstants.MeilisearchTestPort, 7700)
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

        // Setup fake HTTP context with TestSession for testing
        _httpContextAccessor = A.Fake<IHttpContextAccessor>();
        var fakeHttpContext = A.Fake<HttpContext>();
        var testSession = new TestSession();

        // Only set user ID in session if not skipping default users
        if (!_skipDefaultUsers)
        {
            testSession.SetInt32("userId", DefaultSessionUserId);
        }

        // Configure fake HTTP context
        A.CallTo(() => fakeHttpContext.Session).Returns(testSession);
        A.CallTo(() => _httpContextAccessor.HttpContext).Returns(fakeHttpContext);

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
        ApiUserLogin = new UserLoginApiWrapper(this);
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
        await client.Index(MeilisearchIndices.Pages).UpdateFilterableAttributesAsync(["Language", "CreatorName"]);
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

        // Only create default users if not skipped
        if (!_skipDefaultUsers)
        {
            // Create users individually if they don't exist to avoid duplicates
            var userRepo = _lifetimeScope!.Resolve<UserReadingRepo>();
            var sessionUserExists = userRepo.GetById(DefaultSessionUserId) != null;
            var testUserExists = userRepo.GetById(DefaultTestUserId) != null;

            if (!sessionUserExists)
                SetSessionUserInDatabase();

            if (!testUserExists)
                CreateTestUser();
        }

        // Initialize background job scheduler
        await JobScheduler.InitializeAsync();
        LogPerf("JobScheduler initialized");
    }

    /// <summary>
    /// Default session user data for tests
    /// </summary>
    public User DefaultSessionUser => new User
    {
        Id = DefaultSessionUserId,
        Name = "SessionUser",
        EmailAddress = "sessionUser@dev.test"
    };

    /// <summary>
    /// Gets the default session user from the database
    /// </summary>
    public User GetDefaultSessionUserFromDb()
    {
        var userRepo = R<UserReadingRepo>();
        return userRepo.GetById(DefaultSessionUserId) ??
               throw new InvalidOperationException("Default session user not found in database");
    }

    /// <summary>
    /// Create a test user in the database for session handling
    /// </summary>
    private void SetSessionUserInDatabase()
    {
        var testUser = DefaultSessionUser;

        // Set a simple password "test123"
        SetUserPassword.Run("test123", testUser);

        ContextUser
            .New(_lifetimeScope!.Resolve<UserWritingRepo>())
            .Add(testUser)
            .Persist();
    }

    /// <summary>
    /// Create additional test user for testing scenarios
    /// </summary>
    private void CreateTestUser()
    {
        var testUser = new User { Id = DefaultTestUserId, Name = "TestUser" };

        ContextUser
            .New(_lifetimeScope!.Resolve<UserWritingRepo>())
            .Add(testUser)
            .Persist();
    }

    // --------------------------------------------------------------------
    // REST helpers (names preserved)
    // --------------------------------------------------------------------
    /// <summary>
    /// Deserialize HTTP response with proper error handling and formatting
    /// </summary>
    private async Task<T> DeserializeHttpResponse<T>(HttpResponseMessage httpResponse)
    {
        httpResponse.EnsureSuccessStatusCode();
        var jsonContent = await httpResponse.Content.ReadAsStringAsync();
        var parsedJson = Newtonsoft.Json.Linq.JToken.Parse(jsonContent);
        var formattedJson = parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);

        if (typeof(T) == typeof(string))
            return (T)(object)formattedJson;

        var result = JsonSerializer.Deserialize<T>(formattedJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return result ?? throw new InvalidOperationException($"Failed to deserialize response to {typeof(T).Name}");
    }

    /// <summary>
    /// Perform GET request and deserialize response to specified type
    /// </summary>
    public async Task<TResult> ApiGet<TResult>([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        AddCookiesToRequest(request);
        var httpResponse = await Client.SendAsync(request);
        return await DeserializeHttpResponse<TResult>(httpResponse);
    }

    /// <summary>
    /// Perform POST request with JSON body and return deserialized response
    /// </summary>
    public async Task<T> ApiPost<T>([StringSyntax(StringSyntaxAttribute.Uri)] string uri, object body)
    {
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        // Add cookies to the request
        using var request = new HttpRequestMessage(HttpMethod.Post, uri) { Content = jsonContent };

        AddCookiesToRequest(request);
        var httpResponse = await Client.SendAsync(request);
        return await DeserializeHttpResponse<T>(httpResponse);
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
        var json = JsonSerializer.Serialize(requestBody,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        AddCookiesToRequest(request);
        var response = await Client.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TResult>(responseJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    /// <summary>POST helper that returns the raw response for negative-case tests.</summary>
    public async Task<HttpResponseMessage> ApiCall<TRequest>(string endpoint, TRequest requestBody)
    {
        var json = JsonSerializer.Serialize(requestBody,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        AddCookiesToRequest(request);
        return await Client.SendAsync(request);
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

        // Use Meilisearch task waiter instead of fixed delay for more reliable testing
        var indexingCompleted = await WaitForMeilisearchIndexing(timeoutMs: 15000);
        if (!indexingCompleted)
        {
            var taskStatus = await GetMeilisearchTaskStatus();
            throw new TimeoutException(
                $"Meilisearch indexing did not complete within timeout in GetDefaultPageVerificationDataAsync. " +
                $"Pending tasks: {taskStatus.HasPendingTasks}, " +
                $"Failed tasks: {taskStatus.Failed}");
        }

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
        if (!_enablePerformanceLogging)
        {
            _stopwatch ??= Stopwatch.StartNew();
            return;
        }

        _stopwatch ??= Stopwatch.StartNew();
        Console.WriteLine(
            $"[PERF {DateTime.Now:HH:mm:ss.fff}] {message} ({_stopwatch.ElapsedMilliseconds:N0} ms)");
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

    // --------------------------------------------------------------------
    // Test Session Implementation
    // --------------------------------------------------------------------
    /// <summary>
    /// Simple in-memory session implementation for testing that actually stores values
    /// </summary>
    private sealed class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new();

        public bool IsAvailable => true;
        public string Id => "test-session-id";
        public IEnumerable<string> Keys => _store.Keys;

        public void Clear() => _store.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _store.Remove(key);

        public void Set(string key, byte[] value) => _store[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value!);
    }

    /// <summary>
    /// Add stored cookies to an HTTP request
    /// </summary>
    private void AddCookiesToRequest(HttpRequestMessage request)
    {
        if (Cookies.Any())
        {
            var cookieString = string.Join("; ", Cookies.Select(kv => $"{kv.Key}={kv.Value}"));
            request.Headers.Add("Cookie", cookieString);
        }
    }

    /// <summary>
    /// Add or update a cookie for subsequent HTTP requests
    /// </summary>
    public void AddOrUpdateCookie(string key, string value) => Cookies[key] = value;

    /// <summary>
    /// Remove a cookie from subsequent HTTP requests
    /// </summary>
    public void RemoveCookie(string key) => Cookies.Remove(key);

    public void MockSessionUserLoginForDI(User? creator = null)
    {
        if (creator == null)
            creator = GetDefaultSessionUserFromDb();

        var sessionUser = R<SessionUser>();
        var pageViewRepo = R<PageViewRepo>();
        sessionUser.Login(creator, pageViewRepo);
    }
}