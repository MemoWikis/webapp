﻿using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Infrastructure
{
    public class DbSettingsRepository : RepositoryDb<DbSettings>
    {
        public DbSettingsRepository(ISession session)
            : base(session)
        {
        }

        public DbSettings Get()
        {
            return base.GetById(1);
        }

        public int GetAppVersion()
        {
            return Session
                .CreateSQLQuery("SELECT AppVersion FROM setting WHERE Id = 1")
                .UniqueResult<int>();
        }

        public void UpdateAppVersion(int newAppVersion)
        {
            var dbSettings = Get();
            dbSettings.AppVersion = newAppVersion;
            base.Update(dbSettings);
            base.Flush();
        }
    }
}
