using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs220
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `category`
                    ADD COLUMN `Visibility` INT(11) NULL DEFAULT 0 AFTER `SkipMigration`"
                ).ExecuteUpdate();
        }
    }
}