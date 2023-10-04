using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Domain.User;

namespace VueApp;

public class UserStoreController(VueSessionUser vueSessionUser,
        SessionUser sessionUser,
        RegisterUser registerUser,
        PersistentLoginRepo persistentLoginRepo,
        GetUnreadMessageCount getUnreadMessageCount,
        PasswordRecovery passwordRecovery,
        Login login,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        PermissionCheck permissionCheck,
        GridItemLogic gridItemLogic,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        CategoryViewRepo categoryViewRepo,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IActionContextAccessor actionContextAccessor,
        UserReadingRepo userReadingRepo,
        QuestionReadingRepo questionReadingRepo)
    : Controller
{
    [HttpPost]
    public JsonResult Login([FromBody] LoginJson loginJson)
    {
        var isLoginSuccessfull = login.UserLogin(loginJson);

        if (isLoginSuccessfull)
        {
            return Json(new RequestResult
            {
                success = true,
                data = vueSessionUser.GetCurrentUserData()
            });
        }
        return Json(new RequestResult
        {
            success = false,
            messageKey = FrontendMessageKeys.Error.User.LoginFailed
        });
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult LogOut()
    {
        RemovePersistentLoginFromCookie.Run(persistentLoginRepo, httpContextAccessor);
        sessionUser.Logout();

        if (!sessionUser.IsLoggedIn)
            return Json(new RequestResult
            {
                success = true,
            });

        return Json(new RequestResult
        {
            success = false,
            messageKey = FrontendMessageKeys.Error.Default
        });
    }

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public int GetUnreadMessagesCount()
    {
        return getUnreadMessageCount.Run(sessionUser.UserId);
    }

    [HttpPost]
    public JsonResult ResetPassword(string email)
    {
        var result = passwordRecovery.RunForNuxt(email);
        //Don't reveal if email exists 
        return Json(new RequestResult { success = result.Success || result.EmailDoesNotExist });
    }

    [HttpPost]
    public JsonResult Register([FromBody] RegisterJson json)
    {
       

        if (!IsEmailAddressAvailable.Yes(json.Email, userReadingRepo))
            return Json(new RequestResult
        {
            success = false,
            messageKey = FrontendMessageKeys.Error.User.EmailInUse
        });

        if (!IsUserNameAvailable.Yes(json.Email, userReadingRepo))
            return Json(new RequestResult
        {
            success = false,
            messageKey = FrontendMessageKeys.Error.User.UserNameInUse
        });

        registerUser.SetUser(json);

        return Json(new RequestResult
        {
            success = true,
            data = new
            {
                IsLoggedIn = sessionUser.IsLoggedIn,
                Id = sessionUser.UserId,
                Name = sessionUser.IsLoggedIn? sessionUser.User.Name : "",
                IsAdmin = sessionUser.IsInstallationAdmin,
                PersonalWikiId = sessionUser.IsLoggedIn ? sessionUser.User.StartTopicId : 1,
                Type = UserType.Normal,
                ImgUrl = sessionUser.IsLoggedIn
                    ? new UserImageSettings(sessionUser.UserId,
                            httpContextAccessor,
                            webHostEnvironment)
                        .GetUrl_20px(sessionUser.User)
                        .Url
                    : "",
                Reputation = sessionUser.IsLoggedIn ? sessionUser.User.Reputation : 0,
                ReputationPos = sessionUser.IsLoggedIn ? sessionUser.User.ReputationPos : 0,
                PersonalWiki = new TopicControllerLogic(sessionUser,
                        permissionCheck, 
                        gridItemLogic, 
                        knowledgeSummaryLoader, 
                        categoryViewRepo, 
                        imageMetaDataReadingRepo, 
                        httpContextAccessor,
                        webHostEnvironment, 
                        actionContextAccessor,
                        questionReadingRepo)
                    .GetTopicData(sessionUser.IsLoggedIn ? sessionUser.User.StartTopicId : 1)
            }

        });
    }
}



