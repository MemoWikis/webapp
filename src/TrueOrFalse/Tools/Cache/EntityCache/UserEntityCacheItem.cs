using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static System.String;

public class UserCacheItem
{
    public int UserId;
    public string Name;
    public string EmailAddress;
    public string FacebookId;
    public bool IsFacebookUser => !IsNullOrEmpty(FacebookId);
    public string GoogleId;
    public bool IsGoogleUser => !IsNullOrEmpty(GoogleId);
    public virtual bool IsMemuchoUser => Settings.MemuchoUserId == UserId;
    public int Reputation;
    public int ReputationPos;
    public int FollowerCount;
    public bool ShowWishKnowledge;

    public int StartTopicId;

    public virtual IList<Membership> MembershipPeriods { get; set; }
    public virtual bool IsMember()
    {
        if (MembershipPeriods.Count == 0)
            return false;

        return MembershipPeriods.Any(x => x.IsActive(DateTime.Now));
    }

    public static UserCacheItem ToCacheUser(User user)
    {
        return new UserCacheItem
        {
            UserId = user.Id,
            Name = user.Name,
            EmailAddress = user.EmailAddress,
            FacebookId = user.FacebookId,
            GoogleId = user.GoogleId,
            Reputation = user.Reputation,
            ReputationPos = user.ReputationPos,
            FollowerCount = user.FollowerCount,
            ShowWishKnowledge = user.ShowWishKnowledge,

            StartTopicId = user.StartTopicId,
        };
    }

    public static IEnumerable<UserCacheItem> ToCacheUsers(IEnumerable<User> users) => users.Select(ToCacheUser);

}