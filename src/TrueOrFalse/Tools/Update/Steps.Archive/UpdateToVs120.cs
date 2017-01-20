using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs120
    {
        public static void Run()
        {
            Sl.R<ISession>().CreateSQLQuery(@"
                ALTER TABLE `message`
	                ADD COLUMN `WasSendPerEmail` TINYINT NULL AFTER `IsRead`,
	                ADD COLUMN `WasSendToSmartphone` TINYINT NULL AFTER `WasSendPerEmail`,
	                ADD COLUMN `TrainingDate_id` INT NULL AFTER `WasSendToSmartphone`,
	                ADD COLUMN `TrainingPlan_id` INT NULL AFTER `TrainingDate_id`,
	                ADD INDEX `TrainingDate_id` (`TrainingDate_id`),
	                ADD INDEX `TrainingPlan_id` (`TrainingPlan_id`),
	                ADD CONSTRAINT `FK_TrainingDate_id` FOREIGN KEY (`TrainingDate_id`) REFERENCES `trainingdate` (`Id`),
	                ADD CONSTRAINT `FK_TrainingPlan_id` FOREIGN KEY (`TrainingPlan_id`) REFERENCES `trainingplan` (`Id`);")
                    .ExecuteUpdate();
        }
    }
}