using Meilisearch;

public class MeiliSearchReIndexAllQuestions : IRegisterAsInstancePerLifetime
{
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly MeilisearchClient _client;

    public MeiliSearchReIndexAllQuestions(
        QuestionValuationReadingRepo questionValuationReadingRepo,
        QuestionReadingRepo questionReadingRepo)
    {
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _questionReadingRepo = questionReadingRepo;
        _client = new MeilisearchClient(MeiliSearchConstants.Url,
            MeiliSearchConstants.MasterKey);
    }

    public async Task Run()
    {
        var taskId = (await _client.DeleteIndexAsync(MeiliSearchConstants.Questions)).TaskUid;
        await _client.WaitForTaskAsync(taskId);

        var allQuestionsFromDb = _questionReadingRepo.GetAll();
        var allValuations = _questionValuationReadingRepo.GetAll();
        var meiliSearchQuestions = new List<MeiliSearchQuestionMap>();

        foreach (var question in allQuestionsFromDb)
        {
            var id = question.Id;
            var questionValuations = allValuations
                .Where(qv => qv.Question.Id == question.Id && qv.User != null)
                .Select(qv => qv.ToCacheItem())
                .ToList();
            meiliSearchQuestions.Add(
                MeiliSearchToQuestionMap.Run(question, questionValuations));
        }

        var index = _client.Index(MeiliSearchConstants.Questions);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });
        await index.AddDocumentsAsync(meiliSearchQuestions);
    }

    public async Task RunCache()
    {
        var taskId = (await _client.DeleteIndexAsync(MeiliSearchConstants.Questions)).TaskUid;
        await _client.WaitForTaskAsync(taskId);

        var allQuestions = EntityCache.GetAllQuestions();
        var allValuations = _questionValuationReadingRepo.GetAll();
        var meiliSearchQuestions = new List<MeiliSearchQuestionMap>();

        foreach (var question in allQuestions)
        {
            var id = question.Id;
            var questionValuations = allValuations
                .Where(qv => qv.Question.Id == question.Id && qv.User != null)
                .Select(qv => qv.ToCacheItem())
                .ToList();
            meiliSearchQuestions.Add(
                MeiliSearchToQuestionMap.Run(question, questionValuations));
        }

        var index = _client.Index(MeiliSearchConstants.Questions);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });
        await index.AddDocumentsAsync(meiliSearchQuestions);
    }
}