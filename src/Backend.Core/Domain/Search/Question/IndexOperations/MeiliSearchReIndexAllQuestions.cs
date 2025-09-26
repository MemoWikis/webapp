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
        var allValuations = _questionValuationReadingRepo.GetAll();
        var meiliSearchQuestions = new List<MeilisearchQuestionMap>();

        var currentQuestionCount = 1;
        var totalQuestions = allQuestionsFromDb.Count();
        foreach (var question in allQuestionsFromDb)
        {
            Log.Information("Meilisearch Reindex Question: {id}", question.Id);

            var questionValuations = allValuations
                .Where(qv => qv.Question.Id == question.Id && qv.User != null)
                .Select(qv => qv.ToCacheItem())
                .ToList();

            meiliSearchQuestions.Add(
                MeilisearchToQuestionMap.Run(question, questionValuations));

            if (jobTrackingId != null)
            {
                JobTracking.UpdateJobStatus(jobTrackingId, JobStatus.Running,
                    $"Re-indexing question {currentQuestionCount} of {totalQuestions} (ID: {question.Id})...",
                    "MeiliReIndexAllQuestions");
            }

            currentQuestionCount++;
        }

        Log.Information("Adding {Count} documents to MeiliSearch index", meiliSearchQuestions.Count);
        var index = _client.Index(MeilisearchIndices.Questions);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });
        await index.AddDocumentsAsync(meiliSearchQuestions);
    }

    public async Task RunCache()
    {
        var taskId = (await _client.DeleteIndexAsync(MeilisearchIndices.Questions)).TaskUid;
        await _client.WaitForTaskAsync(taskId);

        var allQuestions = EntityCache.GetAllQuestions();
        var allValuations = _questionValuationReadingRepo.GetAll();
        var meiliSearchQuestions = new List<MeilisearchQuestionMap>();

        foreach (var question in allQuestions)
        {
            Log.Information("Meilisearch Reindex Cache Question: {id}", question.Id);

            var questionValuations = allValuations
                .Where(qv => qv.Question.Id == question.Id && qv.User != null)
                .Select(qv => qv.ToCacheItem())
                .ToList();

            meiliSearchQuestions.Add(
                MeilisearchToQuestionMap.Run(question, questionValuations));
        }

        var index = _client.Index(MeilisearchIndices.Questions);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });
        await index.AddDocumentsAsync(meiliSearchQuestions);
        Log.Information("Completed reindexing all questions");
    }
}