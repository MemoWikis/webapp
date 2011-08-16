using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class SubCategoryItemRepository : RepositoryDb<SubCategory>
    {
        public SubCategoryItemRepository(ISession session)
            : base(session)
        {
        }
    }
}
