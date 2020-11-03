using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs215
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `category`
                    ADD COLUMN `SkipMigration` BIT NULL DEFAULT NULL AFTER `FormerSetId`"
                ).ExecuteUpdate();
        }
    }
}