using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace TrueOrFalse.Updates
{
    public class SolrCoreReload
    {
        public static event EventHandler<string> Message = delegate {};

        private static string _solrUrl;
        private static string _solrPath;
        private static string _coreSuffix { get { return Settings.SolrCoresSuffix; } }

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

            _solrUrl = Settings.SolrUrl;
            _solrPath = Settings.SolrPath;
        }

        public static void Set(string url, string path)
        {
            _solrUrl = url;
            _solrPath = path;
        }

        public static void ReloadAllSchemas(bool testSchemas = false)
        {
            _coreNames.ToList().ForEach(c => CopySchema(c + (testSchemas ? "Test" : "")));
            _coreNames.ToList().ForEach(c => ReoladCore(c + (testSchemas ? "Test" : "")));
        }

        public static void ReloadAllConfigs(bool testSchemas = false)
        {
            _coreNames.ToList().ForEach(c => CopyConfig(c + (testSchemas ? "Test" : "")));
            _coreNames.ToList().ForEach(c => ReoladCore(c + (testSchemas ? "Test" : "")));
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
                true
            );
        }

        private static void CopyConfig(string coreName)
        {
            File.Copy(
                PathTo.SolrSchema("solrconfig" + coreName.Replace("Test", "") + ".xml"),
                _solrPath + "/tof" + coreName + "/conf/" + "solrconfig.xml", true
            );            
        }

        private static void ReoladCore(string coreName)
        {
            var url = _solrUrl + "admin/cores?wt=json&action=RELOAD&core=tof" + coreName;

            try
            {
                using (var response = WebRequest.Create(url).GetResponse())
                {
                    Message(null, GetValue(response));
                }
            }
            catch (WebException ex)
            {
                var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                Message(null, "ERROR ->  Response Code " +  statusCode + "; Response" + GetValue(ex.Response));
            }
        }

        private static string GetValue(WebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
    }
}
