public class UsersController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    MeilisearchUsers _meilisearchUsers,
    GetTotalUsers _totalUsers,
    UserSummary _userSummary,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _extendedUserCache) : ApiBaseController
{
    public record struct UserResult(
        int CreatedQuestionsCount,
        int CreatedPagesCount,
        int Id,
        string ImgUrl,
        string Name,
        int Rank,
        int ReputationPoints,
        bool ShowWishKnowledge,
        int WikiId,
        int WishKnowledgeQuestionsCount,
        int WishKnowledgePagesCount,
        List<string> ContentLanguages);

    public readonly record struct UsersResult(IEnumerable<UserResult> Users, int TotalItems);

    [HttpGet]
    public async Task<UsersResult> Get(
        int page,
        int pageSize,
        [FromQuery(Name = "languages")] string[] languages,
        string searchTerm = "",
        SearchUsersOrderBy orderBy = SearchUsersOrderBy.Rank)
    {
        var pager = new Pager { PageSize = pageSize, IgnorePageCount = true, CurrentPage = page };

        var result = await _meilisearchUsers.GetUsersByPagerAsync(
            searchTerm,
            pager,
            orderBy,
            languages);

        var users = EntityCache.GetUsersByIds(result.searchResultUser.Select(u => u.Id));
        var usersResult = users.Select(GetUserResult);

        return new UsersResult
        {
            Users = usersResult,
            TotalItems = result.pager.TotalItems
        };
    }

    [HttpGet]
    public int GetTotalUserCount()
    {
        return _totalUsers.Run();
    }

    private UserResult GetUserResult(UserCacheItem user)
    {
        var wishQuestionCount = 0;
        var pagesWithWishQuestionCount = 0;

        if (user.Id > 0 && (user.ShowWishKnowledge || user.Id == _sessionUser.UserId))
        {
            var valuations = new QuestionValuationCache(_extendedUserCache)
                .GetByUserFromCache(user.Id)
                .QuestionIds().ToList();
            //var wishQuestions = EntityCache.GetQuestionsByIds(valuations)
            //    .Where(_permissionCheck.CanView);
            var wishQuestions = EntityCache.GetQuestionsByIds(valuations);
            wishQuestionCount = wishQuestions.Count();
            pagesWithWishQuestionCount = wishQuestions.QuestionsInPages().Count();
        }

        return new UserResult
        {
            Name = user.Name,
            Id = user.Id,
            ReputationPoints = user.Reputation,
            Rank = user.ReputationPos,
            CreatedQuestionsCount = _userSummary.GetCreatedQuestionCount(user.Id, _sessionUser.UserId == user.Id),
            CreatedPagesCount = _userSummary.GetCreatedPagesCount(user.Id, _sessionUser.UserId == user.Id),
            ShowWishKnowledge = user.ShowWishKnowledge,
            WishKnowledgeQuestionsCount = wishQuestionCount,
            WishKnowledgePagesCount = pagesWithWishQuestionCount,
            ImgUrl = new UserImageSettings(user.Id, _httpContextAccessor)
                .GetUrl_128px_square(user)
                .Url,
            WikiId = _permissionCheck.CanViewPage(user.FirstWikiId) ? user.FirstWikiId : -1,
            ContentLanguages = user.ContentLanguages.Select(l => l.GetCode()).ToList()
        };
    }
}