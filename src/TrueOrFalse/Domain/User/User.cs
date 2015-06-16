using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    public virtual IList<Membership> MembershipPeriods { get; set; }

    public virtual IList<User> Followers { get; set; }
    
    /// <summary>Users I follow</summary>
    public virtual IList<User> Following { get; set; }

    public User()
    {
        MembershipPeriods = new List<Membership>();
        Followers = new List<User>();
        Following = new List<User>();
    }

    public virtual bool IsMember()
    {
        if (MembershipPeriods.Count == 0)
            return false;

        return MembershipPeriods.Any(x => x.IsActive(DateTime.Now));
    }

    public virtual Membership CurrentMembership()
    {
        return MembershipPeriods.FirstOrDefault(x => x.IsActive());
    }
}