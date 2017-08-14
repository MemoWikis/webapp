using System;

namespace Seedworks.Lib.Settings
{
    /// <summary>
    /// Convenience class for generating strings used as <see cref="SettingType"/>.
    /// </summary>
    [Serializable]
    public class SettingType
    {
        /// <summary>
        /// Always returns the same string for a given object/type to use as 
        /// SettingType when going to the DB.
        /// </summary>
        public static string From(object obj)
        {
            return From(obj.GetType());
        }

        /// <summary>
        /// Always returns the same string for a given object/type to use as 
        /// SettingType when going to the DB.
        /// </summary>
        public static string From(Type type)
        {
            return From(type.Name);
        }

        /// <summary>
        /// Always returns the same string for a given object/type/string to use as 
        /// SettingType when going to the DB.
        /// </summary>
        public static string From(string settingTypeString)
        {
            return settingTypeString;
        }
    }
}
