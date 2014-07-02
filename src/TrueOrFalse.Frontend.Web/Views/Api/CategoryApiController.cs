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

        public JsonResult ByName(string term, string type, string parentName)
        {
            IList<Category> categories;

            string searchTerm = "%" + term + "%";

            if (type == "Daily")
            {
                categories = _categoryRepo.Session
                    .QueryOver<Category>()
                    .Where(c => c.Type == CategoryType.Daily)
                    .WhereRestrictionOn(c => c.Name)
                    .IsLike(searchTerm)
                    .List();
            }
            else if (type == "DailyIssue"){
                categories = _categoryRepo.GetChildren(CategoryType.Daily, CategoryType.DailyIssue, parentName, searchTerm);
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
                        }, JsonRequestBehavior.AllowGet);
        }

        public string GetUrl(string categoryName)
        {
            var category = _categoryRepo.GetByName(categoryName);
            return Links.CategoryDetail(category);
        }
    }
}
