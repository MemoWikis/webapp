using System;
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

    public ActionResult Search(string searchTerm, CategoriesModel model, string orderBy = null)
    {
        _sessionUiData.SearchSpecCategory.SearchTerm = model.SearchTerm = searchTerm;
        return Categories(null, model, orderBy);
    }


    [SetMenu(MenuEntry.Categories)]
    public ActionResult Categories(int? page, CategoriesModel model, string orderBy = null)
    {
        SetCategoriesOrderBy(orderBy);

        _sessionUiData.SearchSpecCategory.PageSize = 30;
        if (page.HasValue) 
            _sessionUiData.SearchSpecCategory.CurrentPage = page.Value;

        model.Init(_categorySearch.Run());

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

    public void SetCategoriesOrderBy(string orderByCommand)
    {
        var searchSpec = _sessionUiData.SearchSpecCategory;

        if (searchSpec.OrderBy.Current == null && String.IsNullOrEmpty(orderByCommand))
            orderByCommand = "byQuestions";

        if (orderByCommand == "byQuestions") searchSpec.OrderBy.QuestionCount.Desc();
        else if (orderByCommand == "byDate") searchSpec.OrderBy.CreationDate.Desc();
    }

}
