using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VueApp;

public class SideSheetController(
    SessionUser _sessionUser,
    WikiCreator _wikiCreator,
    UserWritingRepo _userWritingRepo,
    ExtendedUserCache _extendedUserCache) : Controller
{
    // Section: Wikis

    [HttpGet]
    public IList<WikiItem> GetWikis()
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
        {
            return new List<WikiItem>();
        }

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        var wikis = userCacheItem.Wikis
            .Select(w => new WikiItem(w.Id, w.Name, w.Parents().Any()))
            .ToList();
        var userStartPage = EntityCache.GetPage(userCacheItem.StartPageId);
        wikis.Insert(0, new WikiItem(userStartPage.Id, userStartPage.Name, userStartPage.Parents().Any()));
        return wikis;
    }

    public readonly record struct WikiItem(int Id, string Name, bool HasParents);

    [HttpPost]
    public CreateWikiResponse CreateWiki([FromBody] CreateWikiRequest req)
    {
        if (req == null || string.IsNullOrEmpty(req.Name))
        {
            throw new ArgumentNullException(nameof(req), "Invalid request");
        }

        var createWikiResult = _wikiCreator.Create(req.Name, _sessionUser);

        if (!createWikiResult.Success)
        {
            return new CreateWikiResponse(false, createWikiResult.MessageKey);
        }

        if (createWikiResult.TinyWikiItem == null)
        {
            return new CreateWikiResponse(false, createWikiResult.MessageKey);
        }

        var newWiki = createWikiResult.TinyWikiItem ?? throw new Exception("Failed to create wiki.");

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        userCacheItem.AddWiki(newWiki.Id);

        return new CreateWikiResponse(true, null, new CreateWikiData(newWiki.Id, newWiki.Name));
    }

    public readonly record struct CreateWikiRequest(string Name);
    public readonly record struct CreateWikiResponse(bool Success, string? MessageKey = null, CreateWikiData? Data = null);
    public readonly record struct CreateWikiData(int Id, string Name);


    // Section: Favorites

    [HttpGet]
    public IList<FavoriteItem> GetFavorites()
    {
        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);

        var favorites = userCacheItem.Favorites
            .Select(f => new FavoriteItem(f.Id, f.Name))
            .ToList();
        return favorites;
    }

    public readonly record struct FavoriteItem(int Id, string Name);

    [HttpPost]
    public AddToFavoriteResponse AddToFavorites([FromRoute] int id)
    {
        if (id <= 0)
        {
            return new AddToFavoriteResponse(false, FrontendMessageKeys.Error.Default);
        }
        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);

        userCacheItem.AddFavorite(id);

        EntityCache.AddOrUpdate(userCacheItem);
        _userWritingRepo.Update(userCacheItem);

        return new AddToFavoriteResponse(true);
    }
    public readonly record struct AddToFavoriteResponse(bool Success, string? MessageKey = null);

    [HttpPost]
    public void RemoveFavorites([FromBody] RemoveFavoritesRequest req)
    {
        if (req == null || req.Id <= 0)
        {
            throw new ArgumentNullException(nameof(req), "Invalid request");
        }
        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        userCacheItem.RemoveFavorite(req.Id);

        EntityCache.AddOrUpdate(userCacheItem);
        _userWritingRepo.Update(userCacheItem);
    }

    public readonly record struct RemoveFavoritesRequest(int Id);

    // Section: Recent Pages

    [HttpGet]
    public IList<RecentPageItem> GetRecentPages()
    {
        if (!_sessionUser.IsLoggedIn)
            return new List<RecentPageItem>();

        var userCacheItem = _extendedUserCache.GetUser(_sessionUser.UserId);

        var recentPages = userCacheItem.RecentPages?.GetRecentPages()
            .Select(rp => new RecentPageItem(rp.Name, rp.Id))
            .ToList();
        return recentPages;
    }

    public readonly record struct RecentPageItem(string Name, int Id);
}
