using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs148
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `setting`
	                ADD COLUMN `SuggestedSetsIdString` VARCHAR(800) NULL DEFAULT NULL AFTER `AppVersion`;"
            ).ExecuteUpdate();
        }
    }
}