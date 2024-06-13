using NHibernate;

public class PersistentLoginRepo
{
    private readonly ISession _session;

    public PersistentLoginRepo(ISession session)
    {
        _session = session;
    }

    public PersistentLogin Get(int userId, string guid)
    {
        //var hashedGuid = HashPassword.Run(guid, Settings.SaltCookie);
        return _session.QueryOver<PersistentLogin>()
            .Where(x => x.UserId == userId &&
                        x.LoginGuid == guid)
            .SingleOrDefault();
    }

    public void Create(PersistentLogin persistentLogin)
    {
        //persistentLogin.LoginGuid = HashPassword.Run(persistentLogin.LoginGuid, Settings.SaltCookie);
        persistentLogin.Created = DateTime.Now;
        _session.Save(persistentLogin);
    }

    public void Delete(PersistentLogin persistentLogin)
    {
        var persistentLoginCookieGetValuesResult = new PersistentLoginCookieGetValuesResult
        {
            LoginGuid = persistentLogin.LoginGuid,
            UserId = persistentLogin.UserId
        };
    }

    public void Delete(PersistentLoginCookieGetValuesResult persistentLogin)
    {
        _session.CreateQuery("DELETE FROM PersistentLogin WHERE UserId = :userId AND LoginGuid = :loginGuid")
            .SetParameter("userId", persistentLogin.UserId)
            .SetParameter("loginGuid", persistentLogin.LoginGuid)
            .ExecuteUpdate();
        _session.Flush();
    }
}