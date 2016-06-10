using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs130
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `trainingplan`
	                CHANGE COLUMN `SpacingBetweenSessionsInMinutes` `MinSpacingBetweenSessionsInMinutes` INT(11) NULL DEFAULT NULL AFTER `QuestionsPerDate_Minimum`,
	                ADD COLUMN `EqualizeSpacingBetweenSessions` BIT(1) NULL DEFAULT NULL AFTER `MinSpacingBetweenSessionsInMinutes`,
	                ADD COLUMN `EqualizedSpacingMaxMultiplier` INT(11) NULL DEFAULT NULL AFTER `EqualizeSpacingBetweenSessions`,
	                ADD COLUMN `EqualizedSpacingDelayerDays` INT(11) NULL DEFAULT NULL AFTER `EqualizedSpacingMaxMultiplier`;"
            ).ExecuteUpdate();


        }
    }
}