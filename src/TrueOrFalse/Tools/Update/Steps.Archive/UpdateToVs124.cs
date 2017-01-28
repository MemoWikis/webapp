using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs124
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `setting`
	                ADD COLUMN `SuggestedGames` VARCHAR(800) NULL DEFAULT NULL AFTER `AppVersion`;"
            ).ExecuteUpdate();
        } 
    }
}