using System.Web.Mvc;

namespace VueApp;

public class AuthorController : BaseController
{
    [HttpPost]
    public JsonResult GetAuthor(int id)
    {
        var author = Sl.UserRepo.GetById(id);
        if (author != null)
            return Json(new
            {
                ImageUrl = new UserImageSettings(author.Id).GetUrl_20px(author).Url,
                Reputation = author.Reputation,
                Name = author.Name,
                ReputationPos = author.ReputationPos
            });
        return Json(new { });
    }
}