using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Scalar.AspNetCore;
using TrueOrFalse.Environment;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Updates;
using TrueOrFalse.Utilities.ScheduledJobs;
using TrueOrFalse.View.Web.Middlewares;
using static System.Int32;

try
{

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, _, log) =>
    {
        log.Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console()
            .WriteTo.Seq(Settings.SeqUrl);

        log.MinimumLevel.Is(
            ctx.HostingEnvironment.IsDevelopment()
                ? Serilog.Events.LogEventLevel.Information
                : Serilog.Events.LogEventLevel.Error);
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
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition =
                System.Text.Json.Serialization.JsonIgnoreCondition.Never;
        });

    builder.Services.AddEndpointsApiExplorer();

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
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
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
    app.UseMiddleware<RequestTimingMiddleware>();

    app.MapControllerRoute(
        name: "default", 
        pattern: "apiVue/{controller}/{action}/{id?}"
        ).WithOpenApi();
    
    app.MapHealthChecks("healthcheck_backend");

    app.Urls.Add("http://*:5069");

    var entityCacheInitializer = app.Services.GetRequiredService<EntityCacheInitializer>();
    entityCacheInitializer.Init();

    await JobScheduler.InitializeAsync();

    app.Run();

}
catch (Exception e)
{
    Logg.r.Fatal(e, "Fatal error");
    throw;
}