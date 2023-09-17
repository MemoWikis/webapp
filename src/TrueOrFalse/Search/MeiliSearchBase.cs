using Meilisearch;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TrueOrFalse.Search
{
    internal class MeiliSearchBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MeilisearchClient _client;
        internal MeiliSearchBase(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
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
