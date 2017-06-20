using System;
using Seedworks.Lib.Persistence;

namespace Seedworks.Lib.Settings
{
    [Serializable]
    public class Setting : ICloneable, IPersistable
    {
        protected object _default;

        public virtual int Id { get; set; }

        public virtual string Key { get; set; }
        public virtual string ValueStr { get; set; }

        /// <summary>
        /// The entity type (e.g. Company) to which this Setting belongs.
        /// </summary>
        public virtual string SettingType { get; set; }

        /// <summary>
        /// The ID of the entity to which this setting belongs.
        /// </summary>
        public virtual int SettingTypeId { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime DateModified { get; set; }

        /// <summary>
        /// This c'tor is used by generic methods to create a new Setting.
        /// Set the key to null for early detection of misuse.
        /// </summary>
        public Setting() : this(null)
        {
        }

        public Setting(string key) : this(key, String.Empty)
        {
        }

        public Setting(string key, object defaultValue)
        {
            Key = key;
            _default = defaultValue ?? String.Empty;
            ValueStr = _default.ToString();
        }

        public virtual bool IsDefault()
        {
            return _default.Equals(ValueStr);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public virtual Setting Clone()
        {
            return Clone<Setting>();
        }

        protected T Clone<T>() where T : Setting, new()
        {
            return new T
                       {
                           _default = _default,
                           DateCreated = DateCreated,
                           Id = Id,
                           Key = Key,
                           DateModified = DateModified,
                           SettingType = SettingType,
                           SettingTypeId = SettingTypeId,
                           ValueStr = ValueStr
                       };
        }
    }
}
