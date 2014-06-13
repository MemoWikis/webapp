using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        public JsonResult ByName(string term, string type)
        {
            IList<Category> categories;
            
            if (type == "Daily")
            {
                categories = _categoryRepo.Session
                    .QueryOver<Category>()
                    .Where(c => c.Type == CategoryType.Daily)
                    .WhereRestrictionOn(c => c.Name).IsLike(term + "%")
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
                        }, JsonRequestBehavior.AllowGet); 
        }
    }
}
