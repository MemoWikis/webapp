using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs196
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"delete questionValuation
                    from questionValuation
                    left join question
                    on question.Id = questionValuation.questionid
                    where question.Id is null;"
                ).ExecuteUpdate();
        }
    }
}