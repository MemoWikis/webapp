using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class RegisterController : BaseController
{
    private string _viewRegisterPath = "~/Views/Welcome/Registration/Register.aspx";
    private string _viewCategoryDetailPath = "~/Views/Categories/Detail/Category.aspx"; 

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

        var category = PersonalTopic.GetPersonalCategory(user); 
        user.StartTopicId = category.Id;
        Sl.CategoryRepo.Create(category);
        _sessionUser.User.StartTopicId = category.Id;

        return Category(EntityCache.GetCategoryCacheItem(category.Id)) ;
    }

    public ActionResult Category(CategoryCacheItem category) => View(_viewCategoryDetailPath, new CategoryModel(category));

    [HttpPost]
    public string GetUserTopic()
    {
        var userCategory = EntityCache.GetCategoryCacheItem(Sl.SessionUser.User.StartTopicId);
        return Links.CategoryDetail(userCategory);
    }
}