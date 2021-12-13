using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs231
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE jobqueue  MODIFY COLUMN JobContent TEXT;"
                ).ExecuteUpdate();
        }
    }
}