using NHibernate;
using Seedworks.Lib.Persistence;

public class ReferenceRepository : RepositoryDb<Reference>
{
    public ReferenceRepository(ISession session) : base(session)
    {
    }

}