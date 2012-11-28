using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs013
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>()
                .CreateSQLQuery("ALTER TABLE `trueorfalse`.`question` " +
                                "ADD COLUMN `SolutionMetadata` VARCHAR(100) NULL  " +
                                "AFTER `SolutionType`")
                .ExecuteUpdate();
        }
    }
}
