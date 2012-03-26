using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Settings;

namespace TrueOrFalse.Core.Infrastructure
{
    public class DbSettings : SettingBag
    {
        public static readonly string TypeName = SettingType.From(typeof(DbSettings));

        public SettingInteger AppVersion { get { return Get<SettingInteger>("0"); } }

        public DbSettings(IList<Setting> settings)
            : base(settings, TypeName)
        {
        }
    }
}
