using System;
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

    public string RenderTrainingDates(int dateId)
    {
        var trainingDatesModel = new TrainingSettingsDatesModel();
        trainingDatesModel.Dates.Add(new TrainingDateModel
        {
            DateTime = DateTime.Now.AddHours(4),
            QuestionCount = 12,
            Date = new Date { Details = "Klassenarbeit DE" }
        });
        trainingDatesModel.Dates.Add(new TrainingDateModel
        {
            DateTime = DateTime.Now.AddHours(24),
            QuestionCount = 21,
            Date = new Date { Details = "Klassenarbeit DE" }
        });
        trainingDatesModel.Dates.Add(new TrainingDateModel
        {
            DateTime = DateTime.Now.AddHours(57),
            QuestionCount = 19,
            Date = new Date { Details = "Mündliche Prüfung am Fr." }
        });

        return ViewRenderer.RenderPartialView(
            "~/Views/Dates/Modals/TrainingSettingsDates.ascx",
            trainingDatesModel,
            ControllerContext
        );
    }

}