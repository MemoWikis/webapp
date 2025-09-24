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
        var finalResults = new List<MeilisearchQuestionMap>();

        // Search for questions in specified languages first (if provided)
        if (languages != null && languages.Any())
        {
            var clauses = languages
                .Select(lang => lang.GetCode())
                .Select(code => $"Language = \"{code}\"")
                .ToList();

            var sqLanguageFiltered = new SearchQuery
            {
                Q = searchTerm,
                Limit = _count,
                Filter = string.Join(" OR ", clauses)
            };
            var languageResults = await index.SearchAsync<MeilisearchQuestionMap>(searchTerm, sqLanguageFiltered);
            finalResults.AddRange(languageResults.Hits);
        }

        // Then search for all other questions
        var searchQuery = new SearchQuery
        {
            Q = searchTerm,
            Limit = _count
        };
        var allResults = await index.SearchAsync<MeilisearchQuestionMap>(searchTerm, searchQuery);
        
        // Add results that aren't already in the list
        var existingIds = finalResults.Select(result => result.Id).ToHashSet();
        var additionalResults = allResults.Hits.Where(result => !existingIds.Contains(result.Id));
        finalResults.AddRange(additionalResults);

        var questionMaps = finalResults;

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