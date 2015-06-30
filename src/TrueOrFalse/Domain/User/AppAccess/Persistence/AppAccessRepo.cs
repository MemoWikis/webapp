using System.Linq;
using NHibernate;
using NHibernate.Linq;

public class AppAccessRepo : RepositoryDbBase<AppAccess>
{
    public AppAccessRepo(ISession session) : base(session)
    {
    }

    public AppAccess GetByUser(User user)
    {
        return _session
            .Query<AppAccess>()
            .FirstOrDefault(u => u.Id == user.Id);
    }

    public AppAccess GetByAccessToken(string accessToken)
    {
        return _session
            .Query<AppAccess>()
            .FirstOrDefault(u => u.AccessToken == accessToken);        
    }
}