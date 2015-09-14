using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs103
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `category`
	                ADD COLUMN `CorrectnessProbabilityAnswerCount` INT NULL DEFAULT '0' AFTER `CorrectnessProbability`;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `question`
	                ADD COLUMN `CorrectnessProbabilityAnswerCount` INT NULL DEFAULT '0' AFTER `CorrectnessProbability`;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questionvaluation`
	                ADD COLUMN `CorrectnessProbabilityAnswerCount` INT NULL DEFAULT '0' AFTER `CorrectnessProbability`;"
            ).ExecuteUpdate();
        }
    }
}