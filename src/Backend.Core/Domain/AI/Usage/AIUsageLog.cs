
public class AiUsageLog : Entity, WithDateCreated
{
    public virtual int UserId { get; set; }
    public virtual int PageId { get; set; }
    public virtual int TokenIn { get; set; }
    public virtual int TokenOut { get; set; }
    public virtual DateTime DateCreated { get; set; }
    public virtual string Model { get; set; }
}