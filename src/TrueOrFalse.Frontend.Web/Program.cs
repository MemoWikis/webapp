using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrueOrFalse.Infrastructure;

var startUp = new Startup();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacCoreModule>();
    });



startUp.ConfigureServices(builder.Services);
var app = builder.Build();
startUp.Configure(app, app.Environment);
var myRepo = app.Services.GetRequiredService<CategoryRepository>();

app.Run();