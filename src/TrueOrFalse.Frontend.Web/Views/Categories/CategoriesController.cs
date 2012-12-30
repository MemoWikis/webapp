using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

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

    public ActionResult Delete(int id){
        _categoryRepository.Delete(id);
        return View(Links.Categories, new CategoriesModel(_categoryRepository.GetAll()));
    }
}
