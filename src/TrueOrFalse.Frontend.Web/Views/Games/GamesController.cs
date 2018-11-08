using System.Web.Mvc;

public class GamesController : BaseController
{
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
        R<GameStatusChange>().Cancel(gameId);
        return new EmptyResult();
    }

    [HttpPost]
    public EmptyResult StartGame(int gameId)
    {
        R<GameStatusChange>().Start(gameId);
        return new EmptyResult();
    }
}