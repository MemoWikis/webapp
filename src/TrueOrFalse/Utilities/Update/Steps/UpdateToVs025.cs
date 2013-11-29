using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs025
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ExecuteSqlFile>().Run(
                PathTo.Scrips("025-new-tbl-categories_to_sets.sql"));
        }
    }
}
