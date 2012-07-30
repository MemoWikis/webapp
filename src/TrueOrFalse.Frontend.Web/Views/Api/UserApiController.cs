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
            return Json(from u in _userSearch.Run(term)
                        select new
                                   {
                                       id = u.Id,
                                       name = u.Name,
                                       numberOfQuestions = "?",
                                       imageUrl = string.Format(new GetUserImageUrl().Run(u).Url, 50)
                                   }, JsonRequestBehavior.AllowGet);
        }

    }
}