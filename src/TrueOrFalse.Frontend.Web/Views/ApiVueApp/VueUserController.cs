using System.Web.Mvc;

namespace VueApp;

public class VueUserController : BaseController
{
    [HttpPost]
    public JsonResult GetUser(int id)
    {
        var user = EntityCache.GetUserById(id);
        if (user != null)
            return Json(new
            {
                ImgUrl = new UserImageSettings(user.Id).GetUrl_20px(user).Url,
                Reputation = user.Reputation,
                Name = user.Name,
                ReputationPos = user.ReputationPos
            });
        return Json(new { });
    }
}