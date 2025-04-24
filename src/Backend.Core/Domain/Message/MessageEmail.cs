using System.Diagnostics;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("Id={Id} User={User.Name} MessageType={MessageEmailType}")]
public class MessageEmail : DomainEntity
{

    public virtual User User { get; set; }

    public virtual MessageEmailTypes MessageEmailType { get; set; }

    public MessageEmail()
    {
        
    }

    public MessageEmail(User user, MessageEmailTypes messageEmailType)
    {
        User = user;
        MessageEmailType = messageEmailType;
    }


}