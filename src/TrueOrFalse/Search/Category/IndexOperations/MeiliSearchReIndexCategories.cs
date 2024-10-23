using Meilisearch;

namespace TrueOrFalse.Search;

public class MeiliSearchReIndexCategories : IRegisterAsInstancePerLifetime

{
    private readonly CategoryRepository _categoryRepository;

    public MeiliSearchReIndexCategories(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _client = new MeilisearchClient(MeiliSearchConstants.Url, MeiliSearchConstants.MasterKey);
    }

    public MeilisearchClient _client { get; }

    public async Task Run()
    {
        await _client.DeleteIndexAsync(MeiliSearchConstants.Categories);
        var allCateogoriesFromDb = _categoryRepository.GetAll();

        var meiliSearchCategories = allCateogoriesFromDb.Select(c => new MeiliSearchCategoryMap
        {
            Id = c.Id,
            Name = c.Name,
            CreatorName = c.Creator == null ? "Unbekannt" : c.Creator.Name,
            DateCreated = c.DateCreated,
            Description = c.Description,
            Content = c.Content,
        });

        var index = _client.Index(MeiliSearchConstants.Categories);
        await index.AddDocumentsAsync(meiliSearchCategories);
    }
}