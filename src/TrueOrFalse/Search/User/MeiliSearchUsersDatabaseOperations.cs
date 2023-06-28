using Meilisearch;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

[assembly: InternalsVisibleTo("TrueOrFalse.Tests")]
namespace TrueOrFalse.Search
{
    internal class MeiliSearchUsersDatabaseOperations : MeiliSearchBase
    {
        /// <summary>
        /// CreateUserAsync in MeilieSearch
        /// </summary>
        /// <param name="user"></param>
        /// <param name="indexConstant"></param>
        /// <returns></returns>
        public async Task CreateAsync(User user, string indexConstant = MeiliSearchKonstanten.Users)
        {

            var userMap = CreateUserMap(user, indexConstant, out var index);
            var taskInfo = await index.AddDocumentsAsync(new List<MeiliSearchUserMap> { userMap })
            .ConfigureAwait(false);
            await CheckStatus(taskInfo).ConfigureAwait(false);
        }

        /// <summary>
        /// UpdateUserAsync in MeiliSearch
        /// </summary>
        /// <param name="user"></param>
        /// <param name="indexConstant"></param>
        /// <returns></returns>
        public async Task UpdateAsync(User user, string indexConstant = MeiliSearchKonstanten.Users)
        {

            var userMapAndIndex = CreateUserMap(user, indexConstant, out var index);
            var taskInfo = await index.UpdateDocumentsAsync(new List<MeiliSearchUserMap> { userMapAndIndex })
                .ConfigureAwait(false);
            await CheckStatus(taskInfo).ConfigureAwait(false);
        }

        /// <summary>
        /// DeleteUserAsync in Meiliesearch
        /// </summary>
        /// <param name="user"></param>
        /// <param name="indexConstant"></param>
        /// <returns></returns>
        public async Task DeleteAsync(User user, string indexConstant = MeiliSearchKonstanten.Users)
        {
            var userMapAndIndex = CreateUserMap(user, indexConstant, out var index);
            var taskInfo = await index
                 .DeleteOneDocumentAsync(userMapAndIndex.Id.ToString())
                 .ConfigureAwait(false);
            await CheckStatus(taskInfo).ConfigureAwait(false);
        }

        private static MeiliSearchUserMap CreateUserMap(User user, string indexConstant, out Index index)
        {
            var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            index = client.Index(indexConstant);
            var userMap = new MeiliSearchUserMap
            {
                Id = user.Id,
                DateCreated = DateTime.Now,
                Name = user.Name,
                Rank = user.ActivityLevel,
                WishCountQuestions = user.WishCountQuestions
            };
            return userMap;
        }
    }
}
