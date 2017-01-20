using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs123
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `trainingplan`
                    CHANGE COLUMN `AnswerProbabilityTreshhold` `AnswerProbabilityThreshold` INT(11) NULL DEFAULT NULL AFTER `Date_id`;"
            ).ExecuteUpdate();
        } 
    }
}