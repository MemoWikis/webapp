using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs014
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>()
                .CreateSQLQuery("ALTER TABLE `question` " +
                                "CHANGE COLUMN `SolutionMetadata` `SolutionMetadataJson` VARCHAR(100) NULL DEFAULT NULL;")
                .ExecuteUpdate();
        }
    }
}
