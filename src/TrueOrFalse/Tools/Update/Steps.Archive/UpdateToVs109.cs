using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs109
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(@"ALTER TABLE `game` DROP COLUMN `Creator_id`;"
            ).ExecuteUpdate();
        }
    }
}