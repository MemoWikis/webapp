using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TrueOrFalse.View.Web
{
    public class Redis
    {
        public static void Configuration(IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =
                    "localhost:6379";
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(Settings.SessionStateTimeoutInMin);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        public static void UsingRedisSession(WebApplication app)
        {
            app.UseSession();
        }
    }
}