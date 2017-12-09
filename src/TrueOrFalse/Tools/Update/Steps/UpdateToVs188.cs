using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs188
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `categorychange`
	                ADD COLUMN `Category_id` INT NULL DEFAULT NULL AFTER `Author_id`,
	                ADD INDEX `FK_Category_id` (`Category_id`),
	                ADD CONSTRAINT `FK_Category_id` FOREIGN KEY (`Category_id`) REFERENCES `category` (`Id`);"
            ).ExecuteUpdate();
        }
    }
}

