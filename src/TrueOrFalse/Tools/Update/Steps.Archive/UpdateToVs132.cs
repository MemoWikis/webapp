using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs132
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `trainingplan`
                    ADD COLUMN `LearningGoalIsReached` BIT(1) NULL DEFAULT NULL AFTER `Date_id`;"
            ).ExecuteUpdate();

        }
    }
}