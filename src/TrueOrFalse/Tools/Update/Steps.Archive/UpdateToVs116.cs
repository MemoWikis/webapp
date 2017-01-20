using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs116
    {
        public static void Run()
        {
            Sl.R<ISession>().CreateSQLQuery(@"
                DELETE a
                FROM answer a
                LEFT JOIN question q
                ON a.QuestionId = q.Id
                WHERE q.Id is null").ExecuteUpdate();
        }
    }
}