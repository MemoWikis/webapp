using Meilisearch;

public class MeilisearchReIndexAllQuestions(
    QuestionValuationReadingRepo _questionValuationReadingRepo,
    QuestionReadingRepo _questionReadingRepo)
    : IRegisterAsInstancePerLifetime
{
    private readonly MeilisearchClient _client = new(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);

    public async Task Run(string? jobTrackingId = null)
    {
        var taskId = (await _client.DeleteIndexAsync(MeilisearchIndices.Questions)).TaskUid;
        await _client.WaitForTaskAsync(taskId);

        var allQuestionsFromDb = _questionReadingRepo.GetAll();
        var meiliSearchQuestions = new List<MeilisearchQuestionMap>();

        var currentQuestionCount = 0;
        var totalQuestions = allQuestionsFromDb.Count();

        foreach (var question in allQuestionsFromDb)
        {
            currentQuestionCount++;

            Log.Information("Meilisearch Reindex Question: {id}", question.Id);

            meiliSearchQuestions.Add(MeilisearchToQuestionMap.Run(question));

            JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                $"Re-indexing question {currentQuestionCount} of {totalQuestions} (ID: {question.Id})...",
                "MeiliReIndexAllQuestions");
        }

        var index = _client.Index(MeilisearchIndices.Questions);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });

        await index.UpdateRankingRulesAsync(MeilisearchSort.Default);

        await index.AddDocumentsAsync(meiliSearchQuestions);
        Log.Information("Meilisearch Reindex Adding {Count} documents to MeiliSearch index", meiliSearchQuestions.Count);
    }

    public async Task RunCache()
    {
        var taskId = (await _client.DeleteIndexAsync(MeilisearchIndices.Questions)).TaskUid;
        await _client.WaitForTaskAsync(taskId);

        var allQuestions = EntityCache.GetAllQuestions();
        var meiliSearchQuestions = new List<MeilisearchQuestionMap>();

        foreach (var question in allQuestions)
        {
            Log.Information("Meilisearch Reindex Cache Question: {id}", question.Id);

            meiliSearchQuestions.Add(
                MeilisearchToQuestionMap.Run(question));
        }

        var index = _client.Index(MeilisearchIndices.Questions);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });

        await index.UpdateRankingRulesAsync(MeilisearchSort.Default);

        await index.AddDocumentsAsync(meiliSearchQuestions);

        Log.Information("Meilisearch Reindex Cache Adding {Count} documents to MeiliSearch index", meiliSearchQuestions.Count);
    }
}