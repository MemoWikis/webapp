using FluentNHibernate.Mapping;

public class TrainingDateMap : ClassMap<TrainingDate>
{
    public TrainingDateMap()
    {
        Id(x => x.Id);

        References(x => x.TrainingPlan)
            .Cascade.None().ReadOnly();

        Map(x => x.DateTime);

        Map(x => x.ExpiresAt);
        
        Map(x => x.AllQuestionsJson).CustomSqlType("varchar(8000)");


        References(x => x.LearningSession);

        Map(x => x.NotificationStatus);

        Map(x => x.IsBoostingDate);

        Map(x => x.MarkedAsMissed);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}