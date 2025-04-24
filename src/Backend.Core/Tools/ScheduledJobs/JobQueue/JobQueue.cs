using Seedworks.Lib.Persistence;


public class JobQueue : Entity
{
    public virtual JobQueueType JobQueueType { get; set; }
    public virtual string JobContent { get; set; }
    public virtual int Priority { get; set; }
    public virtual DateTime DateCreated { get; set; }
}