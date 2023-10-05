using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp
{

    public class SearchController(IGlobalSearch search,
            SessionUser sessionUser,
            PermissionCheck permissionCheck,
            ImageMetaDataReadingRepo imageMetaDataReadingRepo,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment,
            QuestionReadingRepo questionReadingRepo)
        : BaseController(sessionUser)
    {

        private readonly IGlobalSearch _search = search ?? throw new ArgumentNullException(nameof(search));

        [HttpGet]
        public async Task<string> All(string term)
        {
            var topicItems = new List<SearchTopicItem>();
            var questionItems = new List<SearchQuestionItem>();
            var userItems = new List<SearchUserItem>();
            var elements = await _search.Go(term);

            var searchHelper = new SearchHelper(imageMetaDataReadingRepo,
                actionContextAccessor,
                httpContextAccessor,
                webHostEnvironment,
                questionReadingRepo);

            if (elements.Categories.Any())
                searchHelper.AddTopicItems(topicItems, elements, permissionCheck, UserId);

            if (elements.Questions.Any())
                searchHelper.AddQuestionItems(questionItems, elements, permissionCheck, questionReadingRepo);

            if (elements.Users.Any())
                searchHelper.AddUserItems(userItems, elements);

            return JsonConvert.SerializeObject(new
            {
                topics = topicItems,
                topicCount = elements.CategoriesResultCount,
                questions = questionItems,
                questionCount = elements.QuestionsResultCount,
                users = userItems,
                userCount = elements.UsersResultCount,
                userSearchUrl = Links.UsersSearch(term)
            });
        }

        [HttpGet]
        public async Task<JsonResult> Topic(string term, string topicIdsToFilter)
        {
            var items = new List<SearchTopicItem>();
            var idArray = topicIdsToFilter.Split(',').Select(int.Parse).ToArray();

            var elements = await _search.GoAllCategories(term, idArray);

            if (elements.Categories.Any())
                new SearchHelper(imageMetaDataReadingRepo,
                    actionContextAccessor,
                    httpContextAccessor,
                    webHostEnvironment,
                    questionReadingRepo).AddTopicItems(items, elements, permissionCheck, UserId);

            return Json(new
            {
                totalCount = elements.CategoriesResultCount,
                topics = items,
            });
        }

        [HttpGet]
        public async Task<JsonResult> TopicInPersonalWiki(string term, string topicIdsToFilter)
        {
            var items = new List<SearchTopicItem>();
            var idArray = topicIdsToFilter.Split(',').Select(int.Parse).ToArray();

            var elements = await _search.GoAllCategories(term, idArray);

            if (elements.Categories.Any())
                new SearchHelper(imageMetaDataReadingRepo,
                        actionContextAccessor,
                        httpContextAccessor,
                        webHostEnvironment,
                        questionReadingRepo)
                    .AddTopicItems(items,
                        elements,
                        permissionCheck,
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
        public JsonResult GetPersonalWikiData(int id)
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
                    recentlyUsedRelationTargetTopicIds.Add(new SearchHelper(imageMetaDataReadingRepo,
                        actionContextAccessor,
                        httpContextAccessor,
                        webHostEnvironment,
                        questionReadingRepo).FillSearchCategoryItem(c, UserId));
                }
            }

            var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);

            return Json(new
            {
                success = true,
                personalWiki = new SearchHelper(imageMetaDataReadingRepo,
                        actionContextAccessor,
                        httpContextAccessor,
                        webHostEnvironment,
                        questionReadingRepo)
                    .FillSearchCategoryItem(personalWiki, UserId),
                addToWikiHistory = recentlyUsedRelationTargetTopicIds.ToArray()
            });
        }
    }
}
