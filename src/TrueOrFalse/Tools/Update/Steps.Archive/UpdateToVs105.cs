using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs105
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `game_round`
	                ENGINE=InnoDB;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `game_player`
	                ENGINE=InnoDB;"
            ).ExecuteUpdate();
            
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questionview`
	                ADD COLUMN `Round_id` INT NULL AFTER `UserId`,
	                ADD COLUMN `Player_id` INT NULL AFTER `Round_id`,
	                ADD COLUMN `LearningSessionStep_id` INT NULL AFTER `Player_id`,

	                ADD CONSTRAINT `FK_questionview_game_player` FOREIGN KEY (`Player_id`) REFERENCES `game_player` (`Id`),
	                ADD CONSTRAINT `FK_questionview_learningsessionstep` FOREIGN KEY (`LearningSessionStep_id`) REFERENCES `learningsessionstep` (`Id`),
	                ADD CONSTRAINT `FK_questionview_game_round` FOREIGN KEY (`Round_id`) REFERENCES `game_round` (`Id`);"
            ).ExecuteUpdate();

        }
    }
}