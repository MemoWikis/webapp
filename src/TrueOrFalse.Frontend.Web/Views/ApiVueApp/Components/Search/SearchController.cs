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
        List<SearchPageItem> Pages,
        int PageCount,
        List<SearchQuestionItem> Questions,
        int QuestionCount,
        List<SearchUserItem> Users,
        int UserCount,
        string UserSearchUrl);

    [HttpPost]
    public async Task<AllResult> All([FromBody] SearchAllJson json)
    {
        var pageItems = new List<SearchPageItem>();
        var questionItems = new List<SearchQuestionItem>();
        var userItems = new List<SearchUserItem>();
        var elements = await _search.Go(json.term);

        var searchHelper = new SearchHelper(_imageMetaDataReadingRepo,
            _httpContextAccessor,
            _questionReadingRepo);

        if (elements.Pages.Any())
            searchHelper.AddPageItems(pageItems, elements, _permissionCheck,
                _sessionUser.UserId);

        if (elements.Questions.Any())
            searchHelper.AddQuestionItems(questionItems, elements, _permissionCheck,
                _questionReadingRepo);

        if (elements.Users.Any())
            searchHelper.AddUserItems(userItems, elements);
        var result = new AllResult(
            Pages: pageItems,
            PageCount: elements.PageCount,
            Questions: questionItems,
            QuestionCount: elements.QuestionsResultCount,
            Users: userItems,
            UserCount: elements.UsersResultCount,
            UserSearchUrl: Links.UsersSearch(json.term)
        );

        return result;
    }

    public readonly record struct SearchPageJson(
        string term,
        int[] pageIdsToFilter,
        bool? includePrivatePages = null
    );

    public readonly record struct PageResult(List<SearchPageItem> Pages, int TotalCount);

    [HttpPost]
    public async Task<PageResult> Page([FromBody] SearchPageJson json)
    {
        var items = new List<SearchPageItem>();
        var elements = await _search.GoAllPagesAsync(json.term);

        if (elements.Pages.Any())
        {
            bool includePrivatePages = json.includePrivatePages ?? true;

            if (includePrivatePages)
                new SearchHelper(_imageMetaDataReadingRepo, _httpContextAccessor, _questionReadingRepo)
                    .AddPageItems(items, elements, _permissionCheck, _sessionUser.UserId, json.pageIdsToFilter);
            else
                new SearchHelper(_imageMetaDataReadingRepo, _httpContextAccessor, _questionReadingRepo)
                    .AddPublicPageItems(items, elements, _sessionUser.UserId, json.pageIdsToFilter);
        }

        return new
        (
            TotalCount: elements.PageCount,
            Pages: items
        );
    }


    [HttpPost]
    public async Task<PageResult> PageInPersonalWiki(
        [FromBody] SearchPageJson json)
    {
        var items = new List<SearchPageItem>();
        var elements = await _search.GoAllPagesAsync(json.term);

        if (elements.Pages.Any())
            new SearchHelper(_imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo)
                .AddPageItems(items,
                    elements,
                    _permissionCheck,
                    _sessionUser.UserId,
                    json.pageIdsToFilter);

        var wikiChildren = GraphService.Descendants(_sessionUser.User.StartPageId);
        items = items.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

        return new
        (
            TotalCount: elements.PageCount,
            Pages: items
        );
    }
}
