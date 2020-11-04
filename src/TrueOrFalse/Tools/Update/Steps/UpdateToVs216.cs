using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs216
    {
        public static void Run()
        {
            TemplateMigrator.MigrateTopicMarkdownToContent();
        }
    }
}