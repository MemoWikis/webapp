using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs126
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `trainingdate`
	                ADD COLUMN `LearningSession_id` INT(11) NULL DEFAULT NULL AFTER `TrainingPlan_id`;"
            ).ExecuteUpdate();

        } 
    }
}