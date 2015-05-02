using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs072
    {
        public static void Run()
        {
            ServiceLocator
                .Resolve<ISession>()
                .CreateSQLQuery(
                    @"  ALTER TABLE `game`
	                    ADD COLUMN `MaxPlayers` TINYINT UNSIGNED NULL DEFAULT NULL AFTER `WillStartAt`;"
            ).ExecuteUpdate();
        } 
    }
}