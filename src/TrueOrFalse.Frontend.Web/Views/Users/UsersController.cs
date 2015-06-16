using System;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class UsersController : BaseController
{
    private const string _viewLocationUsers = "~/Views/Users/Users.aspx";
    private const string _viewLocationNetwork = "~/Views/Users/Network.aspx";

    private readonly UsersControllerSearch _usersControllerSearch;

    public UsersController(UsersControllerSearch usersControllerSearch){
        _usersControllerSearch = usersControllerSearch;
    }

    public ActionResult Search(string searchTerm, UsersModel model, string orderBy = null)
    {
        _sessionUiData.SearchSpecUser.SearchTerm = model.SearchTerm = searchTerm;
        return Users(null, model, orderBy);
    }

    [SetMenu(MenuEntry.Users)]
    public ActionResult Users(int? page, UsersModel model, string orderBy = null)
    {
        SetUsersOrderBy(orderBy);

        _sessionUiData.SearchSpecUser.PageSize = 10;
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
        model.Message = new SuccessMessage("Nun bist du angemeldet als <b>\"" + user.Name +  "\"</b>");
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
        userToFollow.Followers.Add(_sessionUser.User);

        userRepo.Update(userToFollow);
    }

    [HttpPost][AccessOnlyAsLoggedIn]
    public void UnFollow(int userId)
    {
        var userRepo = R<UserRepo>();
        var userToUnfollow = userRepo.GetById(userId);
        userToUnfollow.Followers.Remove(_sessionUser.User);

        userRepo.Update(userToUnfollow);
    }
}