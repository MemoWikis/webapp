using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TrueOrFalse.Search;

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

    public async Task<GlobalSearchResult> Go(string term)
    {
        var result = new GlobalSearchResult();
        result.CategoriesResult = await new MeiliSearchCategories(_permissionCheck).RunAsync(term);
        result.QuestionsResult = await new MeiliSearchQuestions(_permissionCheck).RunAsync(term);
        result.UsersResult = await new MeiliSearchUsers(_httpContextAccessor, _webHostEnvironment)
            .RunAsync(term);
        return result;
    }

    public async Task<GlobalSearchResult> GoAllCategoriesAsync(
        string term)
    {
        var result = new GlobalSearchResult();
        result.CategoriesResult =
            await new MeiliSearchCategories(_permissionCheck, 10).RunAsync(term)
                .ConfigureAwait(false);
        return result;
    }

    public async Task<GlobalSearchResult> GoNumberOfCategories(string term, int size)
    {
        var result = new GlobalSearchResult();
        result.CategoriesResult =
            await new MeiliSearchCategories(_permissionCheck, size).RunAsync(term);
        return result;
    }
}