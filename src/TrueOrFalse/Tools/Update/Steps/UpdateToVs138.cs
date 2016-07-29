using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs138
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `answer`
	                ADD COLUMN `LearningSessionStepGuid` VARCHAR(36) NULL DEFAULT NULL AFTER `Player_id`,
	                ADD COLUMN `LearningSession_id` INT(11) NULL DEFAULT NULL AFTER `Player_id`,
                    DROP FOREIGN KEY `FK_LearningSessionStep`,
                    DROP COLUMN `LearningSessionStep_id`;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
             .CreateSQLQuery(
               @"ALTER TABLE `questionview`
	                ADD COLUMN `LearningSessionStepGuid` VARCHAR(36) NULL DEFAULT NULL AFTER `Player_id`,
	                ADD COLUMN `LearningSession_id` INT(11) NULL DEFAULT NULL AFTER `Player_id`,
                    DROP FOREIGN KEY `FK_questionview_learningsessionstep`,
                    DROP COLUMN `LearningSessionStep_id`;"
           ).ExecuteUpdate();

        } 
    }
}