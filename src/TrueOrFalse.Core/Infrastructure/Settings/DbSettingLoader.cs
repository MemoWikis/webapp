using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Settings;

namespace TrueOrFalse.Core.Infrastructure
{
    public class DbSettingsLoader : IRegisterAsInstancePerLifetime
    {
        private readonly SettingStorage _settingStorage;

        public DbSettingsLoader(SettingStorage settingStorage){
            _settingStorage = settingStorage;
        }

        public DbSettings Get()
        {
            return new DbSettings(_settingStorage.GetAllCached());
        }

        public void Update(DbSettings dbSettings)
        {
            _settingStorage.Update(dbSettings.SettingList);
        }
    }
}
