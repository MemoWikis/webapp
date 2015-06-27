using System.Web.Mvc;

public class GamesController : BaseController
{
    [SetMenu(MenuEntry.Play)]
    public ActionResult Games()
    {
        return View(new GamesModel(R<GameRepo>().GetAllActive()));
    }

    public string RenderGameRow(int gameId)
    {
        return ViewRenderer.RenderPartialView(
            "~/Views/Games/GameRow.ascx",
            new GameRowModel(R<GameRepo>().GetById(gameId)), 
            ControllerContext
        );
    }

    [HttpPost]
    public EmptyResult CancelGame(int gameId)
    {
        R<CancelGame>().Run(gameId);
        return new EmptyResult();
    }

    [HttpPost]
    public EmptyResult StartGame(int gameId)
    {
        return new EmptyResult();
    }
}