using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;

public class KnowledgeController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly GetAnswerStatsInPeriod _getAnswerStatsInPeriod;
    private readonly GetWishKnowledgeCount _getWishKnowledgeCount;

    public KnowledgeController(
        SessionUser sessionUser, 
        GetAnswerStatsInPeriod getAnswerStatsInPeriod,
        GetWishKnowledgeCount getWishKnowledgeCount)
    {
        _sessionUser = sessionUser;
        _getAnswerStatsInPeriod = getAnswerStatsInPeriod;
        _getWishKnowledgeCount = getWishKnowledgeCount;
    }

    public ActionResult Knowledge()
    {
        var answersThisWeek = _getAnswerStatsInPeriod.RunForThisWeek(_sessionUser.User.Id);
        var answersThisMonth = _getAnswerStatsInPeriod.RunForThisMonth(_sessionUser.User.Id);
        var answersPreviousWeek = _getAnswerStatsInPeriod.RunForPreviousWeek(_sessionUser.User.Id);
        var answersPreviousMonth = _getAnswerStatsInPeriod.RunForPreviousMonth(_sessionUser.User.Id);

        return View(
            new KnowledgeModel(_sessionUser)
                {
                    WishKnowledgeCount = _getWishKnowledgeCount.Run(_sessionUser.User.Id),
                    TotalAnswerThisWeek = answersThisWeek.TotalAnswers,
                    TotalAnswerThisMonth = answersThisMonth.TotalAnswers,
                    TotalAnswerPreviousWeek = answersPreviousWeek.TotalAnswers,
                    TotalAnswerLastMonth = answersPreviousMonth.TotalAnswers
                }
        );
    }
}
