﻿public interface ISearchPagesResult
{
    int Count { get; set; }
    List<int> PageIds { get; set; }
    IPager Pager { get; set; }

    List<PageCacheItem> GetPages();
}