using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;


public static class WebHostEnvironmentProvider
{
    private static IServiceProvider _serviceProvider;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static IWebHostEnvironment GetWebHostEnvironment()
    {
        return _serviceProvider.GetRequiredService<IWebHostEnvironment>();
    }
}