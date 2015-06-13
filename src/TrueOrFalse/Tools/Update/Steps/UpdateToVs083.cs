using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs083
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `membership`
	                ADD COLUMN `AutoRenewal` tinyint(1) DEFAULT NULL AFTER `PeriodEnd`;"

            ).ExecuteUpdate();
        } 
    }
}