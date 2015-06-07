using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs085
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `date`
	                ADD COLUMN `User_id` INT(11) NULL DEFAULT NULL AFTER `DateModified`;"
            ).ExecuteUpdate();
        }
    }
}