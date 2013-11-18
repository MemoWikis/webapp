using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs022
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>()
                .CreateSQLQuery(@"
ALTER TABLE `trueorfalse`.`questioninset` DROP FOREIGN KEY `FKD34040F3A925A27` ;
ALTER TABLE `trueorfalse`.`questioninset` CHANGE COLUMN `QuestionSet_id` `Set_id` INT(11) NULL DEFAULT NULL  , 
  ADD CONSTRAINT `FKD34040F3A925A27`
  FOREIGN KEY (`Set_id` )
  REFERENCES `trueorfalse`.`questionset` (`Id` );
").ExecuteUpdate();
        }
    }
}
