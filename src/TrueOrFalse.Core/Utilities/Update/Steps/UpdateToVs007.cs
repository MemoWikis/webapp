using NHibernate;
using TrueOrFalse.Core;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs007
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                "UPDATE Question SET SolutionType = 1 WHERE SolutionType IS NULL OR SolutionType = 0").ExecuteUpdate();
        }

    }
}
