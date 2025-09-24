using Meilisearch;

public class MeilisearchReIndexAllQuestions(
    QuestionValuationReadingRepo _questionValuationReadingRepo,
    QuestionReadingRepo _questionReadingRepo)
    : IRegisterAsInstancePerLifetime
{
    private readonly MeilisearchClient _client = new(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);

    public async Task Run()
    {
        var taskId = (await _client.DeleteIndexAsync(MeilisearchIndices.Questions)).TaskUid;
        await _client.WaitForTaskAsync(taskId);

        var allQuestionsFromDb = _questionReadingRepo.GetAll();
        var meiliSearchQuestionsMap = allQuestionsFromDb.Select(MeilisearchToQuestionMap.Run);

        var index = _client.Index(MeilisearchIndices.Questions);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });

        await index.UpdateRankingRulesAsync(new string[]
        {
            "words", "exactness", "typo", "proximity", "attribute", "sort"
        });

        await index.AddDocumentsAsync(meiliSearchQuestionsMap);
    }

    public async Task RunCache()
    {
        var taskId = (await _client.DeleteIndexAsync(MeilisearchIndices.Questions)).TaskUid;
        await _client.WaitForTaskAsync(taskId);

        var allQuestions = EntityCache.GetAllQuestions();
        var meiliSearchQuestionsMap = allQuestions.Select(MeilisearchToQuestionMap.Run);

        var index = _client.Index(MeilisearchIndices.Questions);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });

        await index.UpdateRankingRulesAsync(new string[]
        {
            "words", "exactness", "typo", "proximity", "attribute", "sort"
        });

        await index.AddDocumentsAsync(meiliSearchQuestionsMap);
    }
}