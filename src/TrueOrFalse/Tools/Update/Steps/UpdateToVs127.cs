using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs127
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `trainingdate`
	                ADD COLUMN `MarkedAsMissed` BIT(1) NULL DEFAULT NULL AFTER `LearningSession_id`;"
            ).ExecuteUpdate();

        } 
    }
}