﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueOrFalse.Search;

public class CategoryApiController : BaseController
{
    private readonly MeiliSearchCategories _searchCategories;
    private readonly CategoryRepository _categoryRepo;
    private readonly IGlobalSearch _globalSearch;

    public CategoryApiController(
        CategoryRepository categoryRepo,
        IGlobalSearch globalSearch)
    {
        _categoryRepo = categoryRepo;
        _globalSearch = globalSearch;
    }

    public async Task<JsonResult> ByName(string term, string type, int? parentId)
    {
        IList<Category> categories;

        string searchTerm = "%" + term + "%";

        if (type == "Book")
        {
            categories = _categoryRepo.Session
                .QueryOver<Category>()
                .Where(c => c.Type == CategoryType.Book)
                .WhereRestrictionOn(c => c.Name)
                .IsLike(searchTerm)
                .List();
        }
        else if (type == "Article")
        {
            categories = _categoryRepo.Session
                .QueryOver<Category>()
                .Where(c => c.Type == CategoryType.DailyArticle || c.Type == CategoryType.MagazineArticle)
                .WhereRestrictionOn(c => c.Name)
                .IsLike(searchTerm)
                .List();
        }
        else if (type == "Daily")
        {
            categories = _categoryRepo.Session
                .QueryOver<Category>()
                .Where(c => c.Type == CategoryType.Daily)
                .WhereRestrictionOn(c => c.Name)
                .IsLike(searchTerm)
                .List();
        }
        else if (type == "Magazine")
        {
            categories = _categoryRepo.Session
                .QueryOver<Category>()
                .Where(c => c.Type == CategoryType.Magazine)
                .WhereRestrictionOn(c => c.Name)
                .IsLike(searchTerm)
                .List();
        }
        else if (type == "DailyIssue"){
            categories = _categoryRepo.GetChildren(CategoryType.Daily, CategoryType.DailyIssue, parentId.Value, searchTerm);
        }
        else if (type == "MagazineIssue")
        {
            categories = _categoryRepo.GetChildren(CategoryType.Magazine, CategoryType.MagazineIssue, parentId.Value, searchTerm);
        }
        else if (type == "VolumeChapter")
        {
            categories = _categoryRepo.Session
                .QueryOver<Category>()
                .Where(c => c.Type == CategoryType.VolumeChapter)
                .WhereRestrictionOn(c => c.Name)
                .IsLike(searchTerm)
                .List();
        }
        else if (type == "WebsiteArticle")
        {
            categories = _categoryRepo.Session
                .QueryOver<Category>()
                .Where(c => c.Type == CategoryType.WebsiteArticle)
                .WhereRestrictionOn(c => c.Name)
                .IsLike(searchTerm)
                .List();
        }
        else
        {
            var categoryIds = (await _globalSearch.GoAllCategories(term,5)).Categories.Select(c => c.Id);
            categories = _categoryRepo.GetByIds(categoryIds.ToArray());
        }

        var result = categories.Select(c => 
                new CategoryJsonResult {
                    id = c.Id, 
                    name = c.Name, 
                    numberOfQuestions = c.CountQuestionsAggregated,
                    imageUrl = new CategoryImageSettings(c.Id).GetUrl_50px().Url, 
                    type = c.Type.ToString(),
                    typeGroup = c.Type.GetCategoryTypeGroup().ToString(),
                    html =  c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Media 
                            ? ViewRenderer.RenderPartialView("Reference",c, ControllerContext) 
                            : ""
                }
            ).ToList();

        if (TermExistsAsCategory(term, result))
            return Json(result, JsonRequestBehavior.AllowGet);

        result.Insert(0, result.Count == 0
            ? new CategoryJsonResult {name = "", type = "CreateCategoryLink", isOnlyResult = true}
            : new CategoryJsonResult {name = "", type = "CreateCategoryLink" });

        return Json(result, JsonRequestBehavior.AllowGet);
    }


    public static bool TermExistsAsCategory(string term, IEnumerable<CategoryJsonResult> result)
    {
        return (term != null && result.Any(c => String.Equals(c.name, term, StringComparison.CurrentCultureIgnoreCase)));
    }

    [HttpPost]
    public bool UnpinQuestionsInCategory(string categoryId)
    {
        if (SessionUser.User == null)
            return false;

        CategoryInKnowledge.UnpinQuestionsInCategory(Convert.ToInt32(categoryId), SessionUser.User);
        return true;
    }

#if DEBUG

    [HttpGet]
    public void ForDebugging_CreateDeepCloneError(string categoryId)
    {
        var questions = Sl.QuestionRepo.GetAllEager();
        var categories = Sl.CategoryRepo.GetAllEager();

        categories.First().DeepClone();
    }

#endif 
}

public class CategoryJsonResult
{
    public int id { get; set; }
    public string name { get; set; }
    public int numberOfQuestions { get; set; }
    public string imageUrl { get; set; }
    public string type { get; set; }
    public string typeGroup { get; set; }
    public string html { get; set; }

    public bool isOnlyResult = false;
}