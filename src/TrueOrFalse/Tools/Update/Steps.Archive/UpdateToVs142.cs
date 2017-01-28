using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs142
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `answer`
	                ADD COLUMN `QuestionViewGuid` VARCHAR(36) NULL DEFAULT NULL AFTER `QuestionId`,
	                ADD COLUMN `InteractionNumber` INT(11) NULL DEFAULT NULL AFTER `QuestionViewGuid`;"
            ).ExecuteUpdate();
        } 
    }
}