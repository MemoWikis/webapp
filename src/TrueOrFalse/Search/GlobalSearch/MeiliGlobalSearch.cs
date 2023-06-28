using System.Threading.Tasks;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
public class MeiliGlobalSearch : IGlobalSearch
{
    private readonly PermissionCheck _permissionCheck;

    public MeiliGlobalSearch(PermissionCheck permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }
    public async Task<GlobalSearchResult> Go(string term, string type)
    {
        var result = new GlobalSearchResult();
        result.CategoriesResult = await new MeiliSearchCategories(_permissionCheck).RunAsync(term);
        result.QuestionsResult = await new MeiliSearchQuestions(_permissionCheck).RunAsync(term);
        result.UsersResult = await new MeiliSearchUsers().RunAsync(term);
        return result;
    }

    public async Task<GlobalSearchResult> GoAllCategories(string term, int[] categoriesToFilter = null)
    {
        var result = new GlobalSearchResult();
        result.CategoriesResult = await new MeiliSearchCategories(_permissionCheck, 10).RunAsync(term);
        return result;
    }

    public async Task<GlobalSearchResult> GoNumberOfCategories(string term, int size)
    {
        var result = new GlobalSearchResult();
        result.CategoriesResult = await new MeiliSearchCategories(_permissionCheck, size).RunAsync(term);
        return result;
    }
}