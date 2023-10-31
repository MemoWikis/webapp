using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using TrueOrFalse.Frontend.Web1.Middlewares;
using TrueOrFalse.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using TrueOrFalse.Environment;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 1073741824;
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";  
    options.InstanceName = "SessionInstace";
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(480);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacCoreModule>();
    });
//builder.Services.AddDistributedMemoryCache();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
});



builder.Services.AddHttpContextAccessor();

builder.Services.Configure<FormOptions>(options =>
{
    options.MemoryBufferThreshold = Int32.MaxValue;
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.BufferBodyLengthLimit = int.MaxValue;
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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie();

builder.Services.AddAntiforgery(_ => { });

builder.WebHost.ConfigureServices(services =>
{
    WebHostEnvironmentProvider.Initialize(services.BuildServiceProvider());
});
var app = builder.Build();
var env = app.Environment;
App.Environment = env;


if (!env.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors("LocalhostCorsPolicy");
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (string.IsNullOrEmpty(env.WebRootPath))
{
    env.WebRootPath = Path.Combine(AppContext.BaseDirectory, "wwwroot");
}

var imagesPath = Settings.ImagePath;
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/Images"
});

app.UseRouting();
app.UseSession();
app.UseMiddleware<RequestTimingForStaticFilesMiddleware>();
app.UseMiddleware<SessionStartMiddleware>();

app.UseEndpoints(endpoints => endpoints.MapControllerRoute(
    name: "default",
    pattern: "apiVue/{controller}/{action}/{id?}"));


app.UseDeveloperExceptionPage();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.Urls.Add("http://*:5069");

var entityCacheInitilizer = app.Services.GetRequiredService<EntityCacheInitializer>();
entityCacheInitilizer.Init();
app.Run();