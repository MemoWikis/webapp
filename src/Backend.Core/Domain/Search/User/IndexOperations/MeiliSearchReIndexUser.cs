using Meilisearch;

public class MeilisearchReIndexUser(UserReadingRepo _userReadingRepo) : IRegisterAsInstancePerLifetime
{
    private readonly MeilisearchClient _client = new(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);

    public async Task RunAll()
    {
        await _client.DeleteIndexAsync(MeilisearchIndices.Users);
        //await _client.DeleteIndexAsync("MeiliSearchPage");

        var allUser = _userReadingRepo.GetAll();
        var meiliSearchUserList = new List<MeiliSearchUserMap>();

        foreach (var user in allUser)
            meiliSearchUserList.Add(MeiliSearchToUserMap.Run(user));

        await _client.CreateIndexAsync(MeilisearchIndices.Users);
        var index = _client.Index(MeilisearchIndices.Users);
        await index.UpdateFilterableAttributesAsync(new[] { "ContentLanguages" });
        
        await index.UpdateRankingRulesAsync(MeilisearchSort.Default);
        
        await index.AddDocumentsAsync(meiliSearchUserList);
    }

    public async Task RunAllCache()
    {
        await _client.DeleteIndexAsync(MeilisearchIndices.Users);
        //await _client.DeleteIndexAsync("MeiliSearchPage");

        var allUser = EntityCache.GetAllUsers();
        var meiliSearchUserList = new List<MeiliSearchUserMap>();

        foreach (var user in allUser)
            meiliSearchUserList.Add(MeiliSearchToUserMap.Run(user));

        await _client.CreateIndexAsync(MeilisearchIndices.Users);
        var index = _client.Index(MeilisearchIndices.Users);
        await index.UpdateFilterableAttributesAsync(new[] { "ContentLanguages" });
        
        await index.UpdateRankingRulesAsync(MeilisearchSort.Default);
        
        await index.AddDocumentsAsync(meiliSearchUserList);
    }

    public async Task Run(UserCacheItem user)
    {
        var meiliSearchUser = MeiliSearchToUserMap.Run(user);
        var index = _client.Index(MeilisearchIndices.Users);
        await index.AddDocumentsAsync(new List<MeiliSearchUserMap> { meiliSearchUser });
    }

    public async Task Run(User user)
    {
        var meiliSearchUser = MeiliSearchToUserMap.Run(user);
        var index = _client.Index(MeilisearchIndices.Users);
        await index.AddDocumentsAsync(new List<MeiliSearchUserMap> { meiliSearchUser });
    }
}