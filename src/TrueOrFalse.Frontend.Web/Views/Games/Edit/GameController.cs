using System.Web.Mvc;
using TrueOrFalse.Web;

public class GameController : BaseController
{
    private const string _viewLocation = "~/Views/Games/Edit/Game.aspx";

    public ActionResult Create()
    {
        return View(_viewLocation, new GameModel());
    }

    public ActionResult Create(GameModel gameModel)
    {
        var game = gameModel.ToGame();
 
        R<GameRepo>().Create(game);

        return Redirect("/Spielen");
    }

    public ActionResult Edit()
    {
        return View(_viewLocation, new GameModel{IsEditing = true});
    }
}
