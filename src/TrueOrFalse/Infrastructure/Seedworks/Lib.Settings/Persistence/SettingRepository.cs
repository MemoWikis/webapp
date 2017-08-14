using System;
using System.Collections.Generic;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace Seedworks.Lib.Settings
{
    public class SettingRepository : RepositoryDb<Setting>, ISettingRepository 
    {
        public SettingRepository(ISession session) : base(session)
        {
        }

        public override void Update(Setting setting)
        {
            base.Update(setting);
            Flush();
        }

        public override void Delete(Setting setting)
        {
            base.Delete(setting);
            Flush();
        }

        public IList<Setting> GetBy(SettingSearchDesc searchDesc)
        {
            return base.GetBy(searchDesc);
        }

        public Setting GetUnique(SettingSearchDesc searchDesc)
        {
        	var results = GetBy(searchDesc);

			if (results.Count > 1)
				throw new ArgumentException("The DB returned more than 1 result for the given search descriptor!");

        	return results.Count > 0 ? results[0] : null;
        }
    }
}
