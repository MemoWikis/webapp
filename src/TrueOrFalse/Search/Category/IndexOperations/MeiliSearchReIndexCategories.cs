using Meilisearch;

namespace TrueOrFalse.Search;

public class MeiliSearchReIndexCategories : IRegisterAsInstancePerLifetime

{
    private readonly PageRepository _pageRepository;

    public MeiliSearchReIndexCategories(PageRepository pageRepository)
    {
        _pageRepository = pageRepository;
        _client = new MeilisearchClient(MeiliSearchConstants.Url, MeiliSearchConstants.MasterKey);
    }

    public MeilisearchClient _client { get; }

    public async Task Run()
    {
        await _client.DeleteIndexAsync(MeiliSearchConstants.Pages);
        var allCateogoriesFromDb = _pageRepository.GetAll();

        var meiliSearchCategories = allCateogoriesFromDb.Select(c => new MeiliSearchCategoryMap
        {
            Id = c.Id,
            Name = c.Name,
            CreatorName = c.Creator == null ? "Unbekannt" : c.Creator.Name,
            DateCreated = c.DateCreated,
            Description = c.Description,
            Content = c.Content,
        });

        var index = _client.Index(MeiliSearchConstants.Pages);
        await index.AddDocumentsAsync(meiliSearchCategories);
    }
}