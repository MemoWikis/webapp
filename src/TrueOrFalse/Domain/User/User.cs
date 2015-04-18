using System;
using System.Diagnostics;
using Seedworks.Lib.Persistence;

[Serializable]
[DebuggerDisplay("Id={Id} Name={Name}")]
public class User : DomainEntity
{
    public virtual string PasswordHashedAndSalted { get; set; }
    public virtual string Salt { get; set; }
    public virtual string EmailAddress { get; set; }
    public virtual string Name { get; set; }
    public virtual Boolean IsEmailConfirmed { get; set;  }
    public virtual Boolean IsInstallationAdmin { get; set; }
    public virtual Boolean AllowsSupportiveLogin { get; set; }
    public virtual DateTime? Birthday { get; set; }
    public virtual int Reputation { get; set; }
    public virtual int ReputationPos { get; set; }
    public virtual int WishCountQuestions { get; set; }
    public virtual int WishCountSets { get; set; }
    public virtual bool ShowWishKnowledge { get; set; }
}