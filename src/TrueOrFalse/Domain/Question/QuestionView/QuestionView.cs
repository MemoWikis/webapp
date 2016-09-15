using System;
using Seedworks.Lib.Persistence;

public class QuestionView : IPersistable, WithDateCreated
{
    public virtual int Id { get; set; }
    public virtual Guid Guid { get; set; }
    public virtual string GuidString
    {
        get { return Guid == Guid.Empty ? null : Guid.ToString(); }
        set
        {
            if (value == null)
            {
                Guid = Guid.Empty;
                return;
            }

            Guid = new Guid(value);
        }
    }

    public virtual int QuestionId { get; set; }
    public virtual int UserId { get; set; }

    public virtual int Milliseconds { get; set; }

    public virtual Player Player { get; set; }
    public virtual Round Round { get; set; }
    public virtual LearningSession LearningSession { get; set; }

    public virtual Guid LearningSessionStepGuid { get; set; }
    public virtual string LearningSessionStepGuidString
    {
        get { return LearningSessionStepGuid == Guid.Empty ? null : LearningSessionStepGuid.ToString(); }
        set
        {
            if (value == null)
            {
                LearningSessionStepGuid = Guid.Empty;
                return;
            }

            LearningSessionStepGuid = new Guid(value);
        }
    }

    public virtual bool Migrated { get; set; }

    public virtual DateTime DateCreated { get; set; }
}