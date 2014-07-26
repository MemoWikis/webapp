using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs058
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `questionvaluation`
	                ADD COLUMN `KnowledgeStatus` INT(2) NULL DEFAULT NULL AFTER `CorrectnessProbability`")
                .ExecuteUpdate();
        }
    }
}