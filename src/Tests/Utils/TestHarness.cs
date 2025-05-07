using Autofac;
using Autofac.Extensions.DependencyInjection;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.MySql;

public sealed class TestHarness : IAsyncDisposable, IDisposable
{
    private readonly MySqlContainer _db;
    private IHost? _host;
    private IContainer? _container;
    private ILifetimeScope? _scope;

    public HttpClient Client => _host!.GetTestClient();
    public string ConnectionString => _db.GetConnectionString();
    private IWebHostEnvironment _webHostEnv = null!;
    private IHttpContextAccessor _httpCtxAcc = null!;

    public TestHarness()
    {
        _db = new MySqlBuilder()
            .WithImage("mysql:8.4")
            .WithUsername("test")
            .WithPassword("P@ssw0rd_#123")
            .WithDatabase("appdb")
            .Build();
    }


    public T Resolve<T>() where T : notnull => _scope!.Resolve<T>();
    public T R<T>() where T : notnull => Resolve<T>();   // alias


    public async Task InitAsync()
    {
        await _db.StartAsync();
        BuildAutofacContainer();
        await StartWebHostAsync();
        await RunLegacyInitializersAsync();
    }


    public async ValueTask DisposeAsync()
    {
        JobScheduler.Clear();
        EntityCache.Clear();

        _scope?.Dispose();
        _container?.Dispose();

        if (_host is not null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        await _db.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    public void Dispose() => DisposeAsync().AsTask().GetAwaiter().GetResult();


    private void BuildAutofacContainer()
    {
        _webHostEnv = A.Fake<IWebHostEnvironment>();
        A.CallTo(() => _webHostEnv.EnvironmentName).Returns("TestEnvironment");

        _httpCtxAcc = A.Fake<IHttpContextAccessor>();
        var fakeHttpContext = A.Fake<HttpContext>();
        var fakeSession = A.Fake<ISession>();
        A.CallTo(() => _httpCtxAcc.HttpContext).Returns(fakeHttpContext);
        A.CallTo(() => fakeHttpContext.Session).Returns(fakeSession);

        // Provide 'userId' in session (old logic)
        byte[]? userIdBytes = BitConverter.GetBytes(1);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(userIdBytes);
        A.CallTo(() => fakeSession.TryGetValue("userId", out userIdBytes)).Returns(true);

        // Build container exactly like before
        _container = AutofacWebInitializer.GetTestContainer(_webHostEnv, _httpCtxAcc);
        ServiceLocator.Init(_container);
        _scope = _container.BeginLifetimeScope();
    }

    private async Task StartWebHostAsync()
    {
        _host = await Task.Run(() => Host.CreateDefaultBuilder()
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                // hand over existing registrations
                foreach (var reg in _container!.ComponentRegistry.Registrations)
                    builder.RegisterComponent(reg);
            })
            .ConfigureWebHostDefaults(web =>
            {
                web.UseTestServer();
                web.UseStartup<Program>(); // your real Startup/Program
            })
            .ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Default"] = ConnectionString
                });
            })
            .Start());


        var hostRootScope = (ILifetimeScope)_host!.Services;

        // Dispose the old _scope that was created directly from _container,
        // as it might not see all services registered by the host (e.g., from Program.cs or TestServer).
        _scope?.Dispose();

        // Create the new _scope for test resolutions as a child of the host's root scope.
        // This ensures it can resolve all services available to the running application.
        _scope = hostRootScope.BeginLifetimeScope(builder =>
        {
            // Test-specific overrides or registrations for this scope can be added to the builder here if needed.
            // For example: builder.RegisterInstance(A.Fake<IMyService>()).As<IMyService>();
        });
    }

    private async Task RunLegacyInitializersAsync()
    {
        // NHibernate now knows container connection string
        SessionFactory.BuildTestConfiguration(ConnectionString);
        SessionFactory.TruncateAllTables();

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
}
