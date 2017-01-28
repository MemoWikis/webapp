using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs152
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `comment`
	                ADD COLUMN `IsSettled` BIT NULL DEFAULT b'0' AFTER `ShouldImprove`;"
            ).ExecuteUpdate();
        }
    }
}