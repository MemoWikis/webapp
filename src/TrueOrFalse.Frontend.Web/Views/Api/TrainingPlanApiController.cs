using System.Web.Mvc;

public class TrainingPlanApiController : BaseController
{
    [HttpPost]
    public JsonResult GetByDateId(int trainingPlanId)
    {
        var trainingPlan = R<TrainingPlanRepo>().GetById(trainingPlanId);

        return Json(new
        {
            RemainingTime = "",
            RemainingDates = 1,
            QuestionCount = 1,
        });
    }
}