using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs130
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `trainingdate`
                    ADD COLUMN `IsBoostingDate` BIT(1) NULL DEFAULT NULL AFTER `NotificationStatus`;"

            ).ExecuteUpdate();

        } 
    }
}