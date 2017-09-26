using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs184
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `user`
	                ADD COLUMN `WidgetHostsSpaceSeparated` VARCHAR(1000) NULL DEFAULT NULL AFTER `ActivityLevel`; "
                ).ExecuteUpdate();
        }
    }
}