using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;

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
}
