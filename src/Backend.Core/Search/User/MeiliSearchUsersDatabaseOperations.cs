using Meilisearch;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MemoWikis.Tests")]

internal class MeiliSearchUsersDatabaseOperations : MeiliSearchBase
{
    /// <summary>
    /// CreateUserAsync in MeiliSearch
    /// </summary>
    /// <param name="user"></param>
    /// <param name="indexConstant"></param>
    /// <returns></returns>
    public async Task CreateAsync(User user, string indexConstant = MeiliSearchConstants.Users)
    {
        var userMap = CreateUserMap(user, indexConstant, out var index);
        var taskInfo = await index
            .AddDocumentsAsync(new List<MeiliSearchUserMap> { userMap })
            .ConfigureAwait(false);

        await CheckStatus(taskInfo).ConfigureAwait(false);
    }

    /// <summary>
    /// UpdateUserAsync in MeiliSearch
    /// </summary>
    /// <param name="user"></param>
    /// <param name="indexConstant"></param>
    /// <returns></returns>
    public async Task UpdateAsync(User user, string indexConstant = MeiliSearchConstants.Users)
    {

        var userMapAndIndex = CreateUserMap(user, indexConstant, out var index);
        var taskInfo = await index
            .UpdateDocumentsAsync(new List<MeiliSearchUserMap> { userMapAndIndex })
            .ConfigureAwait(false);

        await CheckStatus(taskInfo).ConfigureAwait(false);
    }

    /// <summary>
    /// DeleteUserAsync in Meiliesearch
    /// </summary>
    /// <param name="user"></param>
    /// <param name="indexConstant"></param>
    /// <returns></returns>
    public async Task DeleteAsync(User user, string indexConstant = MeiliSearchConstants.Users)
    {
        var userMapAndIndex = CreateUserMap(user, indexConstant, out var index);
        var taskInfo = await index
            .DeleteOneDocumentAsync(userMapAndIndex.Id.ToString())
            .ConfigureAwait(false);

        await CheckStatus(taskInfo).ConfigureAwait(false);
    }

    private static MeiliSearchUserMap CreateUserMap(User user, string indexConstant, out Meilisearch.Index index)
    {
        var client = new MeilisearchClient(MeiliSearchConstants.Url, MeiliSearchConstants.MasterKey);
        index = client.Index(indexConstant);

        var userMap = new MeiliSearchUserMap
        {
            Id = user.Id,
            DateCreated = DateTime.Now,
            Name = user.Name,
            Rank = user.ActivityLevel,
            WishCountQuestions = user.WishCountQuestions,
        };
        var userCacheItem = EntityCache.GetUserByIdNullable(user.Id);
        if (userCacheItem != null)
            userMap.ContentLanguages = userCacheItem.ContentLanguages.Select(l => l.GetCode()).ToList();
        else if (LanguageExtensions.CodeExists(user.UiLanguage))
            userMap.ContentLanguages = [user.UiLanguage];

        return userMap;
    }
}