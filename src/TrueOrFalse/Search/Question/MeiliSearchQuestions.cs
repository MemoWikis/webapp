using Meilisearch;

namespace TrueOrFalse.Search;

public class MeiliSearchQuestions : MeiliSearchHelper, IRegisterAsInstancePerLifetime
{
    private readonly PermissionCheck _permissionCheck;
    private List<QuestionCacheItem> _questions = new();
    private MeiliSearchQuestionsResult _result;

    public MeiliSearchQuestions(PermissionCheck permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async Task<ISearchQuestionsResult> RunAsync(
        string searchTerm)
    {
        var client =
            new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
        var index = client.Index(MeiliSearchKonstanten.Questions);
        _result = new MeiliSearchQuestionsResult();

        _result.QuestionIds.AddRange(await LoadSearchResults(searchTerm, index));

        return _result;
    }

    private async Task<List<int>> LoadSearchResults(string searchTerm, Meilisearch.Index index)
    {
        var sq = new SearchQuery
        {
            Limit = _count
        };

        var questionMaps =
            (await index.SearchAsync<MeiliSearchQuestionMap>(searchTerm, sq))
            .Hits;

        var questionMapsSkip = questionMaps
            .Skip(_count - 20)
            .ToList();

        FilterCacheItems(questionMapsSkip);

        if (IsReloadRequired(questionMaps.Count, _questions.Count()))
        {
            _count += 20;
            await LoadSearchResults(searchTerm, index);
        }

        ;
        _result.Count = _questions.Count;
        return _questions
            .Select(c => c.Id)
            .Take(5)
            .ToList();
    }

    private void FilterCacheItems(List<MeiliSearchQuestionMap> questionMaps)
    {
        var questionsTemp = EntityCache.GetQuestionsByIds(
                questionMaps.Select(c => c.Id))
            .Where(_permissionCheck.CanView)
            .ToList();
        _questions.AddRange(questionsTemp);
        _questions = _questions
            .Distinct()
            .ToList();
    }
}