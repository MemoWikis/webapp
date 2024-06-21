using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace VueApp;

public class AppController(
    FrontEndUserData _frontEndUserData,
    SessionUser _sessionUser,
    PersistentLoginRepo _persistentLoginRepo,
    UserReadingRepo _userReadingRepo) : BaseController(_sessionUser)
{
    public record struct SessionStartResult(bool success, string? loginGuid = null, DateTimeOffset? expiryDate = null, bool alreadyLoggedIn = false);

    public record struct SessionStartParam(string sessionStartGuid);
    [HttpPost]
    public SessionStartResult SessionStart([FromBody] SessionStartParam param)
    {
        var cookieString = Request.Cookies[PersistentLoginCookie.Key];
        if (cookieString != null && !IsLoggedIn && param.sessionStartGuid == Settings.NuxtSessionStartGuid)
        {
            var loginResult = LoginFromCookie.Run(_sessionUser, _persistentLoginRepo, _userReadingRepo, cookieString);
            if (loginResult.Success)
                return new SessionStartResult(true, loginResult.LoginGuid, loginResult.ExpiryDate);

            return new SessionStartResult(false);
        }
        if (IsLoggedIn)
        {
            return new SessionStartResult(false, alreadyLoggedIn: true);
        }
        return new SessionStartResult(false);

    }

    public readonly record struct GetCurrentUserResult(
        bool IsLoggedIn,
        int Id,
        string Name,
        string Email,
        bool IsAdmin,
        int PersonalWikiId,
        UserType Type,
        string ImgUrl,
        int Reputation,
        int ReputationPos,
        TopicDataManager.TopicDataResult PersonalWiki,
        ActivityPoints ActivityPoints,
        int UnreadMessagesCount,
        SubscriptionType SubscriptionType,
        bool HasStripeCustomerId,
        string EndDate,
        string SubscriptionStartDate,
        bool IsSubscriptionCanceled,
        bool IsEmailConfirmed);

    public record struct ActivityPoints(
        int Points,
        int Level,
        bool LevelUp,
        int ActivityPointsTillNextLevel,
        int ActivityPointsPercentageOfNextLevel);

    [HttpGet]
    public GetCurrentUserResult GetCurrentUser()
    {
        var currentUser = _frontEndUserData.Get();

        return new GetCurrentUserResult
        {
            IsLoggedIn = currentUser.IsLoggedIn,
            Id = currentUser.Id,
            Name = currentUser.Name,
            Email = currentUser.Email,
            IsAdmin = currentUser.IsAdmin,
            PersonalWikiId = currentUser.PersonalWikiId,
            Type = currentUser.Type,
            ImgUrl = currentUser.ImgUrl,
            Reputation = currentUser.Reputation,
            ReputationPos = currentUser.ReputationPos,
            PersonalWiki = currentUser.PersonalWiki,
            ActivityPoints = new ActivityPoints
            {
                Points = currentUser.ActivityPoints.Points,
                Level = currentUser.ActivityPoints.Level,
                LevelUp = currentUser.ActivityPoints.LevelUp,
                ActivityPointsPercentageOfNextLevel =
                    currentUser.ActivityPoints.ActivityPointsPercentageOfNextLevel,
                ActivityPointsTillNextLevel = currentUser.ActivityPoints.ActivityPointsTillNextLevel
            },
            UnreadMessagesCount = currentUser.UnreadMessagesCount,
            SubscriptionType = currentUser.SubscriptionType,
            HasStripeCustomerId = currentUser.HasStripeCustomerId,
            EndDate = currentUser.EndDate,
            SubscriptionStartDate = currentUser.SubscriptionStartDate,
            IsSubscriptionCanceled = currentUser.IsSubscriptionCanceled,
            IsEmailConfirmed = currentUser.IsEmailConfirmed
        };
    }

    public readonly record struct GetFooterTopicsResult(
        TinyTopicItem RootWiki,
        TinyTopicItem[] MainTopics,
        TinyTopicItem MemoWiki,
        TinyTopicItem[] MemoTopics,
        TinyTopicItem[] HelpTopics,
        TinyTopicItem[] PopularTopics,
        TinyTopicItem Documentation);

    public readonly record struct TinyTopicItem(int Id, string Name);

    [HttpGet]
    public GetFooterTopicsResult GetFooterTopics()
    {
        var footerTopics = new GetFooterTopicsResult
        (
            RootWiki: new TinyTopicItem
            (
                Id: RootCategory.RootCategoryId,
                Name: EntityCache.GetCategory(RootCategory.RootCategoryId)?.Name
            ),
            MainTopics: RootCategory.MainCategoryIds.Select(id => new TinyTopicItem(
                Id: id,
                Name: EntityCache.GetCategory(id).Name
            )).ToArray(),
            MemoWiki: new TinyTopicItem
            (
                Id: RootCategory.MemuchoWikiId,
                Name: EntityCache.GetCategory(RootCategory.MemuchoWikiId).Name
            ),
            MemoTopics: RootCategory.MemuchoCategoryIds.Select(id => new TinyTopicItem(
                Id: id,
                Name: EntityCache.GetCategory(id).Name
            )).ToArray(),
            HelpTopics: RootCategory.MemuchoHelpIds.Select(id => new TinyTopicItem(
                Id: id,
                Name: EntityCache.GetCategory(id).Name
            )).ToArray(),
            PopularTopics: RootCategory.PopularCategoryIds.Select(id => new TinyTopicItem(
                Id: id,
                Name: EntityCache.GetCategory(id).Name
            )).ToArray(),
            Documentation: new TinyTopicItem(
                Id: RootCategory.IntroCategoryId,
                Name: EntityCache.GetCategory(RootCategory.IntroCategoryId).Name
            )
        );
        return footerTopics;
    }
}