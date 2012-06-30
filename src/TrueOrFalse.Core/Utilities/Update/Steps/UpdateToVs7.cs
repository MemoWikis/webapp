using NHibernate;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs7
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                "UPDATE Question SET SolutionType = 1 WHERE SolutionType IS NULL OR SolutionType = 0");
        }

    }
}
