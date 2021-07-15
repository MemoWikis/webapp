using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs225
    {
        public static void Run()
        {
            QuestionSolutionType3To7Migration.Run();
        }
    }
}