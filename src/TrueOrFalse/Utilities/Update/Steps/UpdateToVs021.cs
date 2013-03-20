using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs021
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>()
                .CreateSQLQuery(@"ALTER TABLE `trueorfalse`.`questioninset` ADD COLUMN `Sort` INT NULL DEFAULT 0  AFTER `QuestionSet_id`;")
                .ExecuteUpdate();
        }
    }
}
