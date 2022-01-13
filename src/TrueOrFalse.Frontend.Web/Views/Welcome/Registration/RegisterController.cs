using System.Web.Mvc;
using Quartz;
using Quartz.Impl;
using TrueOrFalse.Frontend.Web.Code;

public class RegisterController : BaseController
{
    private string _viewRegisterPath = "~/Views/Welcome/Registration/Register.aspx";
    private string _viewCategoryDetailPath = "~/Views/Categories/Detail/Category.aspx"; 

    public ActionResult Register() { return View(_viewRegisterPath, new RegisterModel()); }

    [HttpPost]
    public ActionResult Register(RegisterModel model)
    {
        if (!IsEmailAddressAvailable.Yes(model.Login))
            ModelState.AddModelError("E-Mail", "Diese E-Mail-Adresse ist bereits registriert.");

        if (!global::IsUserNameAvailable.Yes(model.UserName))
            ModelState.AddModelError("Name", "Dieser Benutzername ist bereits vergeben.");

        if (!ModelState.IsValid)
            return View(_viewRegisterPath, model);

        var user = RegisterModelToUser.Run(model);
        RegisterUser.Run(user);
        ISchedulerFactory schedFact = new StdSchedulerFactory();
        var x = schedFact.AllSchedulers;

        _sessionUser.Login(user);

        var category = PersonalTopic.GetPersonalCategory(user);
        category.Visibility = CategoryVisibility.Owner;
        user.StartTopicId = category.Id;
        Sl.CategoryRepo.Create(category);
        _sessionUser.User.StartTopicId = category.Id;
        UserCache.AddOrUpdate(user);
        return Category(EntityCache.GetCategoryCacheItem(category.Id));
    }

    public ActionResult Category(CategoryCacheItem category) => Redirect(Links.CategoryDetail(category));

    [HttpPost]
    public string GetUserTopic()
    {
        var userCategory = EntityCache.GetCategoryCacheItem(_sessionUser.User.StartTopicId);
        return Links.CategoryDetail(userCategory);
    }

    public ActionResult EmailConfirmation(string emailKey)
    {
        var validator = new ValidateEmailConfirmationKey(Sl.UserRepo);
        var mailConfirmed = validator.IsValid(emailKey);
        return Category(EntityCache.GetCategoryCacheItem(1));
    }
}