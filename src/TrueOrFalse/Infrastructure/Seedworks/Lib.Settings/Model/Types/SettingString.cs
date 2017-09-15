using System;

namespace Seedworks.Lib.Settings
{
    [Serializable]
    public class SettingString : Setting
    {
        public SettingString()
            : this(null)
        {
        }

        public SettingString(string key)
            : this(key, string.Empty)
        {
        }

        public SettingString(string key, string defaultValue)
            : base(key, defaultValue)
        {
        }

        public virtual string Value
        {
            get { return ValueStr; }
            set { ValueStr = value; }
        }

        public override bool IsDefault()
        {
            return _default.Equals(Value);
        }

        public override Setting Clone()
        {
            return Clone<SettingString>();
        }
    }
}
