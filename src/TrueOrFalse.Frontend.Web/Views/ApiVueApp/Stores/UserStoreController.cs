using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class UserStoreController : BaseController
{
    [HttpPost]
    public JsonResult Login(LoginJson loginJson)
    {
        var credentialsAreValid = R<CredentialsAreValid>();

        if (credentialsAreValid.Yes(loginJson.EmailAddress, loginJson.Password))
        {

            if (loginJson.PersistentLogin)
            {
                WritePersistentLoginToCookie.Run(credentialsAreValid.User.Id);
            }

            SessionUser.Login(credentialsAreValid.User);

            TransferActivityPoints.FromSessionToUser();
            Sl.UserRepo.UpdateActivityPointsData();

            return Json(new
            {
                Success = true,
                Message = "",
                CurrentUser = VueSessionUser.GetCurrentUserData()
            });
        }

        return Json(new
        {
            Success = false,
            Message = "Du konntest nicht eingeloggt werden. Bitte überprüfe deine E-Mail-Adresse und das Passwort."
        });
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult LogOut()
    {
        RemovePersistentLoginFromCookie.Run();
        SessionUser.Logout();

        return Json(new
        {
            Success = !SessionUser.IsLoggedIn,
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult Follow(int userId)
    {
        var userRepo = R<UserRepo>();
        var userToFollow = userRepo.GetById(userId);

        userToFollow.AddFollower(User_());
        userRepo.Update(userToFollow);
        userRepo.UpdateUserFollowerCount(userId);
        userRepo.Update(SessionUser.User);

        //var userCacheToFollow = EntityCache.GetUserById(userId);
        //if (userCacheToFollow.FollowerIds.All(id => id != SessionUser.UserId))
        //{
        //    userCacheToFollow.FollowerIds.Add(SessionUser.UserId);
        //    EntityCache.AddOrUpdate(userCacheToFollow);
        //}

        //if (SessionUser.User.FollowingIds.All(id => id != userId))
        //{
        //    SessionUser.User.FollowingIds.Add(userId);
        //    EntityCache.AddOrUpdate(SessionUser.User);
        //}

        return Json(true);
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult UnFollow(int userId)
    {
        var userRepo = R<UserRepo>();
        var userToUnfollow = userRepo.GetById(userId);
        var followerInfoToRemove = userToUnfollow.Followers.First(x => x.Follower.Id == UserId);
        userRepo.RemoveFollowerInfo(followerInfoToRemove);
        R<UserActivityRepo>().DeleteForUser(UserId, userId);
        userRepo.UpdateUserFollowerCount(userId);
        userRepo.Update(userToUnfollow);
        userRepo.Update(SessionUser.User);

        //var userCacheToUnFollow = EntityCache.GetUserById(userId);
        //if (userCacheToUnFollow.FollowerIds.Any(id => id == SessionUser.UserId))
        //{
        //    userCacheToUnFollow.FollowerIds.Remove(SessionUser.UserId);
        //    EntityCache.AddOrUpdate(userCacheToUnFollow);
        //}

        //if (SessionUser.User.FollowingIds.Any(id => id == userId))
        //{
        //    SessionUser.User.FollowingIds.Remove(userId);
        //    EntityCache.AddOrUpdate(SessionUser.User);
        //}
        return Json(true);

    }
}
    
public class StateModel
{
    public bool IsLoggedIn { get; set; }
    public int? UserId { get; set; } = null;

}

public class LoginJson
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public bool PersistentLogin { get; set; }
}