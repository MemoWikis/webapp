using System;
using System.Collections.Concurrent;
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
    public virtual bool IsMemuchoUser => Settings.MemuchoUserId == Id;
    public int Reputation { get; set; }
    public int ReputationPos;
    public int FollowerCount { get; set; }
    public bool ShowWishKnowledge { get; set; }

    public int StartTopicId;
    public int WishCountQuestions;

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
            Id = user.Id,
            Name = user.Name,
            EmailAddress = user.EmailAddress,
            FacebookId = user.FacebookId,
            GoogleId = user.GoogleId,
            Reputation = user.Reputation,
            ReputationPos = user.ReputationPos,
            FollowerCount = user.FollowerCount,
            ShowWishKnowledge = user.ShowWishKnowledge,

            StartTopicId = user.StartTopicId,
            WishCountQuestions = user.WishCountQuestions,
        };
    }

    public static IEnumerable<UserCacheItem> ToCacheUsers(IEnumerable<User> users) => users.Select(ToCacheUser);

    public virtual string RecentlyUsedRelationTargetTopics { get; set; }    
    public virtual List<int> RecentlyUsedRelationTargetTopicIds => RecentlyUsedRelationTargetTopics?
        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Convert.ToInt32(x)).Distinct()
        .ToList();
}