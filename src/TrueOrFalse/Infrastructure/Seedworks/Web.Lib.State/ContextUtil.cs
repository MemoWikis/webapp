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
            if (IsWebContext)
                return Path.Combine(_webHostEnvironment.WebRootPath, $@"~/{fileName}");

            if (UseWebConfig)
                return Path.Combine(new DirectoryInfo(AssemblyDirectory).Parent.FullName, fileName);

            return Path.Combine(new DirectoryInfo(AssemblyDirectory).Parent.Parent.FullName, fileName);
        }

        private string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }


}
