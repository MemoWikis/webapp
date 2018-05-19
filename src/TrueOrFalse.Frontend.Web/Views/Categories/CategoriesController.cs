using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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
        _util = new CategoriesControllerUtil(_categorySearch, this);
    }

    public ActionResult Search(string searchTerm, CategoriesModel model, string orderBy = null)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecCategory, model, searchTerm);
        return Categories(null, model, orderBy);
    }

    public ActionResult SearchApi(string searchTerm) =>
        _util.SearchApi(searchTerm, _sessionUiData.SearchSpecCategory, SearchTabType.All, ControllerContext);

    [SetMenu(MenuEntry.Categories)]
    [SetThemeMenu]
    public ActionResult Categories(int? page, CategoriesModel model, string orderBy = null)
    {
        _util.SetSearchSpecVars(_sessionUiData.SearchSpecCategory, page, orderBy);

        var categories = _categorySearch.Run(_sessionUiData.SearchSpecCategory);
        var categoriesModel = new CategoriesModel(categories, _sessionUiData.SearchSpecCategory, SearchTabType.All);
        return View(_viewLocation, categoriesModel);
    }

    [SetMenu(MenuEntry.Categories)]
    [SetThemeMenu]
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

    public ActionResult SearchWish(string searchTerm, CategoriesModel model, string orderBy = null)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecCategoryWish, model, searchTerm);
        return Categories(null, model, orderBy);
    }

    public ActionResult SearchApiWish(string searchTerm) =>
        _util.SearchApi(searchTerm, _sessionUiData.SearchSpecCategoryWish, SearchTabType.Wish, ControllerContext);

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
        private readonly CategoriesController _controller;
        private ControllerContext _controllerContext => _controller.ControllerContext;

        public CategoriesControllerUtil(CategoriesControllerSearch categoriesControllerSearch, CategoriesController controller)
        {
            _categoriesControllerSearch = categoriesControllerSearch;
            _controller = controller;
        }


        public JsonResult SearchApi(
            string searchTerm,
            CategorySearchSpec searchSpec,
            SearchTabType searchTab,
            ControllerContext controllerContext)
        {

            searchSpec.SearchTerm = searchTerm;


            var totalInSystem = 0;
            switch (searchTab)
            {
                case SearchTabType.All:
                    totalInSystem = GetCategoriesCount.All();
                    break;
                case SearchTabType.Wish:
                    totalInSystem = GetCategoriesCount.Wish(_sessionUser.UserId);
                    searchSpec.Filter.ValuatorId = _sessionUser.UserId;
                    break;
            }

            var categoriesModel = new CategoriesModel();
            SetSearchFilter(searchSpec, categoriesModel, searchTerm);
            categoriesModel.Init(_categoriesControllerSearch.Run(searchSpec), searchSpec, searchTab);

            return new JsonResult
            {
                Data = new
                {
                    Html = ViewRenderer.RenderPartialView(
                        "CategoriesSearchResult",
                        new CategoriesSearchResultModel(categoriesModel),
                        _controllerContext),
                    TotalInResult = searchSpec.TotalItems,
                    TotalInSystem = totalInSystem,
                    Tab = searchTab.ToString()
                }
            };
        }

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

        public void SetSearchFilter(
            CategorySearchSpec searchSpec,
            CategoriesModel model,
            string searchTerm)
        {
            if (searchSpec.SearchTerm != searchTerm)
                searchSpec.CurrentPage = 1;

            searchSpec.SearchTerm = model.SearchTerm = searchTerm;
        }
    }
    
    public void SetDeleteMarker(int Id)
    {
        
        var catId = Id;
        IList<CategoryChange> filteredCategoryObjects = new List<CategoryChange>();


    var categoryObjects = Sl.CategoryChangeRepo.GetAll();

        foreach (var categoryObject in categoryObjects)
        {
            if (categoryObject.Category.Id == catId)
             filteredCategoryObjects.Add(categoryObject);

        }

        var lastCategoryChangeObject = filteredCategoryObjects[0];

        for (int i = 1; i < filteredCategoryObjects.Count; i++)
        {
            if (lastCategoryChangeObject.Id < filteredCategoryObjects[i].Id)
                lastCategoryChangeObject = filteredCategoryObjects[i];
        }

        //change JsonObject
        dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(lastCategoryChangeObject.Data);
        jsonObj["Delete"] = "true";
        lastCategoryChangeObject.Data = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
        // DB update
        Sl.CategoryChangeRepo.Update(lastCategoryChangeObject);
        Console.WriteLine(lastCategoryChangeObject);        
    }
}
