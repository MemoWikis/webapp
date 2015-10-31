using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs112
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(@"
                    CREATE TABLE `questionfeature` (
	                    `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                    `Id2` VARCHAR(1000) NULL DEFAULT NULL,
	                    `Name` VARCHAR(1000) NULL DEFAULT NULL,
	                    `Description` VARCHAR(1000) NULL DEFAULT NULL,
	                    `DateCreated` DATETIME NULL DEFAULT NULL,
	                    `DateModified` DATETIME NULL DEFAULT NULL,
	                    PRIMARY KEY (`Id`)
                    )
                    COLLATE='utf8_general_ci'
                    ENGINE=innodb;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(@"            
                    CREATE TABLE `questionfeature_to_question` (
            	        `QuestionFeature_id` INT(11) NOT NULL,
            	        `Question_id` INT(11) NOT NULL,
                        INDEX `Question_id` (`Question_id`),
            	        INDEX `QuestionFeature_id` (`QuestionFeature_id`)
                    )
                    COLLATE = 'utf8_general_ci'
                    ENGINE = innodb;"
            ).ExecuteUpdate();
        }
    }
}