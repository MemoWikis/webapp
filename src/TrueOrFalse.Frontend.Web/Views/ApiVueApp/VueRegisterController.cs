using System.Web.Mvc;
using Quartz;
using Quartz.Impl;
using TrueOrFalse.Frontend.Web.Code;

public class VueRegisterController : BaseController
{
    public class RegisterJson
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    private static User SetUser(RegisterJson json)
    {
        var user = new User();
        user.EmailAddress = json.Email.TrimAndReplaceWhitespacesWithSingleSpace();
        user.Name = json.Name.TrimAndReplaceWhitespacesWithSingleSpace();
        SetUserPassword.Run(json.Password.Trim(), user);
        return user;
    }

    [HttpPost]
    public JsonResult Register(RegisterJson json)
    {
        if (!IsEmailAddressAvailable.Yes(json.Email))
            return Json(new
            {
                Data = new
                {
                    Success = false,
                    Message = "emailInUse",
                }
            });

        if (!global::IsUserNameAvailable.Yes(json.Name))
            return Json( new
                {
                    Data = new
                    {
                        Success = false,
                        Message = "userNameInUse",
                    }
                });

        var user = SetUser(json);

        RegisterUser.Run(user);
        ISchedulerFactory schedFact = new StdSchedulerFactory();
        var x = schedFact.AllSchedulers;

        SessionUser.Login(user);

        var category = PersonalTopic.GetPersonalCategory(user);
        category.Visibility = CategoryVisibility.Owner;
        user.StartTopicId = category.Id;
        Sl.CategoryRepo.Create(category);
        SessionUser.User.StartTopicId = category.Id;
        SessionUserCache.AddOrUpdate(user);

            return Json(new
        {
            Success = true,
            Message = "",
            Id = SessionUser.UserId,
            WikiId = SessionUser.User.StartTopicId,
            IsAdmin = SessionUser.IsInstallationAdmin,
            Name = SessionUser.User.Name,
            PersonalWikiId = SessionUser.User.StartTopicId
        });
    }
}