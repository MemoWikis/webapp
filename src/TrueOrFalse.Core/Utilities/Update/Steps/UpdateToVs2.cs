using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs2
    {
        public static void Run(){
            ServiceLocator.Resolve<ExecuteSqlFile>().Run(
                ScriptPath.Get("2-new-total-fields-tbl-question.sql"));
        }
    }
}
