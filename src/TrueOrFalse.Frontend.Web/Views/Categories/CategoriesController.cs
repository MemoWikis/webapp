using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class CategoriesController : BaseController
{
    private readonly CategoryRepository _categoryRepo;
    private readonly CategoriesControllerSearch _categorySearch;
    private const string _viewLocation = "~/Views/Categories/Categories.aspx";

    public CategoriesController(
        CategoryRepository categoryRepo,
        CategoriesControllerSearch categorySearch)
    {
        _categoryRepo = categoryRepo;
        _categorySearch = categorySearch;
    }

    public ActionResult Search(string searchTerm, CategoriesModel model)
    {
        _sessionUiData.SearchSpecCategory.SearchTearm = model.SearchTerm = searchTerm;
        return Categories(null, model);
    }


    [SetMenu(MenuEntry.Categories)]
    public ActionResult Categories(int? page, CategoriesModel model)
    {
        _sessionUiData.SearchSpecCategory.PageSize = 30;
        if (page.HasValue) 
            _sessionUiData.SearchSpecCategory.CurrentPage = page.Value;

        model.Init(_categorySearch.Run(), _sessionUiData);

        return View(_viewLocation, model);
    }

    public ActionResult Delete(int id)
    {
        var category = _categoryRepo.GetById(id);

        if (category == null)
            return Categories(null, new CategoriesModel {Message = new ErrorMessage("Die Kategorie existiert nicht mehr.")});

        Resolve<CategoryDeleter>().Run(category);

        var model = new CategoriesModel{
            Message = new SuccessMessage("Die Kategorie '" + category.Name + "' wurde gelöscht")
        };
        
        return Categories(null, model);
    }    
}
