﻿using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Search;

public interface ISearchPagesResult
{
    int Count { get; set; }
    List<int> PageIds { get; set; }
    IPager Pager { get; set; }

    IList<PageCacheItem> GetPages();
}