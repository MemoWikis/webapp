using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs157
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `answer`
	                ADD INDEX `AnswerredCorrectly` (`AnswerredCorrectly`),
	                ADD INDEX `UserId` (`UserId`),
	                ADD INDEX `QuestionId` (`QuestionId`);"
            ).ExecuteUpdate();
        }
    }
}

