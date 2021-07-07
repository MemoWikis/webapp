using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs218
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `question`
	                    ADD INDEX `Visibility` (`Visibility`),
	                    ADD INDEX `TotalRelevancePersonalEntries` (`TotalRelevancePersonalEntries`),
	                    ADD INDEX `DateCreated` (`DateCreated`);
                    ")
                .ExecuteUpdate();
        }
    }
}