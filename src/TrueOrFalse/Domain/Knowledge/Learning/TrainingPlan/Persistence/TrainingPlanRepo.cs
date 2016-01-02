
using NHibernate;

public class TrainingPlanRepo : RepositoryDbBase<TrainingPlan>
{
    public TrainingPlanRepo(ISession session) : base(session)
    {
    }
}