using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs197
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"delete categoryValuation from categoryValuation
                    left join category 
                    on categoryValuation.categoryid = category.id
                    where category.id is null;"
                ).ExecuteUpdate();
        }
    }
}