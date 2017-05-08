using System.Collections.Generic;
using NHibernate;
using Seedworks.Lib.Persistence;
using Seedworks.Lib.ValueObjects;

public class CategoryRelationRepo : RepositoryDb<CategoryRelation>
{
    public CategoryRelationRepo(ISession session) : base(session) { }
}