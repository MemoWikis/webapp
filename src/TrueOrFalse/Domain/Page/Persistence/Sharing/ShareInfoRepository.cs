// Repositories/ShareInfoRepository.cs
using NHibernate;

public class ShareInfoRepository(ISession session) : RepositoryDbBase<ShareInfo>(session)
{
    public IList<ShareInfo> GetAll()
    {
        return base.GetAll();
    }

    public void Create(ShareInfo shareInfo)
    {
        base.Create(shareInfo);
        Flush();
    }

    public void Update(ShareInfo shareInfo)
    {
        base.Update(shareInfo);
        Flush();
    }

    public void Delete(ShareInfo shareInfo)
    {
        base.Delete(shareInfo);
        Flush();
    }
}