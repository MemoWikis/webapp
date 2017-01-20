using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs098
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `answerhistory_test` (
	                  `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                  `AlgoId` INT(11) NULL DEFAULT NULL,
	                  `Probability` INT(11) NULL DEFAULT NULL,
	                  `IsCorrect` TINYINT(1) NULL DEFAULT NULL,
	                  `AnswerHistory_id` INT(11) NULL DEFAULT NULL,
	                  PRIMARY KEY (`Id`),
	                  INDEX `AnswerHistory_id` (`AnswerHistory_id`)
                  )
                  COLLATE='utf8_general_ci'
                  ENGINE=InnoDB
                  ;"
            ).ExecuteUpdate();

        }
    }
}