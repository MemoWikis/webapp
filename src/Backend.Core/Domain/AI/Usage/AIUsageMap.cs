using FluentNHibernate.Mapping;

public class AiUsageLogMap : ClassMap<AiUsageLog>
{
    public AiUsageLogMap()
    {
        Table("ai_usage_log");

        Id(x => x.Id);

        Map(x => x.UserId).Column("User_id");
        Map(x => x.PageId).Column("Page_id");
        Map(x => x.TokenIn);
        Map(x => x.TokenOut);
        Map(x => x.DateCreated);
        Map(x => x.Model);
    }
}