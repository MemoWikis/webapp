using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs102
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `user`
	                ADD COLUMN `CorrectnessProbability` INT NULL DEFAULT '50' AFTER `WishCountSets`,
	                ADD COLUMN `CorrectnessProbabilityAnswerCount` INT NULL DEFAULT '0' AFTER `CorrectnessProbability`;"
            ).ExecuteUpdate();

        }
    }
}