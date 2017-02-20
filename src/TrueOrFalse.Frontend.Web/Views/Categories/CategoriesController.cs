﻿using System;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse.Web;

public class CategoriesController : BaseController
{
    private readonly CategoryRepository _categoryRepo;
    private readonly CategoriesControllerSearch _categorySearch;
    private const string _viewLocation = "~/Views/Categories/Categories.aspx";

    public CategoriesController(CategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
        _categorySearch = new CategoriesControllerSearch();
    }

    public ActionResult SearchApi(string searchTerm)
    {
        var searchSpec = _sessionUiData.SearchSpecCategory;
        searchSpec.SearchTerm = searchTerm;

        var categoriesModel = new CategoriesModel();
        categoriesModel.Init(_categorySearch.Run());

        return new JsonResult
        {
            Data = new
            {
                Html = ViewRenderer.RenderPartialView(
                    "CategoriesSearchResult",
                    new CategoriesSearchResultModel(categoriesModel),
                    ControllerContext),
                TotalInResult = searchSpec.TotalItems,
                TotalInSystem = GetCategoriesCount.All(),
                Tab = "All"
            }
        };
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

    [HttpPost]
    [AccessOnlyAsAdmin]
    public JsonResult DeleteDetails(int id)
    {
        var category = _categoryRepo.GetById(id);

        return new JsonResult
        {
            Data = new
            {
                categoryTitle = category.Name.WordWrap(50),
            }
        };
    }

    [AccessOnlyAsAdmin]
    public EmptyResult Delete(int id)
    {
        var category = _categoryRepo.GetById(id);

        if (category == null)
            throw new Exception("Category couldn't be deleted. Category with specified Id cannot be found.");

        Resolve<CategoryDeleter>().Run(category);

        return new EmptyResult();
    }

    public void SetCategoriesOrderBy(string orderByCommand)
    {
        var searchSpec = _sessionUiData.SearchSpecCategory;

        if (searchSpec.OrderBy.Current == null && String.IsNullOrEmpty(orderByCommand))
            orderByCommand = "byBestMatch";

        if (orderByCommand == "byBestMatch")
            searchSpec.OrderBy.BestMatch.Asc();
        else if (orderByCommand == "byQuestions") 
            searchSpec.OrderBy.QuestionCount.Desc();
        else if (orderByCommand == "byDate") 
            searchSpec.OrderBy.CreationDate.Desc();
    }

}
