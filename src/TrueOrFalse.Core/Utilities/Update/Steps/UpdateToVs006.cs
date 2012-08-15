using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs006
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ExecuteSqlFile>().Run(
              ScriptPath.Get("006-new-column-solutiontype.sql"));
        }
    }
}
