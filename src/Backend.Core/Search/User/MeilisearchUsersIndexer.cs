using Meilisearch;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MemoWikis.Tests")]

internal class MeilisearchUsersIndexer : MeilisearchIndexerBase
{
    public async Task CreateAsync(User user, string indexConstant = MeilisearchIndices.Users)
    {
        var userMap = CreateUserMap(user, indexConstant, out var index);
        var taskInfo = await index
            .AddDocumentsAsync(new List<MeiliSearchUserMap> { userMap })
            .ConfigureAwait(false);

        await CheckStatus(taskInfo).ConfigureAwait(false);
    }

    public async Task UpdateAsync(User user, string indexConstant = MeilisearchIndices.Users)
    {
        var userMapAndIndex = CreateUserMap(user, indexConstant, out var index);
        var taskInfo = await index
            .UpdateDocumentsAsync(new List<MeiliSearchUserMap> { userMapAndIndex })
            .ConfigureAwait(false);

        await CheckStatus(taskInfo).ConfigureAwait(false);
    }

    public async Task DeleteAsync(User user, string indexConstant = MeilisearchIndices.Users)
    {
        var userMapAndIndex = CreateUserMap(user, indexConstant, out var index);
        var taskInfo = await index
            .DeleteOneDocumentAsync(userMapAndIndex.Id.ToString())
            .ConfigureAwait(false);

        await CheckStatus(taskInfo).ConfigureAwait(false);
    }

    private static MeiliSearchUserMap CreateUserMap(User user, string indexConstant, out Meilisearch.Index index)
    {
        var client = new MeilisearchClient(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);
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