using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Tests;

namespace TrueOrFalse.View.Web.Views.Api
{
    public class CategoryApiController : Controller
    {
        private readonly CategorySearch _categorySearch;

        public CategoryApiController(CategorySearch categorySearch)
        {
            _categorySearch = categorySearch;
        }

        public JsonResult ByName(string term)
        {
            return Json(from c in _categorySearch.Run(term) 
                        select new {
                            name = c.Name,
                            numberOfQuestions = c.QuestionCount,
                            imageUrl = string.Format(new GetCategoryImageUrl().Run(c).Url, 50)
                        }, JsonRequestBehavior.AllowGet); 
        }

    }
}
