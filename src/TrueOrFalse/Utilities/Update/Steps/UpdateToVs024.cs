using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs024
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>()
                .CreateSQLQuery(@"ALTER TABLE `trueorfalse`.`question` ADD COLUMN `TextExtended` TEXT NULL  AFTER `Text`;").ExecuteUpdate();
        }
    }
}
