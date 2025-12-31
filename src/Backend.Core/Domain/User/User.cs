using System.Diagnostics;
using static System.String;

[Serializable]
[DebuggerDisplay("Id={Id} Name={Name}")]
public class User : DomainEntity, IUserTinyModel
{
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

    public virtual IList<FollowerInfo> Followers { get; set; } = new List<FollowerInfo>();

    /// <summary>Users I follow</summary>
    public virtual IList<FollowerInfo> Following { get; set; } = new List<FollowerInfo>();

    public virtual bool IsEmailConfirmed { get; set; }
    public virtual bool IsInstallationAdmin { get; set; }
    public virtual UserSettingNotificationInterval KnowledgeReportInterval { get; set; }
    public virtual string LearningSessionOptions { get; set; }
    public virtual string MailBounceReason { get; set; }
    public virtual string PasswordHashedAndSalted { get; set; }

    public virtual string RecentlyUsedRelationTargetPages { get; set; }
    public virtual int ReputationPos { get; set; }
    public virtual string Salt { get; set; }
    public virtual string StripeId { get; set; }
    public virtual DateTime? SubscriptionStartDate { get; set; }
    public virtual int TotalInOthersWishKnowledge { get; set; }
    public virtual string WidgetHostsSpaceSeparated { get; set; }
    public virtual int WishCountQuestions { get; set; }
    public virtual int WishCountSets { get; set; }
    public virtual string EmailAddress { get; set; }
    public virtual string Name { get; set; }
    public virtual int Reputation { get; set; }
    public virtual bool ShowWishKnowledge { get; set; }
    public virtual string? AboutMeText { get; set; }
    public virtual bool IsFacebookUser => !IsNullOrEmpty(FacebookId);
    public virtual bool IsGoogleUser => !IsNullOrEmpty(GoogleId);
    public virtual int FollowerCount { get; set; }

    public virtual string FacebookId { get; set; }
    public virtual string GoogleId { get; set; }

    public virtual string? FavoriteIds { get; set; }
    public virtual string UiLanguage { get; set; } = "en";
    
    /// <summary>
    /// Remaining tokens from subscription for current period
    /// </summary>
    public virtual int SubscriptionTokensBalance { get; set; } = 0;
    
    /// <summary>
    /// Purchased tokens that don't expire
    /// </summary>
    public virtual int PaidTokensBalance { get; set; } = 0;

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