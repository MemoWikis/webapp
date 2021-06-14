using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs226
    {
        public static void Run()
        {
            QuestionSolutionType3To7Migration.Run();
        }
    }
}