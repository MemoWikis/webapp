using System.Web;
using System.Web.Mvc;

namespace VueApp;

public class AuthorController :Controller
{
    [HttpPost]
    public JsonResult GetAuthor(int id)
    {
        var author = EntityCache.GetUserById(id);
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