using System;
using System.Collections.Generic;
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
        var setsString = gameModel.Sets;
        var sets = GetSetsFromString(setsString);
        if (sets.Count == 0)
        {
            gameModel.Message = new ErrorMessage("Bitte gib mind. einen Fragesatz ein.");
            return View(_viewLocation, gameModel);            
        }

        var game = new Game();
        game.Id = game.Id;
        game.Sets = sets;
        game.MaxPlayers = gameModel.MaxPlayers;
        game.WillStartAt = DateTime.Now.AddMinutes(Convert.ToInt32(gameModel.StartsInMinutes));
        game.Comment = gameModel.Comment;
        game.Creator = _sessionUser.User;
        game.Status = GameStatus.Ready;
        game.Rounds = gameModel.Rounds;
 
        R<GameRepo>().Create(game);

        return Redirect("/Spielen");
    }

    private List<Set> GetSetsFromString(string setsString)
    {
        if (String.IsNullOrEmpty(setsString))
            return new List<Set>();

        setsString = setsString.Trim();
        var setIds = setsString.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
        var sets = new List<Set>();
        var setRepo = R<SetRepository>();
        foreach (var setId in setIds)
        {
            var set = setRepo.GetById(Convert.ToInt32(setId));
            if (set != null)
                sets.Add(set);
        }
        return sets;
    }
}
