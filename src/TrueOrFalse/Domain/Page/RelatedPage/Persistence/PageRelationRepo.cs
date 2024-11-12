using NHibernate;
using Seedworks.Lib.Persistence;

public class PageRelationRepo : RepositoryDb<PageRelation>
{
    public PageRelationRepo(ISession session) : base(session)
    {
    }

    public List<PageRelation> GetByRelationId(int relationId)
    {
        return Session.QueryOver<PageRelation>()
            .Where(r => r.Parent.Id == relationId)
            .List()
            .ToList();
    }
}