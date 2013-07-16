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

        public static string EmailDefaultFrom { get { return Get<string>("EmailDefaultFrom"); } }
        
        private static T Get<T>(string settingKey){
            return (T)_settingReader.GetValue(settingKey, typeof(T));
        }
    }
}   
