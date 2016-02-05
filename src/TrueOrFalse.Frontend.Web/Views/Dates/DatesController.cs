using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class DatesController : BaseController
{
    [SetMenu(MenuEntry.Dates)]
    public ActionResult Dates()
    {
        return View(new DatesModel());
    }

    [HttpPost]
    public JsonResult DeleteDetails(int id)
    {
        var date = R<DateRepo>().GetById(id);

        return new JsonResult{
            Data = new{
                DateInfo = date.GetTitle(),
            }
        };
    }

    [HttpPost]
    public EmptyResult Delete(int id)
    {
        R<DeleteDate>().Run(id);
        return new EmptyResult();
    }

    public string RenderPreviousDates()
    {
        var previousDates = R<DateRepo>()
            .GetBy(UserId, onlyPrevious: true)
            .OrderByDescending(x => x.DateTime)
            .Select(d => new DateRowModel(d))
            .ToList();

        return ViewRenderer.RenderPartialView(
            "~/Views/Dates/PreviousDates.ascx",
            previousDates,
            ControllerContext
        );
    }

    public ActionResult StartLearningSession(int dateId)
    {
        var date = Resolve<DateRepo>().GetById(dateId);

        var learningSession = new LearningSession
        {
            DateToLearn = date,
            Steps = GetLearningSessionSteps.Run(date),
            User = _sessionUser.User
        };

        R<LearningSessionRepo>().Create(learningSession);

        return Redirect(Links.LearningSession(learningSession));
    }

    private string RenderTrainingDates(Date date)
    {
        var trainingDatesModel = new TrainingSettingsDatesModel(date);

        return ViewRenderer.RenderPartialView(
            "~/Views/Dates/Modals/TrainingSettingsDates.ascx",
            trainingDatesModel,
            ControllerContext
        );
    }

    public JsonResult GetUpcomingDatesJson()
    {
        return Json(new
        {
            AllUpcomingDates = R<DateRepo>()
                .GetBy(UserId, onlyUpcoming: true)
                .Select(x => new
                {
                    DateId = x.Id,
                    Title = x.GetTitle()
                })
        });
    }

    public JsonResult TrainingPlanGet(int dateId)
    {
        var date = R<DateRepo>().GetById(dateId);
        return TrainingPlanInfo2Json(date);
    }

    public JsonResult TrainingPlanUpdate(int dateId, TrainingPlanSettings planSettings)
    {
        var date = R<DateRepo>().GetById(dateId);

        TrainingPlanUpdater.Run(date.TrainingPlan, planSettings);

        return TrainingPlanInfo2Json(date);
    }

    private JsonResult TrainingPlanInfo2Json(Date date)
    {
        return Json(new
        {   
            Html = RenderTrainingDates(date),
            RemainingDates = date.TrainingPlan.DatesInFuture.Count,
            RemainingTime = new TimeSpanLabel(date.TrainingPlan.TimeRemaining).Full,
            QuestionCount = date.CountQuestions()
        });
    }
}