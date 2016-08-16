using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs139
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"DROP TABLE `learningsessionstep`"
            ).ExecuteUpdate();
        }
    }
}