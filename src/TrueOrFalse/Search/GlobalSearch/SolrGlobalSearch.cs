using System.Threading.Tasks;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Search.GlobalSearch;

public class SolrGlobalSearch : IGlobalSearch
{
    public Task<SolrGlobalSearchResult> Go(string term, string type)
    {
        var result = new SolrGlobalSearchResult();

        var pageSize = 5;
        result.CategoriesResult = Sl.SearchCategories.Run(term, new Pager { PageSize = pageSize });

        result.UsersResult = Sl.SearchUsers.Run(term, new Pager { PageSize = pageSize }, SearchUsersOrderBy.None);

        var searchSpec = Sl.SessionUiData.SearchSpecQuestionSearchBox;
        searchSpec.OrderBy.BestMatch.Desc();
        searchSpec.Filter.SearchTerm = term;
        searchSpec.Filter.IgnorePrivates = true;
        searchSpec.PageSize = pageSize;

        result.QuestionsResult = Sl.SolrSearchQuestions.Run(term, searchSpec);

        if (type != null)
            result.Ensure_max_elements_per_type_count_of_9("Categories");
        else
            result.Ensure_max_element_count_of_12();

        return Task.FromResult(result);
    }

    public SolrGlobalSearchResult GoAllCategories(string term, int[] categoriesToFilter = null)
    {
        var pager = new Pager {QueryAll = true};

        var result = new SolrGlobalSearchResult
        {
            CategoriesResult = Sl.SearchCategories.Run(term, pager, categoriesToFilter: categoriesToFilter)
        };
        return result;
    }
}