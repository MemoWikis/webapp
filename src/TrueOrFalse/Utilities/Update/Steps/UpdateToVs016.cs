using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs016
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>()
                .CreateSQLQuery("ALTER TABLE `category` ADD COLUMN `QuestionCount` INT(7) ZEROFILL NULL  AFTER `Name`")
                .ExecuteUpdate();
        }
    }
}
