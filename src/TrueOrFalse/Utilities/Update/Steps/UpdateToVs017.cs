using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs017
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ExecuteSqlFile>().Run(
                PathTo.Scrips("017-new-tbl-imageMetaData.sql"));
        }
    }
}
