using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core.Infrastructure
{
    public class WebConfigSettings
    {
        private static readonly AppSettingsReader _settingReader = new AppSettingsReader();

        public static string SmtpServer { get { return Get<string>("SmtpServer"); } }
        public static string SmtpUser { get { return Get<string>("SmtpUser"); } }
        public static string SmtpPass { get { return Get<string>("SmtpPassword"); } }

        public static string EmailDefaultFrom { get { return Get<string>("EmailDefaultFrom"); } }
        
        private static T Get<T>(string settingKey){
            return (T)_settingReader.GetValue(settingKey, typeof(T));
        }
    }
}   
