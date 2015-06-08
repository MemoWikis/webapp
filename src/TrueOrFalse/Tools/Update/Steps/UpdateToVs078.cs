using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs078
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `answerhistory`
	                ADD COLUMN `Round_id` INT NULL DEFAULT NULL AFTER `DateCreated`,
	                ADD COLUMN `Player_id` INT NULL DEFAULT NULL AFTER `Round_id`;
                "
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `answerhistory`
                    ADD INDEX `Round_id` (`Round_id`),
                    ADD INDEX `Player_id` (`Player_id`);"
            ).ExecuteUpdate();
        } 
    }
}