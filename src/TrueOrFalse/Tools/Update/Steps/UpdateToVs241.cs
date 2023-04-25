using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs241
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE user ADD COLUMN subscriptionStartDate DATETIME DEFAULT NULL;"
                ).ExecuteUpdate();
            CategoryAuthorUpdater.UpdateAll();
        }
    }
}