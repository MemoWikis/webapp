using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs117
    {
        public static void Run()
        {
            Sl.R<ISession>().CreateSQLQuery(@"
                CREATE TABLE `trainingplan` (
	                `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                `DateCreated` DATETIME NULL DEFAULT NULL,
	                `DateModified` DATETIME NULL DEFAULT NULL,
	                `Date_id` INT(11) NULL DEFAULT NULL,
	                `AnswerProbabilityTreshhold` INT(11) NULL DEFAULT NULL,
	                `QuestionsPerDate_IdealAmount` INT(11) NULL DEFAULT NULL,
	                `QuestionsPerDate_Minimum` INT(11) NULL DEFAULT NULL,
	                `SpacingBetweenSessionsInMinutes` INT(11) NULL DEFAULT NULL,
	                `Strategy` INT(11) NULL DEFAULT NULL,
	                PRIMARY KEY (`Id`),
	                INDEX `Date_id` (`Date_id`)
                )
                COLLATE='utf8_general_ci'
                ENGINE=innodb;").ExecuteUpdate();

            Sl.R<ISession>().CreateSQLQuery(@"
                CREATE TABLE `trainingdate` (
	                `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                `DateTime` DATETIME NULL DEFAULT NULL,
	                `DateCreated` DATETIME NULL DEFAULT NULL,
	                `DateModified` DATETIME NULL DEFAULT NULL,
	                `TrainingPlan_id` INT(11) NULL DEFAULT NULL,
	                PRIMARY KEY (`Id`),
	                INDEX `TrainingPlan_id` (`TrainingPlan_id`)
                )
                COLLATE='utf8_general_ci'
                ENGINE=innodb;").ExecuteUpdate();

            Sl.R<ISession>().CreateSQLQuery(@"
                CREATE TABLE `trainingdate_question` (
	                `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                `ProbBefore` INT(11) NULL DEFAULT NULL,
	                `ProbAfter` INT(11) NULL DEFAULT NULL,
	                `DateCreated` DATETIME NULL DEFAULT NULL,
	                `DateModified` DATETIME NULL DEFAULT NULL,
	                `IsInTraining` TINYINT(1) NULL DEFAULT NULL,
	                `Question_id` INT(11) NULL DEFAULT NULL,
	                `TrainingDate_id` INT(11) NULL DEFAULT NULL,
	                PRIMARY KEY (`Id`),
	                INDEX `Question_id` (`Question_id`),
	                INDEX `TrainingDate_id` (`TrainingDate_id`)
                )
                COLLATE='utf8_general_ci'
                ENGINE=innodb;").ExecuteUpdate();
        }
    }
}