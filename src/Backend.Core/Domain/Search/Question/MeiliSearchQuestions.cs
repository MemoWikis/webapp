using Meilisearch;
using Index = Meilisearch.Index;

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
        // Questions do not have a language identifier yet
        // await SearchQuestionsInSpecifiedLanguage(searchTerm, index, languages, finalResults);

        // Then search for all other questions
        ISearchable<MeilisearchQuestionMap> allResults = await SearchQuestionsInAllLanguages(searchTerm, index);

        // Add results that aren't already in the list
        MergeNonDuplicateResults(finalResults, allResults);

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

    private static void MergeNonDuplicateResults(List<MeilisearchQuestionMap> finalResults, ISearchable<MeilisearchQuestionMap> allResults)
    {
        var existingIds = finalResults.Select(result => result.Id).ToHashSet();
        var additionalResults = allResults.Hits.Where(result => !existingIds.Contains(result.Id));
        finalResults.AddRange(additionalResults);
    }

    private async Task<ISearchable<MeilisearchQuestionMap>> SearchQuestionsInAllLanguages(string searchTerm, Index index)
    {
        var searchQuery = new SearchQuery
        {
            Q = searchTerm,
            Limit = _count
        };
        var allResults = await index.SearchAsync<MeilisearchQuestionMap>(searchTerm, searchQuery);
        return allResults;
    }

    private async Task SearchQuestionsInSpecifiedLanguage(string searchTerm, Index index, List<Language>? languages,
        List<MeilisearchQuestionMap> finalResults)
    {
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