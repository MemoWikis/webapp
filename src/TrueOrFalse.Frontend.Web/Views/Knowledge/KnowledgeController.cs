using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

public class KnowledgeController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly GetAnswerStatsInPeriod _getAnswerStatsInPeriod;
    private readonly GetWishKnowledgeCountCached _getWishKnowledgeCount;

    public KnowledgeController(
        SessionUser sessionUser, 
        GetAnswerStatsInPeriod getAnswerStatsInPeriod,
        GetWishKnowledgeCountCached getWishKnowledgeCount)
    {
        _sessionUser = sessionUser;
        _getAnswerStatsInPeriod = getAnswerStatsInPeriod;
        _getWishKnowledgeCount = getWishKnowledgeCount;
    }


    [SetMenu(MenuEntry.Knowledge)]
    public ActionResult Knowledge()
    {
        var answersThisWeek = _getAnswerStatsInPeriod.RunForThisWeek(_sessionUser.User.Id);
        var answersThisMonth = _getAnswerStatsInPeriod.RunForThisMonth(_sessionUser.User.Id);
        var answersPreviousWeek = _getAnswerStatsInPeriod.RunForPreviousWeek(_sessionUser.User.Id);
        var answersPreviousMonth = _getAnswerStatsInPeriod.RunForPreviousMonth(_sessionUser.User.Id);

        return View(
            new KnowledgeModel(_sessionUser)
                {
                    QuestionsCount = _getWishKnowledgeCount.Run(_sessionUser.User.Id),
                    TotalAnswerThisWeek = answersThisWeek.TotalAnswers,
                    TotalAnswerThisMonth = answersThisMonth.TotalAnswers,
                    TotalAnswerPreviousWeek = answersPreviousWeek.TotalAnswers,
                    TotalAnswerLastMonth = answersPreviousMonth.TotalAnswers
                }
        );
    }
}
