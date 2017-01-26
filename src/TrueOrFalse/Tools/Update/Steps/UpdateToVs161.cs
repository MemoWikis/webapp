using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs161
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsession`
	                ADD COLUMN `CategoryToLearn_id` INT(11) NULL DEFAULT NULL AFTER `SetToLearn_Id`;"
            ).ExecuteUpdate();
        }
    }
}