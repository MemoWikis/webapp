using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs200
    {
        public static void Run()
        {
            TemplateMigration.TemplateMigrator.Start();
        }
    }
}