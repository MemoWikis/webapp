using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Seedworks.Web.State;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class LearningController : BaseController
{
    private class SessionData
    {
        public SessionData(string currentSessionHeader = "", int currentStepIdx = -1, bool isLastStep = false, int skipStepIdx = -1, int learningSessionId = -1)
        {
            CurrentSessionHeader = currentSessionHeader;
            SkipStepIdx = skipStepIdx;
            IsLastStep = isLastStep;
            LearningSessionId = learningSessionId;
        }

        public string CurrentSessionHeader { get; private set; }
        public int SkipStepIdx { get; private set; }
        public bool IsLastStep { get; private set; }
        public Guid CurrentStepGuid { get; private set; }
        public int LearningSessionId { get; private set; }
    }

    [HttpPost]
    public JsonResult GetNewAnswerBodyForTopic(LearningSessionConfig config)
    {
        if (config.CurrentUserId == 0 && SessionUser.IsLoggedIn)
            config.CurrentUserId = SessionUser.UserId;

        var learningSession = LearningSessionCreator.BuildLearningSession(config);

        LearningSessionCache.AddOrUpdate(learningSession);

        var firstStep = 0;

        return GetNewAnswerBodyForNewLearningSession(firstStep, counter: learningSession.QuestionCounter);
    }

    [HttpGet]
    public bool Test()
    {
        return true;
    }

    [HttpPost]
    public JsonResult GetNewAnswerBodyForNewLearningSession(int skipStepIdx = -1, int index = -1, QuestionCounter counter = null)
    {
        var learningSession = LearningSessionCache.GetLearningSession();
        if (learningSession.Steps.Count == 0)
        {
            return Json(new
            {
                counter
            });
        }

        if (index != -1)
        {
            learningSession.LoadSpecificQuestion(index);
        }
        else
        {
            if (skipStepIdx != -1 && skipStepIdx != 0)
                learningSession.SkipStep();
            else if (skipStepIdx != 0)
                learningSession.NextStep();
        }

        if (learningSession.IsLastStep)
            return Json(new
            {
                forwardToResult = true
            });


        learningSession.QuestionViewGuid = Guid.NewGuid();

        if (counter == null || counter.Max == 0)
            counter = learningSession.QuestionCounter;

        var question = learningSession.Steps[learningSession.CurrentIndex].Question;

        var sessionUserId = IsLoggedIn ? SessionUser.UserId : -1;

        Sl.SaveQuestionView.Run(
            learningSession.QuestionViewGuid,
            question,
            sessionUserId);

        var answerQuestionModel = new AnswerQuestionModel(learningSession, false);

        string currentSessionHeader = "Frage <span id = \"CurrentStepNumber\">" + (answerQuestionModel.CurrentLearningStepIdx + 1) +
                                      "</span> von <span id=\"StepCount\">" + answerQuestionModel.LearningSession.Steps.Count +
                                      "</span>";
        int currentStepIdx = learningSession.CurrentIndex;
        bool isLastStep = answerQuestionModel.IsLastLearningStep;
        string currentUrl = Links.LearningSession(learningSession);

        var sessionData = new SessionData(currentSessionHeader, currentStepIdx, isLastStep, skipStepIdx);
        var config = learningSession.Config;
        return GetQuestionPageData(answerQuestionModel, currentUrl, sessionData, isSession: true,
            isInLearningTab: config.IsInLearningTab, isInTestMode: config.IsInTestMode, counter: counter);
    }

    private JsonResult GetQuestionPageData(
        AnswerQuestionModel answerQuestionModel,
        string currentUrl,
        SessionData sessionData,
        bool isSession = false,
        int testSessionId = -1,
        bool includeTestSessionHeader = false,
        bool isInLearningTab = false,
        bool isInTestMode = false,
        QuestionCounter counter = null)
    {
        string nextPageLink = "", previousPageLink = "";

        if (answerQuestionModel.HasNextPage)
            nextPageLink = answerQuestionModel.NextUrl(Url);

        if (answerQuestionModel.HasPreviousPage)
            previousPageLink = answerQuestionModel.PreviousUrl(Url);


        var answerBody = new AnswerBodyModel(answerQuestionModel, isInLearningTab, isInTestMode);
        var learningSession = LearningSessionCache.GetLearningSession();

        var data = new
        {
            counter,
            //answerBody = answerBody,
            navBarData = new
            {
                nextUrl = nextPageLink,
                previousUrl = previousPageLink,
                currentHtml = isSession
                    ? null
                    : ViewRenderer.RenderPartialView(
                        "~/Views/Questions/Answer/AnswerQuestionPager.ascx",
                        answerQuestionModel,
                        ControllerContext
                    )
            },
            sessionData = isSession
                ? new
                {
                    currentStepIdx = learningSession.CurrentIndex,
                    skipStepIdx = sessionData.SkipStepIdx,
                    isLastStep = sessionData.IsLastStep,
                    currentStepGuid = sessionData.CurrentStepGuid,
                    currentSessionHeader = sessionData.CurrentSessionHeader,
                    learningSessionId = sessionData.LearningSessionId,
                    stepCount = learningSession.Steps.Count
                }
                : null,
            url = currentUrl,
            commentsAsHtml = ViewRenderer.RenderPartialView(
                "~/Views/Questions/Answer/Comments/CommentsSectionComponent.vue.ascx", answerQuestionModel,
                ControllerContext),
            isInTestMode
        };
        var serializedPageData = Json(data);

        return serializedPageData;
    }

}