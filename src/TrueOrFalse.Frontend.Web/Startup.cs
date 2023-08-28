using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Frontend.Web1.Middlewares;

internal class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();
       

        services.AddCors(options =>
        {
            options.AddPolicy("LocalhostCorsPolicy",
                builder => builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        services.AddAntiforgery(options =>
        {
            
        });

        services.AddAuthorizationCore();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCors("LocalhostCorsPolicy");
        }

        app.UseMiddleware<RequestTimingForStaticFilesMiddleware>();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseMiddleware<SessionStartMiddleware>();
    }
}

