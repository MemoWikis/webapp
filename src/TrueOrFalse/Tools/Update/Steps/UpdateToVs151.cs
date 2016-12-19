using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs151
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `category`
                    ADD COLUMN `TopicMarkdown` TEXT NULL DEFAULT NULL AFTER `WikipediaURL`,
	                ADD COLUMN `FeaturedSetsIdsString` VARCHAR(100) NULL DEFAULT NULL AFTER `WikipediaURL`;"
            ).ExecuteUpdate();
        }
    }
}

