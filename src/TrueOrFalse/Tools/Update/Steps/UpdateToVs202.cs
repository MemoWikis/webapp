using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs202
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `answer`
	                    ADD INDEX `LearningSessionId` (`LearningSession_id`);"
                ).ExecuteUpdate();
        }
    }
}