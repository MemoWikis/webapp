using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs104
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"
                    CREATE TABLE `answerfeature` (
	                    `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                    `Id2` VARCHAR(1000) NULL DEFAULT NULL,
	                    `GroupName` VARCHAR(1000) NULL DEFAULT NULL,
	                    `Name` VARCHAR(1000) NULL DEFAULT NULL,
	                    `Description` VARCHAR(1000) NULL DEFAULT NULL,
	                    `DateCreated` DATETIME NULL DEFAULT NULL,
	                    `DateModified` DATETIME NULL DEFAULT NULL,
	                    PRIMARY KEY (`Id`)
                    )
                    COLLATE='utf8_general_ci'
                    ENGINE=INNODB;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"
                    CREATE TABLE `answerfeature_to_answerhistory` (
	                    `AnswerFeature_id` INT(11) NOT NULL,
	                    `AnswerHistory_id` INT(11) NOT NULL,
	                    INDEX `AnswerHistory_id` (`AnswerHistory_id`),
	                    INDEX `AnswerFeature_id` (`AnswerFeature_id`)
                    )
                    COLLATE='utf8_general_ci'
                    ENGINE=INNODB
                    ;"
            ).ExecuteUpdate();

        }
    }
}