using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs097
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsession`
	                ADD COLUMN `DateToLearn_id` INT NULL AFTER `SetToLearn_id`,
	                ADD INDEX `DateToLearn_id` (`DateToLearn_id`),
	                ADD CONSTRAINT `FK3DateToLearn` FOREIGN KEY (`DateToLearn_id`) REFERENCES `date` (`Id`);"
            ).ExecuteUpdate();

        }
    }
}