using NHibernate;

public class PersistentLoginRepo
{
    private readonly ISession _session;

    public PersistentLoginRepo(ISession session){
        _session = session;
    }

    public PersistentLogin Get(int userId, string guid)
    {
        var hashedGuid = HashPassword.Run(guid, Settings.SaltCookie);
        return _session.QueryOver<PersistentLogin>()
                        .Where(x => x.UserId == userId &&
                                    x.LoginGuid == hashedGuid)
                        .SingleOrDefault();
    }

    public void Create(PersistentLogin persistentLogin)
    {
        persistentLogin.LoginGuid = HashPassword.Run(persistentLogin.LoginGuid, Settings.SaltCookie);
        persistentLogin.Created = DateTime.Now;
        _session.Save(persistentLogin);
    }

    public void Delete(PersistentLogin persistentLogin)
    {
        Delete(persistentLogin.UserId);
    }
    public void Delete(int userId){
        _session.CreateQuery($"DELETE PersistentLogin WHERE UserId= {userId}").ExecuteUpdate();
        _session.Flush();
    }
}