using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs163
    {
        public static void Run()
        {
            //Sl.Resolve<ISession>()
            //  .CreateSQLQuery(
            //    @"ALTER TABLE `questionset`
	           //     ADD COLUMN `VideoUrl` VARCHAR(255) NULL AFTER `Text`;"
            //).ExecuteUpdate();
        }
    }
}

