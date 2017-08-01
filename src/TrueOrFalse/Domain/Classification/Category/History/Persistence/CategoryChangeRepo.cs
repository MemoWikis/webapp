using NHibernate;

public class CategoryChangeRepo : RepositoryDbBase<CategoryChange>
{
    public CategoryChangeRepo(ISession session) : base(session)
    {
    }
}

