using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Seedworks.Web.State;

namespace TrueOrFalse.Infrastructure
{
    public static class ReadOverwrittenConfig
    {
        public static ReadOverwrittenConfigValueResult ConnectionString(){
            return Value("connectionString");
        }

        public static ReadOverwrittenConfigValueResult SolrUrl(){
            return Value("sorlUrl");
        }

        internal static ReadOverwrittenConfigValueResult SolrPath(){
            return Value("pathToSolr");
        }

        private static ReadOverwrittenConfigValueResult Value(string itemName)
        {
            string filePath;
            if (ContextUtil.IsWebContext)
                filePath = HttpContext.Current.Server.MapPath(@"~/Web.overwritten.config");
            else
                filePath = Path.Combine(new DirectoryInfo(AssemblyDirectory).Parent.Parent.FullName, "App.overwritten.config");

            if (!File.Exists(filePath))
                return new ReadOverwrittenConfigValueResult(false, null);

            var xDoc = XDocument.Load(filePath);
            
            if(xDoc.Root.Element(itemName) == null)
                return new ReadOverwrittenConfigValueResult(false, null);

            var value = xDoc.Root.Element(itemName).Value;

            return new ReadOverwrittenConfigValueResult(true, value);
        }

        static private string AssemblyDirectory
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
