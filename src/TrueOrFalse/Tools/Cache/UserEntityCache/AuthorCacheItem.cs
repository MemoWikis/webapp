using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.String;


[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class AuthorCacheItem
{
    public virtual int Id { get; set; }
    public virtual int Reputation { get; set; }
    public virtual int ReputationPos { get; set; }
    public virtual string Name { get; set; }
    public virtual string EmailAddress { get; set; }
    public virtual string FacebookId { get; set; }
    public virtual string GoogleId { get; set; }
    public virtual bool IsFacebookUser => !IsNullOrEmpty(FacebookId);
    public virtual bool IsGoogleUser => !IsNullOrEmpty(GoogleId);

    public static IList<AuthorCacheItem> FromUserTinyModels(IList<UserTinyModel> users) =>
        users.Select(FromUserTinyModel).ToList();
    public static AuthorCacheItem FromUserTinyModel(UserTinyModel user)
    {
        return new AuthorCacheItem()
        {
            Id = user.Id,
            Name = user.Name,
            Reputation = user.Reputation,
            ReputationPos = user.ReputationPos,
            EmailAddress = user.EmailAddress,
            FacebookId = user.FacebookId,
            GoogleId = user.GoogleId
        };
    }

    internal static AuthorCacheItem FromUserTinyModel(IUserTinyModel user)
    {
        return new AuthorCacheItem()
        {
            Id = user.Id,
            Name = user.Name,
            Reputation = user.Reputation,
            EmailAddress = user.EmailAddress,
            FacebookId = user.FacebookId,
            GoogleId = user.GoogleId
        };
    }
    internal static AuthorCacheItem FromUser(User user)
    {
        return new AuthorCacheItem()
        {
            Id = user.Id,
            Name = user.Name,
            Reputation = user.Reputation,
            EmailAddress = user.EmailAddress,
            FacebookId = user.FacebookId,
            GoogleId = user.GoogleId
        };
    }
}