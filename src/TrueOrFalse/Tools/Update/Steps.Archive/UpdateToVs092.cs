using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs092
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsessionstep`
                    DROP FOREIGN KEY `FKD0DB1D0AE15938F9`,
	                DROP COLUMN `AnswerHistory_id`;"
            ).ExecuteUpdate();

        }
    }
}