using System;
using System.Web.Mvc;
public class PlayController : BaseController
{
    private const string _viewLocation = "~/Views/Games/Play/Play.aspx";
    private const string _viewLocationBodyControls = "~/Views/Games/Play/BodyControls/";

    public ActionResult Play(int gameId) => 
        View(_viewLocation, new PlayModel(R<GameRepo>().GetById(gameId)));

    public string RenderGameInProgressPlayer(int gameId) => 
        RenderPartialView("GameInProgressPlayer.ascx", new GameInProgressPlayerModel(Game(gameId)));

    public string RenderGameInProgressWatch(int gameId) => 
        RenderPartialView("GameInProgressWatch.ascx", new GameInProgressWatchModel(Game(gameId)));

    public string RenderGameNeverStarted(int gameId) => 
        RenderPartialView("GameNeverStarted.ascx", new GameNeverStartedModel(Game(gameId)));

    public string RenderGameCompleted(int gameId) => 
        RenderPartialView("GameCompleted.ascx", new GameCompletedModel(Game(gameId)));

    public string RenderAnswerBody(int questionId, int gameId, int playerId, int roundId){
        return ViewRenderer.RenderPartialView(
            "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx", 
            new AnswerBodyModel(
                R<QuestionRepo>().GetById(questionId),
                R<GameRepo>().GetById(gameId),
                R<PlayerRepo>().GetById(playerId),
                R<RoundRepo>().GetById(roundId)),
            ControllerContext
        );
    }

    [HttpPost]
    public JsonResult SendAnswerGame(
        int questionId,
        Guid questionViewGuid,
        int InteractionNumber,
        int gameId, 
        int userId,
        int roundId,
        string answer,
        int millisecondsSinceQuestionView = -1
        )
    {
        var result = R<AnswerQuestion>().Run(questionId, answer, UserId, questionViewGuid, InteractionNumber, millisecondsSinceQuestionView, userId, roundId);
        var solution = GetQuestionSolution.Run(questionId);

        R<GameHubConnection>().SendAnswered(gameId, userId, result);

        return new JsonResult
        {
            Data = new
            {
                correct = result.IsCorrect,
                correctAnswer = result.CorrectAnswer,
                choices = solution.GetType() == typeof(QuestionSolutionMultipleChoice_SingleSolution) ? 
                    ((QuestionSolutionMultipleChoice_SingleSolution)solution).Choices : 
                    null
            }
        };
    }

    private string RenderPartialView(string name, object model) => 
        ViewRenderer.RenderPartialView(
            _viewLocationBodyControls + name, model, ControllerContext
        );

    private Game Game(int gameId) => R<GameRepo>().GetById(gameId);
}