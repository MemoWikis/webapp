﻿using Seedworks.Lib.Persistence;
using System.Diagnostics;
using static System.String;

[Serializable]
[DebuggerDisplay("Id={Id} Name={Name}")]
public class User : DomainEntity, IUserTinyModel
{
    private bool _isFacebookUser;
    private bool _isGoogleUser;

    public User()
    {
        Followers = new List<FollowerInfo>();
        Following = new List<FollowerInfo>();
    }
    public virtual int ActivityLevel { get; set; }

    public virtual int ActivityPoints { get; set; }
    public virtual bool AllowsSupportiveLogin { get; set; }
    public virtual DateTime? Birthday { get; set; }
    public virtual bool BouncedMail { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    /// <summary>
    /// End of premium period
    /// </summary>
    public virtual DateTime? EndDate { get; set; }

    public virtual IList<FollowerInfo> Followers { get; set; }

    /// <summary>Users I follow</summary>
    public virtual IList<FollowerInfo> Following { get; set; }

    public virtual bool IsEmailConfirmed { get; set; }
    public virtual bool IsInstallationAdmin { get; set; }
    public virtual UserSettingNotificationInterval KnowledgeReportInterval { get; set; }
    public virtual string LearningSessionOptions { get; set; }
    public virtual string MailBounceReason { get; set; }
    public virtual string PasswordHashedAndSalted { get; set; }

    public virtual string RecentlyUsedRelationTargetPages { get; set; }
    public virtual int ReputationPos { get; set; }
    public virtual string Salt { get; set; }
    public virtual int StartPageId { get; set; }
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

    public virtual string? WikiIds { get; set; }
    public virtual string? FavoriteIds { get; set; }
    public virtual string UiLanguage { get; set; } = "en";

    public virtual int FreeTokens { get; set; }
    public virtual int PaidTokens { get; set; }
    public virtual DateTime? FreeTokenResetDate { get; set; }

    public virtual bool IsStartPagePageId(int pageId)
    {
        return pageId == StartPageId;
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