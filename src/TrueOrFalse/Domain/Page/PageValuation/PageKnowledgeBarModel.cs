﻿public class PageKnowledgeBarModel
{
    public PageCacheItem Page;
    public KnowledgeSummary PageKnowledgeSummary;

    public PageKnowledgeBarModel(PageCacheItem page, int userId, KnowledgeSummaryLoader knowledgeSummaryLoader)
    {
        Page = page;
        PageKnowledgeSummary = knowledgeSummaryLoader.RunFromMemoryCache(page.Id, userId);
    }
}
