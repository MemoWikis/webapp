using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs158
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questionvaluation`
	                ADD INDEX `QuestionId` (`QuestionId`);"
            ).ExecuteUpdate();
        }
    }
}

