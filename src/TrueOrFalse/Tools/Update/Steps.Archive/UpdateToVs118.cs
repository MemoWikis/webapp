using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs118
    {
        public static void Run()
        {
            Sl.R<ISession>().CreateSQLQuery(@"
                ALTER TABLE `date`
	                ADD COLUMN `TrainingPlan_id` INT NULL AFTER `User_id`,
	                ADD INDEX `TrainingPlan_id` (`TrainingPlan_id`),
	                ADD INDEX `User_id` (`User_id`);").ExecuteUpdate();
        }
    }
}