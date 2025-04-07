// Repositories/ShareInfoRepository.cs
using NHibernate;

public class SharesRepository(ISession session) : RepositoryDbBase<Share>(session)
{
    public IList<Share> GetAll()
    {
        return base.GetAll();
    }

    public void Create(Share share)
    {
        base.Create(share);
        Flush();
    }

    public void Update(Share share)
    {
        base.Update(share);
        Flush();
    }

    public void Delete(Share share)
    {
        base.Delete(share);
        Flush();
    }
}