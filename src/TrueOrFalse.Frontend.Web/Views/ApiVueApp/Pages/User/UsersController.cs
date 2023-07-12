using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

namespace VueApp;

public class VueUsersController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly MeiliSearchUsers _meiliSearchUsers;
    private readonly GetTotalUsers _totalUsers;
    private readonly UserSummary _userSummary;
    private readonly QuestionValuationRepo _questionValuationRepo;

    public VueUsersController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        MeiliSearchUsers meiliSearchUsers,
        GetTotalUsers totalUsers,
        UserSummary userSummary,
        QuestionValuationRepo questionValuationRepo
        )
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _meiliSearchUsers = meiliSearchUsers;
        _totalUsers = totalUsers;
        _userSummary = userSummary;
        _questionValuationRepo = questionValuationRepo;
    }

    [HttpGet]
    public async Task<JsonResult> Get(
        int page,
        int pageSize,
        string searchTerm = "",
        SearchUsersOrderBy orderBy = SearchUsersOrderBy.None)
    {
        var result = await _meiliSearchUsers.GetUsersByPagerAsync(searchTerm,
            new Pager { PageSize = pageSize, IgnorePageCount = true, CurrentPage = page }, orderBy);

        var users = EntityCache.GetUsersByIds(result.searchResultUser.Select(u => u.Id));
        var usersResult = users.Select(GetUserResult);

        return Json(new
        {
            users = usersResult.ToArray(),
            totalItems = result.pager.TotalItems
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetTotalUserCount()
    {
        return Json(_totalUsers.Run(), JsonRequestBehavior.AllowGet);
    }

    public UserResult GetUserResult(UserCacheItem user)
    {
        var wishQuestionCount = 0;
        var topicsWithWishQuestionCount = 0;

        if (user.Id > 0 && (user.ShowWishKnowledge || user.Id == _sessionUser.UserId))
        {
            var valuations = _questionValuationRepo
                .GetByUserFromCache(user.Id)
                .QuestionIds().ToList();
            var wishQuestions = EntityCache.GetQuestionsByIds(valuations).Where(_permissionCheck.CanView);
            wishQuestionCount = wishQuestions.Count();
            topicsWithWishQuestionCount = wishQuestions.QuestionsInCategories().Count();
        }

        return new UserResult
        {
            name = user.Name,
            id = user.Id,
            reputationPoints = user.Reputation,
            rank = user.ReputationPos,
            createdQuestionsCount =
               _userSummary.AmountCreatedQuestions(user.Id, _sessionUser.UserId == user.Id),
            createdTopicsCount = _userSummary.AmountCreatedCategories(user.Id, _sessionUser.UserId == user.Id),
            showWuwi = user.ShowWishKnowledge,
            wuwiQuestionsCount = wishQuestionCount,
            wuwiTopicsCount = topicsWithWishQuestionCount,
            imgUrl = new UserImageSettings(user.Id).GetUrl_128px_square(user).Url,
            wikiId = _permissionCheck.CanViewCategory(user.StartTopicId) ? user.StartTopicId : -1
        };
    }

    public class UserResult
    {
        public int createdQuestionsCount { get; set; }
        public int createdTopicsCount { get; set; }
        public int id { get; set; }
        public string imgUrl { get; set; }
        public string name { get; set; }
        public int rank { get; set; }
        public int reputationPoints { get; set; }
        public bool showWuwi { get; set; }
        public int wikiId { get; set; }
        public int wuwiQuestionsCount { get; set; }
        public int wuwiTopicsCount { get; set; }
    }
}