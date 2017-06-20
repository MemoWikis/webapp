using System;

namespace Seedworks.Lib.Settings
{
    [Serializable]
    public class SettingInteger : Setting
    {
        public SettingInteger()
            : this(null)
        {
        }

        public SettingInteger(string key)
            : this(key, 0)
        {
        }

        public SettingInteger(string key, int defaultValue)
            : base(key, defaultValue)
        {
        }

        public virtual int Value
        {
            get { return Convert.ToInt32(ValueStr); }
            set { ValueStr = value.ToString(); }
        }

        public override bool IsDefault()
        {
            return _default.Equals(Value);
        }

        public override Setting Clone()
        {
            return Clone<SettingInteger>();
        }
    }
}
