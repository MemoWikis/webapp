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
    public virtual bool IsEmailConfirmed { get; set;  }
    public virtual bool IsInstallationAdmin { get; set; }
    public virtual bool AllowsSupportiveLogin { get; set; }
    public virtual DateTime? Birthday { get; set; }
    public virtual int Reputation { get; set; }
    public virtual int ReputationPos { get; set; }
    public virtual int WishCountQuestions { get; set; }
    public virtual int WishCountSets { get; set; }
    public virtual bool ShowWishKnowledge { get; set; }
    public virtual IList<Membership> MembershipPeriods { get; set; }

    public virtual IList<User> Followers { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    /// <summary>Users I follow</summary>
    public virtual IList<User> Following { get; set; }

    public virtual void AddFollower(User userFollows)
    {
        Followers.Add(userFollows);
        Sl.R<UserRepo>().Flush();
        UserActivityAdd.FollowedUser(userFollows, this);
    }
    public virtual IList<int> FollowerIds(){
        return Followers.Select(f => f.Id).ToList();
    }

    public virtual IList<int> FollowingIds(){
        return Following.Select(f => f.Id).ToList();
    }

    /// <summary>Joined list of FollowerIds and FollowingIds</summary>
    public virtual IList<int> NetworkIds(){
        return FollowerIds().Union(FollowingIds()).ToList();
    }

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