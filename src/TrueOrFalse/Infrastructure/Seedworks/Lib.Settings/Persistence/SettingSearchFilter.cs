using System;
using Seedworks.Lib.Persistence;

namespace Seedworks.Lib.Settings
{
    [Serializable]
    public class SettingSearchFilter : ConditionContainer
    {
        public ConditionDisjunction<int> SettingTypeIds { get; private set; }
        public ConditionString SettingType { get; private set; }
        public ConditionString Key { get; private set; }
        public ConditionDisjunction<int> CatalogIds;

        public SettingSearchFilter()
        {
            SettingTypeIds = new ConditionDisjunction<int>(this, "SettingTypeId");
            SettingType = new ConditionString(this, "SettingType");
            Key = new ConditionString(this, "Key");
            CatalogIds = new ConditionDisjunction<int>(this, "Catalog.Id");
        }

    }
}
