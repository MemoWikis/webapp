using System;
using System.Web.Mvc;

[SetMenu(MenuEntry.None)]
public class WidgetController : BaseController
{
    public ActionResult Question(int questionId, bool? hideAddToKnowledge, string host)
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

        SaveWidgetView.Run()
        Sl.SaveQuestionView.Run(questionViewGuid, question, _sessionUser.User);

        return View(
            "~/Views/Widgets/WidgetQuestion.aspx",
            new WidgetQuestionModel(answerQuestionModel, host));
    }

    public ActionResult Set(int setId, bool? hideAddToKnowledge, string host)
    {
        return View(
            "~/Views/Widgets/WidgetSetStart.aspx",
            new WidgetSetStartModel(setId, Convert.ToBoolean(hideAddToKnowledge), host));
    }

    public ActionResult SetStart(int setId, bool? hideAddToKnowledge, string host)
    {
        var set = Sl.SetRepo.GetById(setId);
        var testSession = new TestSession(set);

        if(hideAddToKnowledge.HasValue)
            testSession.HideAddKnowledge = hideAddToKnowledge.Value;

        Sl.SessionUser.AddTestSession(testSession);

        return RedirectToAction(
            "SetTestStep", 
            "Widget", 
            new {testSessionId = testSession.Id, host = host}
        );
    }

    public ActionResult SetTestStep(int testSessionId, string host)
    {
        return AnswerQuestionController.TestActionShared(testSessionId,
            testSession => RedirectToAction("SetTestResult", "Widget", new {testSessionId = testSessionId, host}
            ),
            (testSession, questionViewGuid, question) => {
                var answerModel = new AnswerQuestionModel(testSession, questionViewGuid, question);
                answerModel.NextUrl = url => url.Action("SetTestStep", "Widget", new { testSessionId = testSession.Id, host = host });
                answerModel.IsInWidget = true;
                answerModel.DisableAddKnowledgeButton = testSession.HideAddKnowledge;

                return View("~/Views/Widgets/WidgetSet.aspx", new WidgetSetModel(answerModel, host));
            }
        );
    }

    public ActionResult SetTestResult(int testSessionId, string host)
    {
        var testSession = TestSessionResultController.GetTestSession(testSessionId);
        var testSessionResultModel = new TestSessionResultModel(testSession);
        var setModel = new WidgetSetResultModel(testSessionResultModel, host);

        return View("~/Views/Widgets/WidgetSetResult.aspx", setModel);
    }


    public ActionResult SetVideo(int setId, bool? hideAddToKnowledge, string host)
    {
        var set = Sl.SetRepo.GetById(setId);

        return View("~/Views/Widgets/WidgetSetVideo.aspx", new WidgetSetVideoModel(set, hideAddToKnowledge ?? false, host));
    }
}