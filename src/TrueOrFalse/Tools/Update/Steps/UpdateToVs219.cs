using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs219
    {
        public static void Run()
        {
            TemplateMigrator.MigrateQuestions();
        }
    }
}