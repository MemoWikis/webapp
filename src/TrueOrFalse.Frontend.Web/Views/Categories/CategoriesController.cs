using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Code;

public class CategoriesController : Controller
{
    private readonly CategoryRepository _categoryRepository;

    public CategoriesController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }


    public ActionResult Categories()
    {
        return View(new CategoriesModel(_categoryRepository.GetAll()));
    }

    public ActionResult Delete(int id)
    {
        _categoryRepository.Delete(id);
        return View(Links.Categories, new CategoriesModel(_categoryRepository.GetAll()));
    }
}
