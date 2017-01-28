using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs096
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questionvaluation`
	                ADD UNIQUE INDEX `Unique_pair_of_userId_questionId` (`UserId`, `QuestionId`);"
            ).ExecuteUpdate();

        }
    }
}