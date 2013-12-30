using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs034
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `user`
	                ADD COLUMN `WishCount` INT(11) NULL DEFAULT NULL AFTER `ReputationPos`").ExecuteUpdate();

        }
    }
}
