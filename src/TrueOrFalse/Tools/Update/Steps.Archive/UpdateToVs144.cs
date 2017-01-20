using System;
using System.Linq;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs144
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questionview`
                 ADD COLUMN `Migrated` BIT NULL DEFAULT NULL AFTER `LearningSessionStepGuid`;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `answer`
                 ADD COLUMN `Migrated` BIT NULL DEFAULT NULL AFTER `LearningSessionStepGuid`;"
            ).ExecuteUpdate();
        }
    }
}