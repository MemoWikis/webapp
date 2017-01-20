using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs129
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `date`
	                ADD COLUMN `CopiedFrom` INT(11) NULL DEFAULT NULL AFTER `TrainingPlan_id`;"
            ).ExecuteUpdate();
        }
    }
}