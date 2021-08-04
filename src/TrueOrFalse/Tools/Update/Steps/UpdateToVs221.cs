using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs221
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `question`
                    ADD COLUMN `TextHtml` TEXT NULL DEFAULT NULL AFTER `Text`"
                ).ExecuteUpdate();
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `question`
                    ADD COLUMN `TextExtendedHtml` TEXT NULL DEFAULT NULL AFTER `TextExtended`"
                ).ExecuteUpdate();
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `question`
                    ADD COLUMN `DescriptionHtml` TEXT NULL DEFAULT NULL AFTER `Description`"
                )
                .ExecuteUpdate();
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `question`
                    ADD COLUMN `SkipMigration` BIT NULL DEFAULT NULL AFTER `DateModified`"
                )
                .ExecuteUpdate();
        }
    }
}