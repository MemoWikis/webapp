using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs182
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `category`
	                ADD COLUMN `Url` VARCHAR(255) NULL DEFAULT NULL AFTER `WikipediaURL`; "
            ).ExecuteUpdate();
        }
    }
}