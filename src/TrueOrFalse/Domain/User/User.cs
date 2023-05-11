using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Seedworks.Lib.Persistence;
using static System.String;


[Serializable]
[DebuggerDisplay("Id={Id} Name={Name}")]
public class User : DomainEntity, IUserTinyModel
{
    private bool _isFacebookUser;
    private bool _isGoogleUser;

    public User()
    {
        MembershipPeriods = new List<Membership>();
        Followers = new List<FollowerInfo>();
        Following = new List<FollowerInfo>();
    }

    public virtual bool IsBeltz => 356 == Id;

    public virtual bool IsMemuchoUser => Settings.MemuchoUserId == Id;
    public virtual int ActivityLevel { get; set; }

    public virtual int ActivityPoints { get; set; }
    public virtual bool AllowsSupportiveLogin { get; set; }
    public virtual DateTime? Birthday { get; set; }
    public virtual bool BouncedMail { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }
    public virtual DateTime? EndDate { get; set; }

    public virtual IList<FollowerInfo> Followers { get; set; }

    /// <summary>Users I follow</summary>
    public virtual IList<FollowerInfo> Following { get; set; }

    public virtual bool IsEmailConfirmed { get; set; }
    public virtual bool IsInstallationAdmin { get; set; }
    public virtual UserSettingNotificationInterval KnowledgeReportInterval { get; set; }
    public virtual string LearningSessionOptions { get; set; }
    public virtual string MailBounceReason { get; set; }
    public virtual IList<Membership> MembershipPeriods { get; set; }
    public virtual string PasswordHashedAndSalted { get; set; }

    public virtual string RecentlyUsedRelationTargetTopics { get; set; }
    public virtual int ReputationPos { get; set; }
    public virtual string Salt { get; set; }
    public virtual int StartTopicId { get; set; }
    public virtual string StripeId { get; set; }
    public virtual DateTime? SubscriptionStartDate { get; set; }
    public virtual int TotalInOthersWishknowledge { get; set; }
    public virtual string WidgetHostsSpaceSeparated { get; set; }
    public virtual int WishCountQuestions { get; set; }
    public virtual int WishCountSets { get; set; }
    public virtual string EmailAddress { get; set; }
    public virtual string Name { get; set; }
    public virtual int Reputation { get; set; }
    public virtual bool ShowWishKnowledge { get; set; }
    public virtual bool IsFacebookUser => !IsNullOrEmpty(FacebookId);
    public virtual bool IsGoogleUser => !IsNullOrEmpty(GoogleId);
    public virtual int FollowerCount { get; set; }

    public virtual string FacebookId { get; set; }
    public virtual string GoogleId { get; set; }

    public virtual void AddFollower(User follower)
    {
        Followers.Add(new FollowerInfo
            { Follower = follower, User = this, DateCreated = DateTime.Now, DateModified = DateTime.Now });
        Sl.UserRepo.Flush();
        UserActivityAdd.FollowedUser(follower, this);
        UserActivityUpdate.NewFollower(follower, this);
        ReputationUpdate.ForUser(this);
    }

    public virtual Membership CurrentMembership()
    {
        return MembershipPeriods.FirstOrDefault(x => x.IsActive());
    }

    public virtual IList<int> FollowerIds()
    {
        return Followers.Select(f => f.Follower.Id).ToList();
    }

    public virtual IList<int> FollowingIds()
    {
        return Following.Select(f => f.User.Id).ToList();
    }

    public virtual bool IsMember()
    {
        if (MembershipPeriods == null || MembershipPeriods.Count == 0)
        {
            return false;
        }

        return MembershipPeriods.Any(x => x.IsActive(DateTime.Now));
    }

    public virtual bool IsStartTopicTopicId(int categoryId)
    {
        return categoryId == StartTopicId;
    }

    /// <summary>Joined list of FollowerIds and FollowingIds</summary>
    public virtual IList<int> NetworkIds()
    {
        return FollowerIds().Union(FollowingIds()).ToList();
    }

    public virtual IList<string> WidgetHosts()
    {
        if (IsNullOrEmpty(WidgetHostsSpaceSeparated))
        {
            return new List<string>();
        }

        return WidgetHostsSpaceSeparated
            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim()).ToList();
    }
}

public class FacebookUserCreateParameter
{
    public string email { get; set; }

    /// <summary>
    ///     Facebook user id
    /// </summary>
    public string id { get; set; }

    public string name { get; set; }
}

public class GoogleUserCreateParameter
{
    public string Email { get; set; }
    public string GoogleId { get; set; }
    public string ProfileImage { get; set; }
    public string UserName { get; set; }
}