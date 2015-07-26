using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs089
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `learningsession` (
                        `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                    `DateCreated` DATETIME NULL DEFAULT NULL,
	                    `DateModified` DATETIME NULL DEFAULT NULL,
	                    `User_id` INT(11) NULL DEFAULT NULL,
	                    `SetToLearn_id` INT(11) NULL DEFAULT NULL,
	                    PRIMARY KEY (`Id`),
	                    INDEX `User_id` (`User_id`),
	                    INDEX `SetToLearn_id` (`SetToLearn_id`),
	                    CONSTRAINT `FK946A748A5C428B0` FOREIGN KEY (`SetToLearn_id`) REFERENCES `questionset` (`Id`),
	                    CONSTRAINT `FK946A74881B215E3` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id`)
                    )
                    COLLATE='utf8_general_ci'
                    ENGINE=InnoDB;"

            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `learningsessionstep` (
	                    `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                    `DateCreated` DATETIME NULL DEFAULT NULL,
	                    `DateModified` DATETIME NULL DEFAULT NULL,
	                    `Question_id` INT(11) NULL DEFAULT NULL,
	                    `AnswerHistory_id` INT(11) NULL DEFAULT NULL,
	                    `LearningSession_id` INT(11) NULL DEFAULT NULL,
	                    PRIMARY KEY (`Id`),
	                    INDEX `Question_id` (`Question_id`),
	                    INDEX `AnswerHistory_id` (`AnswerHistory_id`),
	                    INDEX `LearningSession_id` (`LearningSession_id`),
	                    CONSTRAINT `FKD0DB1D0A1F919DF1` FOREIGN KEY (`LearningSession_id`) REFERENCES `learningsession` (`Id`),
	                    CONSTRAINT `FKD0DB1D0AC0321409` FOREIGN KEY (`Question_id`) REFERENCES `question` (`Id`),
	                    CONSTRAINT `FKD0DB1D0AE15938F9` FOREIGN KEY (`AnswerHistory_id`) REFERENCES `answerhistory` (`Id`)
                    )
                    COLLATE='utf8_general_ci'
                    ENGINE=InnoDB;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `answerhistory`
	                    ADD COLUMN `LearningSessionStep_id` INT(11) NULL DEFAULT NULL AFTER `Player_id`;"
            ).ExecuteUpdate();

        }
    }
}