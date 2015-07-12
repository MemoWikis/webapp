using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs088
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `appaccess`
	                ADD COLUMN `AppInfoJson` VARCHAR(255) NULL DEFAULT NULL AFTER `AccessToken`,
	                ADD COLUMN `DeviceKey` VARCHAR(255) NULL DEFAULT NULL AFTER `AppInfoJson`;
                "
            ).ExecuteUpdate();
        }
    }
}