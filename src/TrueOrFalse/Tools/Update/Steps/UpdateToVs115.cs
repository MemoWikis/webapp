using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs115
    {
        public static void Run()
        {
            Sl.R<ISession>().CreateSQLQuery(@"ALTER TABLE `answer_test` CHANGE COLUMN `AnswerHistory_id` `Answer_id` INT(11) NULL DEFAULT NULL AFTER `IsCorrect`;").ExecuteUpdate();

            Sl.R<ISession>().CreateSQLQuery(@"
                ALTER TABLE `answerfeature_to_answer`
            	    ALTER `AnswerHistory_id` DROP DEFAULT;
                ALTER TABLE `answerfeature_to_answer`
	                CHANGE COLUMN `AnswerHistory_id` `Answer_id` INT(11) NOT NULL AFTER `AnswerFeature_id`;").ExecuteUpdate();
        }
    }
}