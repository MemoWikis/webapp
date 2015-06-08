using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs069
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `answerhistory`
	                CHANGE COLUMN `AnswerredCorrectly` `AnswerredCorrectly` INT(10) NULL DEFAULT NULL AFTER `AnswerText`;")
                 .ExecuteUpdate();
        } 
    }
}
