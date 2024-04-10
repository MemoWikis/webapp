using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp
{
    public class SearchController : BaseController
    {
        private readonly PermissionCheck _permissionCheck;
        private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly QuestionReadingRepo _questionReadingRepo;
        private readonly IGlobalSearch _search;

        public SearchController(IGlobalSearch search,
            SessionUser sessionUser,
            PermissionCheck permissionCheck,
            ImageMetaDataReadingRepo imageMetaDataReadingRepo,
            IHttpContextAccessor httpContextAccessor,
            QuestionReadingRepo questionReadingRepo) :base(sessionUser)
        {
            _search = search;
            _permissionCheck = permissionCheck;
            _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
            _httpContextAccessor = httpContextAccessor;
            _questionReadingRepo = questionReadingRepo;
        }
        public readonly record struct SearchAllJson(string term);
        [HttpPost]
        public async Task<JsonResult> All([FromBody] SearchAllJson json)
        {
            var topicItems = new List<SearchTopicItem>();
            var questionItems = new List<SearchQuestionItem>();
            var userItems = new List<SearchUserItem>();
            var elements = await _search.Go(json.term);

            var searchHelper = new SearchHelper(_imageMetaDataReadingRepo,
                _httpContextAccessor,
                _questionReadingRepo);

            if (elements.Categories.Any())
                searchHelper.AddTopicItems(topicItems, elements, _permissionCheck, UserId);

            if (elements.Questions.Any())
                searchHelper.AddQuestionItems(questionItems, elements, _permissionCheck, _questionReadingRepo);

            if (elements.Users.Any())
                searchHelper.AddUserItems(userItems, elements);
            var result = Json(new
            {
                topics = topicItems,
                topicCount = elements.CategoriesResultCount,
                questions = questionItems,
                questionCount = elements.QuestionsResultCount,
                users = userItems,
                userCount = elements.UsersResultCount,
                userSearchUrl = Links.UsersSearch(json.term)
            });

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
                    _questionReadingRepo).AddTopicItems(items, elements, _permissionCheck, UserId);

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
                        UserId);

            var wikiChildren = GraphService.Descendants(_sessionUser.User.StartTopicId);
            items = items.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

            return Json(new
            {
                totalCount = elements.CategoriesResultCount,
                topics = items,
            });
        }
    }
}
