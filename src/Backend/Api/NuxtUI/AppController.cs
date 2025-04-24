using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace VueApp;

public class AppController(FrontEndUserData _frontEndUserData) : Controller
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
        PageDataManager.PageDataResult PersonalWiki,
        ActivityPoints ActivityPoints,
        int UnreadMessagesCount,
        SubscriptionType SubscriptionType,
        bool HasStripeCustomerId,
        string EndDate,
        string SubscriptionStartDate,
        bool IsSubscriptionCanceled,
        bool IsEmailConfirmed,
        string CollaborationToken,
        string UiLanguage);

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
            IsEmailConfirmed = currentUser.IsEmailConfirmed,
            CollaborationToken = currentUser.CollaborationToken,
            UiLanguage = currentUser.UiLanguage
        };
    }

    public readonly record struct GetFooterPagesResult(
        TinyPageItem RootWiki,
        TinyPageItem[] MainPages,
        TinyPageItem MemoWiki,
        TinyPageItem[] MemoPages,
        TinyPageItem[] HelpPages,
        TinyPageItem[] PopularPages,
        TinyPageItem Documentation);

    public readonly record struct TinyPageItem(int Id, string Name);

    [HttpGet]
    public GetFooterPagesResult GetFooterPages()
    {
        var footerPages = new GetFooterPagesResult
        (
            RootWiki: new TinyPageItem
            (
                Id: FeaturedPage.RootPageId,
                Name: EntityCache.GetPage(FeaturedPage.RootPageId)?.Name
            ),
            MainPages: FeaturedPage.MainPageIds.Select(id => new TinyPageItem(
                Id: id,
                Name: EntityCache.GetPage(id).Name
            )).ToArray(),
            MemoWiki: new TinyPageItem
            (
                Id: FeaturedPage.MemoWikisWikiId,
                Name: EntityCache.GetPage(FeaturedPage.MemoWikisWikiId).Name
            ),
            MemoPages: FeaturedPage.MemoWikisPageIds.Select(id => new TinyPageItem(
                Id: id,
                Name: EntityCache.GetPage(id).Name
            )).ToArray(),
            HelpPages: FeaturedPage.MemoWikisHelpIds.Select(id => new TinyPageItem(
                Id: id,
                Name: EntityCache.GetPage(id).Name
            )).ToArray(),
            PopularPages: FeaturedPage.PopularPageIds.Select(id => new TinyPageItem(
                Id: id,
                Name: EntityCache.GetPage(id).Name
            )).ToArray(),
            Documentation: new TinyPageItem(
                Id: FeaturedPage.IntroPageId,
                Name: EntityCache.GetPage(FeaturedPage.IntroPageId).Name
            )
        );
        return footerPages;
    }
}