using System.Diagnostics;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("Id={Id} Title={Text}")]
public class Message : DomainEntity
{
    public virtual int ReceiverId { get; set; }
    public virtual string Subject { get; set; }
    public virtual string Body { get; set;  }

    public virtual string MessageType { get; set; }
        
    public virtual bool IsRead { get; set; }

    public virtual bool WasSendPerEmail { get; set; }
    public virtual bool WasSendToSmartphone { get; set; }
}