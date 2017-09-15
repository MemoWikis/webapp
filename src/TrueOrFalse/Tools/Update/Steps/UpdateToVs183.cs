using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs183
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `category`
	                ADD COLUMN `UrlLinkText` VARCHAR(255) NULL DEFAULT NULL AFTER `Url`; "
                ).ExecuteUpdate();
        }
    }
}