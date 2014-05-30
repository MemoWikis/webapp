using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NHibernate.Linq;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Updates
{
    public class SolrCoreReload
    {
        private static string _solrUrl;
        private static string _solrPath;

        private static readonly string[] _coreNames = { "Category", "Question", "Set", "User" };

        public static void ReloadCategory() { RefreshSchema("Category"); }
        public static void ReloadQuestion() { RefreshSchema("Question"); }
        public static void ReloadSet() { RefreshSchema("Set"); }
        public static void ReloadUser() { RefreshSchema("User"); }

        static SolrCoreReload()
        {
            if (HttpContext.Current == null)
                return;

            _solrUrl = WebConfigSettings.SolrUrl;
            _solrPath = WebConfigSettings.SolrPath;
        }

        public static void Set(string url, string path)
        {
            _solrUrl = url;
            _solrPath = path;
        }

        public static void ReloadAllSchemas(bool testSchemas = false)
        {
            _coreNames.ForEach(c => CopySchema(c + (testSchemas ? "Test" : "")));
            _coreNames.ForEach(c => ReoladCore(c + (testSchemas ? "Test" : "")));
        }

        public static void ReloadAllConfigs(bool testSchemas = false)
        {
            _coreNames.ForEach(c => CopyConfig(c + (testSchemas ? "Test" : "")));
            _coreNames.ForEach(c => ReoladCore(c + (testSchemas ? "Test" : "")));
        }

        public static void RefreshSchema(string coreName){
            CopySchema(coreName);
            ReoladCore(coreName);
        }

        public static void RefreshConfigs(string coreName){
            CopyConfig(coreName);
            ReoladCore(coreName);
        }

        private static void CopySchema(string coreName)
        {
            File.Copy(
                PathTo.SolrSchema("schema" + coreName + ".xml"),
                _solrPath + "/tof" + coreName + "/conf/" + "schema.xml",
                overwrite:true
            );
        }

        private static void CopyConfig(string coreName)
        {
            File.Copy(
                PathTo.SolrSchema("solrconfig" + coreName + ".xml"),
                _solrPath + "/tof" + coreName + "/conf/" + "solrconfig.xml",
                overwrite: true
            );            
        }

        private static void ReoladCore(string coreName)
        {
            var url = _solrUrl + "admin/cores?wt=json&action=RELOAD&core=tof" + coreName;
            WebRequest.Create(url).GetResponse();
        }
    }
}
