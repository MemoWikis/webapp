using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs156
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questionview`
	                CHANGE COLUMN `DateCreated` `DateCreated` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP AFTER `Migrated`;"
            ).ExecuteUpdate();
        }
    }
}

