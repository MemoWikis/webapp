using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class MeiliGlobalSearch : IGlobalSearch
{
    private readonly PermissionCheck _permissionCheck;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MeiliGlobalSearch(
        PermissionCheck permissionCheck,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _permissionCheck = permissionCheck;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<GlobalSearchResult> Go(string term, List<Language> languages)
    {
        var result = new GlobalSearchResult();
        result.PagesResult = await new MeilisearchPages(_permissionCheck).RunAsync(term, languages);
        result.QuestionsResult = await new MeilisearchQuestions(_permissionCheck).RunAsync(term, languages);
        result.UsersResult = await new MeilisearchUsers().RunAsync(term, languages);
        return result;
    }

    public async Task<GlobalSearchResult> GoAllPagesAsync(string term)
    {
        var result = new GlobalSearchResult();
        result.PagesResult =
            await new MeilisearchPages(_permissionCheck, 10)
                .RunAsync(term)
                .ConfigureAwait(false);
        return result;
    }

    public async Task<GlobalSearchResult> GoNumberOfPages(string term, int size)
    {
        var result = new GlobalSearchResult();
        result.PagesResult =
            await new MeilisearchPages(_permissionCheck, size).RunAsync(term);
        return result;
    }
}