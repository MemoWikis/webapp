using System;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class UsersController : BaseController
{
    private const string _viewLocation = "~/Views/Users/Users.aspx";

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

        return View(_viewLocation, model);
    }

    [AccessOnlyAsAdmin]
    public ActionResult LoginAs(int userId)
    {
        var user = Resolve<UserRepository>().GetById(userId);
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

}