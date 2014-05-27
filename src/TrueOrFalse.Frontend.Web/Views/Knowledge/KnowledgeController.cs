using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

public class KnowledgeController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly GetAnswerStatsInPeriod _getAnswerStatsInPeriod;
    private readonly GetWishQuestionCountCached _getWishQuestionCount;

    public KnowledgeController(
        SessionUser sessionUser, 
        GetAnswerStatsInPeriod getAnswerStatsInPeriod,
        GetWishQuestionCountCached getWishQuestionCount)
    {
        _sessionUser = sessionUser;
        _getAnswerStatsInPeriod = getAnswerStatsInPeriod;
        _getWishQuestionCount = getWishQuestionCount;
    }


    [SetMenu(MenuEntry.Knowledge)]
    public ActionResult Knowledge()
    {
        if (!_sessionUser.IsLoggedIn)
            return View(new KnowledgeModel(_sessionUser));

        var answersThisWeek = _getAnswerStatsInPeriod.RunForThisWeek(_sessionUser.User.Id);
        var answersThisMonth = _getAnswerStatsInPeriod.RunForThisMonth(_sessionUser.User.Id);
        var answersPreviousWeek = _getAnswerStatsInPeriod.RunForPreviousWeek(_sessionUser.User.Id);
        var answersPreviousMonth = _getAnswerStatsInPeriod.RunForPreviousMonth(_sessionUser.User.Id);

        return View(
            new KnowledgeModel(_sessionUser)
                {
                    QuestionsCount = _getWishQuestionCount.Run(_sessionUser.User.Id),
                    TotalAnswerThisWeek = answersThisWeek.TotalAnswers,
                    TotalAnswerThisMonth = answersThisMonth.TotalAnswers,
                    TotalAnswerPreviousWeek = answersPreviousWeek.TotalAnswers,
                    TotalAnswerLastMonth = answersPreviousMonth.TotalAnswers
                }
        );
    }
}
