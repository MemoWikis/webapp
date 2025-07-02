using Meilisearch;
using System.Runtime.CompilerServices;
using Index = Meilisearch.Index;

[assembly: InternalsVisibleTo("MemoWikis.Tests")]

internal class MeilisearchUsersIndexer : MeilisearchIndexerBase
{
    public void Create(User user)
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
        var index = GetIndex();

        Task.Run(async () =>
        {
            try
            {
                var userExists = EntityCache.GetUserByIdNullable(user.Id) != null;
                if (!userExists)
                    return;
                // Check if the document exists before updating
                await index.GetDocumentAsync<MeiliSearchUserMap>(user.Id.ToString());
                
                // Document exists, proceed with update
                var userMap = CreateUserMap(user);
                var taskInfo = await index
                    .UpdateDocumentsAsync(new List<MeiliSearchUserMap> { userMap })
                    .ConfigureAwait(false);

                await CheckStatus(taskInfo);
            }
            catch (MeilisearchApiError ex) when (ex.Code == "document_not_found")
            {
                // Document doesn't exist (probably deleted), don't add it back
                // This prevents race conditions where delete happens before update
            }
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
    }

    private static MeiliSearchUserMap CreateUserMap(User user)
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
            userMap.ContentLanguages = userCacheItem.ContentLanguages.Select(language => language.GetCode()).ToList();
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