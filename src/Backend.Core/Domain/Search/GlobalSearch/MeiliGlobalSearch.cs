using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class MeiliGlobalSearch : IGlobalSearch
{
    private readonly PermissionCheck _permissionCheck;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SessionUser _sessionUser;

    public MeiliGlobalSearch(
        PermissionCheck permissionCheck,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        SessionUser sessionUser)
    {
        _permissionCheck = permissionCheck;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _sessionUser = sessionUser;
    }

    public async Task<GlobalSearchResult> Go(string term, List<Language> languages)
    {
        var result = new GlobalSearchResult();
        var currentUserName = _sessionUser.IsLoggedIn ? _sessionUser.User.Name : string.Empty;
        
        result.PagesResult = await new MeilisearchPages(_permissionCheck, 5, currentUserName).RunAsync(term, languages);
        result.QuestionsResult = await new MeilisearchQuestions(_permissionCheck).RunAsync(term, languages);
        result.UsersResult = await new MeilisearchUsers().RunAsync(term, languages);
        return result;
    }

    public async Task<GlobalSearchResult> GoAllPagesAsync(string term)
    {
        var result = new GlobalSearchResult();
        var currentUserName = _sessionUser.IsLoggedIn ? _sessionUser.User.Name : string.Empty;
        
        result.PagesResult =
            await new MeilisearchPages(_permissionCheck, 10, currentUserName)
                .RunAsync(term)
                .ConfigureAwait(false);
        return result;
    }

    public async Task<GlobalSearchResult> GoNumberOfPages(string term, int size)
    {
        var result = new GlobalSearchResult();
        var currentUserName = _sessionUser.IsLoggedIn ? _sessionUser.User.Name : string.Empty;
        
        result.PagesResult =
            await new MeilisearchPages(_permissionCheck, size, currentUserName).RunAsync(term);
        return result;
    }
}