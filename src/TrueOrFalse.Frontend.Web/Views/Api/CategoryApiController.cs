using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Transform;
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
            else if (type == "DailyIssue")
            {
                //categories = _categoryRepo.Session
                //    .QueryOver<Category>()
                //    .Where(c => c.Type == CategoryType.DailyIssue && c.ParentCategories.Any(p => p.Name == "..."))
                //    .WhereRestrictionOn(c => c.Name).IsLike("%" + term + "%")
                //    .List();                


                categories = _categoryRepo.Session.CreateQuery(String.Format(
                    @"FROM category as c /* DailyIssue */
                     INNER JOIN category as categoryParent
                     ON c.P = categoryParent.Id 
                     AND categoryParent.`Type` = '{0}'
                     WHERE c.`Type` = '{1}'
                     AND categoryParent.Id = {2}", (int)CategoryType.Daily, (int)CategoryType.DailyIssue, "105" ))
                            .SetResultTransformer(Transformers.AliasToBean(typeof(Category)))
                            .List<Category>();

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
