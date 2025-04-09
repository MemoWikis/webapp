// Repositories/ShareInfoRepository.cs
using NHibernate;

public class SharesRepository(ISession session) : RepositoryDbBase<Share>(session)
{

    public IList<Share> GetAllEager()
    {
        var shares = _session.QueryOver<Share>().Future().ToList();

        _session.QueryOver<Share>()
            .Fetch(SelectMode.Fetch, x => x.User)
            .Future();

        var result = shares;

        foreach (var share in result)
        {
            NHibernateUtil.Initialize(share.User);
        }

        return result.ToList();
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
    public void Delete(IList<int> shareIds)
    {
        if (shareIds == null || !shareIds.Any())
            return;

        foreach (var shareId in shareIds)
        {
            var share = GetById(shareId);
            if (share != null)
            {
                base.Delete(share);
            }
        }

        Flush();
    }

}