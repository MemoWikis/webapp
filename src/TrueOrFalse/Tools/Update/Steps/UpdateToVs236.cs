using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs236
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE user ADD COLUMN `AddToWikiHistory` Text;"
                ).ExecuteUpdate();
        }
    }
}