using Meilisearch;

namespace TrueOrFalse.Search;

public class MeiliSearchReIndexCategories : IRegisterAsInstancePerLifetime

{
    private readonly CategoryRepository _categoryRepository;

    public MeiliSearchReIndexCategories(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
    }

    public MeilisearchClient _client { get; }

    public async Task Go()
    {
        await _client.DeleteIndexAsync(MeiliSearchKonstanten.Categories);
        var allCateogoriesFromDb = _categoryRepository.GetAll();

        var meiliSearchCategories = allCateogoriesFromDb.Select(c => new MeiliSearchCategoryMap
        {
            Id = c.Id,
            Name = c.Name,
            CreatorId = c.Creator == null ? -1 : c.Creator.Id,
            DateCreated = c.DateCreated,
            Description = c.Description,
            QuestionCount = c.CountQuestions
        });

        var index = _client.Index(MeiliSearchKonstanten.Categories);
        await index.AddDocumentsAsync(meiliSearchCategories);
    }
}