using MySql.Data.MySqlClient;
using NHibernate;
using Seedworks.Lib.Persistence;

public class CategoryRelationRepo : RepositoryDb<CategoryRelation>
{
    public CategoryRelationRepo(ISession session) : base(session)
    {
    }

    public List<CategoryRelation> GetByRelationId(int relationId)
    {
        return Session.QueryOver<CategoryRelation>()
            .Where(r => r.Parent.Id == relationId)
            .List()
            .ToList();
    }
}