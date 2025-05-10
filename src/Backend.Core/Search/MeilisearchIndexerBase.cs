using Meilisearch;

internal class MeilisearchIndexerBase
{
    private readonly MeilisearchClient _client;

    internal MeilisearchIndexerBase()
    {
        _client = new MeilisearchClient(Settings.MeiliSearchUrl, Settings.MeiliSearchMasterKey);
    }

    /// <summary>
    /// Check MeiliSearchStatus
    /// </summary>
    /// <param name="taskInfo"></param>
    /// <returns></returns>
    protected async Task CheckStatus(TaskInfo taskInfo)
    {
        var taskResult = await _client.WaitForTaskAsync(taskInfo.TaskUid);
        
        if (taskResult.Status != TaskInfoStatus.Succeeded) 
            Log.Error("Cannot create question in MeiliSearch {taskResult}", taskResult);
    }
}