using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs174
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `category`
                    	ADD COLUMN `CategoriesToIncludeIdsString` VARCHAR(100) NULL DEFAULT NULL AFTER `WikipediaURL`,
                    	ADD COLUMN `CategoriesToExcludeIdsString` VARCHAR(100) NULL DEFAULT NULL AFTER `WikipediaURL`;"
            ).ExecuteUpdate();
        }
    }
}