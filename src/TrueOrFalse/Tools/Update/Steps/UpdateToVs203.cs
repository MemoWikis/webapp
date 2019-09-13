using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs203
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"CREATE INDEX `IX_QuestionViewGuid` ON `answer`(`QuestionViewGuid`) USING BTREE;"
                ).ExecuteUpdate();

            
        }
    }
}