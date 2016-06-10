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

        Map(x => x.LearningGoalIsReached);

        Component(x => x.Settings, x =>
        {
            x.Map(y => y.AnswerProbabilityThreshold);
            x.Map(y => y.QuestionsPerDate_IdealAmount);
            x.Map(y => y.QuestionsPerDate_Minimum);
            x.Map(y => y.MinSpacingBetweenSessionsInMinutes);
            x.Map(y => y.EqualizeSpacingBetweenSessions);
            x.Map(y => y.EqualizedSpacingMaxMultiplier);
            x.Map(y => y.EqualizedSpacingDelayerDays);
            x.Map(y => y.Strategy);
        });

    }
}