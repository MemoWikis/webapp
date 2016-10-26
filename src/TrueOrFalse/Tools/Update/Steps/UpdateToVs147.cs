using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs147
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questionview`
	                ADD COLUMN `UserAgent` VARCHAR(512) NULL DEFAULT NULL AFTER `Milliseconds`;"
            ).ExecuteUpdate();

        }
    }
}