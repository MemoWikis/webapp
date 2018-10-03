using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Search;

public class CategoryApiController : BaseController
{
    private readonly SearchCategories _searchCategories;
    private readonly CategoryRepository _categoryRepo;

    public CategoryApiController(
        SearchCategories searchCategories,
        CategoryRepository categoryRepo)
    {
        _searchCategories = searchCategories;
        _categoryRepo = categoryRepo;
    }

    public JsonResult ByName(string term, string type, int? parentId)
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
            var categoryIds = _searchCategories.Run(term, searchOnlyWithStartingWith: true, pageSize: 5).CategoryIds;
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
    public void Pin(string categoryId)
    {
        if (_sessionUser.User == null)
            return;

        CategoryInKnowledge.Pin(Convert.ToInt32(categoryId), _sessionUser.User);
    }

    [HttpPost]
    public void Unpin(string categoryId)
    {
        if (_sessionUser.User == null)
            return;
        
        CategoryInKnowledge.Unpin(Convert.ToInt32(categoryId), _sessionUser.User);
    }

    public void UnpinQuestionsInCategory(string categoryId)
    {
        if (_sessionUser.User == null)
            return;

        CategoryInKnowledge.UnpinQuestionsInCategory(Convert.ToInt32(categoryId), _sessionUser.User);
    }
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