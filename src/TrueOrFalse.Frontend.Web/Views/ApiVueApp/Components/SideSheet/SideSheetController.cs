using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VueApp;

public class SideSheetController(
    SessionUser _sessionUser,
    WikiCreator _wikiCreator,
    UserWritingRepo _userWritingRepo) : Controller
{
    // Section: Wikis

    [HttpGet]
    public GetWikisResponse GetWikis()
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
        {
            return new GetWikisResponse(new List<WikiItem>());
        }

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        var wikis = userCacheItem.Wikis
            .Select(w => new WikiItem(w.Id, w.Name, w.Parents().Any()))
            .ToList();
        return new GetWikisResponse(wikis);
    }

    public readonly record struct GetWikisResponse(IList<WikiItem> Wikis);
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
    public GetFavoritesResponse GetFavorites()
    {
        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);

        var favorites = userCacheItem.Favorites
            .Select(f => new FavoriteItem(f.Id, f.Name))
            .ToList();
        return new GetFavoritesResponse(favorites);
    }

    public readonly record struct GetFavoritesResponse(IList<FavoriteItem> Favorites);
    public readonly record struct FavoriteItem(int Id, string Name);

    [HttpPost]
    public void AddToFavorites([FromBody] AddToFavoritesRequest req)
    {
        if (req == null || req.Id <= 0)
        {
            throw new ArgumentNullException(nameof(req), "Invalid request");
        }
        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);

        userCacheItem.AddFavorite(req.Id);

        EntityCache.AddOrUpdate(userCacheItem);
        _userWritingRepo.Update(userCacheItem);
    }

    public readonly record struct AddToFavoritesRequest(int Id);

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
    public GetRecentPagesResponse GetRecentPages()
    {
        if (!_sessionUser.IsLoggedIn)
            return new GetRecentPagesResponse(new List<RecentPageItem>());

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);

        var recentPages = userCacheItem.RecentPages?.GetRecentPages()
            .Select(rp => new RecentPageItem(rp.Name, rp.Id))
            .ToList();
        return new GetRecentPagesResponse(recentPages);
    }

    public readonly record struct GetRecentPagesResponse(IList<RecentPageItem> RecentPages);
    public readonly record struct RecentPageItem(string Name, int Id);
}
