using System.Web.Mvc;

public class QuestionHistoryDetailController : Controller
{
    [AccessOnlyAsQuestionOwner]
    public ActionResult Detail(int questionId, int revisionId)
    {
        var currentRevision = Sl.QuestionChangeRepo.GetByIdEager(revisionId);
        var previousRevision = Sl.QuestionChangeRepo.GetPreviousRevision(currentRevision);
        var nextRevision = Sl.QuestionChangeRepo.GetNextRevision(currentRevision);

        return View("~/Views/Questions/History/Detail/QuestionHistoryDetail.aspx",
            new QuestionHistoryDetailModel(currentRevision, previousRevision, nextRevision));
    }
}