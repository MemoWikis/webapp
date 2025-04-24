using Meilisearch;

namespace TrueOrFalse.Search;

public class MeiliSearchReIndexPages : IRegisterAsInstancePerLifetime

{
    private readonly PageRepository _pageRepository;

    public MeiliSearchReIndexPages(PageRepository pageRepository)
    {
        _pageRepository = pageRepository;
        _client = new MeilisearchClient(MeiliSearchConstants.Url, MeiliSearchConstants.MasterKey);
    }

    public MeilisearchClient _client { get; }

    public async Task Run()
    {
        await _client.DeleteIndexAsync(MeiliSearchConstants.Pages);
        var allPagesFromDb = _pageRepository.GetAll();

        var meiliSearchPageMaps = allPagesFromDb.Select(c => new MeiliSearchPageMap
        {
            Id = c.Id,
            Name = c.Name,
            CreatorName = c.Creator == null ? "Unbekannt" : c.Creator.Name,
            DateCreated = c.DateCreated,
            Description = c.Description,
            Content = c.Content,
            Language = c.Language
        });

        var index = _client.Index(MeiliSearchConstants.Pages);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });

        await index.AddDocumentsAsync(meiliSearchPageMaps);
    }

    public async Task RunCache()
    {
        await _client.DeleteIndexAsync(MeiliSearchConstants.Pages);

        var pages = EntityCache.GetAllPagesList();

        var meiliSearchPageMaps = pages.Select(c => new MeiliSearchPageMap
        {
            Id = c.Id,
            Name = c.Name,
            CreatorName = c.Creator == null ? "Unbekannt" : c.Creator.Name,
            DateCreated = c.DateCreated,
            Description = c.Description,
            Content = c.Content,
            Language = c.Language
        });

        var index = _client.Index(MeiliSearchConstants.Pages);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });

        await index.AddDocumentsAsync(meiliSearchPageMaps);
    }
}