using NHibernate;
using Seedworks.Lib.Persistence;

public class CategoryRelationRepo : RepositoryDb<CategoryRelation>
{
    public CategoryRelationRepo(ISession session) : base(session) { }
}