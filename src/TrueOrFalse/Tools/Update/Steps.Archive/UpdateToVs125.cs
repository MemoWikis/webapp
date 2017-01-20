using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs125
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsession`
	                ADD COLUMN `IsCompleted` BIT(1) NULL DEFAULT b'0' AFTER `DateToLearn_id`;"
            ).ExecuteUpdate();
        } 
    }
}