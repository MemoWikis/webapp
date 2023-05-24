using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
using TrueOrFalse.Web;

namespace VueApp;

public class VueUsersController : BaseController
{
    [HttpGet]
    public JsonResult Get(
        int page,
        int pageSize,
        string searchTerm = "",
        SearchUsersOrderBy orderBy = SearchUsersOrderBy.None)
    {
        var solrResult = Sl.SolrSearchUsers.Run(searchTerm,
            new Pager { PageSize = pageSize, IgnorePageCount = true, CurrentPage = page }, orderBy);

        var users = EntityCache.GetUsersByIds(solrResult.UserIds);
        var usersResult = users.Select(GetUserResult);

        return Json(new
        {
            users = usersResult.ToArray(),
            totalItems = solrResult.Pager.TotalItems
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetTotalUserCount()
    {
        return Json(R<GetTotalUsers>().Run(), JsonRequestBehavior.AllowGet);
    }

    public UserResult GetUserResult(UserCacheItem user)
    {
        var wishQuestionCount = 0;
        var topicsWithWishQuestionCount = 0;

        if (user.Id > 0 && (user.ShowWishKnowledge || user.Id == SessionUser.UserId))
        {
            var valuations = Sl.QuestionValuationRepo
                .GetByUserFromCache(user.Id)
                .QuestionIds().ToList();
            var wishQuestions = EntityCache.GetQuestionsByIds(valuations).Where(PermissionCheck.CanView);
            wishQuestionCount = wishQuestions.Count();
            topicsWithWishQuestionCount = wishQuestions.QuestionsInCategories().Count();
        }

        return new UserResult
        {
            name = user.Name,
            id = user.Id,
            encodedName = UriSanitizer.Run(user.Name, 12),
            reputationPoints = user.Reputation,
            rank = user.ReputationPos,
            createdQuestionsCount =
                Resolve<UserSummary>().AmountCreatedQuestions(user.Id, SessionUser.UserId == user.Id),
            createdTopicsCount = Resolve<UserSummary>().AmountCreatedCategories(user.Id, SessionUser.UserId == user.Id),
            showWuwi = user.ShowWishKnowledge,
            wuwiQuestionsCount = wishQuestionCount,
            wuwiTopicsCount = topicsWithWishQuestionCount,
            imgUrl = new UserImageSettings(user.Id).GetUrl_128px_square(user).Url,
            wikiId = PermissionCheck.CanViewCategory(user.StartTopicId) ? user.StartTopicId : -1
        };
    }

    public class UserResult
    {
        public int createdQuestionsCount { get; set; }
        public int createdTopicsCount { get; set; }
        public string encodedName { get; set; }
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