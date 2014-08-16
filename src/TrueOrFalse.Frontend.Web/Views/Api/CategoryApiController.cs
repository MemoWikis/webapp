using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Transform;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Search;

namespace TrueOrFalse.View.Web.Views.Api
{
    public class CategoryApiController : Controller
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
                var categoryIds = _searchCategories.Run(term, searchStartingWith: true, pageSize: 5).CategoryIds;
                categories = _categoryRepo.GetByIds(categoryIds.ToArray());
            }
            

            

            return Json(from c in categories
                        select new {
                            id = c.Id,
                            name = c.Name,
                            numberOfQuestions = c.CountQuestions,
                            imageUrl = new CategoryImageSettings(c.Id).GetUrl_50px().Url, 
                            type = c.Type.ToString(),
                            html = c.Type == CategoryType.Book ||
                                    c.Type == CategoryType.Daily ||
                                    c.Type == CategoryType.DailyIssue ||
                                    c.Type == CategoryType.DailyArticle ||
                                    c.Type == CategoryType.Magazine ||
                                    c.Type == CategoryType.MagazineIssue ||
                                    c.Type == CategoryType.MagazineArticle ||
                                    c.Type == CategoryType.VolumeChapter ||
                                    c.Type == CategoryType.WebsiteArticle ?
                                    ViewRenderer.RenderPartialView("Reference",c, ControllerContext) :
                                    ""
                        }, JsonRequestBehavior.AllowGet);
        }
    }
}
