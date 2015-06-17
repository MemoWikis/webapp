using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Web;

[SetMenu(MenuEntry.Play)]
public class GameController : BaseController
{
    private const string _viewLocation = "~/Views/Games/Edit/Game.aspx";

    [HttpGet]
    public ViewResult Create()
    {
        return View(_viewLocation, new GameModel
        {
            MaxPlayers = 5,
            StartsInMinutes = 5,
        });
    }

    [HttpPost]
    public ActionResult Create(GameModel gameModel)
    {
        var sets = AutocompleteUtils.GetSetsFromPostData(Request.Form);
        if (sets.Count == 0)
        {
            gameModel.Message = new ErrorMessage("Bitte gib mind. einen Fragesatz ein.");
            return View(_viewLocation, gameModel);            
        }

        if (!sets.SelectMany(x => x.QuestionsInSet).Any())
        {
            gameModel.Message = new ErrorMessage("Die gewählten Fragesätze beinhalten keine Fragen.");
            return View(_viewLocation, gameModel);                        
        }

        var game = new Game();
        game.Id = game.Id;
        game.Sets = sets;
        game.MaxPlayers = gameModel.MaxPlayers;
        game.WillStartAt = DateTime.Now.AddMinutes(Convert.ToInt32(gameModel.StartsInMinutes));
        game.Comment = gameModel.Comment;
        game.AddPlayer(_sessionUser.User, isCreator:true);
        game.Status = GameStatus.Ready;
        game.RoundCount = gameModel.Rounds;
 
        R<GameRepo>().Create(game);
        R<GameHubConnection>().SendCreated(game.Id);

        return Redirect("/Spielen");
    }
}
