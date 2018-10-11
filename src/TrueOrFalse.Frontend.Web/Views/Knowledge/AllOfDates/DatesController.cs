using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class DatesController : BaseController
{
    //[SetMenu(MenuEntry.Dates)]
    //public ActionResult Dates()
    //{
    //    return View(new DatesModel());
    //}

    
    public string  GetDatesOverview()
    {
        return ViewRenderer.RenderPartialView("~/Views/Knowledge/AllOfDates/Dates.ascx", new DatesModel(), ControllerContext);
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
        var copiedDateId = R<CopyDate>().Run(sourceDateId, UserId);
        var dates = R<DateRepo>()
            .GetBy(UserId)
            .OrderBy(d => d.DateTime)
            .Where(d => d.DateTime >= DateTime.Now)
            .ToList();
        var idx = dates.IndexOf(d => d.Id == copiedDateId);
        int precedingDateId = 0;
        if (idx > 0)
            precedingDateId = dates[idx - 1].Id;
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

    [RedirectToErrorPage_IfNotLoggedIn]
    public ActionResult StartLearningSession(int dateId)
    {
        var date = Resolve<DateRepo>().GetById(dateId);
        if (date.User != _sessionUser.User)
            throw new Exception("not logged in or not possessing user");

        var nextTrainingDate = date.TrainingPlan.GetNextTrainingDate(withUpdate: true);

        var learningSession = LearningSession.InitDateSession(date, nextTrainingDate);

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
            DateId = date.Id,
            DateOfDate = date.DateTime.ToString("dd.MM.yyy HH:mm"),
            RemainingDates = date.TrainingPlan.OpenDates.Count,
            RemainingTime = new TimeSpanLabel(date.TrainingPlan.TimeRemaining).Full,
            TimeToNextTrainingDate = new TimeSpanLabel(date.TrainingPlan.TimeToNextDate, showTimeUnit: true).Full,
            QuestionsInNextTrainingDate = date.TrainingPlan.QuestionCountInNextDate,
            QuestionCount = date.CountQuestions(),
            QuestionsPerDateIdealAmount = planSettings.QuestionsPerDate_IdealAmount,
            AnswerProbabilityThreshold = planSettings.AnswerProbabilityThreshold,
            QuestionsPerDateMinimum = planSettings.QuestionsPerDate_Minimum,
            MinSpacingBetweenSessionsInMinutes = planSettings.MinSpacingBetweenSessionsInMinutes,
            EqualizeSpacingBetweenSessions = planSettings.EqualizeSpacingBetweenSessions,
            EqualizedSpacingMaxMultiplier = planSettings.EqualizedSpacingMaxMultiplier,
            EqualizedSpacingDelayerDays = planSettings.EqualizedSpacingDelayerDays,
            ChartTrainingTimeRows = GetChartTrainingTimeRows(date.TrainingPlan),
            LearningGoalIsReached = date.TrainingPlan.LearningGoalIsReached
        });
    }

    private string GetChartTrainingTimeRows(TrainingPlan trainingPlan)
    {
        var tpStats = R<GetTrainingPlanStats>().Run(trainingPlan);
        return R<GetTrainingPlanStats>().TrainingPlanStatsResult2GoogleDataTable(tpStats);

        //return "[[\"01.05.\", 12, 9, \"\"],[\"02.05.\", 3, 4, \"\"],[\"03.05.\", 6, 8, \"\"]]";
    }
}