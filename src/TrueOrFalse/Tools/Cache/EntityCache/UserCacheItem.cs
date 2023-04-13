using System;
using System.Collections.Generic;
using System.Linq;
using static System.String;

public class UserCacheItem : IUserTinyModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string EmailAddress { get; set; }
    public string FacebookId { get; set; }
    public bool IsFacebookUser => !IsNullOrEmpty(FacebookId);
    public string GoogleId { get; set; }
    public bool IsGoogleUser => !IsNullOrEmpty(GoogleId);
    public bool IsMemuchoUser => Settings.MemuchoUserId == Id;
    public int Reputation { get; set; }
    public int ReputationPos;
    public int FollowerCount { get; set; }
    public bool ShowWishKnowledge { get; set; }

    public int StartTopicId;
    public int WishCountQuestions;

    public bool AllowsSupportiveLogin { get; set; }

    public int CorrectnessProbability { get; set; }

    public UserSettingNotificationInterval KnowledgeReportInterval { get; set; }

    public bool IsMember { get; set; }
    public Membership CurrentMembership { get; set; }

    public int TotalInOthersWishknowledge { get; set; }

    public IList<int> FollowerIds { get; set; }
    /// <summary>Users I follow</summary>
    public IList<int> FollowingIds { get; set; }
    public string StripeId { get; set; }

    public static UserCacheItem ToCacheUser(User user)
    {
        var userCacheItem = new UserCacheItem();

        if (user != null)
        {
            userCacheItem.AssignValues(user);
        }

        return userCacheItem;
    }

    public void AssignValues(User user)
    {
        Id = user.Id;
        Name = user.Name;
        EmailAddress = user.EmailAddress;
        FacebookId = user.FacebookId;
        GoogleId = user.GoogleId;
        Reputation = user.Reputation;
        ReputationPos = user.ReputationPos;
        FollowerCount = user.FollowerCount;
        ShowWishKnowledge = user.ShowWishKnowledge;

        StartTopicId = user.StartTopicId;
        WishCountQuestions = user.WishCountQuestions;
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;
        KnowledgeReportInterval = user.KnowledgeReportInterval;
        RecentlyUsedRelationTargetTopics = user.RecentlyUsedRelationTargetTopics;
        WidgetHostsSpaceSeparated = user.WidgetHostsSpaceSeparated;
        CorrectnessProbability = user.CorrectnessProbability;
        IsMember = user.IsMember();
        CurrentMembership = user.MembershipPeriods.FirstOrDefault(x => x.IsActive());
        TotalInOthersWishknowledge = user.TotalInOthersWishknowledge;
        FollowerIds = user.Followers.Select(f => f.Follower.Id).ToList();
        FollowingIds = user.Following.Select(f => f.User.Id).ToList();
        StripeId = user.StripeId;
    }

    public virtual string WidgetHostsSpaceSeparated { get; set; }

    public virtual IList<string> WidgetHosts()
    {
        if (string.IsNullOrEmpty(WidgetHostsSpaceSeparated))
            return new List<string>();

        return WidgetHostsSpaceSeparated
            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim()).ToList();
    }

    public static IEnumerable<UserCacheItem> ToCacheUsers(IEnumerable<User> users) => users.Select(ToCacheUser);

    public virtual string RecentlyUsedRelationTargetTopics { get; set; }    
    public virtual List<int> RecentlyUsedRelationTargetTopicIds => RecentlyUsedRelationTargetTopics?
        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Convert.ToInt32(x)).Distinct()
        .ToList();
}