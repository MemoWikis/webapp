using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet.Impl;

namespace TrueOrFalse.Search
{
    public class SearchUsersResult : ISearchUsersResult
    {
        /// <summary>In milliseconds</summary>
        public int QueryTime;

        /// <summary>Amount of items found</summary>
        public int Count { get; set; }

        public SpellCheckResults SpellChecking;

        public List<int> UserIds { get; set; } = new();

        public IPager Pager;

        public IList<UserCacheItem> GetUsers() => EntityCache.GetUsersByIds(UserIds);
    }
}
