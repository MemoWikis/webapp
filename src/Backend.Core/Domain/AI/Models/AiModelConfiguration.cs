using FluentNHibernate.Mapping;

/// <summary>
/// Whitelist of AI models that users can use.
/// TokenCostMultiplier determines how many memowikis tokens a model costs (e.g., 3 = 3x the base token rate).
/// </summary>
public class AiModelWhitelist : Entity
{
    public virtual string ModelId { get; set; } = string.Empty;
    public virtual AiModelProvider Provider { get; set; }
    
    /// <summary>
    /// Multiplier for memowikis token cost. E.g., 1 = base rate, 3 = 3x base rate
    /// </summary>
    public virtual decimal TokenCostMultiplier { get; set; } = 1m;
    
    public virtual bool IsEnabled { get; set; } = true;
    public virtual bool IsDefault { get; set; }
    public virtual int SortOrder { get; set; }
}

public class AiModelWhitelistMap : ClassMap<AiModelWhitelist>
{
    public AiModelWhitelistMap()
    {
        Id(x => x.Id);
        Map(x => x.ModelId).Length(100).Not.Nullable();
        Map(x => x.Provider).CustomType<AiModelProvider>();
        Map(x => x.TokenCostMultiplier).Precision(10).Scale(2);
        Map(x => x.IsEnabled);
        Map(x => x.IsDefault);
        Map(x => x.SortOrder);
        
        Table("aimodelwhitelist");
    }
}
