using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs133
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `trainingdate`
                    ADD COLUMN `ExpiresAt` DATETIME NULL DEFAULT NULL AFTER `DateTime`;"
            ).ExecuteUpdate();

        }
    }
}