using Meilisearch;
using Index = Meilisearch.Index;

internal class MeilisearchPageIndexer : MeilisearchIndexerBase
{
    public void Create(Page page)
    {
        var searchPageMap = CreatePageMap(page);
        var index = GetIndex();

        Task.Run(async () =>
        {
            var taskInfo = await index
                .AddDocumentsAsync(new List<MeilisearchPageMap> { searchPageMap })
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        });
    }

    public void Update(Page page)
    {
        var searchPageMap = CreatePageMap(page);
        var index = GetIndex();

        Task.Run(async () =>
            {
                var taskInfo = await index
                    .UpdateDocumentsAsync(new List<MeilisearchPageMap> { searchPageMap })
                    .ConfigureAwait(false);

                await CheckStatus(taskInfo);
            }
        );
    }

    public void Delete(Page page)
    {
        var index = GetIndex();

        Task.Run(async () =>
        {
            var taskInfo = await index
                .DeleteDocumentsAsync(new List<string> { page.Id.ToString() })
                .ConfigureAwait(false);
            await CheckStatus(taskInfo);
        });
    }

    private static MeilisearchPageMap CreatePageMap(Page page)
    {
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

    private static Index GetIndex()
    {
        var client = new MeilisearchClient(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);
        return client.Index(MeilisearchIndices.Pages);
    }
}