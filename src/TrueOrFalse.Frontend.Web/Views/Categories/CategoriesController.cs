using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Seedworks.Lib;
using static System.String;

public class CategoriesController : BaseController
{
    private readonly CategoryRepository _categoryRepo;
    private readonly CategoriesControllerSearch _categorySearch;
    private const string _viewLocation = "~/Views/Categories/Categories.aspx";
    private readonly CategoriesControllerUtil _util;

    public CategoriesController(CategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
        _categorySearch = new CategoriesControllerSearch();
        _util = new CategoriesControllerUtil(_categorySearch);
    }

    public ActionResult SearchApi(string searchTerm)
    {
        var searchSpec = _sessionUiData.SearchSpecCategory;
        searchSpec.SearchTerm = searchTerm;

        var categoriesModel = new CategoriesModel();
        categoriesModel.Init(_categorySearch.Run(), searchSpec, SearchTabType.All);

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
        _util.SetSearchSpecVars(_sessionUiData.SearchSpecCategory, page, orderBy);

        var categories = _categorySearch.Run(_sessionUiData.SearchSpecCategory);
        var categoriesModel = new CategoriesModel(categories, _sessionUiData.SearchSpecCategory, SearchTabType.All);
        return View(_viewLocation, categoriesModel);
    }

    [SetMenu(MenuEntry.Categories)]
    public ActionResult CategoriesWish(int? page, SetsModel model, string orderBy)
    {
        var searchSpec = _sessionUiData.SearchSpecCategoryWish;

        if (!_sessionUser.IsLoggedIn)
            return View(_viewLocation,
                new CategoriesModel(new List<Category>(), searchSpec, SearchTabType.Wish));

        _util.SetSearchSpecVars(searchSpec, page, orderBy);

        searchSpec.Filter.ValuatorId = _sessionUser.UserId;

        var categories = _categorySearch.Run(searchSpec);
        return View(_viewLocation, new CategoriesModel(categories, searchSpec, SearchTabType.Wish));
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

    public class CategoriesControllerUtil : BaseUtil
    {
        private readonly CategoriesControllerSearch _categoriesControllerSearch;

        public CategoriesControllerUtil(CategoriesControllerSearch categoriesControllerSearch)
        {
            _categoriesControllerSearch = categoriesControllerSearch;
        }

        //public SetsModel GetSetsModel(
        //    int? page,
        //    SetsModel model,
        //    string orderBy,
        //    SetSearchSpec searchSpec,
        //    SearchTabType searchTab)
        //{
        //    SetSearchSpecVars(searchSpec, page, orderBy);

        //    if (searchTab == SearchTabType.Mine)
        //        searchSpec.Filter.CreatorId = _sessionUser.UserId;
        //    else if (searchTab == SearchTabType.Wish)
        //        searchSpec.Filter.ValuatorId = _sessionUser.UserId;

        //    var questionsModel = new SetsModel(_setsControllerSearch.Run(searchSpec), searchSpec, searchTab);

        //    return questionsModel;
        //}

        //public JsonResult SearchApi(
        //    string searchTerm,
        //    SetSearchSpec searchSpec,
        //    SearchTabType searchTab,
        //    ControllerContext controllerContext)
        //{
        //    var model = new SetsModel();
        //    GetSearchFilter(searchSpec, model, searchTerm);

        //    var totalInSystem = 0;
        //    switch (searchTab)
        //    {
        //        case SearchTabType.All: totalInSystem = R<GetTotalSetCount>().Run(); break;
        //        case SearchTabType.Mine: totalInSystem = R<GetTotalSetCount>().Run(_sessionUser.UserId); break;
        //        case SearchTabType.Wish: totalInSystem = R<GetWishSetCount>().Run(_sessionUser.UserId); break;
        //    }

        //    return new JsonResult
        //    {
        //        Data = new
        //        {
        //            Html = ViewRenderer.RenderPartialView(
        //                "SetsSearchResult",
        //                new SetsSearchResultModel(
        //                    GetSetsModel(
        //                        searchSpec.CurrentPage,
        //                        model,
        //                        "",
        //                        searchSpec,
        //                        searchTab
        //                        )),
        //                controllerContext),
        //            TotalInResult = searchSpec.TotalItems,
        //            TotalInSystem = totalInSystem,
        //            Tab = searchTab.ToString()
        //        },
        //    };
        //}

        public void SetSearchSpecVars(CategorySearchSpec searchSpec, int? page, string orderByCommand)
        {
            searchSpec.PageSize = 30;

            if (page.HasValue)
                searchSpec.CurrentPage = page.Value;

            SetOrderBy(searchSpec, orderByCommand);
        }

        public void SetOrderBy(CategorySearchSpec searchSpec, string orderByCommand)
        {
            if (searchSpec.OrderBy.Current == null && IsNullOrEmpty(orderByCommand))
                orderByCommand = "byBestMatch";

            if (orderByCommand == "byBestMatch") searchSpec.OrderBy.BestMatch.Asc();
            else if (orderByCommand == "byQuestions") searchSpec.OrderBy.QuestionCount.Desc();
            else if (orderByCommand == "byDate") searchSpec.OrderBy.CreationDate.Desc();
        }

        //public void GetSearchFilter(
        //    SetSearchSpec searchSpec,
        //    SetsModel model,
        //    string searchTerm)
        //{
        //    if (searchSpec.SearchTerm != searchTerm)
        //        searchSpec.CurrentPage = 1;

        //    searchSpec.SearchTerm = model.SearchTerm = searchTerm;
        //}
    }

}
