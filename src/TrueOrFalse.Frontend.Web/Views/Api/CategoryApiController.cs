using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueOrFalse.View.Web.Views.Api
{
    public class CategoryApiController : Controller
    {
        public JsonResult ByName(string term)
        {
            var result = new List<string>
            {   
                        "abc",   
                        "cde",   
                        "fgh",   
                        "ijk",   
                        "lmn"
                    };

            result = result.Where(s => s.ToLower().StartsWith(term.ToLower())).ToList();

            return Json(result, JsonRequestBehavior.AllowGet); 
        }

    }
}
