using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs201
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `categorychange`
	                    ADD COLUMN `ShowInSidebar` BIT NULL DEFAULT b'1' AFTER `DataVersion`;"
                ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `questionchange`
	                    ADD COLUMN `ShowInSidebar` BIT NULL DEFAULT b'1' AFTER `DataVersion`;"
                ).ExecuteUpdate();
        }
    }
}