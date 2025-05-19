using Meilisearch;

internal class MeilisearchPageIndexer : MeilisearchIndexerBase
{
    public async Task CreateAsync(Page page)
    {
        var searchPageMap = CreatePageMap(page, out var index);
        
        var taskInfo = await index
            .AddDocumentsAsync(new List<MeilisearchPageMap> { searchPageMap })
            .ConfigureAwait(false);
        
        await CheckStatus(taskInfo);
    }

    public async Task UpdateAsync(Page page)
    {
        var searchPageMap = CreatePageMap(page, out var index);
        
        var taskInfo = await index
            .UpdateDocumentsAsync(new List<MeilisearchPageMap> { searchPageMap })
            .ConfigureAwait(false);

        await CheckStatus(taskInfo);
    }

    public async Task DeleteAsync(Page page)
    {
        var pageMapIndex = CreatePageMap(page, out var index);
        
        var taskInfo = await index
            .DeleteOneDocumentAsync(pageMapIndex.Id.ToString())
            .ConfigureAwait(false);

        await CheckStatus(taskInfo);
    }

    private static MeilisearchPageMap CreatePageMap(Page page, out Meilisearch.Index index)
    {
        var client = new MeilisearchClient(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);
        index = client.Index(MeilisearchIndices.Pages);
        var pageMap = new MeilisearchPageMap
        {
            CreatorName = page.Creator.Name,
            DateCreated = page.DateCreated == DateTime.MinValue
                ? DateTime.Now
                : page.DateCreated,
            Description = page.Description ?? "",
            Name = page.Name,
            Id = page.Id,
            Language = page.Language
        };
        return pageMap;
    }
}