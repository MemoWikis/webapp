using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs143
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questionview`
	                ADD COLUMN `Milliseconds` INT(11) NULL DEFAULT NULL AFTER `UserId`;"
            ).ExecuteUpdate();
        } 
    }
}