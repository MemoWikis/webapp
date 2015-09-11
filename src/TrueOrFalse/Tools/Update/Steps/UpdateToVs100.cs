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
                @"ALTER TABLE `learningsessionstep`
                	ADD COLUMN `Idx` INT(11) NOT NULL DEFAULT '-1' AFTER `LearningSession_id`;"
            ).ExecuteUpdate();

            var learningSessionRepo = Sl.Resolve<LearningSessionRepo>();
            var learningSessionStepRepo = Sl.Resolve<LearningSessionStepRepo>();

            var lss = learningSessionRepo.GetAll();

            foreach (var l in learningSessionRepo.GetAll())
            {
                if (l.Steps.ToList().Any(s => s.Idx < 0))
                {
                    var idx = 0;
                    l.Steps.OrderBy(s => s.Id).ForEach(step =>
                    {
                        step.Idx = idx;
                        idx++;
                        learningSessionStepRepo.Update(step);
                    });
                }
            }

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsessionstep`
                    ADD CONSTRAINT UQC_LearningSession_idx UNIQUE (`LearningSession_id`, `Idx`);"
            ).ExecuteUpdate();

        }
    }
}