using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Domain.Question.QuestionValuation;
using TrueOrFalse.Search;

namespace VueApp;

public class VueUsersController : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly MeiliSearchUsers _meiliSearchUsers;
    private readonly GetTotalUsers _totalUsers;
    private readonly UserSummary _userSummary;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SessionUserCache _sessionUserCache;

    public VueUsersController(
        SessionUser sessionUser,
        PermissionCheck permissionCheck,
        MeiliSearchUsers meiliSearchUsers,
        GetTotalUsers totalUsers,
        UserSummary userSummary,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        SessionUserCache sessionUserCache) : base(sessionUser)
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _meiliSearchUsers = meiliSearchUsers;
        _totalUsers = totalUsers;
        _userSummary = userSummary;
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _sessionUserCache = sessionUserCache;
    }

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
            var valuations = new QuestionValuationCache(_sessionUserCache)
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

    public class UserResult
    {
        public int CreatedQuestionsCount { get; set; }
        public int CreatedTopicsCount { get; set; }
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
        public int ReputationPoints { get; set; }
        public bool ShowWuwi { get; set; }
        public int WikiId { get; set; }
        public int WuwiQuestionsCount { get; set; }
        public int WuwiTopicsCount { get; set; }
    }
}