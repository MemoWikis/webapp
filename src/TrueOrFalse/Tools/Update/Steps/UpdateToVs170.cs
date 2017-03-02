using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs170
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `category`
	                ADD COLUMN `TotalRelevancePersonalEntries` INT(7) NULL AFTER `CorrectnessProbabilityAnswerCount`;"
            ).ExecuteUpdate();
        }
    }
}