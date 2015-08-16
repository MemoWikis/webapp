using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs095
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsessionstep`
	                ADD COLUMN `AnswerState` TINYINT NULL DEFAULT NULL AFTER `LearningSession_id`;"
            ).ExecuteUpdate();

        }
    }
}