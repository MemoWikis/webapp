public class SchemaVersion
{
    public virtual int Id { get; set; }
    public virtual string SchemaHash { get; set; } = string.Empty;

    // Last time the schema was updated
    public virtual DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
