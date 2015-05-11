using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs073
    {
        public static void Run()
        {
            ServiceLocator
                .Resolve<ISession>()
                .CreateSQLQuery(
                    @"  ALTER TABLE `game`
	                        ADD COLUMN `Rounds` TINYINT(3) NULL AFTER `MaxPlayers`;"
            ).ExecuteUpdate();
        } 
    }
}