using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Infrastructure
{
    public class WebConfigSettings
    {
        private static readonly AppSettingsReader _settingReader = new AppSettingsReader();

        public static string SolrUrl{
            get{
                return GetValue(OverwrittenConfig.SolrUrl(), "SolrUrl");
            }
        }

        public static string SolrPath{
            get {
                return GetValue(OverwrittenConfig.SolrPath(), "SolrPath");
            }
        }

        public static string SolrCoresSuffix{
            get{
                return GetValue(OverwrittenConfig.SolrCoresSuffix(), "SolrCoresSuffix");
            }
        }

        public static bool GoogleKeyIsSet = false;
        public static string GoogleKey = "";

        private static string GetValue(OverwrittenConfigValueResult overwrittenConfigValueResult, string configKey)
        {
            if (overwrittenConfigValueResult.HasValue)
                return overwrittenConfigValueResult.Value;

            return Get<string>(configKey);
        }

        private static T Get<T>(string settingKey){
            return (T)_settingReader.GetValue(settingKey, typeof(T));
        }

        static WebConfigSettings()
        {
            GoogleKey = Get<string>("GoogleAnalyticsKey");
            GoogleKeyIsSet = !String.IsNullOrEmpty(GoogleKey);
        }
    }
}   
