using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs003
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ExecuteSqlFile>().Run(
              ScriptPath.Get("003-new-table-questionValuation.sql"));
        }
    }
}
