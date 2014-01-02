using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs035
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `user`
	                CHANGE COLUMN `WishCount` `WishCountQuestions` INT(11) NULL DEFAULT NULL AFTER `ReputationPos`,
	                ADD COLUMN `WishCountSets` INT(11) NULL DEFAULT NULL AFTER `WishCountQuestions`;").ExecuteUpdate();

        }
    }
}
