using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs150
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsession`
	                ADD COLUMN `IsWishSession` BIT NULL DEFAULT b'0' AFTER `DateToLearn_id`;"
            ).ExecuteUpdate();
        }
    }
}