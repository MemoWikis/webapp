using System;
using System.Collections.Generic;
using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs218
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `categorychange` CHANGE COLUMN `Data` `Data` LONGTEXT NULL DEFAULT NULL;")
                .ExecuteUpdate();
        }
    }
}