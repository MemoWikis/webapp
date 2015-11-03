using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs113
    {
        public static void Run()
        {
            Sl.R<ISession>().CreateSQLQuery(@"RENAME TABLE `answerhistory` TO `answer`;").ExecuteUpdate();
            Sl.R<ISession>().CreateSQLQuery(@"RENAME TABLE `answerhistory_test` TO `answer_test`;").ExecuteUpdate();
        }
    }
}