using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class RegisterController : BaseController
{
    private string _viewRegisterPath = "~/Views/Welcome/Registration/Register.aspx";
    private string _viewRegisterSuccessPath = "~/Views/Welcome/Registration/RegisterSuccess.aspx";

    public ActionResult Register() { return View(_viewRegisterPath, new RegisterModel()); }

    [HttpPost]
    public ActionResult Register(RegisterModel model)
    {
        if (!IsEmailAddressAvailable.Yes(model.Email))
            ModelState.AddModelError("E-Mail", "Diese E-Mail-Adresse ist bereits registriert.");

        if (!global::IsUserNameAvailable.Yes(model.Name))
            ModelState.AddModelError("Name", "Dieser Benutzername ist bereits vergeben.");

        if (!ModelState.IsValid)
            return View(model);

        var user = RegisterModelToUser.Run(model);
        RegisterUser.Run(user);
        _sessionUser.Login(user);

        var category = new Category
        {
            Name = user.Name + "s Startseite",
            Content = "<h3>Herzlich willkommen, dies ist dein persönliches Startthema!</h3>" +
                      "<br />" +
                      "<p> Du kannst diesen Text leicht ändern, in dem du einfach hier anfängt zu tippen.Probier mal!</p>" +
                      " <p> Achtung: Dieses Thema ist(noch) öffentlich.Du kannst diese Seite im 3 - Punkte - Menü rechts auf privat stellen.</p> " +
                      "<p>Dann ist dieses Thema nur für dich zu erreichen.Wir helfen die gerne! Wenn du Fragen hast, melde dich. :-)</p> " +
                      "<br />" +
                      "<p><b>Liebe Grüße, dein memucho - Team.</b></p>",
            Visibility = 0,
            Creator = user,
            Type = CategoryType.Standard,
            IsUserStartTopic = true
        };
        user.StartTopicId = category.Id;
        Sl.CategoryRepo.Create(category);
        _sessionUser.User.StartTopicId = category.Id;

        return RedirectToAction(Links.RegisterSuccess, Links.RegisterController);
    }

    public ActionResult RegisterSuccess() => View(_viewRegisterSuccessPath, new RegisterSuccessModel());

    [HttpPost]
    public string GetUserTopic()
    {
        var userCategory = EntityCache.GetCategoryCacheItem(Sl.SessionUser.User.StartTopicId);
        return Links.CategoryDetail(userCategory);
    }
}