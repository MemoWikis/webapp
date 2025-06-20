using Meilisearch;
using Index = Meilisearch.Index;

internal class MeilisearchQuestionsIndexer : MeilisearchIndexerBase
{
    public void Create(Question question)
    {
        var searchQuestionMap = CreateQuestionMap(question);
        var index = GetIndex();

        Task.Run(async () =>
        {
            var taskInfo = await index
                .AddDocumentsAsync(new List<MeilisearchQuestionMap> { searchQuestionMap })
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        });
    }

    public void Update(Question question)
    {
        var searchQuestionMap = CreateQuestionMap(question);
        var index = GetIndex();

        Task.Run(async () =>
        {
            var taskInfo = await index
                .UpdateDocumentsAsync(new List<MeilisearchQuestionMap> { searchQuestionMap })
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        });
    }

    public void Delete(Question question)
    {
        var index = GetIndex();

        Task.Run(async () =>
        {
            var taskInfo = await index
                .DeleteOneDocumentAsync(question.Id.ToString())
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        });
    }    private static MeilisearchQuestionMap CreateQuestionMap(Question question)
    {
        var questionMap = new MeilisearchQuestionMap
        {
            CreatorId = question.Creator.Id,
            Description = question.Description ?? "",
            Pages = question.Pages.Select(c => c.Name).ToList(),
            PageIds = question.Pages.Select(c => c.Id).ToList(),
            Id = question.Id,
            Solution = question.Solution,
            SolutionType = (int)question.SolutionType,
            Text = question.Text
        };
        return questionMap;
    }

    private static Index GetIndex()
    {
        var client = new MeilisearchClient(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);
        return client.Index(MeilisearchIndices.Questions);
    }
}