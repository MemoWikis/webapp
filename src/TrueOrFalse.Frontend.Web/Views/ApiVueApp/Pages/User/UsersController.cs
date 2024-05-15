using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Domain.Question.QuestionValuation;
using TrueOrFalse.Search;

namespace VueApp;

public class UsersController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    MeiliSearchUsers _meiliSearchUsers,
    GetTotalUsers _totalUsers,
    UserSummary _userSummary,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _extendedUserCache) : Controller
{
    public record struct UserResult(
        int CreatedQuestionsCount,
        int CreatedTopicsCount,
        int Id,
        string ImgUrl,
        string Name,
        int Rank,
        int ReputationPoints,
        bool ShowWuwi,
        int WikiId,
        int WuwiQuestionsCount,
        int WuwiTopicsCount
    );

    public readonly record struct UsersResult(IEnumerable<UserResult> Users, int TotalItems);

    [HttpGet]
    public async Task<UsersResult> Get(
        int page,
        int pageSize,
        string searchTerm = "",
        SearchUsersOrderBy orderBy = SearchUsersOrderBy.None)
    {
        var result = await _meiliSearchUsers.GetUsersByPagerAsync(searchTerm,
            new Pager { PageSize = pageSize, IgnorePageCount = true, CurrentPage = page }, orderBy);

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

    public UserResult GetUserResult(UserCacheItem user)
    {
        var wishQuestionCount = 0;
        var topicsWithWishQuestionCount = 0;

        if (user.Id > 0 && (user.ShowWishKnowledge || user.Id == _sessionUser.UserId))
        {
            var valuations = new QuestionValuationCache(_extendedUserCache)
                .GetByUserFromCache(user.Id)
                .QuestionIds().ToList();
            var wishQuestions = EntityCache.GetQuestionsByIds(valuations)
                .Where(_permissionCheck.CanView);
            wishQuestionCount = wishQuestions.Count();
            topicsWithWishQuestionCount = wishQuestions.QuestionsInCategories().Count();
        }

        return new UserResult
        {
            Name = user.Name,
            Id = user.Id,
            ReputationPoints = user.Reputation,
            Rank = user.ReputationPos,
            CreatedQuestionsCount =
                _userSummary.AmountCreatedQuestions(user.Id, _sessionUser.UserId == user.Id),
            CreatedTopicsCount =
                _userSummary.AmountCreatedCategories(user.Id, _sessionUser.UserId == user.Id),
            ShowWuwi = user.ShowWishKnowledge,
            WuwiQuestionsCount = wishQuestionCount,
            WuwiTopicsCount = topicsWithWishQuestionCount,
            ImgUrl = new UserImageSettings(user.Id, _httpContextAccessor)
                .GetUrl_128px_square(user)
                .Url,
            WikiId = _permissionCheck.CanViewCategory(user.StartTopicId) ? user.StartTopicId : -1
        };
    }
}