using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs233
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE category ADD COLUMN `AuthorIds` Text;"
                ).ExecuteUpdate();
            CategoryAuthorUpdater.UpdateAll();
        }
    }
}