﻿public class SearchController(
    IGlobalSearch _search,
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo,
    SearchResultBuilder _searchResultBuilder
) : ApiBaseController
{
    public readonly record struct SearchAllRequest(string term, string[] languages);

    public readonly record struct AllResult(
        List<SearchPageItem> Pages,
        int PageCount,
        List<SearchQuestionItem> Questions,
        int QuestionCount,
        List<SearchUserItem> Users,
        int UserCount,
        string UserSearchUrl);

    [HttpPost]
    public async Task<AllResult> All([FromBody] SearchAllRequest request)
    {
        var pageItems = new List<SearchPageItem>();
        var questionItems = new List<SearchQuestionItem>();
        var userItems = new List<SearchUserItem>();

        var languages = request.languages?.Length > 0 ? LanguageExtensions.GetLanguages(request.languages) : LanguageExtensions.GetLanguages();
        var elements = await _search.Go(request.term, languages);

        if (elements.Pages.Any())
            _searchResultBuilder.AddPageItems(
                pageItems,
                elements,
                _permissionCheck,
                _sessionUser.UserId,
                languages);

        if (elements.Questions.Any())
            _searchResultBuilder.AddQuestionItems(questionItems, elements, _permissionCheck,
                _questionReadingRepo);

        if (elements.Users.Any())
            _searchResultBuilder.AddUserItems(userItems, elements);

        var result = new AllResult(
            Pages: pageItems,
            PageCount: elements.PageCount,
            Questions: questionItems,
            QuestionCount: elements.QuestionsResultCount,
            Users: userItems,
            UserCount: elements.UsersResultCount,
            UserSearchUrl: Links.UsersSearch(request.term)
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
                _searchResultBuilder.AddPageItems(items, elements, _permissionCheck, _sessionUser.UserId, json.pageIdsToFilter);
            else
                _searchResultBuilder.AddPublicPageItems(items, elements, _sessionUser.UserId, json.pageIdsToFilter);
        }

        return new PageResult(
            TotalCount: elements.PageCount,
            Pages: items
        );
    }

    [HttpPost]
    public async Task<PageResult> PageInPersonalWiki([FromBody] SearchPageJson json)
    {
        var resultPage = new List<SearchPageItem>();
        var elements = await _search.GoAllPagesAsync(json.term);

        if (elements.Pages.Any())
            _searchResultBuilder
                .AddPageItems(
                    resultPage,
                    elements,
                    _permissionCheck,
                    _sessionUser.UserId,
                    json.pageIdsToFilter);

        var wikiChildren = GraphService.Descendants(_sessionUser.FirstWikiId());
        resultPage = resultPage.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

        return new PageResult(
            TotalCount: elements.PageCount,
            Pages: resultPage
        );
    }

    // New section for user-only search

    public readonly record struct SearchUsersRequest(string term, string[] languages);

    public readonly record struct UsersResult(List<SearchUserItem> Users, int UserCount);

    [HttpPost]
    public async Task<UsersResult> Users([FromBody] SearchUsersRequest request)
    {
        var userItems = new List<SearchUserItem>();
        var languages = request.languages?.Length > 0 ? LanguageExtensions.GetLanguages(request.languages) : LanguageExtensions.GetLanguages();
        var elements = await _search.Go(request.term, languages);

        if (elements.Users.Any())
            _searchResultBuilder.AddUserItems(userItems, elements);

        return new UsersResult(
            Users: userItems,
            UserCount: elements.UsersResultCount
        );
    }
}
