using System;
using Meilisearch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrueOrFalse.Search
{
    internal class MeiliSearchUsersDatabaseOperations
    {
        public static async Task CreateAsync(User user)
        {
            try
            {
                var userMapAndIndex = CreateUserMap(user, out var index);
                await index.AddDocumentsAsync(new List<MeiliSearchUserMap> { userMapAndIndex })
                .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot create question in MeiliSearch", e);
            }
        }

        public static async Task UpdateAsync(User user)
        {
            try
            {
                var userMapAndIndex = CreateUserMap(user, out var index);
                await index.UpdateDocumentsAsync(new List<MeiliSearchUserMap> { userMapAndIndex })
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot updated question in MeiliSearch", e);
            }
        }

        /// <summary>
        /// DeleteUserAsync in Meiliesearch
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task DeleteAsync(User user)
        {
            try
            {
                var userMapAndIndex = CreateUserMap(user, out var index);
                await index
                    .DeleteOneDocumentAsync(userMapAndIndex.Id.ToString())
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot updated question in MeiliSearch", e);
            }
        }

        private static MeiliSearchUserMap CreateUserMap(User user, out Index index)
        {
            var client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
            index = client.Index(MeiliSearchKonstanten.Users);
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
