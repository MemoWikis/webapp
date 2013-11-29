using System.Collections.Generic;
using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web.Context;

public class CategoriesControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly CategoryRepository _categoryRepo;
    private readonly SessionUiData _sessionUiData;
    private readonly SearchCategories _searchCategories;

    public CategoriesControllerSearch(
        CategoryRepository categoryRepo, 
        SessionUiData sessionUiData,
        SearchCategories searchCategories)
    {
        _categoryRepo = categoryRepo;
        _sessionUiData = sessionUiData;
        _searchCategories = searchCategories;
    }

    public IList<Category> Run(){
                    
        var solrResult = _searchCategories.Run(_sessionUiData.SearchSpecCategory);
        return _categoryRepo.GetByIds(solrResult.CategoryIds.ToArray());
    }
}