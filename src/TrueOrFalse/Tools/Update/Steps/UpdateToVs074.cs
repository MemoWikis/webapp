using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs074
    {
        public static void Run()
        {
            ServiceLocator
                .Resolve<ISession>()
                .CreateSQLQuery(
                    @"  ALTER TABLE `game`
	                        CHANGE COLUMN `Rounds` `RoundCount` INT(11) NULL DEFAULT NULL AFTER `MaxPlayers`;"
            ).ExecuteUpdate();
        } 
    }
}