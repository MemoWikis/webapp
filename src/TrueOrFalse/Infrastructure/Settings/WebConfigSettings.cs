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

        public static string SolrUrl;
        public static string SolrPath;
        public static string SolrCoresSuffix;
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
            SolrCoresSuffix = GetValue(OverwrittenConfig.SolrCoresSuffix(), "SolrCoresSuffix");
            SolrPath = GetValue(OverwrittenConfig.SolrPath(), "SolrPath");
            SolrUrl = GetValue(OverwrittenConfig.SolrUrl(), "SolrUrl");
        }
    }
}   
