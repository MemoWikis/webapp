using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs175
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `category`
                    CHANGE COLUMN `CategoriesToIncludeIdsString` `CategoriesToIncludeIdsString` VARCHAR(1000) NULL DEFAULT NULL AFTER `CategoriesToExcludeIdsString`,
                    CHANGE COLUMN `CategoriesToExcludeIdsString` `CategoriesToExcludeIdsString` VARCHAR(1000) NULL DEFAULT NULL AFTER `WikipediaURL`,
                    ADD COLUMN `AggregatedContentJson` TEXT NULL DEFAULT NULL AFTER `TopicMarkdown`,
                    DROP COLUMN `CountCreators`;"
            ).ExecuteUpdate();
        }
    }
}