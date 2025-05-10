using Meilisearch;
using Meilisearch.QueryParameters;

public class RawSearchDataLoader : IDisposable
{
    private readonly MeilisearchClient _client;
    private bool _disposed;

    public RawSearchDataLoader(string meiliSearchUrl, string masterKey)
    {
        if (string.IsNullOrWhiteSpace(meiliSearchUrl))
            throw new ArgumentException("MeiliSearch URL cannot be null or empty.", nameof(meiliSearchUrl));

        if (string.IsNullOrWhiteSpace(masterKey))
            throw new ArgumentException("Master key cannot be null or empty.", nameof(masterKey));

        _client = new MeilisearchClient(meiliSearchUrl, masterKey);
    }

    public async Task<List<T>> SearchAsync<T>(string indexName, string query, int limit = 20) where T : class
    {
        var index = _client.Index(indexName);
        var searchQuery = new SearchQuery { Limit = limit };
        var result = await index.SearchAsync<T>(query, searchQuery);
        return result.Hits.ToList();
    }

    public async Task<List<T>> GetAllDocumentsAsync<T>(string indexName, int limit = 100) where T : class
    {
        var index = _client.Index(indexName);
        var documents = await index.GetDocumentsAsync<T>(new DocumentsQuery { Limit = limit });
        return documents.Results.ToList();
    }

    public async Task CreateIndexAsync(string indexName)
    {
        var taskInfo = await _client.CreateIndexAsync(indexName);
        await WaitForTaskAsync(taskInfo.TaskUid);
    }

    public async Task SetFilterableAttributesAsync(string indexName, string[] attributes)
    {
        var index = _client.Index(indexName);
        var taskInfo = await index.UpdateFilterableAttributesAsync(attributes);
        await WaitForTaskAsync(taskInfo.TaskUid);
    }

    private async Task WaitForTaskAsync(int taskUid)
    {
        var taskInfo = await _client.WaitForTaskAsync(taskUid);
        if (taskInfo.Status != TaskInfoStatus.Succeeded)
        {
            throw new InvalidOperationException($"MeiliSearch task failed: {taskInfo.Status}");
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // MeiliSearchClient doesn't have a Dispose method, 
            // but we keep the pattern for future-proofing
        }

        _disposed = true;
    }
}