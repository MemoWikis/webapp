using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs135
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"DROP TABLE `trainingdate_question`"
            ).ExecuteUpdate();
        }
    }
}