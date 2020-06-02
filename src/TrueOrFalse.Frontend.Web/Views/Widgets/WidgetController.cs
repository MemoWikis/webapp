using System;
using System.Web.Mvc;
using static System.String;

[SetMainMenu(MainMenuEntry.None)]
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