using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Environment
{
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
}
