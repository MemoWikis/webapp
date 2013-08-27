using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs026
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                "ALTER TABLE `category` CHANGE COLUMN `QuestionCount` `CountQuestions` INT(7) UNSIGNED ZEROFILL NULL DEFAULT NULL  , ADD COLUMN `CountSets` INT(7) UNSIGNED ZEROFILL NULL  AFTER `CountQuestions` , ADD COLUMN `CountCreators` INT(7) UNSIGNED ZEROFILL NULL  AFTER `CountSets` ;").ExecuteUpdate();
        }
    }
}
