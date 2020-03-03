
using ISession = NHibernate.ISession;


namespace TrueOrFalse.Updates
{
    public class UpdateToVs211
    {
        public static void Run()
        {
            TemplateMigration.TemplateMigrator.MigrateContentModules();
        }
    }
}