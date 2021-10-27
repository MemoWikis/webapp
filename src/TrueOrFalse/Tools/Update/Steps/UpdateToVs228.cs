using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs228
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `comment`
                    ADD COLUMN `Title` TEXT"
                ).ExecuteUpdate();
        }
    }
}