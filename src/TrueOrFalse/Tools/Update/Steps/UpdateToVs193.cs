using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs193
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questionview`
	                ADD INDEX `QuestionId` (`QuestionId`),
	                ADD INDEX `UserId` (`UserId`);"
            ).ExecuteUpdate();
        }
    }
}

