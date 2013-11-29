using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs012
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ExecuteSqlFile>().Run(
                PathTo.Scrips("012-new-tbl-passwordRecoveryToken.sql"));
        }
    }
}
