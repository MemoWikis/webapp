using FluentNHibernate.Mapping;

public class TrainingPlanMap : ClassMap<TrainingPlan>
{
    public TrainingPlanMap()
    {
        Id(x => x.Id);

        References(x => x.Date);
        HasMany(x => x.Dates).Cascade.AllDeleteOrphan().Not.KeyNullable();

        Map(x => x.DateCreated);
        Map(x => x.DateModified);

        Component(x => x.Settings, x =>
        {
            x.Map(y => y.AnswerProbabilityTreshhold);
            x.Map(y => y.QuestionsPerDate_IdealAmount);
            x.Map(y => y.QuestionsPerDate_Minimum);
            x.Map(y => y.SpacingBetweenSessionsInMinutes);
            x.Map(y => y.Strategy);
        });

    }
}