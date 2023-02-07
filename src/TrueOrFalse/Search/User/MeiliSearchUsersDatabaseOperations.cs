using System;
using Meilisearch;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

[assembly: InternalsVisibleTo("TrueOrFalse.Tests")]
namespace TrueOrFalse.Search
{
    internal class MeiliSearchUsersDatabaseOperations
    {
        /// <summary>
        /// CreateUserAsync in MeilieSearch
        /// </summary>
        /// <param name="user"></param>
        /// <param name="indexConstant"></param>
        /// <returns></returns>
        public static async Task<TaskInfo> CreateAsync(User user, string indexConstant = MeiliSearchKonstanten.Users)
        {
            try
            {
                var userMap = CreateUserMap(user, indexConstant, out var index);
                return await index.AddDocumentsAsync(new List<MeiliSearchUserMap> { userMap })
                .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot create user in MeiliSearch", e);
            }

            return null;
        }

        /// <summary>
        /// UpdateUserAsync in MeiliSearch
        /// </summary>
        /// <param name="user"></param>
        /// <param name="indexConstant"></param>
        /// <returns></returns>
        public static async Task<TaskInfo> UpdateAsync(User user, string indexConstant = MeiliSearchKonstanten.Users)
        {
            try
            {
                var userMapAndIndex = CreateUserMap(user, indexConstant, out var index);
                return await index.UpdateDocumentsAsync(new List<MeiliSearchUserMap> { userMapAndIndex })
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot updated user in MeiliSearch", e);
            }

            return null; 
        }

        /// <summary>
        /// DeleteUserAsync in Meiliesearch
        /// </summary>
        /// <param name="user"></param>
        /// <param name="indexConstant"></param>
        /// <returns></returns>
        public static async Task<TaskInfo> DeleteAsync(User user, string indexConstant = MeiliSearchKonstanten.Users)
        {
            try
            {
                var userMapAndIndex = CreateUserMap(user, indexConstant, out var index);
                return await index
                    .DeleteOneDocumentAsync(userMapAndIndex.Id.ToString())
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logg.r().Error("Cannot delete user in MeiliSearch", e);
            }

            return null;
        }

        private static MeiliSearchUserMap CreateUserMap(User user, string indexConstant,  out Index index)
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
