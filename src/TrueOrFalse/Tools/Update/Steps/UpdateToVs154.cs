using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs154
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `user`
	                ADD COLUMN `GoogleId` VARCHAR(25) NULL DEFAULT NULL AFTER `FacebookId`,
	                ADD UNIQUE INDEX `GoogleId` (`GoogleId`);"
            ).ExecuteUpdate();
        }
    }
}