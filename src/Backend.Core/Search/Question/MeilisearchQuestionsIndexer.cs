using Meilisearch;

internal class MeilisearchQuestionsIndexer : MeilisearchIndexerBase
{
    public async Task CreateAsync(Question question)
    {
        var searchQuestionMap = CreateQuestionMap(question, out var index);
        var taskInfo = await index
            .AddDocumentsAsync(new List<MeilisearchQuestionMap> { searchQuestionMap })
            .ConfigureAwait(false);

        await CheckStatus(taskInfo).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Question question)
    {
        var searchQuestionMap = CreateQuestionMap(question, out var index);
        var taskInfo = await index
            .UpdateDocumentsAsync(new List<MeilisearchQuestionMap> { searchQuestionMap })
            .ConfigureAwait(false);
        
        await CheckStatus(taskInfo).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Question question)
    {
        var searchQuestionMap = CreateQuestionMap(question, out var index);
        var taskInfo = await index
            .DeleteOneDocumentAsync(searchQuestionMap.Id.ToString())
            .ConfigureAwait(false);

        await CheckStatus(taskInfo).ConfigureAwait(false);
    }

    private MeilisearchQuestionMap CreateQuestionMap(Question question, out Meilisearch.Index index)
    {
        var client = new MeilisearchClient(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);

        index = client.Index(MeilisearchIndices.Questions);
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
}