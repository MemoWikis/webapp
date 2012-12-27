using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs015
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ExecuteSqlFile>().Run(
                ScriptPath.Get("015-new-tbl-questionSet.sql"));
        }
    }
}
