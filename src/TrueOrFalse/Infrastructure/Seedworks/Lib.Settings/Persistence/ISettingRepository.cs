using System.Collections.Generic;

namespace Seedworks.Lib.Settings
{
    public interface ISettingRepository
    {
        void Create(Setting setting);
        void Update(Setting setting);
        void CreateOrUpdate(Setting setting);
        void Delete(Setting setting);

        IList<Setting> GetAll();
        Setting GetById(int settingId);
        IList<Setting> GetBy(SettingSearchDesc searchDesc);
        Setting GetUnique(SettingSearchDesc searchDesc);
    }
}
