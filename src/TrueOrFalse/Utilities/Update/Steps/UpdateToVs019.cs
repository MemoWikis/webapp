using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs019
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>()
                .CreateSQLQuery("drop table `trueorfalse`.`questionset_to_question`;")
                .ExecuteUpdate();
        }
    }
}
