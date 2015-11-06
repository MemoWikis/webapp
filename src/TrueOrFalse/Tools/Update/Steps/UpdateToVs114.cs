using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs114
    {
        public static void Run()
        {
            Sl.R<ISession>().CreateSQLQuery(@"RENAME TABLE `answerfeature_to_answerhistory` TO `answerfeature_to_answer`;").ExecuteUpdate();
        }
    }
}