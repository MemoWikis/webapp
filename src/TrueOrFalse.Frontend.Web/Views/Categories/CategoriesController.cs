using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class CategoriesController : BaseController
{
    private readonly CategoryRepository _categoryRepository;

    public CategoriesController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public ActionResult Categories(){
        return View(new CategoriesModel(_categoryRepository.GetAll()));
    }

    public ActionResult Delete(int id)
    {
        var category = _categoryRepository.GetById(id);

        Resolve<CategoryDeleter>().Run(category);

        var categoriesModel = new CategoriesModel(_categoryRepository.GetAll());
        categoriesModel.Message = new SuccessMessage("Die Kategorie '" + category.Name + "' wurde gelöscht");
        
        return View(Links.Categories, categoriesModel);
    }
}
