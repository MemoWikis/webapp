using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Utilities.ScheduledJobs;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacCoreModule>();
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Application Stop
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
var webHostEnvironment = app.Services.GetRequiredService<IWebHostEnvironment>(); 

lifetime.ApplicationStopping.Register(() =>
{
    JobScheduler.Shutdown();
    new Logg(httpContextAccessor, webHostEnvironment).r().Information("=== Application Stop ===============================");
});

app.Run();