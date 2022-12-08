using System.Web.Mvc;

namespace VueApp;

public class AuthorController : BaseController
{
    [HttpPost]
    public JsonResult GetAuthor(int id)
    {
        var author = UserCache.GetUser(id);
        if (author != null)
            return Json(new
            {
                ImgUrl = new UserImageSettings(author.Id).GetUrl_20px(author).Url,
                Reputation = author.Reputation,
                Name = author.Name,
                ReputationPos = author.ReputationPos
            });
        return Json(new { });
    }
}