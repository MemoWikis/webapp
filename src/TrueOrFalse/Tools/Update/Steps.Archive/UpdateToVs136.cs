using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs136
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsessionstep`
	                ADD COLUMN `IsRepetition` BIT(1) NULL DEFAULT b'0' AFTER `LearningSession_id`;"
            ).ExecuteUpdate();
        } 
    }
}