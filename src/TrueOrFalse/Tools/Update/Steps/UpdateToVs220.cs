using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs220
    {
        public static void Run()
        {
            TemplateMigrator.MigrateQuestions();
        }
    }
}