using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class CategoriesController : BaseController
{
    private readonly CategoryRepository _categoryRepo;

    public CategoriesController(CategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    [SetMenu(MenuEntry.Categories)]
    public ActionResult Categories(int? page)
    {
        return View(new CategoriesModel(GetCurrentPage(page), _sessionUiData));
    }

    private IList<Category> GetCurrentPage(int? page)
    {
        _sessionUiData.SearchSpecCategory.PageSize = 10;
        if (page.HasValue) _sessionUiData.SearchSpecCategory.CurrentPage = page.Value;

        var categories = _categoryRepo.GetBy(_sessionUiData.SearchSpecCategory);
        return categories;
    }

    public ActionResult Delete(int id)
    {
        var category = _categoryRepo.GetById(id);

        Resolve<CategoryDeleter>().Run(category);

        var categoriesModel = new CategoriesModel(GetCurrentPage(_sessionUiData.SearchSpecCategory.CurrentPage), _sessionUiData);
        categoriesModel.Message = new SuccessMessage("Die Kategorie '" + category.Name + "' wurde gelöscht");
        
        return View(Links.Categories, categoriesModel);
    }
}
