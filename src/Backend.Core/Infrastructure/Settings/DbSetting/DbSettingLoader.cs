using NHibernate;

public class DbSettingsRepo(ISession _session) : RepositoryDb<DbSettings>(_session)
{
    public DbSettings Get()
    {
        return base.GetById(1);
    }

    public int GetAppVersion()
    {
        return Session
            .CreateSQLQuery("SELECT AppVersion FROM Setting WHERE Id = 1")
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