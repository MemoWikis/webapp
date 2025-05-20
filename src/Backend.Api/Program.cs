using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using Serilog.Events;
using Serilog.Exceptions;
using Stripe;
using System.Text.Json;
using static System.Int32;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, _, log) =>
    {
        log.Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .Enrich.WithExceptionDetails()
            .WriteTo.Console()
            .WriteTo.Seq(Settings.SeqUrl, apiKey: Settings.SeqApiKey);

        log.MinimumLevel.Is(LogEventLevel.Information);
    });

    builder.WebHost.ConfigureKestrel(serverOptions => { serverOptions.Limits.MaxRequestBodySize = 1073741824; });
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterModule<AutofacCoreModule>();
        });

    builder.Services
        .AddDistributedMemoryCache()
        .AddHttpContextAccessor()
        .AddControllersWithViews() //with views, since we need "ValidateAntiforgeryTokenAuthorizationFilter"
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition =
                System.Text.Json.Serialization.JsonIgnoreCondition.Never;
        });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHostedService<MissingHttpVerbLogger>();

    Settings.Initialize(builder.Configuration);

    if (Settings.UseRedisSession)
        builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = Settings.RedisUrl; });

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(Settings.SessionStateTimeoutInMin);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.Configure<FormOptions>(options =>
    {
        options.MemoryBufferThreshold = MaxValue;
        options.ValueLengthLimit = MaxValue;
        options.MultipartBodyLengthLimit = long.MaxValue;
        options.BufferBodyLengthLimit = MaxValue;
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("LocalhostCorsPolicy",
            builder => builder
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });

    builder.Services.AddAntiforgery(options => { options.HeaderName = "X-CSRF-TOKEN"; });

    var relativeKeysDir = Path.Combine(
        Directory.GetCurrentDirectory(),
        "DataProtectionKeys");

    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(relativeKeysDir))
        .SetApplicationName("MemoWikis.Api")
        .SetDefaultKeyLifetime(TimeSpan.FromDays(180));

    builder.Services.AddHealthChecks();
    builder.Services.AddOpenApi();


    builder.WebHost.ConfigureServices(services =>
    {
        WebHostEnvironmentProvider.Initialize(services.BuildServiceProvider());
    });


    var app = builder.Build();

    var env = app.Environment;
    App.Environment = env;

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.DefaultHttpClient =
                new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.Curl);
        });
    }

    ImageDirectoryCreator.CreateImageDirectories(env.ContentRootPath);

    if (string.IsNullOrEmpty(env.WebRootPath))
    {
        env.WebRootPath = Path.Combine(AppContext.BaseDirectory, "wwwroot");
    }

    var update = app.Services.GetRequiredService<Update>();
    update.Run();

    StripeConfiguration.ApiKey = Settings.StripeSecurityKey;

    var imagesPath = Settings.ImagePath;
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(imagesPath),
        RequestPath = "/Images"
    });

    app.UseSession();
    app.UseRouting();
    app.MapControllers();

    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseMiddleware<AutoLoginMiddleware>();

    app.MapHealthChecks("healthcheck_backend");

    app.Urls.Add("http://*:5069");

    var entityCacheInitializer = app.Services.GetRequiredService<EntityCacheInitializer>();
    entityCacheInitializer.Init();

    await JobScheduler.InitializeAsync();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Fatal error");
    throw;
}

// make Program discoverable for integration tests 
public partial class Program;
