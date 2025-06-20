using Meilisearch;
using Index = Meilisearch.Index;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MemoWikis.Tests")]

internal class MeilisearchUsersIndexer : MeilisearchIndexerBase
{    public void Create(User user)
    {
        var userMap = CreateUserMap(user);
        var index = GetIndex();

        Task.Run(async () =>
        {
            var taskInfo = await index
                .AddDocumentsAsync(new List<MeiliSearchUserMap> { userMap })
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        });
    }

    public void Update(User user)
    {
        var userMap = CreateUserMap(user);
        var index = GetIndex();

        Task.Run(async () =>
        {
            var taskInfo = await index
                .UpdateDocumentsAsync(new List<MeiliSearchUserMap> { userMap })
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        });
    }

    public void Delete(User user)
    {
        var index = GetIndex();

        Task.Run(async () =>
        {
            var taskInfo = await index
                .DeleteOneDocumentAsync(user.Id.ToString())
                .ConfigureAwait(false);

            await CheckStatus(taskInfo);
        });
    }    private static MeiliSearchUserMap CreateUserMap(User user)
    {
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

    private static Index GetIndex()
    {
        var client = new MeilisearchClient(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);
        return client.Index(MeilisearchIndices.Users);
    }
}