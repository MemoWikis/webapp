using System.Collections.Generic;

namespace TrueOrFalse.Search;

public interface ISearchUsersResult
{
    int Count { get; set; }
    List<int> UserIds { get; set; }
    IList<UserCacheItem> GetUsers();
}