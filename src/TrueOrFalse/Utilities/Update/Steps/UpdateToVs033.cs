using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs033
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `user`
	                ADD COLUMN `Reputation` INT NULL DEFAULT NULL AFTER `Birthday`,
	                ADD COLUMN `ReputationPos` INT NULL DEFAULT NULL AFTER `Reputation`;").ExecuteUpdate();

        }
    }
}
