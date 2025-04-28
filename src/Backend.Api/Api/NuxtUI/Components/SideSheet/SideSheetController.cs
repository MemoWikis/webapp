public class SideSheetController(
    SessionUser _sessionUser,
    UserWritingRepo _userWritingRepo,
    ExtendedUserCache _extendedUserCache) : ApiBaseController
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
        userCacheItem.CleanupWikiIdsAndFavoriteIds();

        var wikis = userCacheItem.Wikis
            .Where(w => w != null && w.IsWiki)
            .Select(w => new WikiItem(w.Id, w.Name, w.Parents().Any()))
            .ToList();
        var userStartPage = EntityCache.GetPage(userCacheItem.StartPageId);
        wikis.Insert(0, new WikiItem(userStartPage.Id, userStartPage.Name, userStartPage.Parents().Any()));
        return wikis;
    }

    public readonly record struct WikiItem(int Id, string Name, bool HasParents);

    // Section: Favorites

    [HttpGet]
    public IList<FavoriteItem> GetFavorites()
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
        {
            return new List<FavoriteItem>();
        }

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        userCacheItem.CleanupWikiIdsAndFavoriteIds();

        var favorites = userCacheItem.Favorites
            .Where(f => f != null)
            .Select(f => new FavoriteItem(f.Id, f.Name))
            .ToList();
        return favorites;
    }

    public readonly record struct FavoriteItem(int Id, string Name);

    [HttpPost]
    public AddToFavoriteResponse AddToFavorites([FromRoute] int id)
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
        {
            return new AddToFavoriteResponse(false, FrontendMessageKeys.Error.User.NotLoggedIn);
        }
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
    public RemoveFromFavoritesResponse RemoveFromFavorites([FromRoute] int id)
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
        {
            return new RemoveFromFavoritesResponse(false, FrontendMessageKeys.Error.User.NotLoggedIn);
        }

        if (id <= 0)
        {
            return new RemoveFromFavoritesResponse(false, FrontendMessageKeys.Error.Default);
        }


        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        userCacheItem.RemoveFavorite(id);

        EntityCache.AddOrUpdate(userCacheItem);
        _userWritingRepo.Update(userCacheItem);

        return new RemoveFromFavoritesResponse(true);
    }

    public readonly record struct RemoveFromFavoritesResponse(bool Success, string? MessageKey = null);

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

    // Section: Shared Pages

    [HttpGet]
    public IList<SharedPageItem> GetSharedPages()
    {
        if (!_sessionUser.IsLoggedIn)
            return new List<SharedPageItem>();

        var userCacheItem = _extendedUserCache.GetUser(_sessionUser.UserId);

        return userCacheItem.SharedPages
            .Where(p => p != null)
            .Select(p => new SharedPageItem(p.Name, p.Id))
            .ToList();
    }

    public readonly record struct SharedPageItem(string Name, int Id);
}
