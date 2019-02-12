using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs199
    {
        public static void Run()
        {
            InitializeQuestionChanges.Go();
        }
    }
}