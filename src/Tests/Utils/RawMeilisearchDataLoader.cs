using Meilisearch;
using Meilisearch.QueryParameters;

public class RawMeilisearchDataLoader : IDisposable
{
    private readonly MeilisearchClient _client;
    private bool _disposed;

    public RawMeilisearchDataLoader(string meiliSearchUrl, string masterKey)
    {
        if (string.IsNullOrWhiteSpace(meiliSearchUrl))
            throw new ArgumentException("MeiliSearch URL cannot be null or empty.", nameof(meiliSearchUrl));

        if (string.IsNullOrWhiteSpace(masterKey))
            throw new ArgumentException("Master key cannot be null or empty.", nameof(masterKey));

        _client = new MeilisearchClient(meiliSearchUrl, masterKey);
    }

    public async Task<List<SearchPageItem>> GetAllPages() 
        => await GetAllDocumentsAsync<SearchPageItem>(MeilisearchIndices.Pages);

    public async Task<List<SearchUserItem>> GetAllUsers()
        => await GetAllDocumentsAsync<SearchUserItem>(MeilisearchIndices.Users);

    public async Task<List<SearchQuestionItem>> GetAllQuestions()
        => await GetAllDocumentsAsync<SearchQuestionItem>(MeilisearchIndices.Questions);

    private async Task<List<T>> GetAllDocumentsAsync<T>(string indexName, int limit = 100) where T : class
    {
        var index = _client.Index(indexName);
        var documents = await index.GetDocumentsAsync<T>(new DocumentsQuery { Limit = limit });
        return documents.Results.ToList();
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