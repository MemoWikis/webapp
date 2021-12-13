using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs230
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE jobqueue  MODIFY COLUMN JobContent VARCHAR(3000);"
                ).ExecuteUpdate();
        }
    }
}