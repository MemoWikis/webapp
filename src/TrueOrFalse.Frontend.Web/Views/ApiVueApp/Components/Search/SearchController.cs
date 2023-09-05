using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class SearchController : BaseController
{

    private readonly IGlobalSearch _search;
    private readonly PermissionCheck _permissionCheck;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SearchController(IGlobalSearch search,
        SessionUser sessionUser,
        PermissionCheck permissionCheck,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment) :base(sessionUser)
    {
        _search = search ?? throw new ArgumentNullException(nameof(search));
        _permissionCheck = permissionCheck;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _actionContextAccessor = actionContextAccessor;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<JsonResult> All(string term, string type)
    {
        var topicItems = new List<SearchTopicItem>();
        var questionItems = new List<SearchQuestionItem>();
        var userItems = new List<SearchUserItem>();
        var elements = await _search.Go(term, type);

        var searchHelper = new SearchHelper(_imageMetaDataReadingRepo,
            _actionContextAccessor,
            _httpContextAccessor,
            _webHostEnvironment);

        if (elements.Categories.Any())
            searchHelper.AddTopicItems(topicItems, elements, _permissionCheck, UserId);

        if (elements.Questions.Any())
            searchHelper.AddQuestionItems(questionItems, elements,_permissionCheck);

        if (elements.Users.Any())
            searchHelper.AddUserItems(userItems, elements);

        return Json(new
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
            new SearchHelper(_imageMetaDataReadingRepo,
                _actionContextAccessor,
                _httpContextAccessor,
                _webHostEnvironment).AddTopicItems(items, elements,_permissionCheck, UserId);

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
            new SearchHelper(_imageMetaDataReadingRepo,
                    _actionContextAccessor, 
                    _httpContextAccessor, 
                    _webHostEnvironment )
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
    public JsonResult GetPersonalWikiData(int id)
    {
        if (EntityCache.GetAllChildren(id).Any(c => c.Id == _sessionUser.User.StartTopicId))
            return Json(new
            {
                success = false,
            });

        var recentlyUsedRelationTargetTopicIds = new List<SearchCategoryItem>();

        if (_sessionUser.User.RecentlyUsedRelationTargetTopicIds != null && _sessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var categoryId in _sessionUser.User.RecentlyUsedRelationTargetTopicIds)
            {
                var c = EntityCache.GetCategory(categoryId);
                recentlyUsedRelationTargetTopicIds.Add(new SearchHelper(_imageMetaDataReadingRepo,
                    _actionContextAccessor,
                    _httpContextAccessor,
                    _webHostEnvironment).FillSearchCategoryItem(c, UserId));
            }
        }

        var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);

        return Json(new
        {
            success = true,
            personalWiki = new SearchHelper(_imageMetaDataReadingRepo,
                _actionContextAccessor,
                _httpContextAccessor,
                _webHostEnvironment)
                .FillSearchCategoryItem(personalWiki, UserId),
            addToWikiHistory = recentlyUsedRelationTargetTopicIds.ToArray()
        });
    }
}