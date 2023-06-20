using System.Threading.Tasks;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
public class MeiliGlobalSearch : IGlobalSearch
{
    public async Task<GlobalSearchResult> Go(string term, string type)
    {
        var result = new GlobalSearchResult();
        result.CategoriesResult = await new MeiliSearchCategories().RunAsync(term);
        result.QuestionsResult = await new MeiliSearchQuestions().RunAsync(term);
        result.UsersResult = await new MeiliSearchUsers().RunAsync(term);

        return result;
    }

    public async Task<GlobalSearchResult> GoAllCategories(string term, int[] categoriesToFilter = null)
    {
        var result = new GlobalSearchResult();
        result.CategoriesResult = await new MeiliSearchCategories(10).RunAsync(term);
        return result;
    }

    public async Task<GlobalSearchResult> GoAllCategories(string term, int size)
    {
        var result = new GlobalSearchResult();
        result.CategoriesResult = await new MeiliSearchCategories(size).RunAsync(term);
        return result;
    }
}