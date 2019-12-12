using TrueOrFalse;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;


namespace TrueOrFalse.Updates
{
    public class UpdateToVs206
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"CREATE INDEX `CategoryRelationType`  ON `relatedcategoriestorelatedcategories` (CategoryRelationType) COMMENT '' ALGORITHM DEFAULT LOCK DEFAULT;"
                ).ExecuteUpdate();
        }
    }
}