using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs053
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `comment`
	                ADD COLUMN `Creator_id` INT(11) NULL DEFAULT NULL AFTER `AnswerTo`,
	                DROP COLUMN `Creator`;")
                .ExecuteUpdate();
        }
    }
}
