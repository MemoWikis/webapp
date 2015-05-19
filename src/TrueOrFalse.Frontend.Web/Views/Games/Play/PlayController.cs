using System.Web.Mvc;
[SetMenu(MenuEntry.Play)]
public class PlayController : BaseController
{
    private const string _viewLocation = "~/Views/Games/Play/Play.aspx";

    public ActionResult Play(int gameId)
    {
        return View(_viewLocation, new PlayModel(R<GameRepo>().GetById(gameId)));
    }
}