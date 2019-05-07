using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs202
    {
        public static void Run()
        {
            TemplateMigration.DescriptionMigration.Start();
        }
    }
}