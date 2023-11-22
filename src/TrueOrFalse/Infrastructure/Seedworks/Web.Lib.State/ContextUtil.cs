using System.IO;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Seedworks.Web.State
{
    public class ContextUtil
    {
        private readonly HttpContext? _httpContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContextUtil(HttpContext? httpContext, IWebHostEnvironment webHostEnvironment)
        {
            _httpContext = httpContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public bool UseWebConfig => Settings.UseWebConfig;

        public bool IsWebContext => _httpContext != null;

        public string GetFilePath(string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory, $@"~/{fileName}");
        }
    }


}
