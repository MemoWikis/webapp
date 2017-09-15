using System;
using System.Web.Mvc;
using static System.String;

[SetMenu(MenuEntry.None)]
public class WidgetController : BaseController
{
    public ActionResult Question(int questionId, bool? hideAddToKnowledge, string host, string widgetKey)
    {
        var questionViewGuid = Guid.NewGuid();
        var question = R<QuestionRepo>().GetById(questionId);

        var answerQuestionModel = new AnswerQuestionModel(
            questionViewGuid, 
            question,
            new QuestionSearchSpec()
        ); 

        answerQuestionModel.DisableCommentLink = true;
        answerQuestionModel.IsInWidget = true;

        if (hideAddToKnowledge.HasValue)
            answerQuestionModel.DisableAddKnowledgeButton = hideAddToKnowledge.Value;

        answerQuestionModel.QuestionViewGuid = questionViewGuid;

        Sl.SaveQuestionView.Run(
            questionViewGuid, 
            question, 
            _sessionUser.User, 
            SaveWidgetView.Run(
                host, 
                !IsNullOrEmpty(widgetKey) ? widgetKey : questionId.ToString(), 
                WidgetType.Question,
                questionId
            )
        );

        return View(
            "~/Views/Widgets/WidgetQuestion.aspx",
            new WidgetQuestionModel(answerQuestionModel, host));
    }

    public ActionResult SetWithoutStartScreen(int setId, bool? hideAddToKnowledge, string host, string widgetKey, int questionCount = -1, string title = null, string text = null)
    {
        var set = Sl.SetRepo.GetById(setId);

        var testSession = new TestSession(set, questionCount);

        if (hideAddToKnowledge.HasValue)
            testSession.HideAddKnowledge = hideAddToKnowledge.Value;

        Sl.SessionUser.AddTestSession(testSession);

        return RedirectToAction(
            "SetTestStep",
            "Widget",
            new { testSessionId = testSession.Id, host = host, widgetKey = widgetKey, questionCount = questionCount, title = title, text = text }
        );
    }

    public ActionResult Set(int setId, bool? hideAddToKnowledge, string host, string widgetKey, int questionCount = -1)
    {
        SaveWidgetView.Run(
            host, 
            !IsNullOrEmpty(widgetKey) ? widgetKey : setId.ToString(), 
            WidgetType.SetStartPage,
            setId
        );

        return View(
            "~/Views/Widgets/WidgetSetStart.aspx",
            new WidgetSetStartModel(setId, Convert.ToBoolean(hideAddToKnowledge), host, questionCount, widgetKey));
    }

    public ActionResult SetStart(int setId, bool? hideAddToKnowledge, string host, string widgetKey, int questionCount = -1)
    {
        var set = Sl.SetRepo.GetById(setId);

        var testSession = new TestSession(set, questionCount);

        if(hideAddToKnowledge.HasValue)
            testSession.HideAddKnowledge = hideAddToKnowledge.Value;

        Sl.SessionUser.AddTestSession(testSession);

        return RedirectToAction(
            "SetTestStep", 
            "Widget", 
            new {testSessionId = testSession.Id, host = host, widgetKey = widgetKey, questionCount = questionCount}
        );
    }

    public ActionResult SetTestStep(int testSessionId, string host, string widgetKey, int questionCount, string title = null, string text = null)
    {
        var routeValues = new {testSessionId = testSessionId, host = host, widgetKey = widgetKey, questionCount = questionCount};

        return AnswerQuestionController.TestActionShared(
            testSessionId,
            testSession => RedirectToAction("SetTestResult", "Widget", routeValues),
            (testSession, questionViewGuid, question) => {
                var answerModel =
                    new AnswerQuestionModel(testSession, questionViewGuid, question)
                    {
                        NextUrl = url => url.Action("SetTestStep", "Widget", routeValues),
                        IsInWidget = true,
                        DisableAddKnowledgeButton = testSession.HideAddKnowledge
                    };

                return View("~/Views/Widgets/WidgetSet.aspx", new WidgetSetModel(answerModel, host, title, text));
            },
            testSession => SaveWidgetView.Run(
                host, 
                !IsNullOrEmpty(widgetKey) ? widgetKey : testSession.SetToTestId.ToString(),
                WidgetType.SetStepPage,
                testSession.SetToTestId)
        );
    }

    public ActionResult SetTestResult(int testSessionId, string host, string widgetKey, int questionCount)
    {
        var testSession = GetTestSession.Get(testSessionId);
        var testSessionResultModel = new TestSessionResultModel(testSession);
        var setModel = new WidgetSetResultModel(testSessionResultModel, host, questionCount, widgetKey);

        SaveWidgetView.Run(
            host, 
            !IsNullOrEmpty(widgetKey) ? widgetKey : testSession.SetToTestId.ToString(), 
            WidgetType.SetResult,
            testSession.SetToTestId
        );

        return View("~/Views/Widgets/WidgetSetResult.aspx", setModel);
    }


    public ActionResult SetVideo(int setId, bool? hideAddToKnowledge, string host, string widgetKey)
    {
        SaveWidgetView.Run(
            host, !IsNullOrEmpty(widgetKey) ? widgetKey : setId.ToString(), 
            WidgetType.SetVideo,
            setId
        );

        var set = Sl.SetRepo.GetById(setId);

        return View("~/Views/Widgets/WidgetSetVideo.aspx", new WidgetSetVideoModel(set, hideAddToKnowledge ?? false, host));
    }
}