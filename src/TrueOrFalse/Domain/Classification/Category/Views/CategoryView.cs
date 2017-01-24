using System;
using Seedworks.Lib.Persistence;

public class CategoryView : IPersistable, WithDateCreated
{
    public virtual int Id { get; set; }

    public virtual Category Category { get; set; }
    public virtual User User { get; set; }

    public virtual string UserAgent { get; set; }

    public virtual DateTime DateCreated { get; set; }
}