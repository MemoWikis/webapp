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
using System.IO;
using System.Text.Json;
using TrueOrFalse.Environment;
using TrueOrFalse.Frontend.Web.Middlewares;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Updates;
using TrueOrFalse.Utilities.ScheduledJobs;
using static System.Int32;

Console.WriteLine("Builder: Started");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 1073741824;
});
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacCoreModule>();
    });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
    });

Settings.Initialize(builder.Configuration);
Console.WriteLine("Builder: Settings Initialized");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console()
    .WriteTo.Seq(Settings.SeqUrl)
    .CreateLogger();

if (Settings.UseRedisSession)
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = Settings.RedisUrl;
    });

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

builder.WebHost.ConfigureServices(services =>
{
    WebHostEnvironmentProvider.Initialize(services.BuildServiceProvider());
});

var app = builder.Build();
Console.WriteLine("App: Start - builder.Build()");

var env = app.Environment;
App.Environment = env;

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
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

Console.WriteLine("Update: Start Run");
var update = app.Services.GetRequiredService<Update>();
update.Run();
Console.WriteLine("Update: End Run");

StripeConfiguration.ApiKey = Settings.StripeSecurityKey;

var imagesPath = Settings.ImagePath;
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/Images"
});
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseSession();
app.UseMiddleware<AutoLoginMiddleware>();

app.UseRouting();

app.UseMiddleware<RequestTimingForStaticFilesMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "apiVue/{controller}/{action}/{id?}");

    endpoints.MapHealthChecks("healthcheck_backend");
});

app.Urls.Add("http://*:5069");

var entityCacheInitializer = app.Services.GetRequiredService<EntityCacheInitializer>();
entityCacheInitializer.Init();

//var runningJobRepo = app.Services.GetRequiredService<RunningJobRepo>();
//await JobScheduler.Start(runningJobRepo);

await JobScheduler.InitializeAsync();

Console.WriteLine("App: Run");

app.Run();