using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Tests;

namespace TrueOrFalse.View.Web.Views.Api
{
    public class UserApiController : Controller
    {
        private readonly UserSearch _userSearch;

        public UserApiController(UserSearch userSearch)
        {
            _userSearch = userSearch;
        }

        public JsonResult ByName(string term)
        {
            return Json(from c in _userSearch.Run(term)
                        select new
                                   {
                                       name = c.Name,
                                       numberOfQuestions = "14"
                                   }, JsonRequestBehavior.AllowGet);
        }

    }
}