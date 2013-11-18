using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs028
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                "ALTER TABLE `questionset` " +
                    "ADD COLUMN `TotalRelevancePersonalAvg` INT NULL  AFTER `Text` , " +
                    "ADD COLUMN `TotalRelevancePersonalEntries` INT NULL  AFTER `TotalRelevancePersonalAvg`").ExecuteUpdate();
        }
    }
}
