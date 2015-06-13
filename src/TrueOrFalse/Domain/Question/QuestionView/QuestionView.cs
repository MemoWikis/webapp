using System;
using Seedworks.Lib.Persistence;

public class QuestionView : IPersistable, WithDateCreated
{
    public virtual int Id { get; set; }

    public virtual int QuestionId { get; set; }
    public virtual int UserId { get; set; }

    public virtual DateTime DateCreated { get; set; }
}