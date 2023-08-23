using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacCoreModule>();
    });

var app = builder.Build();

app.UseMiddleware<RequestTimingForStaticFilesMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<SessionStartMiddleware>(); 

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

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var code = 500; // Default zu Internal Server Error

        // Optional: Eigene benutzerdefinierte Ausnahmen prüfen
        if (exception is YourCustomHttpException customException)
        {
            code = customException.StatusCode;
        }

        // Loggen des Fehlers
        new Logg(context.RequestServices.GetRequiredService<IHttpContextAccessor>(), context.RequestServices.GetRequiredService<IWebHostEnvironment>())
            .Error(exception);

        // Weiterleitung zu einer Fehlerseite, außer es handelt sich um einen 404-Fehler
        if (code != 404)
        {
            context.Response.Redirect($"/Fehler/{code}");
        }
    });
});
app.Run();