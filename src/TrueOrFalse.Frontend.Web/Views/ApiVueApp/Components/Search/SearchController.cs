using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class SearchController(
    IGlobalSearch _search,
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo
) : Controller
{
    public readonly record struct SearchAllJson(string term);

    public readonly record struct AllResult(
        List<SearchTopicItem> Topics,
        int TopicCount,
        List<SearchQuestionItem> Questions,
        int QuestionCount,
        List<SearchUserItem> Users,
        int UserCount,
        string UserSearchUrl);

    [HttpPost]
    public async Task<AllResult> All([FromBody] SearchAllJson json)
    {
        var topicItems = new List<SearchTopicItem>();
        var questionItems = new List<SearchQuestionItem>();
        var userItems = new List<SearchUserItem>();
        var elements = await _search.Go(json.term);

        var searchHelper = new SearchHelper(_imageMetaDataReadingRepo,
            _httpContextAccessor,
            _questionReadingRepo);

        if (elements.Categories.Any())
            searchHelper.AddTopicItems(topicItems, elements, _permissionCheck,
                _sessionUser.UserId);

        if (elements.Questions.Any())
            searchHelper.AddQuestionItems(questionItems, elements, _permissionCheck,
                _questionReadingRepo);

        if (elements.Users.Any())
            searchHelper.AddUserItems(userItems, elements);
        var result = new AllResult(
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

    public readonly record struct SearchTopicJson(
        string term,
        int[] topicIdsToFilter,
        bool? includePrivateTopics = null
    );

    public readonly record struct TopicResult(List<SearchTopicItem> Topics, int TotalCount);

    [HttpPost]
    public async Task<TopicResult> Topic([FromBody] SearchTopicJson json)
    {
        var items = new List<SearchTopicItem>();
        var elements = await _search.GoAllCategoriesAsync(json.term);

        if (elements.Categories.Any())
        {
            bool includePrivateTopics = json.includePrivateTopics ?? true;

            if (includePrivateTopics)
                new SearchHelper(_imageMetaDataReadingRepo, _httpContextAccessor, _questionReadingRepo)
                    .AddTopicItems(items, elements, _permissionCheck, _sessionUser.UserId, json.topicIdsToFilter);
            else
                new SearchHelper(_imageMetaDataReadingRepo, _httpContextAccessor, _questionReadingRepo)
                    .AddPublicTopicItems(items, elements, _sessionUser.UserId, json.topicIdsToFilter);
        }

        return new
        (
            TotalCount: elements.CategoriesResultCount,
            Topics: items
        );
    }


    [HttpPost]
    public async Task<TopicResult> TopicInPersonalWiki(
        [FromBody] SearchTopicJson json)
    {
        var items = new List<SearchTopicItem>();
        var elements = await _search.GoAllCategoriesAsync(json.term);

        if (elements.Categories.Any())
            new SearchHelper(_imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo)
                .AddTopicItems(items,
                    elements,
                    _permissionCheck,
                    _sessionUser.UserId,
                    json.topicIdsToFilter);

        var wikiChildren = GraphService.Descendants(_sessionUser.User.StartTopicId);
        items = items.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

        return new
        (
            TotalCount: elements.CategoriesResultCount,
            Topics: items
        );
    }
}
