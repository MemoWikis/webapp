using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs221
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `category`
                    ADD COLUMN `CustomSegments` TEXT NULL DEFAULT NULL AFTER `Content`"
                ).ExecuteUpdate();
        }
    }
}