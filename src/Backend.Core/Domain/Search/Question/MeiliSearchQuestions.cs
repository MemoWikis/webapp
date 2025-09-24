using Meilisearch;

public class MeilisearchQuestions(PermissionCheck _permissionCheck) : MeilisearchBase, IRegisterAsInstancePerLifetime
{
    private List<QuestionCacheItem> _questions = new();

    public async Task<ISearchQuestionsResult> RunAsync(string searchTerm, List<Language>? languages = null)
    {
        var client = new MeilisearchClient(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);
        var index = client.Index(MeilisearchIndices.Questions);

        return await LoadSearchResults(searchTerm, index, languages);
    }

    private async Task<MeilisearchQuestionsResult> LoadSearchResults(
        string searchTerm, 
        Meilisearch.Index index, 
        List<Language>? languages = null
    )
    {
        var sq = new SearchQuery
        {
            Q = searchTerm,
            Limit = _count
        };

        if (languages != null && languages.Any())
        {
            var clauses = languages
                .Select(lang => lang.GetCode())
                .Select(code => $"Language = \"{code}\"")
                .ToList();

            sq.Filter = string.Join(" OR ", clauses);
        }

        var questionMaps =
            (await index.SearchAsync<MeilisearchQuestionMap>(searchTerm, sq))
            .Hits;

        var questionMapsSkip = questionMaps
            .Skip(_count - 20)
            .ToList();

        AddDistinctQuestionsToResult(questionMapsSkip);

        if (IsReloadRequired(questionMaps.Count, _questions.Count()))
        {
            _count += 20;
            await LoadSearchResults(searchTerm, index, languages);
        }

        var result = new MeilisearchQuestionsResult
        {
            Count = _questions.Count,
            QuestionIds = _questions
                .Select(c => c.Id)
                .Take(5)
                .ToList()
        };

        return result;
    }

    private void AddDistinctQuestionsToResult(List<MeilisearchQuestionMap> questionMaps)
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