using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs239
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE user ADD stripeId VARCHAR(255);"
                ).ExecuteUpdate();
            CategoryAuthorUpdater.UpdateAll();
        }
    }
}