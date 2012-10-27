using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs011
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ExecuteSqlFile>().Run(
                ScriptPath.Get("012-new-tbl-passwordRecoveryToken.sql"));
        }
    }
}
