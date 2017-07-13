using System;
using System.Collections.Generic;
using System.Linq;

namespace Seedworks.Lib.Settings
{
    /// <summary>
    /// Base class for all setting lists for specific entities.
    /// </summary>
    [Serializable]
    public class SettingBag
    {
        private readonly IList<Setting> _settings;
        private readonly string _settingTypeName;
        private readonly int _settingGroupId;

        public IList<Setting> SettingList { get { return _settings; } }
        public string SettingTypeName { get { return _settingTypeName; } }
        public int SettingGroupId { get { return _settingGroupId; } }

        public SettingBag(IList<Setting> settings, string settingTypeName, int settingGroupId = 0)
        {
            _settings = settings;
            _settingTypeName = settingTypeName;
            _settingGroupId = settingGroupId;
        }

        /// <summary>
        /// Gets an existing Setting with the given key from the internal list or creates a new one, adds it to the list and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : Setting, new()
        {
            var setting = _settings.ToList().Find(s => s.Key.Equals(key));

            if (setting != null)
                return (T)setting;

            setting = new T { Key = key, SettingType = _settingTypeName, SettingTypeId = _settingGroupId };

            _settings.Add(setting);

            return (T)setting;
        }

        /// <summary>
        /// Gets an existing Setting with the given key from the internal list or creates a new one, adds it to the list and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Get<T>(string key, string defaultValue) where T : Setting, new()
        {
            var setting = _settings.ToList().Find(s => s.Key.Equals(key));

            if (setting != null)
                return (T)setting;

            setting = new T { Key = key, ValueStr = defaultValue, SettingType = _settingTypeName, SettingTypeId = _settingGroupId };

            _settings.Add(setting);

            return (T)setting;
        }

        /// <summary>
        /// Gets an existing Setting with the given key from the internal list or null if no such setting exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T TryGet<T>(string key) where T : Setting
        {
            var setting = _settings.ToList().Find(s => s.Key.Equals(key));

            if (setting != null)
                return (T)setting;

            return null;
        }
    }
}
