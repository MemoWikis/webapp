using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class AuthorController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AuthorController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    [HttpPost]
    public JsonResult GetAuthor(int id)
    {
        var author = EntityCache.GetUserById(id);
        if (author != null)
            return Json(new
            {
                ImgUrl = new UserImageSettings(author.Id, _httpContextAccessor, _webHostEnvironment)
                    .GetUrl_20px(author)
                    .Url,
                Reputation = author.Reputation,
                Name = author.Name,
                ReputationPos = author.ReputationPos
            });
        return Json(new { });
    }
}