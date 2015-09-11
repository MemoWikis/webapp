using System.Linq;
using NHibernate;
using NHibernate.Util;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs100
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"UPDATE `answerhistory`
                    SET `LearningSessionStep_id` = null;"
            ).ExecuteUpdate();
            
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"DELETE FROM `learningsessionstep`;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"DELETE FROM `learningsession`;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsessionstep`
                    ADD CONSTRAINT UQC_LearningSession_idx UNIQUE (`LearningSession_id`, `Idx`);"
            ).ExecuteUpdate();

        }
    }
}