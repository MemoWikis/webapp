using FluentNHibernate.Mapping;

public class AiModelWhitelistMap : ClassMap<AiModelWhitelist>
{
    public AiModelWhitelistMap()
    {
        Id(x => x.Id);
        Map(x => x.ModelId).Length(100).Not.Nullable();
        Map(x => x.DisplayName).Length(200);
        Map(x => x.Provider).CustomType<AiModelProvider>();
        Map(x => x.TokenCostMultiplier).Precision(10).Scale(2);
        Map(x => x.IsEnabled);

        Table("aimodelwhitelist");
    }
}
