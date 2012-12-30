using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class CategoriesController : BaseController
{
    private readonly CategoryRepository _categoryRepository;

    public CategoriesController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public ActionResult Categories()
    {
        var categories = Resolve<ISession>().QueryOver<Category>().Fetch(c => c.Questions).Eager().List<Category>();
        return View(new CategoriesModel(categories));
    }
}
