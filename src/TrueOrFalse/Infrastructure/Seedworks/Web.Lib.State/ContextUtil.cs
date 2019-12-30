using System;
using System.IO;
using System.Reflection;
using System.Web;

namespace Seedworks.Web.State
{
    public static class ContextUtil
    {
        public static bool UseWebConfig => Settings.UseWebConfig;

        public static bool IsLocal
        {
            get
            {
                if (!IsWebContext) // helps unit testing
                    return false;

                return HttpContext.Current.Request.IsLocal;
            }
        }

        public static bool IsWebContext => HttpContext.Current != null;    


        public static string GetFilePath(string fileName) 
        {
            if (IsWebContext)
                return HttpContext.Current.Server.MapPath($@"~/{fileName}");
            
            if (UseWebConfig)
                return Path.Combine(new DirectoryInfo(AssemblyDirectory).Parent.FullName, fileName);
            
            return Path.Combine(new DirectoryInfo(AssemblyDirectory).Parent.Parent.FullName, fileName);
        }

        private static string AssemblyDirectory
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
