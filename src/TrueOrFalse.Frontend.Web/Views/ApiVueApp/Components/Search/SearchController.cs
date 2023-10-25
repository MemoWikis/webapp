using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelperClassesControllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

            var wikiChildren = EntityCache.GetAllChildren(_sessionUser.User.StartTopicId);
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
            if (EntityCache.GetAllChildren(id).Any(c => c.Id == _sessionUser.User.StartTopicId))
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
                        _questionReadingRepo).FillSearchCategoryItem(c, UserId));
                }
            }

            var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);

            return Json(new
            {
                success = true,
                personalWiki = new SearchHelper(_imageMetaDataReadingRepo,
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .FillSearchCategoryItem(personalWiki, UserId),
                addToWikiHistory = recentlyUsedRelationTargetTopicIds.ToArray()
            });
        }
    }
}

namespace HelperClassesControllers
{
    public class SearchAllJson
    {
        public string term { get; set; }
    }
    public class SearchTopicJson
    {
        public string term { get; set; }
        public int[] topicIdsToFilter { get; set; } = null;
    }
}
