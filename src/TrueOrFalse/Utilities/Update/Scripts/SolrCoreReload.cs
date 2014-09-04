using System.IO;
using System.Net;
using System.Web;
using NHibernate.Linq;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Updates
{
    public class SolrCoreReload
    {
        private static string _solrUrl;
        private static string _solrPath;
        private static string _coreSuffix { get { return WebConfigSettings.SolrCoresSuffix; } }

        private static readonly string[] _coreNames = { 
                "Category" + _coreSuffix, 
                "Question" + _coreSuffix, 
                "Set" + _coreSuffix, 
                "User" + _coreSuffix 
        };

        public static void ReloadCategory() { RefreshSchema("Category" + _coreSuffix); }
        public static void ReloadQuestion() { RefreshSchema("Question" + _coreSuffix); }
        public static void ReloadSet() { RefreshSchema("Set" + _coreSuffix); }
        public static void ReloadUser() { RefreshSchema("User" + _coreSuffix); }

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
                PathTo.SolrSchema("schema" + coreName.Replace("Test", "") + ".xml"),
                _solrPath + "/tof" + coreName + "/conf/" + "schema.xml",
                overwrite:true
            );
        }

        private static void CopyConfig(string coreName)
        {
            File.Copy(
                PathTo.SolrSchema("solrconfig" + coreName.Replace("Test", "") + ".xml"),
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
