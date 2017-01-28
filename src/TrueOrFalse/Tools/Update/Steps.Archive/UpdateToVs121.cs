using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs121
    {
        public static void Run()
        {
            Sl.R<ISession>().CreateSQLQuery(@"
                ALTER TABLE `trainingdate`
	                ADD COLUMN `NotificationStatus` INT NULL DEFAULT '0' AFTER `Id`;")
                    .ExecuteUpdate();
        }
    }
}