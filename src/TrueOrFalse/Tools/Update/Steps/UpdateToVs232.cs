using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs232
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE jobqueue ADD COLUMN `Priority` Int DEFAULT 0;"
                ).ExecuteUpdate();
        }
    }
}