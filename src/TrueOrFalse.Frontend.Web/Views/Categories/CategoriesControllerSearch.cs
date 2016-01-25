using System.Collections.Generic;
using TrueOrFalse.Search;

public class CategoriesControllerSearch
{
    public IList<Category> Run(){
                    
        var solrResult = Sl.R<SearchCategories>().Run(Sl.R<SessionUiData>().SearchSpecCategory);
        return Sl.R<CategoryRepository>().GetByIds(solrResult.CategoryIds.ToArray());
    }
}