using System;
using System.Collections.Generic;
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

    [HttpPost]
    public JsonResult CopyDetails(int id)
    {
        var date = R<DateRepo>().GetById(id);

        return new JsonResult
        {
            Data = new
            {
                DateInfo = date.GetTitle(),
                DateOwner = date.User.Name
            }
        };
    }

    [HttpPost]
    public JsonResult Copy(int sourceDateId)
    {
        var copiedDateId = R<CopyDate>().Run(sourceDateId);
        var copiedDateDateTime = R<DateRepo>().GetById(copiedDateId).DateTime;
        var dates = R<DateRepo>()
            .GetBy(UserId)
            .OrderBy(d => d.DateTime)
            .Where(d => d.DateTime >= DateTime.Now)
            .ToList();
        var idx = dates.IndexOf(d => d.Id == copiedDateId);
        int precedingDateId = 0;
        if (idx > 0)
            precedingDateId = dates[idx - 1].Id;
        //followingDate = R<DateRepo>()
        //    .GetBy(UserId)
        //    .OrderBy(d => d.DateTime)
        //    .Where(d => d.Id != copiedDateId)
        //    .LastOrDefault(d => d.DateTime >= copiedDateDateTime);
        //precedingDateId = Date.IsNullOrEmpty(precedingDate) ? 0 : precedingDate.Id;
        return new JsonResult
        {
            Data = new
            {
                CopiedDateId = copiedDateId,
                PrecedingDateId = precedingDateId
            }
        };
    }

    public string RenderCopiedDate(int id)
    {
        var date = R<DateRepo>().GetById(id);
        var dateRowModel = new DateRowModel(date);
        return ViewRenderer.RenderPartialView("~/Views/Dates/DateRow.ascx", dateRowModel, ControllerContext);
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
        if (date.User != _sessionUser.User)
            throw new Exception("not logged in or not possessing user");

        var trainingDate = date.TrainingPlan?.GetNextTrainingDate();

        //var steps = trainingDate != null
        //                ? GetLearningSessionSteps.Run(trainingDate)
        //                : GetLearningSessionSteps.Run(date.Sets.SelectMany(s => s.Questions()).ToList());

        var learningSession = new LearningSession
        {
            DateToLearn = date,
            Steps = GetLearningSessionSteps.Run(date),
        //    Steps = steps,
            User = _sessionUser.User
        };

        R<LearningSessionRepo>().Create(learningSession);

        if (trainingDate.LearningSession != null)
        {
            var previousLearningSession = trainingDate.LearningSession;
            previousLearningSession.CompleteSession();
            R<LearningSessionRepo>().Update(previousLearningSession);
        }
        trainingDate.LearningSession = learningSession;
        R<TrainingDateRepo>().Update(trainingDate);

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
        var planSettings = date.TrainingPlan.Settings;

        return Json(new
        {   
            Html = RenderTrainingDates(date),
            RemainingDates = date.TrainingPlan.OpenDates.Count,
            RemainingTime = new TimeSpanLabel(date.TrainingPlan.TimeRemaining).Full,
            QuestionCount = date.CountQuestions(),
            QuestionsPerDateIdealAmount = planSettings.QuestionsPerDate_IdealAmount,
            AnswerProbabilityThreshold = planSettings.AnswerProbabilityThreshold,
            QuestionsPerDateMinimum = planSettings.QuestionsPerDate_Minimum,
            SpacingBetweenSessionsInMinutes = planSettings.SpacingBetweenSessionsInMinutes,
        });
    }
}