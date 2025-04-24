using Seedworks.Lib.Persistence;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class QuestionView : IPersistable, WithDateCreated
{
    public virtual int Id { get; set; }
    public virtual Guid Guid { get; set; }
    public virtual string GuidString
    {
        get => Guid == Guid.Empty ? null : Guid.ToString();
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
    public virtual string UserAgent { get; set; }
    public virtual bool Migrated { get; set; }
    public virtual DateTime DateCreated { get; set; }
    //that it only uses Date and not exact time
    public virtual DateTime DateOnly { get; set; }
}