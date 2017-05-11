using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet.Impl;

namespace TrueOrFalse.Search
{
    public class SearchSetsResult
    {
        /// <summary>In milliseconds</summary>
        public int QueryTime;

        /// <summary>Amount of items found</summary>
        public int Count;

        public SpellCheckResults SpellChecking;

        public List<int> SetIds = new List<int>();

        public IPager Pager;

        public IList<Set> GetSets() => Sl.SetRepo.GetByIds(SetIds);
    }
}
