using Meilisearch;

public class MeilisearchReIndexAllQuestions(
    QuestionValuationReadingRepo _questionValuationReadingRepo,
    QuestionReadingRepo _questionReadingRepo)
    : IRegisterAsInstancePerLifetime
{
    private readonly MeilisearchClient _client = new(Settings.MeiliSearchUrl, Settings.MeiliSearchMasterKey);

    public async Task Run()
    {
        var taskId = (await _client.DeleteIndexAsync(MeilisearchIndices.Questions)).TaskUid;
        await _client.WaitForTaskAsync(taskId);

        var allQuestionsFromDb = _questionReadingRepo.GetAll();
        var allValuations = _questionValuationReadingRepo.GetAll();
        var meiliSearchQuestions = new List<MeilisearchQuestionMap>();

        foreach (var question in allQuestionsFromDb)
        {
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
    }
}