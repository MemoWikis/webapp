using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs218
    {
        public static void Run()
        {
            TemplateMigrator.MigrateQuestions();
        }
    }
}