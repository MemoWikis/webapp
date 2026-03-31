public class SideSheetController(
    SessionUser _sessionUser,
    UserWritingRepo _userWritingRepo,
    ExtendedUserCache _extendedUserCache,
    IHttpContextAccessor _httpContextAccessor) : ApiBaseController
{
    // Section: Wikis

    [HttpGet]
    public IList<WikiItem> GetWikis()
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
            return new List<WikiItem>();

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        var wikis = userCacheItem.GetWikis()
            .Select(w => new WikiItem(
                w.Id,
                w.Name,
                w.Parents().Any(),
                new PageImageSettings(w.Id, _httpContextAccessor).GetUrl(50, isSquare: true).Url))
            .ToList();

        return wikis;
    }

    public readonly record struct WikiItem(int Id, string Name, bool HasParents, string ImgUrl);

    // Section: Favorites

    [HttpGet]
    public IList<FavoriteItem> GetFavorites()
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
            return new List<FavoriteItem>();

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        var favorites = userCacheItem.GetFavorites()
            .Select(f => new FavoriteItem(
                f.Id,
                f.Name,
                new PageImageSettings(f.Id, _httpContextAccessor).GetUrl(50, isSquare: true).Url))
            .ToList();

        return favorites;
    }

    public readonly record struct FavoriteItem(int Id, string Name, string ImgUrl);

    [HttpPost]
    public AddToFavoriteResponse AddToFavorites([FromRoute] int id)
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
            return new AddToFavoriteResponse(false, FrontendMessageKeys.Error.User.NotLoggedIn);

        if (id <= 0)
            return new AddToFavoriteResponse(false, FrontendMessageKeys.Error.Default);

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        userCacheItem.AddFavorite(id);

        _userWritingRepo.Update(userCacheItem);

        return new AddToFavoriteResponse(true);
    }

    public readonly record struct AddToFavoriteResponse(bool Success, string? MessageKey = null);

    [HttpPost]
    public RemoveFromFavoritesResponse RemoveFromFavorites([FromRoute] int id)
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
            return new RemoveFromFavoritesResponse(false, FrontendMessageKeys.Error.User.NotLoggedIn);

        if (id <= 0)
            return new RemoveFromFavoritesResponse(false, FrontendMessageKeys.Error.Default);

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        userCacheItem.RemoveFavorite(id);

        _userWritingRepo.Update(userCacheItem);

        return new RemoveFromFavoritesResponse(true);
    }

    public readonly record struct RemoveFromFavoritesResponse(bool Success, string? MessageKey = null);

    [HttpPost]
    public bool ReorderFavorites([FromBody] List<int> orderedIds)
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
            return false;

        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        var existingIds = new HashSet<int>(userCacheItem.FavoriteIds);

        if (orderedIds.Count != existingIds.Count || !orderedIds.All(existingIds.Contains))
            return false;

        userCacheItem.FavoriteIds = orderedIds;
        _userWritingRepo.Update(userCacheItem);

        return true;
    }

    // Section: Recent Pages

    [HttpGet]
    public IList<RecentPageItem> GetRecentPages([FromQuery] int count = 15)
    {
        if (!_sessionUser.IsLoggedIn)
            return new List<RecentPageItem>();

        var userCacheItem = _extendedUserCache.GetUser(_sessionUser.UserId);
        var recentPages = userCacheItem.RecentPages?.GetRecentPages()
            .Take(count)
            .Select(rp => new RecentPageItem(
                rp.Name,
                rp.Id,
                new PageImageSettings(rp.Id, _httpContextAccessor).GetUrl(50, isSquare: true).Url))
            .ToList();

        return recentPages;
    }

    public readonly record struct RecentPageItem(string Name, int Id, string ImgUrl);

    // Section: Shared Pages

    [HttpGet]
    public IList<SharedPageItem> GetSharedPages()
    {
        if (!_sessionUser.IsLoggedIn)
            return new List<SharedPageItem>();

        var userCacheItem = _extendedUserCache.GetUser(_sessionUser.UserId);

        return userCacheItem.SharedPages
            .Where(p => p != null)
            .Select(p => new SharedPageItem(
                p.Name,
                p.Id,
                new PageImageSettings(p.Id, _httpContextAccessor).GetUrl(50, isSquare: true).Url))
            .ToList();
    }

    public readonly record struct SharedPageItem(string Name, int Id, string ImgUrl);
}
