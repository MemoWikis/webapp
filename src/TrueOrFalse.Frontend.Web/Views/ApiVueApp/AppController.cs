using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VueApp;

public class AppController(VueSessionUser _vueSessionUser) : Controller
{
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
        var sessionUser = _vueSessionUser.GetCurrentUserData();

        return new GetCurrentUserResult
        {
            IsLoggedIn = sessionUser.IsLoggedIn,
            Id = sessionUser.Id,
            Name = sessionUser.Name,
            Email = sessionUser.Email,
            IsAdmin = sessionUser.IsAdmin,
            PersonalWikiId = sessionUser.PersonalWikiId,
            Type = sessionUser.Type,
            ImgUrl = sessionUser.ImgUrl,
            Reputation = sessionUser.Reputation,
            ReputationPos = sessionUser.ReputationPos,
            PersonalWiki = sessionUser.PersonalWiki,
            ActivityPoints = new ActivityPoints
            {
                Points = sessionUser.ActivityPoints.Points,
                Level = sessionUser.ActivityPoints.Level,
                LevelUp = sessionUser.ActivityPoints.LevelUp,
                ActivityPointsPercentageOfNextLevel =
                    sessionUser.ActivityPoints.ActivityPointsPercentageOfNextLevel,
                ActivityPointsTillNextLevel = sessionUser.ActivityPoints.ActivityPointsTillNextLevel
            },
            UnreadMessagesCount = sessionUser.UnreadMessagesCount,
            SubscriptionType = sessionUser.SubscriptionType,
            HasStripeCustomerId = sessionUser.HasStripeCustomerId,
            EndDate = sessionUser.EndDate,
            SubscriptionStartDate = sessionUser.SubscriptionStartDate,
            IsSubscriptionCanceled = sessionUser.IsSubscriptionCanceled,
            IsEmailConfirmed = sessionUser.IsEmailConfirmed
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