using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs063
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `reference`
	                ADD COLUMN `Index` INT(11) NULL DEFAULT NULL AFTER `Id`,
	                ADD COLUMN `ReferenceType` INT(11) NULL DEFAULT NULL AFTER `Index`,
                    CHANGE COLUMN `FreeTextReference` `ReferenceText` VARCHAR(255) NULL DEFAULT NULL AFTER `AdditionalInfo`")
                 .ExecuteUpdate();
        } 
    }
}
