using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class GameController : BaseController
{
    private const string _viewLocation = "~/Views/Games/Edit/Game.aspx";

    [HttpGet]
    public ViewResult Create()
    {
        var gameModel = new GameModel{
            MaxPlayers = 5,
            StartsInMinutes = 5,
        };

        if (Request["setId"] != null)
        {
            var set = Sl.R<SetRepo>().GetById(Convert.ToInt32(Request["setId"]));
            if(set != null)
                gameModel.Sets = new List<Set>{set};
        }

        if (Request["setIds"] != null)
        {
            var setIds = Request["setIds"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x)).ToList();

            gameModel.Sets = Sl.R<SetRepo>().GetByIds(setIds);
        }

        if (Request["dateId"] != null)
        {
            var date = Sl.R<DateRepo>().GetById(Convert.ToInt32(Request["dateId"]));
            if (date != null)
                gameModel.Sets = date.Sets;
        }

        if (Request["categoryId"] != null)
        {
            var category = Sl.R<CategoryRepository>().GetById(Convert.ToInt32(Request["categoryId"]));
            if (category != null)
                gameModel.Sets = category.GetAggregatedSetsFromMemoryCache();
        }

        return View(_viewLocation, gameModel);
    }

    [HttpPost]
    public ActionResult Create(GameModel gameModel)
    {
        var sets = AutocompleteUtils.GetSetsFromPostData(Request.Form);
        if (sets.Count == 0)
        {
            gameModel.Message = new ErrorMessage("Bitte gib mindestens ein Lernset ein.");
            return View(_viewLocation, gameModel);            
        }

        bool hasQuestions = true;
        if (gameModel.OnlyMultipleChoice && 
            sets.SelectMany(x => x.QuestionsInSet)
                .All(q => q.Question.SolutionType != SolutionType.MultipleChoice_SingleSolution))
        {
            hasQuestions = false;
        }else if (!sets.SelectMany(x => x.QuestionsInSet).Any()){
            hasQuestions = false;
        }

        if (!hasQuestions)
        {
            gameModel.Message = new ErrorMessage("Die gewählten Lernsets beinhalten keine Multiple-Choice-Fragen.");
            return View(_viewLocation, gameModel);
        }

        
        
        //MatchList Fragen nicht zulassen (temporär)
        bool containsMatchListQuestions = sets.SelectMany(x => x.QuestionsInSet).Any(q => q.Question.SolutionType == SolutionType.MatchList);
        if (containsMatchListQuestions)
        {
            gameModel.Message = new ErrorMessage("Bitte wählen sie keine Fragen vom Typ Zuordnung (Liste). Diese sind leider noch nicht in den Spielen-Modus integriert.");
            return View(_viewLocation, gameModel);
        }
        
        //FlashCard Fragen nicht zulassen
        bool containsFlashCardQuestions= sets.SelectMany(x => x.QuestionsInSet).Any(q => q.Question.SolutionType == SolutionType.FlashCard);
        if (containsFlashCardQuestions)
        {
            gameModel.Message = new ErrorMessage("Bitte wählen sie keine Fragen vom Typ Karteikarte. Diese sind für Spiele ungeeignet!");
            return View(_viewLocation, gameModel);
        }

        var game = new Game();
        game.Id = game.Id;
        game.Sets = sets;
        game.MaxPlayers = gameModel.MaxPlayers;
        game.WillStartAt = DateTime.Now.AddMinutes(Convert.ToInt32(gameModel.StartsInMinutes));
        game.Comment = gameModel.Comment;
        game.WithSystemAvgPlayer = gameModel.WithSystemAvgPlayer;
        game.AddPlayer(_sessionUser.User, isCreator:true);

        if (!_sessionUser.User.IsMemuchoUser && game.WithSystemAvgPlayer)
            game.AddPlayer(this.MemuchoUser());

        game.Status = GameStatus.Ready;
        game.RoundCount = gameModel.Rounds;

        R<GameCreate>().Run(game, gameModel.OnlyMultipleChoice);
        R<GameHubConnection>().SendCreated(game.Id);

        return Redirect(Links.Games());
    }
}
