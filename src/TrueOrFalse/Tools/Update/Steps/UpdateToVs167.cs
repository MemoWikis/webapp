using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs167
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `answer`
	                CHANGE COLUMN `AnswerText` `AnswerText` VARCHAR(1000) NULL DEFAULT NULL AFTER `InteractionNumber`;; "
            ).ExecuteUpdate();
        }
    }
}