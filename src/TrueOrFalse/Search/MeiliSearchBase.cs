using Meilisearch;

namespace TrueOrFalse.Search
{
    internal class MeiliSearchBase
    {
        private readonly MeilisearchClient _client;

        internal MeiliSearchBase()
        {
            _client = new MeilisearchClient(MeiliSearchKonstanten.Url,
                MeiliSearchKonstanten.MasterKey);
        }

        /// <summary>
        /// Check MeiliSearchStatus
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        protected async Task CheckStatus(TaskInfo taskInfo)
        {
            var taskresult = await _client.WaitForTaskAsync(taskInfo.TaskUid);
            if (taskresult.Status != TaskInfoStatus.Succeeded)
            {
                Logg.r.Error("Cannot create question in MeiliSearch", taskresult);
            }
        }
    }
}