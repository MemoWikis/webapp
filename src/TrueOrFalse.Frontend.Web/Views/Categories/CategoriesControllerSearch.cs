using System.Collections.Generic;
using TrueOrFalse.Search;

public class CategoriesControllerSearch
{
    public IList<Category> Run() => Run(Sl.R<SessionUiData>().SearchSpecCategory);

    public IList<Category> Run(CategorySearchSpec searchSpec)
    {
        var solrResult = Sl.R<SearchCategories>().Run(searchSpec);
        return Sl.R<CategoryRepository>().GetByIds(solrResult.CategoryIds.ToArray());
    }
}