using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
using TrueOrFalse.Web;

namespace VueApp;

public class VueUsersController : BaseController
{
    [HttpPost]
    public JsonResult Get(int page, int usersPerPageCount, string searchTerm = "", SearchUsersOrderBy orderBy = SearchUsersOrderBy.None)
    {
        var solrResult = Sl.SolrSearchUsers.Run(searchTerm, new Pager { PageSize = usersPerPageCount, IgnorePageCount = true, CurrentPage = page }, orderBy);

        var users = EntityCache.GetUsersByIds(solrResult.UserIds);

        var usersResult = users.Select(u =>
        {
            var wishQuestionCount = 0;
            var topicsWithWishQuestionCount = 0;

            if (u.ShowWishKnowledge || u.Id == SessionUser.UserId)
            {
                var valuations = Sl.QuestionValuationRepo
                    .GetByUserFromCache(u.Id)
                    .QuestionIds().ToList();
                var wishQuestions = EntityCache.GetQuestionsByIds(valuations).Where(PermissionCheck.CanView);
                wishQuestionCount = wishQuestions.Count();
                topicsWithWishQuestionCount = wishQuestions.QuestionsInCategories().Count();
            }

            return new UserResult
            {
                name = u.Name,
                id = u.Id,
                encodedName = UriSanitizer.Run(u.Name, 12),
                reputationPoints = u.Reputation,
                rank = u.ReputationPos,
                createdQuestionsCount = Resolve<UserSummary>().AmountCreatedQuestions(u.Id, SessionUser.UserId == u.Id),
                createdTopicsCount = Resolve<UserSummary>().AmountCreatedCategories(u.Id, SessionUser.UserId == u.Id),
                showWuwi = u.ShowWishKnowledge,
                wuwiQuestionsCount = wishQuestionCount,
                wuwiTopicsCount = topicsWithWishQuestionCount
            };
        });


        return Json(new
        {
            users = usersResult.ToArray(),
            currentPage = solrResult.Pager.CurrentPage,
            totalPages = solrResult.Pager.PageCount,
            totalItems = solrResult.Pager.TotalItems,
        });
    }

    public class UserResult
    {
        public string name { get; set; }
        public int id { get; set; }
        public string encodedName { get; set; }
        public int reputationPoints { get; set; }
        public int rank { get; set; }
        public int createdQuestionsCount { get; set; }
        public int createdTopicsCount { get; set; }
        public bool showWuwi { get; set; }
        public int wuwiQuestionsCount { get; set; }
        public int wuwiTopicsCount { get; set; }

    }
}