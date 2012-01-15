using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Seedworks.Web.State;

namespace TrueOrFalse.Core.Infrastructure
{
    public static class ReadOverwrittenConnectionString
    {
        public static ReadOverwrittenConnectionStringResult Run()
        {
            string filePath; 
            if (ContextUtil.IsWebContext)
                filePath = HttpContext.Current.Server.MapPath(@"~/Web.overwritten.config");
            else
                filePath = Path.Combine(AssemblyDirectory, "App.overwritten.config");
            
            if (!File.Exists(filePath))
                return new ReadOverwrittenConnectionStringResult(false, null);

            var xDoc = XDocument.Load(filePath);
            var value = xDoc.Root.Element("connectionString").Value;

            return new ReadOverwrittenConnectionStringResult(true, value);
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
