using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp
{
    public class SearchController(IGlobalSearch _search,
        SessionUser _sessionUser,
        PermissionCheck _permissionCheck,
        ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
        IHttpContextAccessor _httpContextAccessor,
        QuestionReadingRepo _questionReadingRepo) : Controller
    {

        public readonly record struct SearchAllJson(string term);

        public readonly record struct AllJson(
           List<SearchTopicItem> Topics,
            int TopicCount,
            List<SearchQuestionItem> Questions,
            int QuestionCount,
            List<SearchUserItem> Users,
            int UserCount,
            string UserSearchUrl);

        [HttpPost]
        public async Task<AllJson> All([FromBody] SearchAllJson json)
        {
            var topicItems = new List<SearchTopicItem>();
            var questionItems = new List<SearchQuestionItem>();
            var userItems = new List<SearchUserItem>();
            var elements = await _search.Go(json.term);

            var searchHelper = new SearchHelper(_imageMetaDataReadingRepo,
                _httpContextAccessor,
                _questionReadingRepo);

            if (elements.Categories.Any())
                searchHelper.AddTopicItems(topicItems, elements, _permissionCheck, _sessionUser.UserId);

            if (elements.Questions.Any())
                searchHelper.AddQuestionItems(questionItems, elements, _permissionCheck, _questionReadingRepo);

            if (elements.Users.Any())
                searchHelper.AddUserItems(userItems, elements);
            var result = new AllJson(
            
                Topics: topicItems,
                TopicCount: elements.CategoriesResultCount,
                Questions: questionItems,
                QuestionCount: elements.QuestionsResultCount,
                Users: userItems,
                UserCount: elements.UsersResultCount,
                UserSearchUrl: Links.UsersSearch(json.term)
            );

            return result;
        }
        public readonly record struct SearchTopicJson(string term, int[] topicIdsToFilter);
        [HttpPost]
        public async Task<JsonResult> Topic([FromBody] SearchTopicJson json)
        {
            var items = new List<SearchTopicItem>();
            var elements = await _search.GoAllCategories(json.term, json.topicIdsToFilter);

            if (elements.Categories.Any())
                new SearchHelper(_imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo).AddTopicItems(items, elements, _permissionCheck, _sessionUser.UserId);

            return Json(new
            {
                totalCount = elements.CategoriesResultCount,
                topics = items,
            });
        }

        [HttpPost]
        public async Task<JsonResult> TopicInPersonalWiki([FromBody] SearchTopicJson json)
        {
            var items = new List<SearchTopicItem>();
            var elements = await _search.GoAllCategories(json.term, json.topicIdsToFilter);

            if (elements.Categories.Any())
                new SearchHelper(_imageMetaDataReadingRepo,
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .AddTopicItems(items,
                        elements,
                        _permissionCheck,
                        _sessionUser.UserId);

            var wikiChildren = GraphService.Descendants(_sessionUser.User.StartTopicId);
            items = items.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

            return Json(new
            {
                totalCount = elements.CategoriesResultCount,
                topics = items,
            });
        }

        [AccessOnlyAsLoggedIn]
        [HttpPost]
        public JsonResult GetPersonalWikiData([FromRoute] int id)
        {
            if (GraphService.VisibleDescendants(id, _permissionCheck, _sessionUser.UserId).Any(c => c.Id == _sessionUser.User.StartTopicId))
                return Json(new
                {
                    success = false,
                });

            var recentlyUsedRelationTargetTopicIds = new List<SearchCategoryItem>();

            if (_sessionUser.User.RecentlyUsedRelationTargetTopicIds != null &&
                _sessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
            {
                foreach (var categoryId in _sessionUser.User.RecentlyUsedRelationTargetTopicIds)
                {
                    var c = EntityCache.GetCategory(categoryId);
                    recentlyUsedRelationTargetTopicIds.Add(new SearchHelper(_imageMetaDataReadingRepo,
                        _httpContextAccessor,
                        _questionReadingRepo).FillSearchCategoryItem(c, _sessionUser.UserId));
                }
            }

            var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);

            return Json(new
            {
                success = true,
                personalWiki = new SearchHelper(_imageMetaDataReadingRepo,
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .FillSearchCategoryItem(personalWiki, _sessionUser.UserId),
                addToWikiHistory = recentlyUsedRelationTargetTopicIds.ToArray()
            });
        }
    }
}
