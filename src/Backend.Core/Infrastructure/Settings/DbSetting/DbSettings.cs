public class DbSettings : DomainEntity
{
    public virtual int AppVersion { get; set; }
    public virtual string SuggestedGames { get; set; }
    public virtual string SuggestedSetsIdString { get; set; }
}