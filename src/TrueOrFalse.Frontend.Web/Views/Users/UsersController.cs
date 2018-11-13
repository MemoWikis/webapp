using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class UsersController : BaseController
{
    private const string _viewLocationUsers = "~/Views/Users/Users.aspx";
    private const string _viewLocationNetwork = "~/Views/Users/Network.aspx";

    private readonly UsersControllerSearch _usersControllerSearch = new UsersControllerSearch();

    public ActionResult Search(string searchTerm, UsersModel model, string orderBy = null)
    {
        _sessionUiData.SearchSpecUser.SearchTerm = model.SearchTerm = searchTerm;
        return Users(null, model, orderBy);
    }

    public JsonResult SearchApi(string searchTerm, UsersModel model, int? page, string orderBy)
    {
        var searchSpec = _sessionUiData.SearchSpecUser;

        if (searchSpec.SearchTerm != searchTerm)
            searchSpec.CurrentPage = 1;

        searchSpec.SearchTerm = model.SearchTerm = searchTerm;

        var usersModel = new UsersModel();
        usersModel.Init(_usersControllerSearch.Run());

        return new JsonResult
        {
            Data = new{
                Html = ViewRenderer.RenderPartialView(
                    "UserSearchResult", 
                    new UserSearchResultModel(usersModel), 
                    this.ControllerContext),
                TotalInResult = searchSpec.TotalItems,
                TotalInSystem = R<GetTotalUsers>(),
            }
        };
    }

    [SetMainMenu(MainMenuEntry.Users)]
    public ActionResult Users(int? page, UsersModel model, string orderBy = null)
    {
        SetUsersOrderBy(orderBy);

        _sessionUiData.SearchSpecUser.PageSize = 30;
        if (page.HasValue) _sessionUiData.SearchSpecUser.CurrentPage = page.Value;

        if(model == null)
            model = new UsersModel();

        var users = _usersControllerSearch.Run();
        model.Init(users);

        return View(_viewLocationUsers, model);
    }

    public ActionResult Network()
    {
        return View(_viewLocationNetwork, new NetworkModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult LoginAs(int userId)
    {
        var user = Resolve<UserRepo>().GetById(userId);
        _sessionUser.Login(user);
        _sessionUser.IsInstallationAdmin = true;

        var model = new UsersModel();
        model.Message = new SuccessMessage("Nun bist du eingeloggt als <b>\"" + user.Name +  "\"</b>");
        return Users(null, model);
    }

    public void SetUsersOrderBy(string orderByCommand)
    {
        var searchSpec = _sessionUiData.SearchSpecUser;

        if (searchSpec.OrderBy.Current == null && String.IsNullOrEmpty(orderByCommand))
            orderByCommand = "byReputation";

        if (orderByCommand == "byReputation") searchSpec.OrderBy.Reputation.Desc();
        else if (orderByCommand == "byWishCount") searchSpec.OrderBy.WishCount.Desc();
    }

    [HttpPost][AccessOnlyAsLoggedIn]
    public void Follow(int userId)
    {
        var userRepo = R<UserRepo>();
        var userToFollow = userRepo.GetById(userId);
        userToFollow.AddFollower(User_());

        userRepo.Update(userToFollow);
    }

    [HttpPost][AccessOnlyAsLoggedIn]
    public void UnFollow(int userId)
    {
        var userRepo = R<UserRepo>();
        var userToUnfollow = userRepo.GetById(userId);
        var followerInfoToRemove = userToUnfollow.Followers.First(x => x.Follower.Id == UserId);
        
        R<UserRepo>().RemoveFollowerInfo(followerInfoToRemove);
        R<UserActivityRepo>().DeleteForUser(UserId, userId);
    }
}