using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs140
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `game`
	                ADD COLUMN `WithSystemAvgPlayer` BIT NULL DEFAULT NULL AFTER `Comment`;"
            ).ExecuteUpdate();
        }
    }
}