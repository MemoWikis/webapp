using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs137
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsession`
	                ADD COLUMN `StepsJson` TEXT NULL DEFAULT NULL AFTER `DateToLearn_id`;"
            ).ExecuteUpdate();

        }
    }
}