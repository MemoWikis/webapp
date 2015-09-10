using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs099
    {
        public static void Run()
        {
            var answerHistoryRepo = Sl.Resolve<AnswerHistoryRepository>();
            
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `answerhistory`
                    ADD CONSTRAINT UQC_LearningSessionStep_id UNIQUE (`LearningSessionStep_id`),
	                ADD CONSTRAINT `FK_LearningSessionStep` FOREIGN KEY (`LearningSessionStep_id`) REFERENCES `learningsessionstep` (`Id`);"
            ).ExecuteUpdate();

        }
    }
}