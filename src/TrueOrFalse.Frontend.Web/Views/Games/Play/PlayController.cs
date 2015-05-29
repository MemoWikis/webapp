using System.Web.Mvc;
[SetMenu(MenuEntry.Play)]
public class PlayController : BaseController
{
    private const string _viewLocation = "~/Views/Games/Play/Play.aspx";
    private const string _viewLocationBodyControls = "~/Views/Games/Play/BodyControls/";

    public ActionResult Play(int gameId)
    {
        return View(_viewLocation, new PlayModel(R<GameRepo>().GetById(gameId)));
    }

    public string RenderGameInProgressPlayer(int gameId){
        return RenderPartialView("GameInProgressPlayer.ascx",
            new GameInProgressPlayerModel(Game(gameId)));
    }

    public string RenderGameInProgressWatch(int gameId){
        return RenderPartialView("GameInProgressWatch.ascx",
            new GameInProgressWatchModel(Game(gameId)));
    }

    public string RenderGameNeverStarted(int gameId){
        return RenderPartialView("GameNeverStarted.ascx",
            new GameNeverStartedModel(Game(gameId)));
    }

    public string RenderGameCompleted(int gameId){
        return RenderPartialView("GameCompleted.ascx",
            new GameCompletedModel(Game(gameId)));
    }

    public string RenderAnswerBody(int questionId, int gameId, int playerId, int roundId){
        return ViewRenderer.RenderPartialView(
            "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx", 
            new AnswerBodyModel(
                R<QuestionRepository>().GetById(questionId),
                R<GameRepo>().GetById(gameId),
                R<PlayerRepo>().GetById(playerId),
                R<RoundRepo>().GetById(roundId)),
            ControllerContext
        );
    }

    [HttpPost]
    public JsonResult SendAnswerGame(
        int questionId,
        int gameId, 
        int playerId,
        int roundId,
        string answer)
    {
        var result = R<AnswerQuestion>().Run(questionId, answer, UserId, playerId, roundId);
        var solution = R<GetQuestionSolution>().Run(questionId);

        return new JsonResult
        {
            Data = new
            {
                correct = result.IsCorrect,
                correctAnswer = result.CorrectAnswer,
                choices = solution.GetType() == typeof(QuestionSolutionMultipleChoice) ? 
                    ((QuestionSolutionMultipleChoice)solution).Choices : 
                    null
            }
        };
    }

    private string RenderPartialView(string name, object model) {
        return ViewRenderer.RenderPartialView(
            _viewLocationBodyControls + name, model, ControllerContext
        );
    }

    private Game Game(int gameId){
        return R<GameRepo>().GetById(gameId);
    }
}