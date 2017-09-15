using System;
using System.Linq;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs171
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `category`
                 ADD COLUMN `DisableLearningFunctions` BIT NULL DEFAULT NULL AFTER `WikipediaURL`;"
            ).ExecuteUpdate();

        }
    }
}