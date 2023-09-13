using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using TrueOrFalse.Frontend.Web1.Middlewares;
using TrueOrFalse.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacCoreModule>();
    });
builder.Services.AddDistributedMemoryCache();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
}); 

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

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
builder.Services.AddAntiforgery(options =>
{

});

/* ----------------------------------------------------APP ----------------------------------------------*/
var app = builder.Build();



var env = app.Environment;

if (!env.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors("LocalhostCorsPolicy");
}

if (string.IsNullOrEmpty(env.WebRootPath))
{
    env.WebRootPath = Path.Combine(AppContext.BaseDirectory, "wwwroot");
}

app.UseStaticFiles();
var imagesPath = ImageSettings.ImageFolderPath();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/Images"
});
app.UseRouting();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "apiVue/{controller}/{action}/{id?}");
});



app.UseDeveloperExceptionPage();
app.UseMiddleware<RequestTimingForStaticFilesMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<SessionStartMiddleware>();
app.Urls.Add("http://localhost:5069");

var entityCacheInitilizer = app.Services.GetRequiredService<EntityCacheInitializer>();
entityCacheInitilizer.Init();
app.Run();

