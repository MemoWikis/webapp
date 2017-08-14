using System;

namespace Seedworks.Lib.Settings
{
    [Serializable]
    public class SettingBoolean : Setting
    {
        public SettingBoolean()
            : this(null)
        {
        }

        public SettingBoolean(string key)
            : this(key, false)
        {
        }

        public SettingBoolean(string key, bool defaultValue)
            : base(key, defaultValue)
        {
        }

        public virtual bool Value
        {
            get { return bool.Parse(ValueStr); }
            set { ValueStr = value.ToString(); }
        }

        public override bool IsDefault()
        {
            return _default.Equals(Value);
        }

        public virtual bool IsTrue()
        {
            return Value;
        }

        public override Setting Clone()
        {
            return Clone<SettingBoolean>();
        }
    }
}
