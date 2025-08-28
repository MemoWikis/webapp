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
        var index = GetIndex();
        var searchPageMap = CreatePageMap(page);

        Task.Run(async () =>
            {
                try
                {
                    var pageExists = EntityCache.GetPage(page.Id) != null;
                    if (!pageExists)
                        return;
                    // Check if the document exists before updating
                    await index.GetDocumentAsync<MeilisearchPageMap>(page.Id.ToString());

                    // Document exists, proceed with update
                    var taskInfo = await index
                        .UpdateDocumentsAsync(new List<MeilisearchPageMap> { searchPageMap })
                        .ConfigureAwait(false);

                    await CheckStatus(taskInfo);
                }
                catch (MeilisearchApiError ex) when (ex.Code == "document_not_found")
                {
                    // Document doesn't exist (probably deleted), don't add it back
                    // This prevents race conditions where delete happens before update
                }
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
            CreatorName = page.Creator != null ? page.Creator.Name : "",
            DateCreated = page.DateCreated == DateTime.MinValue
                ? DateTime.Now
                : page.DateCreated,
            Description = page.Description ?? "",
            Content = page.Content ?? "",
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