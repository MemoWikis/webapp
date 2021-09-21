using System;
using System.Linq;
using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs227
    {
        public static void Run()
        {
            var rootCategoryId = RootCategory.RootCategoryId; 

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"UPDATE Category SET Name='Memucho Wiki' WHERE id=" + rootCategoryId
                ).ExecuteUpdate();
        }
    }
}