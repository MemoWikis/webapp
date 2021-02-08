using FluentNHibernate.Mapping;

public class QuestionViewMap : ClassMap<QuestionView>
{
    public QuestionViewMap()
    {
        Id(x => x.Id);
        Map(x => x.GuidString).Column("Guid").CustomSqlType("varchar(36)").Unique();

        Map(x => x.QuestionId);
        Map(x => x.UserId);

        Map(x => x.Milliseconds);
        Map(x => x.UserAgent);

        References(x => x.WidgetView).Cascade.None();

        Map(x => x.Migrated);

        Map(x => x.DateCreated);
    }
}