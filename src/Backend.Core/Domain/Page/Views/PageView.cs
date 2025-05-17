public class PageView : IPersistable, WithDateCreated
{
    public virtual int Id { get; set; }
    public virtual Page Page { get; set; }
    public virtual User User { get; set; }
    public virtual string UserAgent { get; set; }

    public virtual DateTime DateCreated { get; set; }

    //that it only uses Date and not exact time
    public virtual DateTime DateOnly { get; set; }
}