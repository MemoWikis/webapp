public class MeilisearchUsersResult : ISearchUsersResult
{
    public int Count { get; set; }

    public List<int> UserIds { get; set; } = new();

    public IList<UserCacheItem> GetUsers() => EntityCache.GetUsersByIds(UserIds); 
}